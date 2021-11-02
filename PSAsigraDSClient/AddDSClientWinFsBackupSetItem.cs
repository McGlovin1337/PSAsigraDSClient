using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientWinFsBackupSetItem")]
    [OutputType(typeof(void))]

    sealed public class AddDSClientWinFsBackupSetItem: BaseDSClientBackupSet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set to modify")]
        public int BackupSetId { get; set; }

        [Parameter(HelpMessage = "Include Alternate Data Streams for IncludedItems")]
        public SwitchParameter ExcludeAltDataStreams { get; set; }

        [Parameter(HelpMessage = "Include Permissions for IncludedItems")]
        public SwitchParameter ExcludePermissions { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Items to Include in Backup Set")]
        public string[] IncludeItem { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Max Number of Generations for Included Items")]
        [ValidateRange(1, 9999)]
        public int MaxGenerations { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Items to Exclude from Backup Set")]
        public string[] ExcludeItem { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Regex Item Exclusion Patterns")]
        [ValidateNotNullOrEmpty]
        public string[] RegexExcludePattern { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Path Regex Exclusion Pattern applies to")]
        [ValidateNotNullOrEmpty]
        public string RegexExclusionPath { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to also Match Directory Names with Regex pattern")]
        public SwitchParameter RegexMatchDirectory { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify if Regex Exclusions Items are case insensitive")]
        public SwitchParameter RegexCaseInsensitive { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to exclude Sub-Directories")]
        public SwitchParameter ExcludeSubDirs { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Check DS-Client is Windows
            if (DSClientSessionInfo.OperatingSystem != "Windows")
            {
                ErrorRecord errorRecord = new ErrorRecord(
                    new PlatformNotSupportedException("Windows FileSystem Backup Sets can only be created on a Windows DS-Client"),
                    "PlatformNotSupportedException",
                    ErrorCategory.InvalidOperation,
                    null);
                WriteError(errorRecord);
            }
            else
            {
                // Get the requested Backup Set from DS-Client
                WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId: {BackupSetId}");
                BackupSet backupSet = DSClientSession.backup_set(BackupSetId);
                string computer = backupSet.getComputerName();

                // Create a Data Source Browser
                DataSourceBrowser dataSourceBrowser = backupSet.dataBrowser();

                // Create a List of Items
                List<BackupSetItem> backupSetItems = new List<BackupSetItem>();

                // Process any Exclusion Items
                if (ExcludeItem != null)
                    backupSetItems.AddRange(ProcessExclusionItems(DSClientSessionInfo.OperatingSystem, dataSourceBrowser, computer, ExcludeItem, ExcludeSubDirs));

                if (RegexExcludePattern != null)
                    backupSetItems.AddRange(ProcessRegexExclusionItems(dataSourceBrowser, computer, RegexExclusionPath, RegexMatchDirectory, RegexCaseInsensitive, RegexExcludePattern));

                // Process any Inclusion Items
                if (IncludeItem != null)
                    backupSetItems.AddRange(ProcessWin32FSInclusionItems(dataSourceBrowser, computer, IncludeItem, MaxGenerations, ExcludeAltDataStreams, ExcludePermissions, ExcludeSubDirs));

                // Get the existing specified items and store in the list
                backupSetItems.AddRange(backupSet.items());

                // Strip any duplicates from the list, duplicates cause an error and wipes all the items from the Backup Set
                backupSetItems = backupSetItems.Distinct(new BackupSetItemComparer()).ToList();

                // Add all the items to the Backup Set
                WriteVerbose("Performing Action: Add Items to Backup Set");
                backupSet.setItems(backupSetItems.ToArray());

                dataSourceBrowser.Dispose();
                backupSet.Dispose();
            }
        }
    }
}