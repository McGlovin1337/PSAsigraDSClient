using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientUnixFsBackupSetItem")]

    public class AddDSClientUnixFsBackupSetItem: BaseDSClientBackupSet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set to modify")]
        public int BackupSetId { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Items to Include in Backup Set")]
        public string[] IncludeItem { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Max Number of Generations for Included Items")]
        public int MaxGenerations { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Items to Exclude from Backup Set")]
        public string[] ExcludeItem { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Regex Item Exclusion Patterns")]
        [ValidateNotNullOrEmpty]
        public string[] RegexExcludeItem { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Path for Regex Exclusion Item")]
        [ValidateNotNullOrEmpty]
        public string RegexExclusionPath { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to also Exclude Directories with Regex pattern")]
        public SwitchParameter RegexExcludeDirectory { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify if Regex Exclusions Items are case insensitive")]
        public SwitchParameter RegexCaseInsensitive { get; set; }

        [Parameter(HelpMessage = "Exclude ACLs")]
        public SwitchParameter ExcludeACLs { get; set; }

        [Parameter(HelpMessage = "Exclude POSIX ACLs")]
        public SwitchParameter ExcludePosixACLs { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Check DS-Client is Linux
            if (DSClientOSType.OsType != "Linux")
                throw new Exception("Unix FileSystem Backup Sets can only be created on a Unix DS-Client");

            // Get the requested Backup Set from DS-Client
            WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId: {BackupSetId}");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);
            string computer = backupSet.getComputerName();

            // Create a Data Source Browser
            DataSourceBrowser dataSourceBrowser = backupSet.dataBrowser();

            // Create a List of Items
            List<BackupSetItem> backupSetItems = new List<BackupSetItem>();

            if (ExcludeItem != null)
                backupSetItems.AddRange(ProcessExclusionItems(DSClientOSType, dataSourceBrowser, computer, ExcludeItem));

            if (RegexExcludeItem != null)
                backupSetItems.AddRange(ProcessRegexExclusionItems(dataSourceBrowser, computer, RegexExclusionPath, RegexExcludeDirectory, RegexCaseInsensitive, RegexExcludeItem));

            if (IncludeItem != null)
            {
                foreach (string item in IncludeItem)
                {
                    UnixFS_BackupSetInclusionItem inclusionItem = UnixFS_BackupSetInclusionItem.from(dataSourceBrowser.createInclusionItem(computer, item, MaxGenerations));

                    if (MyInvocation.BoundParameters.ContainsKey("ExcludeACLs"))
                        inclusionItem.setIncludingACL(false);
                    else
                        inclusionItem.setIncludingACL(true);

                    if (computer.Split('\\').First() == "Local File System")
                    {
                        if (MyInvocation.BoundParameters.ContainsKey("ExcludePosixACLs"))
                        {
                            UnixFS_LinuxLFS_BackupSetInclusionItem linuxInclusionItem = UnixFS_LinuxLFS_BackupSetInclusionItem.from(inclusionItem);
                            linuxInclusionItem.setIncludingPosixACL(false);
                        }
                        else
                        {
                            UnixFS_LinuxLFS_BackupSetInclusionItem linuxInclusionItem = UnixFS_LinuxLFS_BackupSetInclusionItem.from(inclusionItem);
                            linuxInclusionItem.setIncludingPosixACL(true);
                        }
                    }

                    backupSetItems.Add(inclusionItem);
                }
            }

            backupSet.setItems(backupSetItems.ToArray());

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