using System;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientRestoreSession", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class RemoveDSClientRestoreSession : DSClientCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Restore Session Id to remove")]
        public int RestoreId { get; set; }

        protected override void DSClientProcessRecord()
        {
            DSClientRestoreSession restoreSession = DSClientSessionInfo.GetRestoreSession(RestoreId);

            if (restoreSession != null)
            {
                if (ShouldProcess($"RestoreId: {restoreSession.RestoreId} for BackupSetId: {restoreSession.BackupSetId}", "Removing Restore Session"))
                    DSClientSessionInfo.RemoveRestoreSession(restoreSession);
            }
            else
            {
                ErrorRecord errorRecord = new ErrorRecord(
                    new Exception("Restore Session not found"),
                    "Exception",
                    ErrorCategory.ObjectNotFound,
                    null);
                WriteError(errorRecord);
            }
        }
    }
}