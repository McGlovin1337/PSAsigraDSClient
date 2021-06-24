using System.Collections.Generic;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientSession : PSCmdlet
    {
        protected abstract void ProcessDSClientSession(IEnumerable<DSClientSession> sessions);

        protected override void ProcessRecord()
        {
            List<DSClientSession> sessions = SessionState.PSVariable.GetValue("DSClientSessions", null) as List<DSClientSession>;

            if (sessions == null)
                sessions = new List<DSClientSession>();

            ProcessDSClientSession(sessions);
        }
    }
}