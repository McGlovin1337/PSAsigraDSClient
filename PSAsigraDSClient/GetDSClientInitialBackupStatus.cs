using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientInitialBackupStatus")]
    [OutputType(typeof(DSClientInitialBackupStatus))]

    public class GetDSClientInitialBackupStatus : DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            InitialBackupManager initialBackupManager = DSClientSession.getInitialBackupManager();

            WriteVerbose("Performing Action: Retrieve Initial Backup Status");
            init_backup_status[] initBackupStatuses = initialBackupManager.statusList();

            initialBackupManager.Dispose();

            List<DSClientInitialBackupStatus> initStatuses = new List<DSClientInitialBackupStatus>();

            foreach (init_backup_status status in initBackupStatuses)
                initStatuses.Add(new DSClientInitialBackupStatus(status));

            initStatuses.ForEach(WriteObject);
        }

        private class DSClientInitialBackupStatus
        {
            public int BackupSetId { get; private set; }
            public string Computer { get; private set; }
            public string BackupSet { get; private set; }
            public string Status { get; private set; }
            public string Path { get; private set; }

            public DSClientInitialBackupStatus(init_backup_status backupStatus)
            {
                Path = backupStatus.path;
                Status = EnumToString(backupStatus.status);
                BackupSetId = backupStatus.setID;
                BackupSet = backupStatus.setName;
                Computer = backupStatus.serverName;
            }
        }
    }
}