using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientSession", SupportsShouldProcess = true, DefaultParameterSetName = "id")]

    sealed public class RemoveDSClientSession : BaseDSClientSessionAction
    {
        private List<DSClientSession> _sessions;
        protected override void ProcessDSClientSession(IEnumerable<DSClientSession> sessions)
        {
            _sessions = sessions.ToList();

            base.ProcessDSClientSession(sessions);

            WriteDebug("Update SessionState");
            SessionState.PSVariable.Remove("DSClientSessions");
            SessionState.PSVariable.Set("DSClientSessions", _sessions);
        }

        protected override void ProcessDSClientSessionAction(DSClientSession session)
        {
            if (ShouldProcess($"'{session.Name}' with Id '{session.Id}'", "Remove DS-Client Session"))
            {
                session.Disconnect();
                _sessions.Remove(session);
            }
        }
    }
}