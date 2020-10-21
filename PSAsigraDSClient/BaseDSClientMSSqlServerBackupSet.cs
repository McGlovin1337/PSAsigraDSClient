using AsigraDSClientApi;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Management.Automation;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientMSSqlServerBackupSet: BaseDSClientBackupSetParams
    {
        [Parameter(HelpMessage = "Specify Computer Credentials")]
        public PSCredential Credential { get; set; }

        [Parameter(HelpMessage = "Specify Database Credentials")]
        public PSCredential DbCredential { get; set; }

        [Parameter(HelpMessage = "Specify Database Dump Method")]
        [ValidateSet("DumpLocal", "DumpBuffer", "DumpPipe")]
        public string DumpMethod { get; set; } = "DumpPipe";

        [Parameter(HelpMessage = "Specify Path to Dump Database")]
        public string DumpPath { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Specify the SQL Backup Method")]
        [ValidateSet("Full", "FullDiff", "FullInc")]
        public string BackupMethod { get; set; }

        [Parameter(HelpMessage = "Specify Day of Month for Full Backup")]
        public int FullMonthlyDay { get; set; }

        [Parameter(HelpMessage = "Specify Time for Monthly Full Backup")]
        public string FullMonthlyTime { get; set; }

        [Parameter(HelpMessage = "Specify Day for Weekly Full Backup")]
        [ValidateSet("Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday")]
        public string FullWeeklyDay { get; set; }

        [Parameter(HelpMessage = "Specify Time for Weekly Full Backup")]
        public string FullWeeklyTime { get; set; }

        [Parameter(HelpMessage = "Specify Periodic Full Backup")]
        [ValidateSet("Seconds", "Minutes", "Hours", "Days", "Weeks", "Months", "Years")]
        public string FullPeriod { get; set; }

        [Parameter(HelpMessage = "Specify Value for Periodic Full Backup")]
        public int FullPeriodValue { get; set; }

        [Parameter(HelpMessage = "Specify Week Days to Skip Full Backups")]
        [ValidateSet("Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday")]
        public string[] SkipWeekDays { get; set; }

        [Parameter(HelpMessage = "Specify Time to Skip Week Day Full Backups From")]
        public string SkipWeekDaysFrom { get; set; }

        [Parameter(HelpMessage = "Specify Time to Skip Week Day Full Backups To")]
        public string SkipWeekDaysTo { get; set; }

        protected abstract void ProcessMsSqlBackupSet();

        protected override void DSClientProcessRecord()
        {
            // Check DS-Client is Windows
            if (DSClientOSType.OsType != "Windows")
                throw new Exception("MS SQL Server Backup Sets can only be created on a Windows DS-Client");

            // Validate the Common Base Parameters
            BaseBackupSetParamValidation(MyInvocation.BoundParameters);

            // Validate Parameters Specific to this Cmdlet
            if (DumpMethod != "DumpPipe" && DumpPath == null)
                throw new ParameterBindingException("DumpPath must be specified");

            if ((MyInvocation.BoundParameters.ContainsKey("FullMonthlyDay") && FullMonthlyTime == null) ||
                FullMonthlyTime != null && !MyInvocation.BoundParameters.ContainsKey("FullMonthlyDay"))
                throw new ParameterBindingException("FullMonthlyDay and FullMonthlyTime must be specified together");

            if ((MyInvocation.BoundParameters.ContainsKey("FullWeeklyDay") && FullWeeklyTime == null) ||
                FullWeeklyTime != null && !MyInvocation.BoundParameters.ContainsKey("FullWeeklyDay"))
                throw new ParameterBindingException("FullWeeklyDay and FullWeeklyTime must be specified together");

            if ((FullPeriod != null && !MyInvocation.BoundParameters.ContainsKey("FullPeriodValue")) ||
                MyInvocation.BoundParameters.ContainsKey("FullPeriodValue") && FullPeriod == null)
                throw new ParameterBindingException("FullPeriod and FullPeriodValue must be specified together");

            ProcessMsSqlBackupSet();
        }

        protected static MSSQL_BackupSet ProcessMsSqlServerBackupSetParams(Dictionary<string, object> sqlParams, MSSQL_BackupSet backupSet)
        {
            // Set the Dump Method
            sqlParams.TryGetValue("DumpMethod", out object DumpMethod);
            sqlParams.TryGetValue("DumpPath", out object DumpPath);

            mssql_dump_parameters dumpParameters = new mssql_dump_parameters
            {
                dump_method = StringToESQLDumpMethod(DumpMethod as string),
                path = (DumpPath != null) ? DumpPath as string : ""
            };
            backupSet.setDumpParameters(dumpParameters);

            // Set the Incremental Policy
            sqlParams.TryGetValue("BackupMethod", out object BackupMethod);
            sqlParams.TryGetValue("FullMonthlyDay", out object FullMonthlyDay);
            sqlParams.TryGetValue("FullMonthlyTime", out object FullMonthlyTime);
            sqlParams.TryGetValue("FullWeeklyDay", out object FullWeeklyDay);
            sqlParams.TryGetValue("FullWeeklyTime", out object FullWeeklyTime);
            sqlParams.TryGetValue("FullPeriod", out object FullPeriod);
            sqlParams.TryGetValue("FullPeriodValue", out object FullPeriodValue);
            sqlParams.TryGetValue("SkipWeekDays", out object SkipWeekDays);
            sqlParams.TryGetValue("SkipWeekDaysFrom", out object SkipWeekDaysFrom);
            sqlParams.TryGetValue("SkipWeekDaysTo", out object SkipWeekDaysTo);

            incremental_policies incrementalPolicies = new incremental_policies
            {
                backup_policy = StringToEBackupPolicy(BackupMethod as string),
                force_full_monthly_day = (FullMonthlyDay as int?).GetValueOrDefault(1),
                force_full_monthly_time = (FullMonthlyTime != null) ? StringTotime_in_day(FullMonthlyTime as string) : StringTotime_in_day("00:00:00"),
                force_full_weekly_day = (FullWeeklyDay != null) ? StringToEWeekDay(FullWeeklyDay as string) : EWeekDay.EWeekDay__UNDEFINED,
                force_full_weekly_time = (FullWeeklyTime != null) ? StringTotime_in_day(FullWeeklyTime as string) : StringTotime_in_day("00:00:00"),
                is_force_full_monthly = (FullMonthlyDay != null) ? true : false,
                is_force_full_periodically = (FullPeriod != null) ? true : false,
                is_force_full_weekly = (FullWeeklyDay != null) ? true : false,
                is_skip_full_on_weekdays = (SkipWeekDays != null) ? true : false,
                skip_full_on_weekdays = (SkipWeekDays != null) ? StringArrayEScheduleWeekDaysToInt(SkipWeekDays as string[]) : (int)EScheduleWeekDays.EScheduleWeekDays__UNDEFINED,
                skip_full_on_weekdays_from = (SkipWeekDaysFrom != null) ? StringTotime_in_day(SkipWeekDaysFrom as string) : StringTotime_in_day("00:00:00"),
                skip_full_on_weekdays_to = (SkipWeekDaysTo != null) ? StringTotime_in_day(SkipWeekDaysTo as string) : StringTotime_in_day("00:00:00"),
                unit_type = (FullPeriod != null) ? StringToETimeUnit(FullPeriod as string) : ETimeUnit.ETimeUnit__UNDEFINED,
                unit_value = (FullPeriodValue as int?).GetValueOrDefault(1)
            };
            backupSet.setIncrementalPolicies(incrementalPolicies);

            return backupSet;
        }

        private static int StringArrayEScheduleWeekDaysToInt(IEnumerable<string> weekdays)
        {
            int WeekDays = 0;

            foreach (string weekday in weekdays)
            {
                switch(weekday)
                {
                    case "Monday":
                        WeekDays += (int)EScheduleWeekDays.EScheduleWeekDays__Monday;
                        break;
                    case "Tuesday":
                        WeekDays += (int)EScheduleWeekDays.EScheduleWeekDays__Tuesday;
                        break;
                    case "Wednesday":
                        WeekDays += (int)EScheduleWeekDays.EScheduleWeekDays__Wednesday;
                        break;
                    case "Thursday":
                        WeekDays += (int)EScheduleWeekDays.EScheduleWeekDays__Thrusday;
                        break;
                    case "Friday":
                        WeekDays += (int)EScheduleWeekDays.EScheduleWeekDays__Friday;
                        break;
                    case "Saturday":
                        WeekDays += (int)EScheduleWeekDays.EScheduleWeekDays__Saturday;
                        break;
                    case "Sunday":
                        WeekDays += (int)EScheduleWeekDays.EScheduleWeekDays__Sunday;
                        break;
                }
            }

            return WeekDays;
        }

        private static EBackupPolicy StringToEBackupPolicy(string backupPolicy)
        {
            EBackupPolicy Policy = EBackupPolicy.EBackupPolicy__UNDEFINED;

            switch(backupPolicy)
            {
                case "Full":
                    Policy = EBackupPolicy.EBackupPolicy__FullBackup;
                    break;
                case "FullDiff":
                    Policy = EBackupPolicy.EBackupPolicy__DiffBackup;
                    break;
                case "FullInc":
                    Policy = EBackupPolicy.EBackupPolicy__IncBackup;
                    break;
            }    

            return Policy;
        }

        private static ESQLDumpMethod StringToESQLDumpMethod(string dumpMethod)
        {
            ESQLDumpMethod DumpMethod = ESQLDumpMethod.ESQLDumpMethod__UNDEFINED;

            switch(dumpMethod)
            {
                case "DumpLocal":
                    DumpMethod = ESQLDumpMethod.ESQLDumpMethod__DumpToSQLPath;
                    break;
                case "DumpBuffer":
                    DumpMethod = ESQLDumpMethod.ESQLDumpMethod__DumpToClientBuffer;
                    break;
                case "DumpPipe":
                    DumpMethod = ESQLDumpMethod.ESQLDumpMethod__DumpToPipe;
                    break;
            }

            return DumpMethod;
        }
    }
}