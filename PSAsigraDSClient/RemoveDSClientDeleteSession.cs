using System;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientDeleteSession", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class RemoveDSClientDeleteSession : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Delete Session to Remove")]
        public int DeleteId { get; set; }

        protected override void DSClientProcessRecord()
        {
            DSClientDeleteSession deleteSession = DSClientSessionInfo.GetDeleteSession(DeleteId);

            if (deleteSession != null)
                if (ShouldProcess($"DeleteId '{deleteSession.DeleteId}'", "Remove Delete Session"))
                    DSClientSessionInfo.RemoveDeleteSession(deleteSession);
            else
                throw new Exception("Delete Session not found");
        }
    }
}