using System;
using System.Collections.Generic;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientBackupSet: DSClientCmdlet
    {
        protected abstract void ProcessBackupSet(IEnumerable<DSClientBackupSetInfo> dSClientBackupSetsInfo);

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Retrieving DSClient Backup Sets...");
            backup_set_info[] backupSetsInfo = DSClientSession.backup_sets_info();

            List<DSClientBackupSetInfo> dSClientBackupSets = new List<DSClientBackupSetInfo>();

            foreach (backup_set_info backupSet in backupSetsInfo)
            {
                DSClientBackupSetInfo backupSetInfo = new DSClientBackupSetInfo(backupSet);
                dSClientBackupSets.Add(backupSetInfo);
            }

            ProcessBackupSet(dSClientBackupSets);
        }

        public class DSClientBackupSetInfo
        {
            public int Id { get; set; }
            public string Computer { get; set; }
            public string Name { get; set; }
            public bool Active { get; set; }
            public DateTime LastBackup { get; set; }
            public DateTime LastSuccess { get; set; }
            public string DataType { get; set; }
            public bool Synchronized { get; set; }
            public DateTime LastSynchronized { get; set; }
            public int ScheduleId { get; set; }
            public string ScheduleName { get; set; }
            public int RetentionId { get; set; }
            public string RetentionRuleName { get; set; }
            public long OnlineDataSize { get; set; }
            public int OnlineFileCount { get; set; }
            public long CompressedSize { get; set; }
            public long LocalStorageDataSize { get; set; }
            public int LocalStorageFileCount { get; set; }
            public string SetType { get; set; }
            public bool UseLocalStorage { get; set; }
            public bool HasItems { get; set; }
            public bool IsCDP { get; set; }
            public bool CreatedByPolicy { get; set; }
            public int OwnerId { get; set; }
            public string OwnerName { get; set; }

            public DSClientBackupSetInfo(backup_set_info backupSetInfo)
            {
                Id = backupSetInfo.id;
                Computer = backupSetInfo.computer;
                Name = backupSetInfo.name;
                Active = backupSetInfo.is_active;
                LastBackup = UnixEpochToDateTime(backupSetInfo.last_backup);
                LastSuccess = UnixEpochToDateTime(backupSetInfo.last_successful_backup);
                DataType = EBackupDataTypeToString(backupSetInfo.data_type);
                Synchronized = backupSetInfo.in_sync;
                LastSynchronized = UnixEpochToDateTime(backupSetInfo.last_sync_time);
                ScheduleId = backupSetInfo.schedule_id;
                ScheduleName = backupSetInfo.schedule_name;
                RetentionId = backupSetInfo.retention_rule_id;
                RetentionRuleName = backupSetInfo.retention_rule_name;
                OnlineDataSize = backupSetInfo.online_data_size;
                OnlineFileCount = backupSetInfo.online_file_count;
                CompressedSize = backupSetInfo.dssys_compressed_size;
                LocalStorageDataSize = backupSetInfo.local_storage_data_size;
                LocalStorageFileCount = backupSetInfo.local_storage_file_count;
                SetType = EBackupSetTypeToString(backupSetInfo.set_type);
                UseLocalStorage = backupSetInfo.using_local_storage;
                HasItems = backupSetInfo.has_items;
                IsCDP = backupSetInfo.is_cdp;
                CreatedByPolicy = backupSetInfo.is_created_by_policy;
                OwnerId = backupSetInfo.owner_id;
                OwnerName = backupSetInfo.owner_name;
            }
        }

        protected static string EBackupDataTypeToString(EBackupDataType dataType)
        {
            string DataType = null;

            switch(dataType)
            {
                case EBackupDataType.EBackupDataType__FileSystem:
                    DataType = "FileSystem";
                    break;
                case EBackupDataType.EBackupDataType__SQLServer:
                    DataType = "MSSQLServer";
                    break;
                case EBackupDataType.EBackupDataType__ExchangeServer:
                    DataType = "MSExchange";
                    break;
                case EBackupDataType.EBackupDataType__Oracle:
                    DataType = "Oracle";
                    break;
                case EBackupDataType.EBackupDataType__Permissions:
                    DataType = "PermissionsOnly";
                    break;
                case EBackupDataType.EBackupDataType__ExchangeEmail:
                    DataType = "MSExchangeItemLevel";
                    break;
                case EBackupDataType.EBackupDataType__OutlookEmail:
                    DataType = "OutlookEmail";
                    break;
                case EBackupDataType.EBackupDataType__SystemI:
                    DataType = "SystemI";
                    break;
                case EBackupDataType.EBackupDataType__MySQL:
                    DataType = "MySQL";
                    break;
                case EBackupDataType.EBackupDataType__PostgreSQL:
                    DataType = "PostgreSQL";
                    break;
                case EBackupDataType.EBackupDataType__DB2:
                    DataType = "DB2";
                    break;
                case EBackupDataType.EBackupDataType__LotusNotesEmail:
                    DataType = "LotusNotesEmail";
                    break;
                case EBackupDataType.EBackupDataType__GroupwiseEmail:
                    DataType = "GroupWiseEmail";
                    break;
                case EBackupDataType.EBackupDataType__Sharepoint:
                    DataType = "SharepointItemLevel";
                    break;
                case EBackupDataType.EBackupDataType__VMWare:
                    DataType = "VMWareVMDK";
                    break;
                case EBackupDataType.EBackupDataType__XenServer:
                    DataType = "XenServer";
                    break;
                case EBackupDataType.EBackupDataType__Sybase:
                    DataType = "Sybase";
                    break;
                case EBackupDataType.EBackupDataType__HyperV:
                    DataType = "HyperVServer";
                    break;
                case EBackupDataType.EBackupDataType__VMwareVADP:
                    DataType = "VMWareVADP";
                    break;
                case EBackupDataType.EBackupDataType__VSSSQLServer:
                    DataType = "MSSQLServerVSS";
                    break;
                case EBackupDataType.EBackupDataType__VSSExchange:
                    DataType = "MSExchangeVSS";
                    break;
                case EBackupDataType.EBackupDataType__VSSSharePoint:
                    DataType = "SharepointVSS";
                    break;
                case EBackupDataType.EBackupDataType__SalesForce:
                    DataType = "SalesForce";
                    break;
                case EBackupDataType.EBackupDataType__GoogleApps:
                    DataType = "GoogleApps";
                    break;
                case EBackupDataType.EBackupDataType__Office365:
                    DataType = "Office365";
                    break;
                case EBackupDataType.EBackupDataType__OracleSBT:
                    DataType = "OracleSBT";
                    break;
                case EBackupDataType.EBackupDataType__LotusDomino:
                    DataType = "LotusDomino";
                    break;
                case EBackupDataType.EBackupDataType__ClusteredHyperV:
                    DataType = "HyperVCluster";
                    break;
                case EBackupDataType.EBackupDataType__PToV:
                    DataType = "P2V";
                    break;
                case EBackupDataType.EBackupDataType__ExchangeEMailEWS:
                    DataType = "MSExchangeEWS";
                    break;
            }

            return DataType;
        }

        protected static string EBackupSetTypeToString(EBackupSetType setType)
        {
            string SetType = null;

            switch(setType)
            {
                case EBackupSetType.EBackupSetType__OffSite:
                    SetType = "OffSite";
                    break;
                case EBackupSetType.EBackupSetType__Statistical:
                    SetType = "Statistical";
                    break;
                case EBackupSetType.EBackupSetType__SelfContained:
                    SetType = "SelfContained";
                    break;
                case EBackupSetType.EBackupSetType__LocalOnly:
                    SetType = "LocalOnly";
                    break;
            }

            return SetType;
        }
    }
}