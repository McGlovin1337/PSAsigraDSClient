using System;
using System.Linq;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientValidationSession")]
    [OutputType(typeof(DSClientValidationSession))]

    sealed public class GetDSClientValidationSession : DSClientCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Validation Session to return")]
        public int ValidationId { get; set; }

        protected override void DSClientProcessRecord()
        {
            if (MyInvocation.BoundParameters.ContainsKey(nameof(ValidationId)))
            {
                DSClientValidationSession validationSession = DSClientSessionInfo.GetValidationSession(ValidationId);

                if (validationSession != null)
                    WriteObject(validationSession);
                else
                    throw new Exception("Validation Session not found");
            }
            else
            {
                DSClientSessionInfo.GetValidationSessions()
                    .ToList()
                    .ForEach(WriteObject);
            }
        }
    }
}