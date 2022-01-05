using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Rename, "DSClientBackupSet", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class RenameDSClientBackupSet : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set Id to Rename")]
        [ValidateNotNullOrEmpty]
        public int BackupSetId { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the New Name for the Backup Set")]
        [ValidateNotNullOrEmpty]
        public string NewName { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Get the Backup Set from DS-Client
            WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId {BackupSetId}");
            BackupSet backupSet = null;
            try
            {
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
                if (ShouldProcess($"Backup Set with Id: {BackupSetId}", $"Rename Backup Set from {backupSet.getName()} to {NewName}"))
                {
                    backupSet.setName(NewName);
                }

                backupSet.Dispose();
            }
        }
    }
}