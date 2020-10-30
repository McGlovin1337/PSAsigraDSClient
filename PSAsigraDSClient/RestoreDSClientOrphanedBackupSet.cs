using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsData.Restore, "DSClientOrphanedBackupSet")]
    [OutputType(typeof(DSClientBackupSetId))]

    public class RestoreDSClientOrphanedBackupSet: BaseDSClientOrphanedBackupSet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Select Orphaned Backup Set by Name to recover")]
        [SupportsWildcards]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Select Orphaned Backup Set by Computer to recover")]
        [ValidateNotNullOrEmpty]
        public string Computer { get; set; }

        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true, HelpMessage = "Select Orphaned Backup Set by Owner to recover")]
        [ValidateNotNullOrEmpty]
        public string Owner { get; set; }

        protected override void ProcessOrphanedBackupSet(SystemActivityManager DSClientSystemActivityMgr, OrphanedBackupSet[] orphanedBackupSets)
        {
            IEnumerable<OrphanedBackupSet> filteredSets = null;

            WildcardOptions wcOptions = WildcardOptions.IgnoreCase
                                            | WildcardOptions.Compiled;

            if (Name != null)
            {
                WildcardPattern wcPattern = new WildcardPattern(Name, wcOptions);

                filteredSets = orphanedBackupSets.Where(set => wcPattern.IsMatch(set.getInfo().name));
            }

            if (Computer != null)
                filteredSets = (filteredSets == null) ? orphanedBackupSets.Where(set => set.getInfo().computer == Computer) : filteredSets.Where(set => set.getInfo().computer == Computer);

            if (Owner != null)
                filteredSets = (filteredSets == null) ? orphanedBackupSets.Where(set => set.getInfo().owner == Owner) : filteredSets.Where(set => set.getInfo().owner == Owner);

            List<DSClientBackupSetId> recoveredSetIds = new List<DSClientBackupSetId>();

            WriteVerbose("Performing Recovery of Orphaned Backup Set(s)...");
            if (filteredSets != null)
                foreach (var orphanedSet in filteredSets)
                {
                    DSClientBackupSetId recoveredSetId = new DSClientBackupSetId(DSClientSystemActivityMgr.recover_orphaned_backup_set(orphanedSet));
                    recoveredSetIds.Add(recoveredSetId);
                }
            else
                foreach (var orphanedSet in filteredSets)
                {
                    DSClientBackupSetId recoveredSetId = new DSClientBackupSetId(DSClientSystemActivityMgr.recover_orphaned_backup_set(orphanedSet));
                    recoveredSetIds.Add(recoveredSetId);
                }

            recoveredSetIds.ForEach(WriteObject);
        }

        private class DSClientBackupSetId
        {
            public int BackupSetId { get; private set; }

            public DSClientBackupSetId(int backupSetId)
            {
                BackupSetId = backupSetId;
            }
        }
    }
}