using AsigraDSClientApi;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientMSSqlServerBackupSet: BaseDSClientBackupSetParams
    {
        [Parameter(HelpMessage = "Specify Database Credentials")]
        public DSClientCredential DbCredential { get; set; }

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
            if (DSClientSessionInfo.OperatingSystem != "Windows")
            {
                ErrorRecord errorRecord = new ErrorRecord(
                    new PlatformNotSupportedException("MS SQL Server Backup Sets can only be created on a Windows DS-Client"),
                    "PlatformNotSupportedException",
                    ErrorCategory.InvalidOperation,
                    null);
                WriteError(errorRecord);
            }
            else
            {
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
        }

        protected static int StringArrayEScheduleWeekDaysToInt(IEnumerable<string> weekdays)
        {
            int WeekDays = 0;

            foreach (string weekday in weekdays)
                WeekDays += (int)StringToEnum<EScheduleWeekDays>(weekday);

            return WeekDays;
        }

        protected static EBackupPolicy StringToEBackupPolicy(string backupPolicy)
        {
            switch(backupPolicy.ToLower())
            {
                case "full":
                    return EBackupPolicy.EBackupPolicy__FullBackup;
                case "fulldiff":
                    return EBackupPolicy.EBackupPolicy__DiffBackup;
                case "fullinc":
                    return EBackupPolicy.EBackupPolicy__IncBackup;
                default:
                    return EBackupPolicy.EBackupPolicy__UNDEFINED;
            }
        }

        public static ESQLDumpMethod StringToESQLDumpMethod(string dumpMethod)
        {
            switch(dumpMethod.ToLower())
            {
                case "dumplocal":
                    return ESQLDumpMethod.ESQLDumpMethod__DumpToSQLPath;
                case "dumpbuffer":
                    return ESQLDumpMethod.ESQLDumpMethod__DumpToClientBuffer;
                case "dumppipe":
                    return ESQLDumpMethod.ESQLDumpMethod__DumpToPipe;
                default:
                    return ESQLDumpMethod.ESQLDumpMethod__UNDEFINED;
            }
        }
    }
}