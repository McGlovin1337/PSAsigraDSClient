using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.BaseDSClientBackupSource;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientStoredItem")]
    [OutputType(typeof(DSClientBackupSetItemInfo))]

    sealed public class GetDSClientStoredItem : BaseDSClientBackupSetDataBrowser
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

        [Parameter(HelpMessage = "Specify Paths to Exclude during Recursion")]
        public string[] ExcludePath { get; set; }

        [Parameter(Position = 3, Mandatory = true, ParameterSetName = "hidefiles", HelpMessage = "Specify to Hide Files")]
        [Parameter(ParameterSetName = "BackupSetId")]
        public SwitchParameter HideFiles { get; set; }

        [Parameter(Position = 3, Mandatory = true, ParameterSetName = "hidedirs", HelpMessage = "Specify to Hide Directories")]
        [Parameter(ParameterSetName = "BackupSetId")]
        public SwitchParameter HideDirectories { get; set; }

        [Parameter(HelpMessage = "Return the Size of Directories/Folders")]
        public SwitchParameter CalculateDirectorySize { get; set; }

        protected override void ProcessBackupSetData(BackedUpDataView backedUpDataView)
        {
            // Apply File & Directory Visibility Filters
            ESelectableItemCategory itemCategory = ESelectableItemCategory.ESelectableItemCategory__FilesAndDirectories;

            if (!MyInvocation.BoundParameters.ContainsKey(nameof(DeleteId)))
            {
                BackedUpDataViewWithFilters filteredBackedUpDataView = BackedUpDataViewWithFilters.from(backedUpDataView);

                if (HideFiles)
                    itemCategory = ESelectableItemCategory.ESelectableItemCategory__DirectoriesOnly;
                else if (HideDirectories)
                    itemCategory = ESelectableItemCategory.ESelectableItemCategory__FilesOnly;
            }

            List<DSClientBackupSetItemInfo> allItems = new List<DSClientBackupSetItemInfo>();
            List<DSClientBackupSetItemInfo> ItemInfo = new List<DSClientBackupSetItemInfo>();

            // Any trailing "\" is unnecessary, remove if any are specified to tidy up output
            string path = Path.TrimEnd('\\');

            // We always return info of the root/first item the user has specified, irrespective of other parameters
            WriteVerbose($"Performing Action: Retrieve Item Info for Path: {path}");
            SelectableItem item = backedUpDataView.getItem(path);
            long itemId = item.id;

            selectable_size itemSize = new selectable_size
            {
                data_size = 0,
                file_count = 0
            };
            if ((!item.is_file && MyInvocation.BoundParameters.ContainsKey(nameof(CalculateDirectorySize))) ||
                item.is_file)
            {
                itemSize = backedUpDataView.getItemSize(itemId);
            }

            DSClientBackupSetItemInfo itemInfo = new DSClientBackupSetItemInfo(path, item, itemSize);
            allItems.Add(itemInfo);
            ItemInfo.Add(itemInfo);

            if (Recursive)
            {
                // Set the Wildcard Options
                WildcardOptions wcOptions = WildcardOptions.IgnoreCase |
                                WildcardOptions.Compiled;

                // Set the Filter Wildcard Pattern
                WildcardPattern wcPattern = null;
                if (Filter != null)
                    wcPattern = new WildcardPattern(Filter, wcOptions);

                // Set the Path Exclusion Wildcard Patterns
                List<WildcardPattern> exclusionPatterns = new List<WildcardPattern>();
                if (ExcludePath?.Count() > 0)
                    foreach (string exclusionPath in ExcludePath)
                        exclusionPatterns.Add(new WildcardPattern(exclusionPath, wcOptions));

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

                    WriteDebug($"Retrieve Info for Path: {currentPath.Path}");
                    item = backedUpDataView.getItem(currentPath.Path);
                    itemId = item.id;

                    // Fetch all the subitems of the current path
                    WriteDebug("Retrieve Sub Item Info");
                    SelectableItem[] subItems = backedUpDataView.getSubItemsByCategory(itemId, itemCategory);

                    int subItemDepth = currentPath.Depth + 1;
                    int index = 1;
                    foreach (SelectableItem subItem in subItems)
                    {
                        selectable_size subItemSize = new selectable_size
                        {
                            data_size = 0,
                            file_count = 0
                        };
                        if (MyInvocation.BoundParameters.ContainsKey(nameof(CalculateDirectorySize)) ||
                            subItem.is_file)
                        {
                            WriteDebug($"Get Size of Item: {subItem.name}");
                            subItemSize = backedUpDataView.getItemSize(subItem.id);
                        }
                        DSClientBackupSetItemInfo currentItemInfo = new DSClientBackupSetItemInfo(currentPath.Path, subItem, subItemSize);
                        allItems.Add(currentItemInfo);

                        if (Filter != null)
                        {
                            if (wcPattern.IsMatch(subItem.name))
                            {
                                WriteDebug("Item Matched Filter");
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
                        {
                            string fullPath = $"{currentPath.Path}\\{subItem.name}";
                            if (!exclusionPatterns.Any(match => match.IsMatch(fullPath)))
                            {
                                newPaths.Insert(index, new ItemPath(fullPath, subItemDepth));
                                index++;
                            }
                        }
                    }

                    // Remove the Path we've just completed enumerating from the list
                    WriteDebug("Remove Item from Enumeration List");
                    newPaths.Remove(currentPath);
                    enumeratedCount++;
                }
            }

            if (MyInvocation.BoundParameters.ContainsKey(nameof(RestoreId)))
            {
                DSClientRestoreSession restoreSession = DSClientSessionInfo.GetRestoreSession(RestoreId);
                restoreSession.AddBrowsedItems(allItems);
            }
            else if (MyInvocation.BoundParameters.ContainsKey(nameof(ValidationId)))
            {
                DSClientValidationSession validationSession = DSClientSessionInfo.GetValidationSession(ValidationId);
                validationSession.AddBrowsedItems(allItems);
            }
            else if (MyInvocation.BoundParameters.ContainsKey(nameof(DeleteId)))
            {
                DSClientDeleteSession deleteSession = DSClientSessionInfo.GetDeleteSession(DeleteId);
                deleteSession.AddBrowsedItems(allItems);
            }

            ItemInfo.ForEach(WriteObject);
        }
    }
}