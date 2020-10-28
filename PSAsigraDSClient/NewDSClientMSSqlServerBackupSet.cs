﻿using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientMSSqlServerBackupSet")]

    public class NewDSClientMSSqlServerBackupSet: BaseDSClientMSSqlServerBackupSet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The name of the Backup Set")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Computer the Backup Set will be Assigned To")]
        [ValidateNotNullOrEmpty]
        public string Computer { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Items to Include in Backup Set")]
        public string[] IncludeItem { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Max Number of Generations for Included Items")]
        public int MaxGenerations { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Items to Exclude from Backup Set")]
        public string[] ExcludeItem { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Regex Item Exclusion Patterns")]
        [ValidateNotNullOrEmpty]
        public string[] RegexExcludeItem { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Path for Regex Exclusion Item")]
        [ValidateNotNullOrEmpty]
        public string RegexExclusionPath { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to also Exclude Directories with Regex pattern")]
        public SwitchParameter RegexExcludeDirectory { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify if Regex Exclusions Items are case insensitive")]
        public SwitchParameter RegexCaseInsensitive { get; set; }

        [Parameter(HelpMessage = "Specify to run Database Consistency Check DBCC")]
        public SwitchParameter RunDBCC { get; set; }

        [Parameter(HelpMessage = "Specify to Stop on DBCC Errors")]
        public SwitchParameter DBCCErrorStop { get; set; }

        [Parameter(HelpMessage = "Specify to Backup Transaction Log")]
        public SwitchParameter BackupLog { get; set; }

        protected override void ProcessMsSqlBackupSet()
        {
            // Create Data Source Browser to Resolve Computer (expandToFullPath method doesn't work when data browser is SQLServer)
            DataSourceBrowser dataSourceBrowser = DSClientSession.createBrowser(EBackupDataType.EBackupDataType__FileSystem);

            // Try to resolve the supplied Computer
            string computer = dataSourceBrowser.expandToFullPath(Computer);
            WriteVerbose("Specified Computer resolved to: " + computer);

            // Change Data Source Browser to correct type
            dataSourceBrowser.Dispose();
            dataSourceBrowser = DSClientSession.createBrowser(EBackupDataType.EBackupDataType__SQLServer);

            // Set Computer Credentials
            Win32FS_Generic_BackupSetCredentials backupSetCredentials = Win32FS_Generic_BackupSetCredentials.from(dataSourceBrowser.neededCredentials(computer));

            if (Credential != null)
            {
                string user = Credential.UserName;
                string pass = Credential.GetNetworkCredential().Password;
                backupSetCredentials.setCredentials(user, pass);
            }
            else
            {
                WriteVerbose("Credentials not specified, using DS-Client Credentials...");
                backupSetCredentials.setUsingClientCredentials(true);
            }
            dataSourceBrowser.setCurrentCredentials(backupSetCredentials);

            // Create Backup Set Object
            WriteVerbose("Creating a new Backup Set object...");
            SQLDataBrowserWithSetCreation setCreation = SQLDataBrowserWithSetCreation.from(dataSourceBrowser);
            BackupSet newBackupSet = setCreation.createBackupSet(Computer);

            // Set Database Credentials
            if (DbCredential != null)
            {
                Win32FS_Generic_BackupSetCredentials databaseCredentials = new Win32FS_Generic_BackupSetCredentials();
                string dbUser = DbCredential.UserName;
                string dbPass = DbCredential.GetNetworkCredential().Password;
                databaseCredentials.setCredentials(dbUser, dbPass);
                setCreation.setDBCredentials(databaseCredentials);
            }
            else
                setCreation.setDBCredentials(backupSetCredentials);

            // Process Common Backup Set Parameters
            newBackupSet = ProcessBaseBackupSetParams(MyInvocation.BoundParameters, newBackupSet);

            MSSQL_BackupSet newSqlBackupSet = MSSQL_BackupSet.from(newBackupSet);

            // Process Inclusion & Exclusion Items
            if (IncludeItem != null || ExcludeItem != null)
            {
                List<BackupSetItem> backupSetItems = new List<BackupSetItem>();

                if (ExcludeItem != null)
                    backupSetItems.AddRange(ProcessExclusionItems(DSClientOSType, dataSourceBrowser, computer, ExcludeItem));

                if (RegexExcludeItem != null)
                    backupSetItems.AddRange(ProcessRegexExclusionItems(dataSourceBrowser, computer, RegexExclusionPath, RegexExcludeDirectory, RegexCaseInsensitive, RegexExcludeItem));

                if (IncludeItem != null)
                    backupSetItems.AddRange(ProcessMsSqlInclusionItems(dataSourceBrowser, computer, IncludeItem, MaxGenerations, BackupLog, RunDBCC, DBCCErrorStop));

                newSqlBackupSet.setItems(backupSetItems.ToArray());
            }

            // Set the Schedule and Retention Rules
            if (MyInvocation.BoundParameters.ContainsKey("ScheduleId"))
            {
                ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();
                Schedule schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);
                newSqlBackupSet.setSchedule(schedule);
            }

            if (MyInvocation.BoundParameters.ContainsKey("RetentionRuleId"))
            {
                RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();
                RetentionRule[] retentionRules = DSClientRetentionRuleMgr.definedRules();
                RetentionRule retentionRule = retentionRules.Single(rule => rule.getID() == RetentionRuleId);
                newSqlBackupSet.setRetentionRule(retentionRule);
            }

            // Process this Cmdlets specific Parameters
            // Process Dump Parameters
            mssql_dump_parameters dumpParameters = new mssql_dump_parameters
            {
                dump_method = StringToESQLDumpMethod(DumpMethod),
                path = DumpPath ?? ""
            };
            newSqlBackupSet.setDumpParameters(dumpParameters);

            // Process Incremental Policies
            incremental_policies incrementalPolicies = new incremental_policies
            {
                backup_policy = StringToEBackupPolicy(BackupMethod),
                force_full_monthly_day = (MyInvocation.BoundParameters.ContainsKey("FullMonthlyDay")) ? FullMonthlyDay : 1,
                force_full_monthly_time = (FullMonthlyTime != null) ? StringTotime_in_day(FullMonthlyTime) : StringTotime_in_day("19:00:00"),
                is_force_full_monthly = (MyInvocation.BoundParameters.ContainsKey("FullMonthlyDay")) ? true : false,
                force_full_weekly_day = (FullWeeklyDay != null) ? StringToEWeekDay(FullWeeklyDay) : EWeekDay.EWeekDay__UNDEFINED,
                force_full_weekly_time = (FullWeeklyTime != null) ? StringTotime_in_day(FullWeeklyTime) : StringTotime_in_day("19:00:00"),
                is_force_full_weekly = (FullWeeklyDay != null) ? true : false,
                unit_type = (FullPeriod != null) ? StringToETimeUnit(FullPeriod) : ETimeUnit.ETimeUnit__UNDEFINED,
                unit_value = (MyInvocation.BoundParameters.ContainsKey("FullPeriodValue")) ? FullPeriodValue : 0,
                is_force_full_periodically = (FullPeriod != null) ? true : false,
                skip_full_on_weekdays = (SkipWeekDays != null) ? StringArrayEScheduleWeekDaysToInt(SkipWeekDays) : (int)EScheduleWeekDays.EScheduleWeekDays__UNDEFINED,
                skip_full_on_weekdays_from = (SkipWeekDaysFrom != null) ? StringTotime_in_day(SkipWeekDaysFrom) : StringTotime_in_day("07:00:00"),
                skip_full_on_weekdays_to = (SkipWeekDaysTo != null) ? StringTotime_in_day(SkipWeekDaysTo) : StringTotime_in_day("19:00:00"),
                is_skip_full_on_weekdays = (SkipWeekDays != null) ? true : false
            };
            newSqlBackupSet.setIncrementalPolicies(incrementalPolicies);

            // Add the Backup Set to the DS-Client
            WriteVerbose("Adding the new Backup Set Object to DS-Client...");
            DSClientSession.addBackupSet(newSqlBackupSet);
            WriteObject("Backup Set Created with BackupSetId: " + newSqlBackupSet.getID());

            newSqlBackupSet.Dispose();
            setCreation.Dispose();
            dataSourceBrowser.Dispose();
        }
    }
}