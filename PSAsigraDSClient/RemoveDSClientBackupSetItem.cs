using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientBackupSetItem", SupportsShouldProcess = true)]

    public class RemoveDSClientBackupSetItem: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set to remove Items from")]
        public int BackupSetId { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Item to remove")]
        [SupportsWildcards]
        [ValidateNotNullOrEmpty]
        public string Item { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Inclusion", HelpMessage = "Specify to only remove Inclusion Item")]
        public SwitchParameter Inclusion { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Exclusion", HelpMessage = "Specify to only remove Exclusion Item")]
        public SwitchParameter Exclusion { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "RegexExclusion", HelpMessage = "Specify to only remove Regex Exclusion Item")]
        public SwitchParameter RegexExclusion { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Get the specified Backup Set
            WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId {BackupSetId}");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            // Get the Items from the Backup Set
            BackupSetItem[] backupSetItems = backupSet.items();
            BackupSetItem[] selectedItems;
            if (Inclusion)
                selectedItems = backupSetItems
                    .Where(item => item.getType() == EBackupSetItemType.EBackupSetItemType__Inclusion)
                    .ToArray();
            else if (Exclusion)
                selectedItems = backupSetItems
                    .Where(item => item.getType() == EBackupSetItemType.EBackupSetItemType__Exclusion)
                    .ToArray();
            else if (RegexExclusion)
                selectedItems = backupSetItems
                    .Where(item => item.getType() == EBackupSetItemType.EBackupSetItemType__RegExExclusion)
                    .ToArray();
            else
                selectedItems = backupSetItems;

            // Select matching items
            WildcardOptions wcOptions = WildcardOptions.IgnoreCase |
                                        WildcardOptions.Compiled;

            WildcardPattern wcPattern = new WildcardPattern(Item, wcOptions);

            List<BackupSetItem> removalItems = new List<BackupSetItem>();

            WriteVerbose("Performing Action: Remove matching items");
            foreach(BackupSetItem item in selectedItems)
            {
                EBackupSetItemType itemType = item.getType();
                if (itemType == EBackupSetItemType.EBackupSetItemType__Exclusion || itemType == EBackupSetItemType.EBackupSetItemType__Inclusion)
                {
                    BackupSetFileItem backupSetFileItem = BackupSetFileItem.from(item);

                    if (wcPattern.IsMatch(backupSetFileItem.getFolder() + backupSetFileItem.getFilter()))
                        if (ShouldProcess($"{item.getFolder()}"))
                            removalItems.Add(item);
                }
                else if (itemType == EBackupSetItemType.EBackupSetItemType__RegExExclusion)
                {
                    BackupSetRegexExclusion backupSetRegexExclusion = BackupSetRegexExclusion.from(item);

                    if (wcPattern.IsMatch(backupSetRegexExclusion.getFolder() + backupSetRegexExclusion.getExpression()))
                        if (ShouldProcess($"{item.getFolder()} with Expression: {backupSetRegexExclusion.getExpression()}"))
                            removalItems.Add(item);
                }
            }

            // Remove items from original list
            backupSetItems = backupSetItems.ToList()
                .Except(removalItems)
                .ToArray();

            // Assign the updated list of items to the Backup Set
            if (ShouldProcess(
                $"Performing Operation Update Backup Set Items on target '{backupSet.getName()}'",
                $"Are you sure you want to update the Backup Set Items belonging to '{backupSet.getName()}'?",
                "Update Backup Set Items"
            ))
            {
                WriteVerbose("Performing Action: Update Backup Set Items");
                backupSet.setItems(backupSetItems);
            }

            backupSet.Dispose();
        }
    }
}