using System;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientBackupSetSessions")]
    [OutputType(typeof(DSClientBackupSessions))]

    sealed public class GetDSClientBackupSetSessions: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Backup Set to retrieve Sessions for")]
        public int BackupSetId { get; set; }

        protected override void DSClientProcessRecord()
        {
            BackupSet backupSet = null;

            // Retrieve the Backup Set
            try
            {
                WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId: {BackupSetId}");
                backupSet = DSClientSession.backup_set(BackupSetId);
            }
            catch (APIException e)
            {
                ErrorRecord errorRecord = new ErrorRecord(
                    e,
                    "APIException",
                    ErrorCategory.ObjectNotFound,
                    null);
                WriteError(errorRecord);
            }

            if (backupSet != null)
            {
                List<DSClientBackupSessions> BackupSessions = new List<DSClientBackupSessions>();

                WriteVerbose("Performing Action: Retrieve Backup Set Sessions");
                backup_sessions[] backupSessions = backupSet.backup_times();

                foreach (backup_sessions session in backupSessions)
                {
                    DSClientBackupSessions backupSession = new DSClientBackupSessions(BackupSetId, session);
                    BackupSessions.Add(backupSession);
                }

                BackupSessions.ForEach(WriteObject);

                backupSet.Dispose();
            }
        }

        private class DSClientBackupSessions
        {
            public int BackupSetId { get; private set; }
            public int ActivityId { get; private set; }
            public DateTime StartTime { get; private set; }
            public DateTime EndTime { get; private set; }
            public int Files { get; private set; }
            public DSClientStorageUnit Size { get; private set; }

            public DSClientBackupSessions(int backupSetId, backup_sessions backupSessions)
            {
                BackupSetId = backupSetId;
                ActivityId = backupSessions.activity_id;
                StartTime = UnixEpochToDateTime(backupSessions.start_time);
                EndTime = UnixEpochToDateTime(backupSessions.end_time);
                Files = backupSessions.files;
                Size = new DSClientStorageUnit(backupSessions.size);
            }
        }
    }
}