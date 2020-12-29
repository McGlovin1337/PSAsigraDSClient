using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientConfiguration: DSClientCmdlet
    {
        protected abstract void ProcessConfiguration(ClientConfiguration clientConfiguration);

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Performing Action: Retrieve DS-Client Configuration");
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            ProcessConfiguration(DSClientConfigMgr);

            DSClientConfigMgr.Dispose();
        }

        protected class DSClientConfiguration
        {
            public bool DailyAdminEnabled { get; private set; }
            public TimeInDay DailyAdminTime { get; private set; }
            public bool WeeklyAdminEnabled { get; private set; }
            public string WeeklyAdminDay { get; private set; }
            public TimeInDay WeeklyAdminTime { get; private set; }
            public bool RebootAfterAdmin { get; private set; }
            public string CDPStrategy { get; private set; }
            public string DatabaseBackup { get; private set; }
            public string KeepDatabaseDump { get; private set; }
            public string LogDuration { get; private set; }
            public int ReconnectAttempts { get; private set; }
            public int ReconnectInterval { get; private set; }
            public bool SkipPreScan { get; private set; }

            public DSClientConfiguration(ClientConfiguration clientConfiguration)
            {
                TimeInDay dailyAdmin = new TimeInDay(clientConfiguration.getDailyAdminRunTime());
                TimeInDay weeklyAdmin = new TimeInDay(clientConfiguration.getWeeklyAdminRunTime());
                string keepDatabaseDump = EnumToString(clientConfiguration.getKeepDBDumpFile());
                if (keepDatabaseDump == "DeleteAtferSuccesfulBackup")
                    keepDatabaseDump = "DeleteAfterSuccessfulBackup"; // Convert due to spelling mistake in the EParametersDSClientKeepDBDumpFile Enum

                DailyAdminEnabled = dailyAdmin.Hour >= 0 && dailyAdmin.Hour <= 23;
                DailyAdminTime = dailyAdmin;
                WeeklyAdminEnabled = weeklyAdmin.Hour >= 0 && dailyAdmin.Hour <= 23;
                WeeklyAdminDay = EnumToString(clientConfiguration.getWeeklyAdminRunDay());
                WeeklyAdminTime = weeklyAdmin;
                RebootAfterAdmin = clientConfiguration.getRebootAfterAdmin();
                CDPStrategy = EnumToString(clientConfiguration.getDefaultCDPStrategy());
                DatabaseBackup = EnumToString(clientConfiguration.getDSClientDatabaseBackupAdmin());
                KeepDatabaseDump = keepDatabaseDump;
                LogDuration = EnumToString(clientConfiguration.getDSClientLogDuration());
                ReconnectAttempts = clientConfiguration.getAttemptToReconnectTryNumber();
                ReconnectInterval = clientConfiguration.getAttemptToReconnectTryInterval();
                SkipPreScan = clientConfiguration.getSkipPreScanForScheduledBackups();
            }
        }
    }
}