using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommunications.Read, "DSClientUnixFsSource")]
    [OutputType(typeof(SourceItemInfo))]

    sealed public class ReadDSClientUnixFsSource: BaseDSClientBackupSource
    {
        protected override void DSClientProcessRecord()
        {
            // Initialize a Data Source Browser
            DataSourceBrowser dataSourceBrowser = DSClientSession.createBrowser(EBackupDataType.EBackupDataType__FileSystem);

            // Try to resolve the supplied Computer
            string computer = dataSourceBrowser.expandToFullPath(Computer);
            WriteVerbose($"Notice: Specified Computer resolved to: {computer}");

            // Set the Credentials
            if (Credential != null)
            {
                dataSourceBrowser.setCurrentCredentials(Credential.GetCredentials());
            }
            else
            {
                Win32FS_Generic_BackupSetCredentials backupSetCredentials = Win32FS_Generic_BackupSetCredentials.from(dataSourceBrowser.neededCredentials(computer));
                backupSetCredentials.setUsingClientCredentials(true);
                dataSourceBrowser.setCurrentCredentials(backupSetCredentials);
            }

            // Set the Starting path
            string path = Path ?? "";

            // Any trailing "\" or "/" is unnecessary, remove if any are specified to tidy up output
            if (path != "")
                while ((path.Last() == '/' || path.Last() == '\\') && path.Length > 1)
                    path = (path.Last() == '/') ? path.TrimEnd('/') : path.TrimEnd('\\');
            WriteDebug($"Path: {path}");

            // Get the items from the specified path
            List<SourceItemInfo> sourceItems = new List<SourceItemInfo>();
            browse_item_info[] browseItems = dataSourceBrowser.getSubItems(computer, path);

            foreach (browse_item_info item in browseItems)
                sourceItems.Add(new SourceItemInfo(path, item));

            if (Recursive)
            {
                List<ItemPath> newPaths = new List<ItemPath>();

                if (!string.IsNullOrEmpty(path) && path.Last() != '/')
                    path += "/";

                if (browseItems != null)
                {
                    foreach (browse_item_info item in browseItems)
                        if (!item.isfile)
                            newPaths.Add(new ItemPath(path + item.name, 0));
                }

                int enumeratedCount = 0;
                int itemCount = 0;
                ProgressRecord progressRecord = new ProgressRecord(1, $"Enumerate Items on: {computer}", $"{enumeratedCount} Paths Enumerated, {itemCount} Items Discovered")
                {
                    PercentComplete = -1,
                };

                while (newPaths.Count() > 0)
                {
                    WriteVerbose($"Notice: Items to enumerate: {newPaths.Count()}");
                    // Select the first item in the list
                    ItemPath currentPath = newPaths.ElementAt(0);

                    WriteVerbose($"Performing Action: Enumerate Path: {currentPath.Path} (Depth: {currentPath.Depth})");

                    progressRecord.StatusDescription = $"{enumeratedCount} Paths Enumerated, {itemCount} Items Discovered";
                    progressRecord.CurrentOperation = $"Enumerating Path: {currentPath.Path}";
                    WriteProgress(progressRecord);

                    // Retrieve all the sub-items for the current selected path
                    try
                    {
                        browse_item_info[] subItems = dataSourceBrowser.getSubItems(computer, currentPath.Path);

                        int subItemDepth = currentPath.Depth + 1;
                        int index = 1;
                        foreach (browse_item_info item in subItems)
                        {
                            sourceItems.Add(new SourceItemInfo(currentPath.Path, item));
                            itemCount++;

                            if (!item.isfile && subItemDepth <= RecursiveDepth)
                            {
                                char delim = (item.type == EBrowseItemType.EBrowseItemType__Directory) ? '\\' : '/';
                                string itemPath = (item.name.First() != delim && currentPath.Path.Last() != delim) ? $"{currentPath.Path}{delim}{item.name}" : $"{currentPath.Path}{item.name}";
                                newPaths.Insert(index, new ItemPath(itemPath, subItemDepth));
                                index++;
                            }
                        }
                    }
                    catch (APIException e)
                    {
                        WriteWarning("Failed to Enumerate Path: " + currentPath.Path + " Exception: " + e);
                    }

                    // Remove the Path we've just completed enumerating from the list
                    newPaths.Remove(currentPath);
                    enumeratedCount++;
                }
            }

            sourceItems.ForEach(WriteObject);

            dataSourceBrowser.Dispose();
        }
    }
}