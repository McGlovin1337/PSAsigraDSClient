using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientVMwareVADPBackupSetItem")]

    public class AddDSClientVMwareVADPBackupSetItem : BaseDSClientBackupSetItemParams
    {
        protected override void DSClientProcessRecord()
        {
            // Get the requested Backup Set from DS-Client
            WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId: {BackupSetId}");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);
            string computer = backupSet.getComputerName();

            // Create a Data Source Browser
            DataSourceBrowser dataSourceBrowser = backupSet.dataBrowser();

            // Create a List of Items
            List<BackupSetItem> backupSetItems = new List<BackupSetItem>();

            if (Exclusion)
                backupSetItems.Add(CreateExclusionItem(dataSourceBrowser, computer, Path, Filter, ExcludeSubDirs));
            else if (RegexExclusion)
                backupSetItems.Add(CreateRegexExclusion(dataSourceBrowser, computer, Path, Filter, RegexMatchDirectory, RegexCaseInsensitive));
            else if (Inclusion)
                backupSetItems.Add(CreateInclusionItem(dataSourceBrowser, computer, Path, Filter, MaxGenerations, ExcludeSubDirs));

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