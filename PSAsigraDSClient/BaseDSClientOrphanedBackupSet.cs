using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientOrphanedBackupSet: DSClientCmdlet
    {
        protected abstract void ProcessOrphanedBackupSet(SystemActivityManager DSClientSystemActivityMgr, OrphanedBackupSet[] orphanedBackupSets);

        protected override void DSClientProcessRecord()
        {
            SystemActivityManager DSClientSystemActivityMgr = DSClientSession.getSystemActivityManager();

            WriteVerbose("Retrieving Orphaned Backup Sets...");
            OrphanedBackupSet[] orphanedSets = DSClientSystemActivityMgr.getOrphanedbackupSets();

            ProcessOrphanedBackupSet(DSClientSystemActivityMgr, orphanedSets);

            DSClientSystemActivityMgr.Dispose();
        }
        protected class DSClientOrphanedBackupSet
        {
            public string Computer { get; set; }
            public string Name { get; set; }
            public string Owner { get; set; }

            public DSClientOrphanedBackupSet(recovery_info recoveryInfo)
            {
                Computer = recoveryInfo.computer;
                Name = recoveryInfo.name;
                Owner = recoveryInfo.owner;
            }
        }
    }
}