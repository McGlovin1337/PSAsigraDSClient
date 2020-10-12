using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientStoredItem")]
    [OutputType(typeof(DSClientBackupSetItemInfo))]

    public class GetDSClientStoredItem : BaseDSClientBackupSetDataBrowser
    {
        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Full Path to the Item")]
        [ValidateNotNullOrEmpty]
        public string Path { get; set; }

        [Parameter(Position = 2, HelpMessage = "Filter returned items")]
        [SupportsWildcards]
        public string Filter { get; set; }

        [Parameter(HelpMessage = "Specify to return items recursively")]
        public SwitchParameter Recursive { get; set; }

        [Parameter(HelpMessage = "Specify the rescursion depth")]
        public int RecursiveDepth { get; set; } = 0;

        protected override void ProcessBackupSetData(BackedUpDataView DSClientBackedUpDataView)
        {
            List<DSClientBackupSetItemInfo> ItemInfo = new List<DSClientBackupSetItemInfo>();

            // We always return info of the root/first item the user has specified, irrespective of other parameters
            WriteVerbose("Retrieving Item Info...");
            SelectableItem item = DSClientBackedUpDataView.getItem(Path);
            long itemId = item.id;

            selectable_size itemSize = DSClientBackedUpDataView.getItemSize(itemId);

            DSClientBackupSetItemInfo itemInfo = new DSClientBackupSetItemInfo(Path, item, itemSize);
            ItemInfo.Add(itemInfo);

            if (Recursive)
            {
                // Set the Wildcard Options
                WildcardOptions wcOptions = WildcardOptions.IgnoreCase |
                                WildcardOptions.Compiled;
                WildcardPattern wcPattern = null;

                if (Filter != null)
                    wcPattern = new WildcardPattern(Filter, wcOptions);

                List<ItemPath> newPaths = new List<ItemPath>();

                newPaths.Add(new ItemPath(Path, 0));

                while (newPaths.Count() > 0)
                {
                    // Select the first item in the list
                    ItemPath currentPath = newPaths.ElementAt(0);

                    // Break out of the loop if the recursion depth exceeds the value specified
                    if (currentPath.Depth > RecursiveDepth)
                        break;

                    WriteVerbose("Enumerating Path: " + currentPath.Path + " (Depth: " + currentPath.Depth + ")");

                    item = DSClientBackedUpDataView.getItem(currentPath.Path);
                    itemId = item.id;

                    // Fetch all the subitems of the current path
                    SelectableItem[] subItems = DSClientBackedUpDataView.getSubItemsByCategory(itemId, ESelectableItemCategory.ESelectableItemCategory__FilesAndDirectories);

                    int subItemDepth = currentPath.Depth + 1;
                    foreach (SelectableItem subItem in subItems)
                    {
                        selectable_size subItemSize = DSClientBackedUpDataView.getItemSize(subItem.id);

                        if (Filter != null)
                        {
                            if (wcPattern.IsMatch(subItem.name))
                                ItemInfo.Add(new DSClientBackupSetItemInfo(currentPath.Path, subItem, subItemSize));
                        }
                        else
                        {
                            ItemInfo.Add(new DSClientBackupSetItemInfo(currentPath.Path, subItem, subItemSize));
                        }

                        if (!subItem.is_file)
                            newPaths.Add(new ItemPath(currentPath.Path + "\\" + subItem.name, subItemDepth));
                    }

                    // Remove the Path we've just completed enumerating from the list
                    newPaths.Remove(currentPath);
                }
            }

            ItemInfo.ForEach(WriteObject);
        }

        private class ItemPath
        {
            public string Path { get; set; }
            public int Depth { get; set; }

            public ItemPath(string path, int depth)
            {
                Path = path;
                Depth = depth;
            }
        }

        private class DSClientBackupSetItemInfo
        {
            public long ItemId { get; set; }
            public string Path { get; set; }
            public string Name { get; set; }
            public string DataType { get; set; }
            public long DataSize { get; set; }
            public int FileCount { get; set; }
            public bool IsFile { get; set; }
            public bool Selectable { get; set; }

            public DSClientBackupSetItemInfo(string path, SelectableItem item, selectable_size itemSize)
            {
                ItemId = item.id;
                Path = path;
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