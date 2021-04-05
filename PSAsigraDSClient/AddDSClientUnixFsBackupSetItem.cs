using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientUnixFsBackupSetItem")]

    public class AddDSClientUnixFsBackupSetItem: BaseDSClientBackupSetItemParams
    {
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

            if (Exclusion)
                backupSetItems.Add(CreateExclusionItem(dataSourceBrowser, computer, Path, Filter, ExcludeSubDirs));
            else if (RegexExclusion)
                backupSetItems.Add(CreateRegexExclusion(dataSourceBrowser, computer, Path, Filter, RegexMatchDirectory, RegexCaseInsensitive));
            else if (Inclusion)
            {
                UnixFS_BackupSetInclusionItem item = UnixFS_BackupSetInclusionItem.from(dataSourceBrowser.createInclusionItem(computer, Path, MaxGenerations));
                item.setSubdirDescend(!ExcludeSubDirs);
                item.setIncludingACL(!ExcludeACLs);

                if (computer.Split('\\').First() == "Local File System")
                {
                    UnixFS_LinuxLFS_BackupSetInclusionItem lfsItem = UnixFS_LinuxLFS_BackupSetInclusionItem.from(item);
                    lfsItem.setIncludingPosixACL(!ExcludePosixACLs);
                    backupSetItems.Add(lfsItem);
                }
                else
                    backupSetItems.Add(item);
            }

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