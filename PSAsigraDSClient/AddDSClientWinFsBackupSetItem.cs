using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientWinFsBackupSetItem")]

    public class AddDSClientWinFsBackupSetItem: BaseDSClientBackupSetItemParams
    {
        [Parameter(ParameterSetName = "inclusion", HelpMessage = "Exclude Alternate Data Streams for Included Items")]
        public SwitchParameter ExcludeAltDataStreams { get; set; }

        [Parameter(ParameterSetName = "inclusion", HelpMessage = "Exclude Permissions for Included Items")]
        public SwitchParameter ExcludePermissions { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Check DS-Client is Windows
            if (DSClientOSType.OsType != "Windows")
                throw new Exception("Windows FileSystem Backup Sets can only be created on a Windows DS-Client");

            // Get the requested Backup Set from DS-Client
            WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId: {BackupSetId}");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);
            string computer = backupSet.getComputerName();

            // Create a Data Source Browser
            DataSourceBrowser dataSourceBrowser = backupSet.dataBrowser();

            // Create a List of Items
            List<BackupSetItem> backupSetItems = new List<BackupSetItem>();

            // Process any Exclusion Items
            if (Exclusion)
                backupSetItems.Add(CreateExclusionItem(dataSourceBrowser, computer, Path, Filter, ExcludeSubDirs));
            else if (RegexExclusion)
                backupSetItems.Add(CreateRegexExclusion(dataSourceBrowser, computer, Path, Filter, RegexMatchDirectory, RegexCaseInsensitive));
            else if (Inclusion)
                backupSetItems.Add(CreateWin32FSInclusionItem(dataSourceBrowser, computer, Path, Filter, MaxGenerations, ExcludeAltDataStreams, ExcludePermissions, ExcludeSubDirs));

            // Get the existing specified items and store in the list
            backupSetItems.AddRange(backupSet.items());

            // Strip any duplicates from the list, duplicates cause an error and wipes all the items from the Backup Set
            backupSetItems = backupSetItems.Distinct(new BackupSetItemComparer()).ToList();

            // Add all the items to the Backup Set
            WriteVerbose("Performing Action: Add Items to Backup Set");
            backupSet.setItems(backupSetItems.ToArray());

            foreach (BackupSetItem item in backupSetItems)
                item.Dispose();

            dataSourceBrowser.Dispose();
            backupSet.Dispose();
        }
    }
}