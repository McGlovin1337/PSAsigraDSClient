using System;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientValidationSession", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class RemoveDSClientValidationSession : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Validation Session to Remove")]
        public int ValidationId { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Performing Action: Retrieve Validation Session");
            DSClientValidationSession validationSession = DSClientSessionInfo.GetValidationSession(ValidationId);

            if (validationSession != null)
            {
                if (ShouldProcess($"ValidationId: '{ValidationId}', for BackupSet '{validationSession.BackupSetId}'", "Removing Validation Session"))
                    DSClientSessionInfo.RemoveValidationSession(validationSession);
            }
            else
            {
                ErrorRecord errorRecord = new ErrorRecord(
                    new Exception("Validation Session not found"),
                    "Exception",
                    ErrorCategory.ObjectNotFound,
                    null);
                WriteError(errorRecord);
            }
        }
    }
}