using AsigraDSClientApi;
using System;
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

        protected static int StringArrayEScheduleWeekDaysToInt(IEnumerable<string> weekdays)
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

        protected static EBackupPolicy StringToEBackupPolicy(string backupPolicy)
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

        protected static ESQLDumpMethod StringToESQLDumpMethod(string dumpMethod)
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