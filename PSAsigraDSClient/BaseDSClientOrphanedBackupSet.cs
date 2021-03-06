﻿using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientOrphanedBackupSet: DSClientCmdlet
    {
        protected abstract void ProcessOrphanedBackupSet(SystemActivityManager DSClientSystemActivityMgr, OrphanedBackupSet[] orphanedBackupSets);

        protected override void DSClientProcessRecord()
        {
            SystemActivityManager DSClientSystemActivityMgr = DSClientSession.getSystemActivityManager();

            WriteVerbose("Performing Action: Retrieve Orphaned Backup Sets");
            OrphanedBackupSet[] orphanedSets = DSClientSystemActivityMgr.getOrphanedbackupSets();

            ProcessOrphanedBackupSet(DSClientSystemActivityMgr, orphanedSets);

            DSClientSystemActivityMgr.Dispose();
        }
        protected class DSClientOrphanedBackupSet
        {
            public string Computer { get; private set; }
            public string Name { get; private set; }
            public string Owner { get; private set; }

            public DSClientOrphanedBackupSet(recovery_info recoveryInfo)
            {
                Computer = recoveryInfo.computer;
                Name = recoveryInfo.name;
                Owner = recoveryInfo.owner;
            }
        }
    }
}