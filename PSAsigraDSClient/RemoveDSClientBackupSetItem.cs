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

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "wcFolder", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Folder to remove")]
        [SupportsWildcards]
        [ValidateNotNullOrEmpty]
        public string Folder { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "literalFolder", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Literal Folder to remove")]
        [ValidateNotNullOrEmpty]
        public string LiteralFolder { get; set; }

        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Filter of the Item")]
        public string Filter { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Specify to only remove Inclusion Item")]
        [ValidateSet("Inclusion", "Exclusion", "RegexExclusion")]
        public string Type { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Get the specified Backup Set
            WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId {BackupSetId}");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            // Get the Items from the Backup Set
            BackupSetItem[] backupSetItems = backupSet.items();
            BackupSetItem[] selectedItems;
            switch (Type)
            {
                case "Inclusion":
                    selectedItems = backupSetItems
                        .Where(item => item.getType() == EBackupSetItemType.EBackupSetItemType__Inclusion)
                        .ToArray();
                    break;
                case "Exclusion":
                    selectedItems = backupSetItems
                        .Where(item => item.getType() == EBackupSetItemType.EBackupSetItemType__Exclusion)
                        .ToArray();
                    break;
                case "RegexExclusion":
                    selectedItems = backupSetItems
                        .Where(item => item.getType() == EBackupSetItemType.EBackupSetItemType__RegExExclusion)
                        .ToArray();
                    break;
                default:
                    selectedItems = backupSetItems;
                    break;
            }

            // Select matching items
            WildcardOptions wcOptions = WildcardOptions.IgnoreCase |
                                        WildcardOptions.Compiled;

            WildcardPattern wcPattern = null;
            if (MyInvocation.BoundParameters.ContainsKey(nameof(Folder)))
                wcPattern = new WildcardPattern(Folder, wcOptions);

            List<BackupSetItem> removalItems = new List<BackupSetItem>();

            WriteVerbose("Performing Action: Match Items");
            foreach(BackupSetItem item in selectedItems)
            {
                EBackupSetItemType itemType = item.getType();
                if (itemType == EBackupSetItemType.EBackupSetItemType__Exclusion || itemType == EBackupSetItemType.EBackupSetItemType__Inclusion)
                {
                    BackupSetFileItem backupSetFileItem = BackupSetFileItem.from(item);
                    string folder = backupSetFileItem.getFolder();
                    WriteDebug($"Got folder: {folder}");
                    string filter = backupSetFileItem.getFilter();
                    WriteDebug($"Got filter: {filter}");
                    backupSetFileItem.Dispose();

                    bool folderMatch = (MyInvocation.BoundParameters.ContainsKey(nameof(Folder))) ? wcPattern.IsMatch(folder) : LiteralFolder == folder;

                    if (folderMatch)
                    {
                        if (MyInvocation.BoundParameters.ContainsKey(nameof(Filter)))
                        {
                            if (Filter == filter)
                            {
                                WriteVerbose($@"Notice: Matched Item: {folder}\{filter}");
                                removalItems.Add(item);
                            }
                        }
                        else
                        {
                            WriteVerbose($"Notice: Matched Item: {folder}");
                            removalItems.Add(item);
                        }
                    }
                }
                else if (itemType == EBackupSetItemType.EBackupSetItemType__RegExExclusion)
                {
                    BackupSetRegexExclusion backupSetRegexExclusion = BackupSetRegexExclusion.from(item);
                    string folder = backupSetRegexExclusion.getFolder();
                    string expression = backupSetRegexExclusion.getExpression();
                    backupSetRegexExclusion.Dispose();

                    string folderExpression = (folder.Last() != '\\') ? $@"{folder}\{expression}" : $"{folder}{expression}";

                    if (wcPattern.IsMatch(folderExpression))
                        if (ShouldProcess($"{folder} with Expression: {expression}"))
                            removalItems.Add(item);                    
                }
            }

            if (removalItems.Count() > 0)
            {
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
            }
            else
                WriteVerbose("Notice: No Items Matched for Removal");

            backupSet.Dispose();
        }
    }
}