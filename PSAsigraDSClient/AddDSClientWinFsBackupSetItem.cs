using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientWinFsBackupSetItem")]

    public class AddDSClientWinFsBackupSetItem: BaseDSClientBackupSetItemParams
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set to modify")]
        public int BackupSetId { get; set; }

        [Parameter(HelpMessage = "Include Alternate Data Streams for IncludedItems")]
        public SwitchParameter ExcludeAltDataStreams { get; set; }

        [Parameter(HelpMessage = "Include Permissions for IncludedItems")]
        public SwitchParameter ExcludePermissions { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Get the requested Backup Set from DS-Client
            WriteVerbose("Retrieving Backup Set from DS-Client...");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);
            string computer = backupSet.getComputerName();

            // Create a Data Source Browser
            DataSourceBrowser dataSourceBrowser = backupSet.dataBrowser();

            // Create a List of Items
            List<BackupSetItem> backupSetItems = new List<BackupSetItem>();

            // Process any Exclusion Items
            if (ExcludeItem != null)
                backupSetItems.AddRange(ProcessExclusionItems(dataSourceBrowser, computer, ExcludeItem));

            if (RegexExcludeItem != null)
                backupSetItems.AddRange(ProcessRegexExclusionItems(dataSourceBrowser, computer, RegexExclusionPath, RegexExcludeDirectory, RegexCaseInsensitive, RegexExcludeItem));

            // Process any Inclusion Items
            if (IncludeItem != null)
                backupSetItems.AddRange(ProcessWin32FSInclusionItems(dataSourceBrowser, computer, IncludeItem, MaxGenerations, ExcludeAltDataStreams, ExcludePermissions));

            // Add all the items to the Backup Set
            WriteVerbose("Adding Items to Backup Set...");
            backupSet.setItems(backupSetItems.ToArray());
        }
    }
}