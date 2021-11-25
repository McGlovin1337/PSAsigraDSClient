using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommunications.Read, "DSClientWinFsSource")]
    [OutputType(typeof(SourceItemInfo))]

    sealed public class ReadDSClientWinFsSource: BaseDSClientBackupSource
    {
        protected override void DSClientProcessRecord()
        {
            // Initialize a Data Source Browser
            DataSourceBrowser dataSourceBrowser = DSClientSession.createBrowser(EBackupDataType.EBackupDataType__FileSystem);

            // Try to resolve the supplied Computer
            string computer = ResolveWinComputer(Computer);
            computer = dataSourceBrowser.expandToFullPath(computer);
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

            // Any trailing "\" is unnecessary, remove if any are specified to tidy up output
            path = path.TrimEnd('\\');

            // Get the items from the specified path
            browse_item_info[] browseItems = dataSourceBrowser.getSubItems(computer, path);

            // If this is a Top Level path, get the Special Items i.e. System State, Service Database etc
            browse_item_info[] specialItems = null;
            if (path == "")
                specialItems = dataSourceBrowser.getSpecialSubItems(computer, path);

            List<SourceItemInfo> sourceItems = new List<SourceItemInfo>();

            if (specialItems != null)
                foreach (browse_item_info item in specialItems)
                    sourceItems.Add(new SourceItemInfo(path, item));

            foreach (browse_item_info item in browseItems)
                    sourceItems.Add(new SourceItemInfo(path, item));

            if (Recursive)
            {
                List<ItemPath> newPaths = new List<ItemPath>();

                if (!string.IsNullOrEmpty(path) && path.Last() != '\\')
                    path += "\\";

                foreach (browse_item_info item in browseItems)
                    if (!item.isfile)
                        newPaths.Add(new ItemPath(path + item.name, 0));

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
                                newPaths.Insert(index, new ItemPath(currentPath.Path + "\\" + item.name, subItemDepth));
                                index++;
                            }
                        }
                    }
                    catch(APIException e)
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