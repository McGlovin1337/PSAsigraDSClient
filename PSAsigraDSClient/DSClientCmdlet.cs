using System;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public abstract class DSClientCmdlet: PSCmdlet
    {
        protected DSClientSession DSClientSessionInfo;
        protected ClientConnection DSClientSession;
        protected abstract void DSClientProcessRecord();

        protected override void ProcessRecord()
        {
            DSClientSessionInfo = SessionState.PSVariable.GetValue("DSClientSession", null) as DSClientSession;

            if (DSClientSessionInfo == null)
                throw new Exception("There is currently no active DS-Client Sessions.");

            DSClientSession = DSClientSessionInfo.GetClientConnection();

            DSClientSessionInfo.TestConnection();

            DSClientProcessRecord();
        }
    }
}
