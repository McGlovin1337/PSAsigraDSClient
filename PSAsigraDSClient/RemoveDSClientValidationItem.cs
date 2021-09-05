using System;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientValidationItem", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class RemoveDSClientValidationItem : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Validation Session to Remove Item(s) from")]
        public int ValidationId { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Item(s) to remove by Id")]
        public long[] ItemId { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Performing Action: Retrieve Validation Session");
            DSClientValidationSession validationSession = DSClientSessionInfo.GetValidationSession(ValidationId);

            if (validationSession != null)
                if (ShouldProcess($"Validation Session with Id '{ValidationId}'", "Remove Items for Validation"))
                    validationSession.RemoveSelectedItems(ItemId);
                else
                    throw new Exception("Validation Session not found");
        }
    }
}