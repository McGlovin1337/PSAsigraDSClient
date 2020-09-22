using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientOrphanedBackupSet")]
    [OutputType(typeof(DSClientOrphanedBackupSet))]

    public class GetDSClientOrphanedBackupSet: BaseDSClientOrphanedBackupSet
    {
        protected override void ProcessOrphanedBackupSet(SystemActivityManager DSClientSystemActivityMgr, OrphanedBackupSet[] orphanedBackupSets)
        {
            List<DSClientOrphanedBackupSet> OrphanedBackupSets = new List<DSClientOrphanedBackupSet>();

            foreach (var set in orphanedBackupSets)
            {
                DSClientOrphanedBackupSet orphanedBackupSet = new DSClientOrphanedBackupSet(set.getInfo());
                OrphanedBackupSets.Add(orphanedBackupSet);
            }

            OrphanedBackupSets.ForEach(WriteObject);
        }
    }
}