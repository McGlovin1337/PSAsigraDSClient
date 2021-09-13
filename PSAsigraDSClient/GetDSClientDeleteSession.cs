using System;
using System.Linq;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientDeleteSession")]
    [OutputType(typeof(DSClientDeleteSession))]

    sealed public class GetDSClientDeleteSession : DSClientCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Delete Session Id to return")]
        public int DeleteId { get; set; }

        protected override void DSClientProcessRecord()
        {
            if (MyInvocation.BoundParameters.ContainsKey(nameof(DeleteId)))
            {
                DSClientDeleteSession deleteSession = DSClientSessionInfo.GetDeleteSession(DeleteId);

                if (deleteSession != null)
                    WriteObject(deleteSession);
                else
                    throw new Exception("Delete Session not found");
            }
            else
            {
                DSClientSessionInfo.GetDeleteSessions()
                    .ToList()
                    .ForEach(WriteObject);
            }
        }
    }
}