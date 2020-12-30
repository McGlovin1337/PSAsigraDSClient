using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientConfiguration")]

    public class SetDSClientConfiguration: BaseDSClientConfiguration
    {
        [Parameter(HelpMessage = "Disable DS-Client Daily Admin")]
        public SwitchParameter DisableDailyAdmin { get; set; }

        [Parameter(HelpMessage = "Set DS-Client Daily Admin Run Time")]
        public string DailyAdminTime { get; set; }

        [Parameter(HelpMessage = "Disable DS-Client Weekly Admin")]
        public SwitchParameter DisableWeeklyAdmin { get; set; }

        [Parameter(HelpMessage = "Set DS-Client Weekly Admin Day")]
        [ValidateSet("Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday")]
        public string WeeklyAdminDay { get; set; }

        [Parameter(HelpMessage = "Set DS-Client Weekly Admin Run Time")]
        public string WeeklyAdminTime { get; set; }

        [Parameter(HelpMessage = "Reboot DS-Client After Running Daily or Weekly Admin")]
        public SwitchParameter RebootAfterAdmin { get; set; }

        [Parameter(HelpMessage = "Specify the CDP Strategy")]
        [ValidateSet("Suspend", "Skip")]
        public string CDPStrategy { get; set; }

        [Parameter(HelpMessage = "Specify DS-Client Database Backup During Admin")]
        [ValidateSet("None", "WithDailyAdmin", "WithWeeklyAdmin")]
        public string DatabaseBackup { get; set; }

        [Parameter(HelpMessage = "Specify Keeping DS-Client Database Dump Following Backup")]
        [ValidateSet("AlwaysDelete", "DeleteAfterSuccessfulBackup", "DoNotDelete")]
        public string KeepDatabaseDump { get; set; }

        [Parameter(HelpMessage = "Specify Duration to keep DS-Client Log Files")]
        [ValidateSet("SixMonths", "OneYear", "TwoYears", "ThreeYears", "FourYears", "FiveYears", "SixYears", "SevenYears")]
        public string LogDuration { get; set; }

        [Parameter(HelpMessage = "Specify the number of times to re-connect during backup")]
        public int ReconnectAttempts { get; set; }

        [Parameter(HelpMessage = "Specify the interval in minutes to re-connect")]
        public int ReconnectInterval { get; set; }

        [Parameter(HelpMessage = "Specify to Skip PreScan for Scheduled Backups")]
        public SwitchParameter SkipPreScan { get; set; }

        protected override void ProcessConfiguration(ClientConfiguration clientConfiguration)
        {
            WriteVerbose("Performing Action: Update DS-Client Configuration");

            if (DisableDailyAdmin)
                clientConfiguration.setDailyAdminRunTime(new time_in_day
                {
                    hour = -1,
                    minute = 0,
                    second = 0
                });
            else if (MyInvocation.BoundParameters.ContainsKey("DailyAdminTime"))
                clientConfiguration.setDailyAdminRunTime(StringTotime_in_day(DailyAdminTime));

            EWeekDay currentWeeklyAdminDay = clientConfiguration.getWeeklyAdminRunDay();
            time_in_day currentWeeklyAdminTime = clientConfiguration.getWeeklyAdminRunTime();

            if (DisableWeeklyAdmin)
                clientConfiguration.setWeeklyAdminRunTime(currentWeeklyAdminDay, new time_in_day
                {
                    hour = -1,
                    minute = 0,
                    second = 0
                });
            else if (MyInvocation.BoundParameters.ContainsKey("WeeklyAdminDay") &&
                    MyInvocation.BoundParameters.ContainsKey("WeeklyAdminTime"))
                clientConfiguration.setWeeklyAdminRunTime((EWeekDay)Enum.Parse(typeof(EWeekDay), $"EWeekDay__{WeeklyAdminDay}", true), StringTotime_in_day(WeeklyAdminTime));
            else if (MyInvocation.BoundParameters.ContainsKey("WeeklyAdminDay"))
                clientConfiguration.setWeeklyAdminRunTime((EWeekDay)Enum.Parse(typeof(EWeekDay), $"EWeekDay__{WeeklyAdminDay}", true), currentWeeklyAdminTime);
            else if (MyInvocation.BoundParameters.ContainsKey("WeeklyAdminTime"))
                clientConfiguration.setWeeklyAdminRunTime(currentWeeklyAdminDay, StringTotime_in_day(WeeklyAdminTime));

            if (MyInvocation.BoundParameters.ContainsKey("RebootAfterAdmin"))
                clientConfiguration.setRebootAfterAdmin(RebootAfterAdmin);

            if (MyInvocation.BoundParameters.ContainsKey("CDPStrategy"))
                clientConfiguration.setDefaultCDPStrategy((EParametersCDPStrategy)Enum.Parse(typeof(EParametersCDPStrategy), "EParametersCDPStrategy__" + CDPStrategy, true));

            if (MyInvocation.BoundParameters.ContainsKey("DatabaseBackup"))
                clientConfiguration.setDSClientDatabaseBackupAdmin((EParametersBackupAdmin)Enum.Parse(typeof(EParametersBackupAdmin), "EParametersBackupAdmin__" + DatabaseBackup, true));

            if (MyInvocation.BoundParameters.ContainsKey("KeepDatabaseDump"))
            {
                if (KeepDatabaseDump == "DeleteAfterSuccessfulBackup")
                    KeepDatabaseDump = "DeleteAtferSuccesfulBackup"; // Convert due to spelling mistake in the EParametersDSClientKeepDBDumpFile Enum
                clientConfiguration.setKeepDBDumpFile((EParametersDSClientKeepDBDumpFile)Enum.Parse(typeof(EParametersDSClientKeepDBDumpFile), "EParametersDSClientKeepDBDumpFile__" + KeepDatabaseDump, true));
            }

            if (MyInvocation.BoundParameters.ContainsKey("LogDuration"))
                clientConfiguration.setDSClientLogDuration((EParametersDSClientLogDuration)Enum.Parse(typeof(EParametersDSClientLogDuration), "EParametersDSClientLogDuration__" + LogDuration, true));

            if (MyInvocation.BoundParameters.ContainsKey("ReconnectAttempts"))
                clientConfiguration.setAttemptToReconnectTryNumber(ReconnectAttempts);

            if (MyInvocation.BoundParameters.ContainsKey("ReconnectInterval"))
                clientConfiguration.setAttemptToReconnectTryInterval(ReconnectInterval);

            if (MyInvocation.BoundParameters.ContainsKey("SkipPreScan"))
                clientConfiguration.setSkipPreScanForScheduledBackups(SkipPreScan);
        }
    }
}