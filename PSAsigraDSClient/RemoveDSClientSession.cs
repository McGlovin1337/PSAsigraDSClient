using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientSession", SupportsShouldProcess = true, DefaultParameterSetName = "id")]

    sealed public class RemoveDSClientSession : BaseDSClientSession
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "id", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify DS-Client Session by Id")]
        public int[] Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "name", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify DS-Client Sessions by Name")]
        [SupportsWildcards]
        public string[] Name { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "hostname", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify DS-Client Sessions by HostName")]
        [SupportsWildcards]
        public string[] HostName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "state", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify DS-Client Sessions by Connection State")]
        [ValidateSet("Connected", "Disconnected")]
        public string State { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "os", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify DS-Client Sessions by Operating System")]
        [ValidateSet("Linux", "Mac", "Windows")]
        public string[] OperatingSystem { get; set; }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "sessionObj", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify DS-Client Session to remove by DSClientSession Object")]
        public DSClientSession[] Session { get; set; }

        private List<DSClientSession> _sessions;

        private void RemoveSession(DSClientSession session)
        {
            if (ShouldProcess($"'{session.Name}' with Id '{session.Id}'", "Remove DS-Client Session"))
                    _sessions.Remove(session);
        }

        protected override void ProcessDSClientSession(IEnumerable<DSClientSession> sessions)
        {
            WriteVerbose("Performing Action: Retrieve DS-Client Sessions");
            if (sessions != null)
                _sessions = sessions.ToList();
            WriteDebug($"Session Count: {_sessions.Count()}");

            if (_sessions != null)
            {
                WildcardOptions wcOptions = WildcardOptions.IgnoreCase |
                            WildcardOptions.Compiled;

                List<DSClientSession> removeSessions = new List<DSClientSession>();

                if (MyInvocation.BoundParameters.ContainsKey(nameof(Session)))
                {
                    foreach (DSClientSession session in Session)
                    {
                        session.Disconnect();
                        removeSessions.Add(session);
                    }
                }
                else
                {
                    if (MyInvocation.BoundParameters.ContainsKey(nameof(Id)))
                        foreach (int id in Id)
                        {
                            DSClientSession session = _sessions.SingleOrDefault(s => s.Id == id);
                            session.Disconnect();
                            removeSessions.Add(session);
                        }

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(Name)))
                        foreach (string name in Name)
                        {
                            WildcardPattern wcPattern = new WildcardPattern(name, wcOptions);
                            IEnumerable<DSClientSession> matchedSessions = _sessions.Where(s => wcPattern.IsMatch(s.Name));
                            foreach (DSClientSession session in matchedSessions)
                                if (!removeSessions.Contains(session))
                                {
                                    session.Disconnect();
                                    removeSessions.Add(session);
                                }
                        }

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(HostName)))
                        foreach (string host in HostName)
                        {
                            WildcardPattern wcPattern = new WildcardPattern(host, wcOptions);
                            IEnumerable<DSClientSession> matchedSessions = _sessions.Where(s => wcPattern.IsMatch(s.HostName));
                            foreach (DSClientSession session in matchedSessions)
                                if (!removeSessions.Contains(session))
                                {
                                    session.Disconnect();
                                    removeSessions.Add(session);
                                }
                        }

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(State)))
                    {
                        IEnumerable<DSClientSession> matchedSessions = _sessions.Where(s => s.State == (DSClientSession.ConnectionState)Enum.Parse(typeof(DSClientSession.ConnectionState), State, true));
                        foreach (DSClientSession session in matchedSessions)
                            if (!removeSessions.Contains(session))
                            {
                                session.Disconnect();
                                removeSessions.Add(session);
                            }
                    }

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(OperatingSystem)))
                    {
                        foreach (string os in OperatingSystem)
                        {
                            IEnumerable<DSClientSession> matchedSessions = _sessions.Where(s => s.OperatingSystem == os);
                            foreach (DSClientSession session in matchedSessions)
                                if (!removeSessions.Contains(session))
                                {
                                    session.Disconnect();
                                    removeSessions.Add(session);
                                }
                        }
                    }
                }

                WriteDebug($"Remove Count: {removeSessions.Count()}");
                foreach (DSClientSession session in removeSessions)
                    RemoveSession(session);

                WriteDebug("Update SessionState");
                SessionState.PSVariable.Remove("DSClientSessions");
                SessionState.PSVariable.Set("DSClientSessions", _sessions);
            }
        }
    }
}