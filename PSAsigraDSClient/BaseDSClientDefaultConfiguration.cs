using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientDefaultConfiguration: DSClientCmdlet
    {
        protected abstract void ProcessDefaultConfiguration(DSClientDefaultConfiguration dSClientDefaultConfiguration);
        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            WriteVerbose("Retrieving current DS-Client Default Configuration...");
            DefaultConfiguration defaultConfiguration = DSClientConfigMgr.getDefaultConfiguration();
            DefaultConfigurationWindowsClient defaultConfigurationWindows;

            DSClientDefaultConfiguration dSClientDefaultConfiguration;

            if (DSClientOSType.OsType == "Windows")
            {
                defaultConfigurationWindows = DefaultConfigurationWindowsClient.from(defaultConfiguration);
                dSClientDefaultConfiguration = new DSClientDefaultConfiguration(defaultConfigurationWindows);

                ProcessDefaultConfiguration(dSClientDefaultConfiguration);

                defaultConfigurationWindows.Dispose();
            }
            else
            {
                dSClientDefaultConfiguration = new DSClientDefaultConfiguration(defaultConfiguration);

                ProcessDefaultConfiguration(dSClientDefaultConfiguration);

                defaultConfiguration.Dispose();
            }

            DSClientConfigMgr.Dispose();
        }

        protected class DSClientDefaultConfiguration
        {
            public string CompressionType { get; private set; }
            public string DSClientBuffer { get; private set; }
            public string LocalStoragePath { get; private set; }
            public BaseDSClientNotification.DSClientBackupSetNotification BackupSetNotification { get; private set; }
            public int OnlineGenerations { get; private set; }
            public BaseDSClientRetentionRule.DSClientRetentionRule RetentionRule { get; private set; }
            public BaseDSClientSchedule.DSClientScheduleInfo Schedule { get; private set; }
            public string OpenFileOperation { get; private set; }
            public int OpenFileRetryInterval { get; private set; }
            public int OpenFileRetryTimes { get; private set; }
            public bool BackupFilePermissions { get; private set; }

            public DSClientDefaultConfiguration(DefaultConfiguration defaultConfiguration)
            {
                ECompressionType compressionType = defaultConfiguration.getDefaultCompressionType();
                BackupSetNotification backupSetNotification = defaultConfiguration.getDefaultNotification();
                notification_info[] notifyInfo = backupSetNotification.listNotification();
                RetentionRule retentionRule = defaultConfiguration.getDefaultRetentionRule();
                Schedule schedule = defaultConfiguration.getDefaultSchedule();

                CompressionType = ECompressionTypeToString(compressionType);
                DSClientBuffer = defaultConfiguration.getDefaultDSClientBuffer();
                LocalStoragePath = defaultConfiguration.getDefaultLocalStoragePath();
                BackupSetNotification = new BaseDSClientNotification.DSClientBackupSetNotification(notifyInfo[0]);
                OnlineGenerations = defaultConfiguration.getDefaultOnlineGenerations();
                if (retentionRule != null)
                    RetentionRule = new BaseDSClientRetentionRule.DSClientRetentionRule(retentionRule);
                if (schedule != null)
                    Schedule = new BaseDSClientSchedule.DSClientScheduleInfo(schedule);
            }

            public DSClientDefaultConfiguration(DefaultConfigurationWindowsClient defaultConfiguration)
            {                
                ECompressionType compressionType = defaultConfiguration.getDefaultCompressionType();
                BackupSetNotification backupSetNotification = defaultConfiguration.getDefaultNotification();
                notification_info[] notifyInfo = backupSetNotification.listNotification();
                RetentionRule retentionRule = defaultConfiguration.getDefaultRetentionRule();
                Schedule schedule = defaultConfiguration.getDefaultSchedule();
                EOpenFileStrategy openFileStrategy = defaultConfiguration.getDefaultOpenFilesOperation();

                CompressionType = ECompressionTypeToString(compressionType);
                DSClientBuffer = defaultConfiguration.getDefaultDSClientBuffer();
                LocalStoragePath = defaultConfiguration.getDefaultLocalStoragePath();
                BackupSetNotification = new BaseDSClientNotification.DSClientBackupSetNotification(notifyInfo[0]);
                OnlineGenerations = defaultConfiguration.getDefaultOnlineGenerations();
                if (retentionRule != null)
                    RetentionRule = new BaseDSClientRetentionRule.DSClientRetentionRule(retentionRule);
                if (schedule != null)
                    Schedule = new BaseDSClientSchedule.DSClientScheduleInfo(schedule);
                OpenFileOperation = EnumToString(openFileStrategy);
                OpenFileRetryInterval = defaultConfiguration.getDefaultOpenFilesRetryInterval();
                OpenFileRetryTimes = defaultConfiguration.getDefaultOpenFilesRetryTimes();
                BackupFilePermissions = defaultConfiguration.getDefaultToBackupPermissions();
            }
        }        
    }
}