using System;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientFileSystemRestore: BaseDSClientBackupSetRestore
    {
        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Select Items by ItemId")]
        public long[] ItemId { get; set; }

        [Parameter(HelpMessage = "Destination to restore items to")]
        public string DestinationPath { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "File Overwrite Option")]
        [ValidateSet("RestoreAll", "RestoreNewer", "RestoreOlder", "RestoreDifferent", "SkipExisting")]
        public string OverwriteOption { get; set; }

        [Parameter(HelpMessage = "File Restore Method")]
        [ValidateSet("Save", "Fast", "UseBuffer")]
        public string RestoreMethod { get; set; } = "Save";

        [Parameter(HelpMessage = "Specify Restoration of Permissions")]
        [ValidateSet("Yes", "Skip", "Only")]
        public string RestorePermissions { get; set; } = "Yes";

        protected abstract void ProcessFileSystemRestore(string computer, IEnumerable<share_mapping> shareMappings, DataSourceBrowser dataSourceBrowser, FS_RestoreActivityInitiator restoreActivityInitiator);

        protected override void DSClientProcessRecord()
        {
            // Check for a Backup Set Restore View stored in SessionState
            WriteVerbose("Performing Action: Check for DS-Client Restore View Session");
            BackupSetRestoreView restoreSession = SessionState.PSVariable.GetValue("RestoreView", null) as BackupSetRestoreView;

            if (restoreSession == null)
                throw new Exception("There is no Backup Set Restore View Session, use Initialize-DSClientBackupSetRestore Cmdlet to create a Restore Session");

            // Check for Data Type for the Restore stored in SessionState
            EBackupDataType dataType = (EBackupDataType)SessionState.PSVariable.GetValue("RestoreType", EBackupDataType.EBackupDataType__UNDEFINED);

            if (dataType != EBackupDataType.EBackupDataType__FileSystem)
                throw new Exception("Incorrect Data Type for Restore");

            // Get the selected items
            List<long> selectedItems = new List<long>();

            if (Items != null)
            {
                foreach (string item in Items)
                {
                    try
                    {
                        selectedItems.Add(restoreSession.getItem(item).id);
                    }
                    catch
                    {
                        WriteWarning("Unable to select item: " + item);
                        continue;
                    }
                }
            }

            if (ItemId != null)
                selectedItems.AddRange(ItemId);

            RestoreActivityInitiator restoreActivityInitiator = restoreSession.prepareRestore(selectedItems.ToArray());

            DataSourceBrowser dataSourceBrowser = DSClientSession.createBrowser(dataType);

            // Resolve the supplied Computer Name
            string computer = dataSourceBrowser.expandToFullPath(Computer);
            computer = dataSourceBrowser.expandToFullPath(computer);
            WriteVerbose("Notice: Specified Computer resolved to: " + computer);

            // Set the Destination Computer Credentials
            BackupSetCredentials backupSetCredentials = dataSourceBrowser.neededCredentials(computer);

            if (DSClientOSType.OsType == "Windows")
            {
                Win32FS_Generic_BackupSetCredentials win32FSBSCredentials = Win32FS_Generic_BackupSetCredentials.from(dataSourceBrowser.neededCredentials(computer));

                if (Credential != null)
                {
                    string user = Credential.UserName;
                    string pass = Credential.GetNetworkCredential().Password;
                    win32FSBSCredentials.setCredentials(user, pass);
                }
                else
                {
                    WriteVerbose("Notice: Credentials not specified, using DS-Client Credentials");
                    win32FSBSCredentials.setUsingClientCredentials(true);
                }
                dataSourceBrowser.setCurrentCredentials(win32FSBSCredentials);
                win32FSBSCredentials.Dispose();
            }
            else if (DSClientOSType.OsType == "Linux")
            {
                UnixFS_Generic_BackupSetCredentials unixFSBackupSetCredentials = UnixFS_Generic_BackupSetCredentials.from(backupSetCredentials);

                if (Credential != null)
                {
                    string user = Credential.UserName;
                    string pass = Credential.GetNetworkCredential().Password;
                    unixFSBackupSetCredentials.setCredentials(user, pass);
                }
                else
                {
                    WriteVerbose("Notice: Credentials not specified, using DS-Client Credentials");
                    unixFSBackupSetCredentials.setUsingClientCredentials(true);
                }
                dataSourceBrowser.setCurrentCredentials(unixFSBackupSetCredentials);
                unixFSBackupSetCredentials.Dispose();
            }

            // Get the selected shares
            selected_shares[] selectedShares = restoreActivityInitiator.selected_shares();

            // Create a share mapping for the selected shares if DestinationPath has been specified
            List<share_mapping> shareMappings = new List<share_mapping>();

            if (DestinationPath != null)
            {
                foreach (selected_shares share in selectedShares)
                {
                    share_mapping shareMap = new share_mapping
                    {
                        destination_path = DestinationPath,
                        source_share = share.share_name,
                        truncate_level = TruncateSource
                    };

                    shareMappings.Add(shareMap);
                }
            }

            // Set Restore Configuration
            restoreActivityInitiator.setRestoreReason(StringToERestoreReason(RestoreReason));
            restoreActivityInitiator.setRestoreClassification(StringToERestoreClassification(RestoreClassification));
            restoreActivityInitiator.setDetailLogReporting(UseDetailedLog);
            restoreActivityInitiator.setLocalStorageHandling(StringToERestoreLocalStorageHandling(LocalStorageMethod));

            FS_RestoreActivityInitiator fsRestoreActivityInitiator = FS_RestoreActivityInitiator.from(restoreActivityInitiator);

            fsRestoreActivityInitiator.setFileOverwriteOption(StringToEFileOverwriteOption(OverwriteOption));
            fsRestoreActivityInitiator.setRestoreMethod(StringToERestoreMethod(RestoreMethod));
            fsRestoreActivityInitiator.setRestorePermission(StringToERestorePermission(RestorePermissions));

            // Process Cmdlet specifics
            ProcessFileSystemRestore(computer, shareMappings, dataSourceBrowser, fsRestoreActivityInitiator);
            
            dataSourceBrowser.Dispose();
            restoreSession.Dispose();
            SessionState.PSVariable.Remove("RestoreView");
        }

        private static EFileOverwriteOption StringToEFileOverwriteOption(string overwriteOption)
        {
            EFileOverwriteOption OverwriteOption;

            switch (overwriteOption.ToLower())
            {
                case "restoreall":
                    OverwriteOption = EFileOverwriteOption.EFileOverwriteOption__RestoreAll;
                    break;
                case "restorenewer":
                    OverwriteOption = EFileOverwriteOption.EFileOverwriteOption__RestoreNewer;
                    break;
                case "restoreolder":
                    OverwriteOption = EFileOverwriteOption.EFileOverwriteOption__RestoreOlder;
                    break;
                case "restoredifferent":
                    OverwriteOption = EFileOverwriteOption.EFileOverwriteOption__RestoreDifferent;
                    break;
                case "skipexisting":
                    OverwriteOption = EFileOverwriteOption.EFileOverwriteOption__SkipExisting;
                    break;
                default:
                    OverwriteOption = EFileOverwriteOption.EFileOverwriteOption__UNDEFINED;
                    break;
            }

            return OverwriteOption;
        }

        private static ERestoreMethod StringToERestoreMethod(string restoreMethod)
        {
            ERestoreMethod RestoreMethod;

            switch (restoreMethod.ToLower())
            {
                case "save":
                    RestoreMethod = ERestoreMethod.ERestoreMethod__Save;
                    break;
                case "fast":
                    RestoreMethod = ERestoreMethod.ERestoreMethod__Fast;
                    break;
                case "usebuffer":
                    RestoreMethod = ERestoreMethod.ERestoreMethod__UseBuffer;
                    break;
                default:
                    RestoreMethod = ERestoreMethod.ERestoreMethod__UNDEFINED;
                    break;
            }

            return RestoreMethod;
        }

        private static ERestorePermission StringToERestorePermission(string restorePermission)
        {
            ERestorePermission RestorePermission;

            switch (restorePermission.ToLower())
            {
                case "yes":
                    RestorePermission = ERestorePermission.ERestorePermission__Yes;
                    break;
                case "skip":
                    RestorePermission = ERestorePermission.ERestorePermission__Skip;
                    break;
                case "only":
                    RestorePermission = ERestorePermission.ERestorePermission__Only;
                    break;
                default:
                    RestorePermission = ERestorePermission.ERestorePermission__UNDEFINED;
                    break;
            }

            return RestorePermission;
        }
    }
}