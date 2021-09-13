using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientBackupSet")]
    [OutputType(typeof(DSClientBackupSet))]

    sealed public class GetDSClientBackupSet: BaseDSClientBackupSet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Backup Set Id")]
        [ValidateNotNullOrEmpty]
        public int BackupSetId { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId: {BackupSetId}");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            WriteDebug("Parsing Backup Set details.");
            DSClientBackupSet dSClientBackupSet = new DSClientBackupSet(backupSet, DSClientSessionInfo.OperatingSystem);

            WriteObject(dSClientBackupSet);
        }
    }
}