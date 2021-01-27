using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.BaseDSClientBackupSource;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientStoredItem")]
    [OutputType(typeof(DSClientBackupSetItemInfo))]

    public class GetDSClientStoredItem : BaseDSClientBackupSetDataBrowser
    {
        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Full Path to the Item")]
        [ValidateNotNullOrEmpty]
        [Alias("Folder")]
        public string Path { get; set; }

        [Parameter(Position = 2, HelpMessage = "Filter returned items")]
        [SupportsWildcards]
        public string Filter { get; set; }

        [Parameter(HelpMessage = "Specify to return items recursively")]
        public SwitchParameter Recursive { get; set; }

        [Parameter(HelpMessage = "Specify the rescursion depth")]
        public int RecursiveDepth { get; set; } = 0;

        [Parameter(Position = 3, Mandatory = true, ParameterSetName = "hidefiles", HelpMessage = "Specify to Hide Files")]
        [Parameter(ParameterSetName = "BackupSetId")]
        public SwitchParameter HideFiles { get; set; }

        [Parameter(Position = 3, Mandatory = true, ParameterSetName = "hidedirs", HelpMessage = "Specify to Hide Directories")]
        [Parameter(ParameterSetName = "BackupSetId")]
        public SwitchParameter HideDirectories { get; set; }

        protected override void ProcessBackupSetData(BackedUpDataView DSClientBackedUpDataView)
        {
            BackedUpDataViewWithFilters backedUpDataView = BackedUpDataViewWithFilters.from(DSClientBackedUpDataView);

            // Apply File & Directory Visibility Filters
            ESelectableItemCategory itemCategory = ESelectableItemCategory.ESelectableItemCategory__FilesAndDirectories;

            if (HideFiles)
                itemCategory = ESelectableItemCategory.ESelectableItemCategory__DirectoriesOnly;
            else if (HideDirectories)
                itemCategory = ESelectableItemCategory.ESelectableItemCategory__FilesOnly;

            List<DSClientBackupSetItemInfo> ItemInfo = new List<DSClientBackupSetItemInfo>();

            // Any trailing "\" is unnecessary, remove if any are specified to tidy up output
            string path = Path.TrimEnd('\\');

            // We always return info of the root/first item the user has specified, irrespective of other parameters
            WriteVerbose($"Performing Action: Retrieve Item Info for Path: {path}");
            SelectableItem item = backedUpDataView.getItem(path);
            long itemId = item.id;

            selectable_size itemSize = backedUpDataView.getItemSize(itemId);

            ItemInfo.Add(new DSClientBackupSetItemInfo(path, item, itemSize));

            if (Recursive)
            {
                // Set the Wildcard Options
                WildcardOptions wcOptions = WildcardOptions.IgnoreCase |
                                WildcardOptions.Compiled;
                WildcardPattern wcPattern = null;

                if (Filter != null)
                    wcPattern = new WildcardPattern(Filter, wcOptions);

                List<ItemPath> newPaths = new List<ItemPath>
                {
                    new ItemPath(path, 0)
                };

                int enumeratedCount = 0;
                int itemCount = 0;
                ProgressRecord progressRecord = new ProgressRecord(1, "Enumerate Stored Backup Set Items", $"{enumeratedCount} Paths Enumerated, {itemCount} Items Discovered")
                {
                    PercentComplete = -1,
                };

                while (newPaths.Count() > 0)
                {
                    // Select the first item in the list
                    ItemPath currentPath = newPaths.ElementAt(0);

                    WriteVerbose($"Performing Action: Enumerate Path: {currentPath.Path} (Depth: {currentPath.Depth})");

                    progressRecord.StatusDescription = $"{enumeratedCount} Paths Enumerated, {itemCount} Items Discovered";
                    progressRecord.CurrentOperation = $"Enumerating Path: {currentPath.Path}";
                    WriteProgress(progressRecord);

                    item = backedUpDataView.getItem(currentPath.Path);
                    itemId = item.id;

                    // Fetch all the subitems of the current path
                    SelectableItem[] subItems = backedUpDataView.getSubItemsByCategory(itemId, itemCategory);

                    int subItemDepth = currentPath.Depth + 1;
                    int index = 1;
                    foreach (SelectableItem subItem in subItems)
                    {
                        selectable_size subItemSize = backedUpDataView.getItemSize(subItem.id);

                        if (Filter != null)
                        {
                            if (wcPattern.IsMatch(subItem.name))
                            {
                                ItemInfo.Add(new DSClientBackupSetItemInfo(currentPath.Path, subItem, subItemSize));
                                itemCount++;
                            }
                        }
                        else
                        {
                            ItemInfo.Add(new DSClientBackupSetItemInfo(currentPath.Path, subItem, subItemSize));
                            itemCount++;
                        }

                        
                        if (!subItem.is_file && subItemDepth <= RecursiveDepth)
                            newPaths.Insert(index, new ItemPath(currentPath.Path + "\\" + subItem.name, subItemDepth));

                        index++;
                    }

                    // Remove the Path we've just completed enumerating from the list
                    newPaths.Remove(currentPath);
                    enumeratedCount++;
                }
            }

            ItemInfo.ForEach(WriteObject);
        }

        private class DSClientBackupSetItemInfo
        {
            public long ItemId { get; private set; }
            public string Path { get; private set; }
            public string Name { get; private set; }
            public string DataType { get; private set; }
            public long DataSize { get; private set; }
            public int FileCount { get; private set; }
            public bool IsFile { get; private set; }
            public bool Selectable { get; private set; }

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
        }
    }
}