using System;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientBackupSetItem")]
    [OutputType(typeof(DSClientBackupSetItemInfo))]

    public class GetDSClientBackupSetItem: BaseDSClientBackupSetDataBrowser
    {
        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Full Path to the Item")]
        public string Path { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to Include Path Sub-Items")]
        public SwitchParameter IncludeSubItems { get; set; }

        protected override void ProcessBackupSetData(BackedUpDataView DSClientBackedUpDataView)
        {
            SelectableItem item = null;
            long itemId = 0;
            List<DSClientBackupSetItemInfo> ItemInfo = new List<DSClientBackupSetItemInfo>();

            WriteVerbose("Retrieving Item Info...");
            if (Path != null)
            {
                item = DSClientBackedUpDataView.getItem(Path);
                itemId = item.id;

                selectable_size itemSize = DSClientBackedUpDataView.getItemSize(itemId);

                DSClientBackupSetItemInfo itemInfo = new DSClientBackupSetItemInfo(item, itemSize);
                ItemInfo.Add(itemInfo);
            }

            if (IncludeSubItems == true)
            {
                WriteVerbose("Retrieving Sub-Item Info...");
                SelectableItem[] subItems = DSClientBackedUpDataView.getSubItemsByCategory(itemId, ESelectableItemCategory.ESelectableItemCategory__FilesAndDirectories);

                foreach (var subItem in subItems)
                {
                    selectable_size itemSize = DSClientBackedUpDataView.getItemSize(subItem.id);

                    DSClientBackupSetItemInfo itemInfo = new DSClientBackupSetItemInfo(item, itemSize);
                    ItemInfo.Add(itemInfo);
                }
            }

            ItemInfo.ForEach(WriteObject);
        }

        private class DSClientBackupSetItemInfo
        {
            public long ItemId { get; set; }
            public string Name { get; set; }
            public string DataType { get; set; }
            public long DataSize { get; set; }
            public int FileCount { get; set; }
            public bool IsFile { get; set; }
            public bool Selectable { get; set; }

            public DSClientBackupSetItemInfo(SelectableItem item, selectable_size itemSize)
            {
                ItemId = item.id;
                Name = item.name;
                DataType = EBrowseItemTypeToString(item.data_type);
                DataSize = itemSize.data_size;
                FileCount = itemSize.file_count;
                IsFile = item.is_file;
                Selectable = item.is_selectable;
            }

            private string EBrowseItemTypeToString(EBrowseItemType itemType)
            {
                string ItemType = null;

                switch(itemType)
                {
                    case EBrowseItemType.EBrowseItemType__Drive:
                        ItemType = "Drive";
                        break;
                    case EBrowseItemType.EBrowseItemType__Share:
                        ItemType = "Share";
                        break;
                    case EBrowseItemType.EBrowseItemType__Directory:
                        ItemType = "Directory";
                        break;
                    case EBrowseItemType.EBrowseItemType__File:
                        ItemType = "File";
                        break;
                    case EBrowseItemType.EBrowseItemType__SystemState:
                        ItemType = "SystemState";
                        break;
                    case EBrowseItemType.EBrowseItemType__ServicesDB:
                        ItemType = "ServicesDatabase";
                        break;
                    case EBrowseItemType.EBrowseItemType__DatabaseInstance:
                        ItemType = "DatabaseInstance";
                        break;
                    case EBrowseItemType.EBrowseItemType__Database:
                        ItemType = "Database";
                        break;
                    case EBrowseItemType.EBrowseItemType__Tablespace:
                        ItemType = "OracleTablespace";
                        break;
                    case EBrowseItemType.EBrowseItemType__ControlFile:
                        ItemType = "OracleControlFile";
                        break;
                    case EBrowseItemType.EBrowseItemType__ArchiveLog:
                        ItemType = "OracleArchiveLog";
                        break;
                    case EBrowseItemType.EBrowseItemType__VirtualMachine:
                        ItemType = "VirtualMachine";
                        break;
                    case EBrowseItemType.EBrowseItemType__VssExchange:
                        ItemType = "VssExchange";
                        break;
                    case EBrowseItemType.EBrowseItemType__VmDisk:
                        ItemType = "VirtualMachineDisk";
                        break;
                }

                return ItemType;
            }
        }
    }
}