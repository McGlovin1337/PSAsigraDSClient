using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class DSClientCmdlet: PSCmdlet
    {
        protected ClientConnection DSClientSession { get; private set; }
        protected DSClientOSType DSClientOSType { get; private set; }
        protected abstract void DSClientProcessRecord();

        protected override void ProcessRecord()
        {
            DSClientSession = SessionState.PSVariable.GetValue("DSClientSession", null) as ClientConnection;
            DSClientOSType = SessionState.PSVariable.GetValue("DSClientOSType", null) as DSClientOSType;

            if (DSClientSession == null)
                throw new Exception("There is currently no active DSClient Sessions.");

            DSClientProcessRecord();
        }
    }
}
