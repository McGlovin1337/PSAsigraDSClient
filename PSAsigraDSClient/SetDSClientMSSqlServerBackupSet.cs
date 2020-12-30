using System;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientMSSqlServerBackupSet")]

    public class SetDSClientMSSqlServerBackupSet: BaseDSClientMSSqlServerBackupSet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set to modify")]
        public int BackupSetId { get; set; }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify a new Name for the Backup Set")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Backup Set Type")]
        [ValidateSet("Offsite", "Statistical", "SelfContained", "LocalOnly")]
        public new string SetType { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Compression Method to use")]
        [ValidateSet("None", "ZLIB", "LZOP", "ZLIB_LO", "ZLIB_MED", "ZLIB_HI")]
        public new string Compression { get; set; }

        [Parameter(HelpMessage = "Specify a Dump Method")]
        [ValidateSet("DumpLocal", "DumpBuffer", "DumpPipe")]
        public new string DumpMethod { get; set; }

        [Parameter(HelpMessage = "Specify the SQL Backup Method")]
        [ValidateSet("Full", "FullDiff", "FullInc")]
        public new string BackupMethod { get; set; }

        [Parameter(HelpMessage = "Disable Monthly Full Backups")]
        public SwitchParameter DisableFullMonthly { get; set; }

        [Parameter(HelpMessage = "Disable Weekly Full Backups")]
        public SwitchParameter DisableFullWeekly { get; set; }

        [Parameter(HelpMessage = "Disable Periodic Full Backups")]
        public SwitchParameter DisableFullPeriod { get; set; }

        [Parameter(HelpMessage = "Disable Skipping Weekday Full Backups")]
        public SwitchParameter DisableSkipWeekDays { get; set; }

        protected override void ProcessMsSqlBackupSet()
        {
            // Get the Backup Set from DS-Client
            WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId {BackupSetId}");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            WriteVerbose("Performing Action: Process Changes to Backup Set");
            // Update Computer Credentials if Credentials specified
            if (Credential != null)
            {
                Win32FS_Generic_BackupSetCredentials backupSetCredentials = Win32FS_Generic_BackupSetCredentials.from(backupSet.getCredentials());

                string user = Credential.UserName;
                string pass = Credential.GetNetworkCredential().Password;
                backupSetCredentials.setCredentials(user, pass);

                backupSet.setCredentials(backupSetCredentials);
            }

            // Process Common Backup Set Parameters
            backupSet = ProcessBaseBackupSetParams(MyInvocation.BoundParameters, backupSet);

            MSSQL_BackupSet sqlBackupSet = MSSQL_BackupSet.from(backupSet);

            // Update Database Credentials if specified
            if (DbCredential != null)
            {
                Win32FS_Generic_BackupSetCredentials dbCredentials = Win32FS_Generic_BackupSetCredentials.from(sqlBackupSet.getDBCredentials());

                string dbUser = DbCredential.UserName;
                string dbPass = DbCredential.GetNetworkCredential().Password;
                dbCredentials.setCredentials(dbUser, dbPass);

                sqlBackupSet.setDBCredentials(dbCredentials);
            }

            // Set the Schedule and Retention Rules
            if (MyInvocation.BoundParameters.ContainsKey("ScheduleId"))
            {
                ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();
                Schedule schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);
                sqlBackupSet.setSchedule(schedule);
            }

            if (MyInvocation.BoundParameters.ContainsKey("RetentionRuleId"))
            {
                RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();
                RetentionRule[] retentionRules = DSClientRetentionRuleMgr.definedRules();
                RetentionRule retentionRule = retentionRules.Single(rule => rule.getID() == RetentionRuleId);
                sqlBackupSet.setRetentionRule(retentionRule);
            }

            // Process this Cmdlets specific Parameters
            // Process Database Dump Method
            if (DumpMethod != null || DumpPath != null)
            {
                mssql_dump_parameters dumpParameters = sqlBackupSet.getDumpParameters();

                if (DumpMethod != null)
                {
                    ESQLDumpMethod dumpMethod = StringToESQLDumpMethod(DumpMethod);
                    if (dumpMethod != dumpParameters.dump_method)
                        dumpParameters.dump_method = dumpMethod;
                }

                if (DumpPath != null)
                    dumpParameters.path = DumpPath;

                sqlBackupSet.setDumpParameters(dumpParameters);
            }

            // Process Incremental Policies
            incremental_policies incrementalPolicies = sqlBackupSet.getIncrementalPolicies();

            if (BackupMethod != null)
                incrementalPolicies.backup_policy = StringToEBackupPolicy(BackupMethod);

            if (MyInvocation.BoundParameters.ContainsKey("FullMonthlyDay") || FullMonthlyTime != null)
            {
                if (MyInvocation.BoundParameters.ContainsKey("FullMonthlyDay"))
                    incrementalPolicies.force_full_monthly_day = FullMonthlyDay;

                if (FullMonthlyTime != null)
                    incrementalPolicies.force_full_monthly_time = StringTotime_in_day(FullMonthlyTime);

                incrementalPolicies.is_force_full_monthly = true;
            }

            if (FullWeeklyDay != null || FullWeeklyTime != null)
            {
                if (FullWeeklyDay != null)
                    incrementalPolicies.force_full_weekly_day = (EWeekDay)Enum.Parse(typeof(EWeekDay), $"EWeekDay__{FullWeeklyDay}", true);

                if (FullWeeklyTime != null)
                    incrementalPolicies.force_full_weekly_time = StringTotime_in_day(FullWeeklyTime);

                incrementalPolicies.is_force_full_weekly = true;
            }

            if (FullPeriod != null || MyInvocation.BoundParameters.ContainsKey("FullPeriodValue"))
            {
                if (FullPeriod != null)
                    incrementalPolicies.unit_type = (ETimeUnit)Enum.Parse(typeof(ETimeUnit), $"ETimeUnit__{FullPeriod}", true);

                if (MyInvocation.BoundParameters.ContainsKey("FullPeriodValue"))
                    incrementalPolicies.unit_value = FullPeriodValue;

                incrementalPolicies.is_force_full_periodically = true;
            }

            if (SkipWeekDays != null || SkipWeekDaysFrom != null || SkipWeekDaysTo != null)
            {
                if (SkipWeekDays != null)
                    incrementalPolicies.skip_full_on_weekdays = StringArrayEScheduleWeekDaysToInt(SkipWeekDays);

                if (SkipWeekDaysFrom != null)
                    incrementalPolicies.skip_full_on_weekdays_from = StringTotime_in_day(SkipWeekDaysFrom);

                if (SkipWeekDaysTo != null)
                    incrementalPolicies.skip_full_on_weekdays_to = StringTotime_in_day(SkipWeekDaysTo);

                if (incrementalPolicies.skip_full_on_weekdays > 0)
                    incrementalPolicies.is_skip_full_on_weekdays = true;
            }

            // Typically these values are enabled by configuring them (see above), however we can also specify "$false" to enable them, giving the user the ability to enable a previous configuration
            if (MyInvocation.BoundParameters.ContainsKey("DisableFullMonthly"))
                incrementalPolicies.is_force_full_monthly = !DisableFullMonthly;

            if (MyInvocation.BoundParameters.ContainsKey("DisableFullWeekly"))
                incrementalPolicies.is_force_full_weekly = !DisableFullWeekly;

            if (MyInvocation.BoundParameters.ContainsKey("DisableFullPeriod"))
                incrementalPolicies.is_force_full_periodically = !DisableFullPeriod;

            if (MyInvocation.BoundParameters.ContainsKey("DisableSkipWeekDays"))
                incrementalPolicies.is_skip_full_on_weekdays = !DisableSkipWeekDays;

            sqlBackupSet.setIncrementalPolicies(incrementalPolicies);
            WriteVerbose("Notice: Completed");

            sqlBackupSet.Dispose();
        }
    }
}