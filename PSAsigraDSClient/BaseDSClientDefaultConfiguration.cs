using AsigraDSClientApi;

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
            public string CompressionType { get; set; }
            public string DSClientBuffer { get; set; }
            public string LocalStoragePath { get; set; }
            public BaseDSClientNotification.DSClientBackupSetNotification BackupSetNotification { get; set; }
            public int OnlineGenerations { get; set; }
            public BaseDSClientRetentionRule.DSClientRetentionRule RetentionRule { get; set; }
            public BaseDSClientSchedule.DSClientScheduleInfo Schedule { get; set; }
            public string OpenFileOperation { get; set; }
            public int OpenFileRetryInterval { get; set; }
            public int OpenFileRetryTimes { get; set; }
            public bool BackupFilePermissions { get; set; }

            public DSClientDefaultConfiguration(DefaultConfiguration defaultConfiguration)
            {
                ECompressionType compressionType = defaultConfiguration.getDefaultCompressionType();
                BackupSetNotification backupSetNotification = defaultConfiguration.getDefaultNotification();
                notification_info[] notifyInfo = backupSetNotification.listNotification();
                RetentionRule retentionRule = defaultConfiguration.getDefaultRetentionRule();
                Schedule schedule = defaultConfiguration.getDefaultSchedule();

                CompressionType = CompressionTypeToString(compressionType);
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

                CompressionType = CompressionTypeToString(compressionType);
                DSClientBuffer = defaultConfiguration.getDefaultDSClientBuffer();
                LocalStoragePath = defaultConfiguration.getDefaultLocalStoragePath();
                BackupSetNotification = new BaseDSClientNotification.DSClientBackupSetNotification(notifyInfo[0]);
                OnlineGenerations = defaultConfiguration.getDefaultOnlineGenerations();
                if (retentionRule != null)
                    RetentionRule = new BaseDSClientRetentionRule.DSClientRetentionRule(retentionRule);
                if (schedule != null)
                    Schedule = new BaseDSClientSchedule.DSClientScheduleInfo(schedule);
                OpenFileOperation = OpenFileOperationToString(openFileStrategy);
                OpenFileRetryInterval = defaultConfiguration.getDefaultOpenFilesRetryInterval();
                OpenFileRetryTimes = defaultConfiguration.getDefaultOpenFilesRetryTimes();
                BackupFilePermissions = defaultConfiguration.getDefaultToBackupPermissions();
            }

            private string OpenFileOperationToString(EOpenFileStrategy openFileStrategy)
            {
                string operation = null;

                switch(openFileStrategy)
                {
                    case EOpenFileStrategy.EOpenFileStrategy__TryDenyWrite:
                        operation = "TryDenyWrite";
                        break;
                    case EOpenFileStrategy.EOpenFileStrategy__DenyWrite:
                        operation = "DenyWrite";
                        break;
                    case EOpenFileStrategy.EOpenFileStrategy__PreventWrite:
                        operation = "PreventWrite";
                        break;
                    case EOpenFileStrategy.EOpenFileStrategy__AllowWrite:
                        operation = "AllowWrite";
                        break;
                    case EOpenFileStrategy.EOpenFileStrategy__UNDEFINED:
                        operation = "Undefined";
                        break;
                }

                return operation;
            }

            private string CompressionTypeToString(ECompressionType compressionType)
            {
                string compressType = null;

                switch (compressionType)
                {
                    case ECompressionType.ECompressionType__NONE:
                        compressType = "None";
                        break;
                    case ECompressionType.ECompressionType__ZLIB:
                        compressType = "ZLIB";
                        break;
                    case ECompressionType.ECompressionType__LZOP:
                        compressType = "LZOP";
                        break;
                    case ECompressionType.ECompressionType__ZLIB_LO:
                        compressType = "ZLIB_LO";
                        break;
                    case ECompressionType.ECompressionType__ZLIB_MED:
                        compressType = "ZLIB_MED";
                        break;
                    case ECompressionType.ECompressionType__ZLIB_HI:
                        compressType = "ZLIB_HI";
                        break;
                    case ECompressionType.ECompressionType__UNDEFINED:
                        compressType = "Undefined";
                        break;
                }

                return compressType;
            }
        }        
    }
}