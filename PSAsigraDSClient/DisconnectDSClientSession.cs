using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommunications.Disconnect, "DSClientSession")]
    [OutputType(typeof(DSClientSession))]

    sealed public class DisconnectDSClientSession : BaseDSClientSession
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "session", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Session to Disconnect")]
        public DSClientSession Session { get; set; }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "id", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Id of the Session to Disconnect")]
        public int Id { get; set; }

        private List<DSClientSession> _sessions;

        protected override void ProcessDSClientSession(IEnumerable<DSClientSession> sessions)
        {
            if (sessions != null)
                _sessions = sessions.ToList();

            DSClientSession session = null;
            if (MyInvocation.BoundParameters.ContainsKey(nameof(Id)))
            {
                bool found = false;
                for (int i = 0; i < sessions.Count(); i++)
                {
                    if (_sessions[i].Id == Id)
                    {
                        session = _sessions[i];
                        found = true;
                        break;
                    }
                }

                if (!found)
                    throw new Exception("Session Not Found");
            }
            else if (MyInvocation.BoundParameters.ContainsKey(nameof(Session)))
                session = Session;

            WriteVerbose("Performing Action: Disconnect DS-Client Session");
            session.Disconnect();

            WriteObject(session);
        }
    }
}