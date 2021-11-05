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
            BackupSet backupSet = null;
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
                WriteDebug("Parsing Backup Set details.");
                DSClientBackupSet dSClientBackupSet = new DSClientBackupSet(backupSet, DSClientSessionInfo.OperatingSystem);

                WriteObject(dSClientBackupSet);
            }
        }
    }
}