using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public abstract class DSClientCmdlet: PSCmdlet
    {
        protected ClientConnection DSClientSession { get; private set; }
        protected abstract void DSClientProcessRecord();

        protected override void ProcessRecord()
        {
            DSClientSession = SessionState.PSVariable.GetValue("DSClientSession", null) as ClientConnection;

            if (DSClientSession == null)
                throw new Exception("There is currently no active DSClient Sessions.");

            DSClientProcessRecord();
        }
    }
}
