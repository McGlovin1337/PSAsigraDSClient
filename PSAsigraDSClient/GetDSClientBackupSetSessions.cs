using System;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientBackupSetSessions")]
    [OutputType(typeof(DSClientBackupSessions))]

    public class GetDSClientBackupSetSessions: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Backup Set to retrieve Sessions for")]
        public int BackupSetId { get; set; }

        protected override void DSClientProcessRecord()
        {
            List<DSClientBackupSessions> BackupSessions = new List<DSClientBackupSessions>();

            WriteVerbose("Retrieving Backup Set from DS-Client...");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            WriteVerbose("Retrieving Backup Set Sessions...");
            backup_sessions[] backupSessions = backupSet.backup_times();

            foreach (backup_sessions session in backupSessions)
            {
                DSClientBackupSessions backupSession = new DSClientBackupSessions(BackupSetId, session);
                BackupSessions.Add(backupSession);
            }

            BackupSessions.ForEach(WriteObject);

            backupSet.Dispose();
        }

        private class DSClientBackupSessions
        {
            public int BackupSetId { get; set; }
            public int ActivityId { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public int Files { get; set; }
            public long Size { get; set; }

            public DSClientBackupSessions(int backupSetId, backup_sessions backupSessions)
            {
                BackupSetId = backupSetId;
                ActivityId = backupSessions.activity_id;
                StartTime = UnixEpochToDateTime(backupSessions.start_time);
                EndTime = UnixEpochToDateTime(backupSessions.end_time);
                Files = backupSessions.files;
                Size = backupSessions.size;
            }
        }
    }
}