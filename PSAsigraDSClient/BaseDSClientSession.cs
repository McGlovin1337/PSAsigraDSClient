using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientSession : PSCmdlet
    {
        protected abstract void ProcessDSClientSession(IEnumerable<DSClientSession> sessions);

        protected override void ProcessRecord()
        {
            WriteVerbose("Performing Action: Retrieve DS-Client Sessions");
            if (!(SessionState.PSVariable.GetValue("DSClientSessions", null) is List<DSClientSession> sessions))
                sessions = new List<DSClientSession>();

            WriteDebug($"Session Count: {sessions.Count()}");

            ProcessDSClientSession(sessions);
        }
    }

    public abstract class BaseDSClientSessionAction : BaseDSClientSession
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

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "sessionObj", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify DS-Client Session by DSClientSession Object")]
        public DSClientSession[] Session { get; set; }

        private List<DSClientSession> _sessions;

        protected abstract void ProcessDSClientSessionAction(DSClientSession session);

        protected override void ProcessDSClientSession(IEnumerable<DSClientSession> sessions)
        {
            _sessions = sessions.ToList();

            if (_sessions.Count() > 0)
            {
                WildcardOptions wcOptions = WildcardOptions.IgnoreCase |
                                    WildcardOptions.Compiled;

                List<DSClientSession> selectedSessions = new List<DSClientSession>();

                if (MyInvocation.BoundParameters.ContainsKey(nameof(Session)))
                {
                    foreach (DSClientSession session in Session)
                        selectedSessions.Add(session);
                }
                else
                {
                    if (MyInvocation.BoundParameters.ContainsKey(nameof(Id)))
                        foreach (int id in Id)
                        {
                            DSClientSession session = _sessions.SingleOrDefault(s => s.Id == id);
                            selectedSessions.Add(session);
                        }

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(Name)))
                        foreach (string name in Name)
                        {
                            WildcardPattern wcPattern = new WildcardPattern(name, wcOptions);
                            IEnumerable<DSClientSession> matchedSessions = _sessions.Where(s => wcPattern.IsMatch(s.Name));
                            foreach (DSClientSession session in matchedSessions)
                                if (!selectedSessions.Contains(session))
                                    selectedSessions.Add(session);
                        }

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(HostName)))
                        foreach (string host in HostName)
                        {
                            WildcardPattern wcPattern = new WildcardPattern(host, wcOptions);
                            IEnumerable<DSClientSession> matchedSessions = _sessions.Where(s => wcPattern.IsMatch(s.HostName));
                            foreach (DSClientSession session in matchedSessions)
                                if (!selectedSessions.Contains(session))
                                    selectedSessions.Add(session);
                        }

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(State)))
                    {
                        IEnumerable<DSClientSession> matchedSessions = _sessions.Where(s => s.State == (DSClientSession.ConnectionState)Enum.Parse(typeof(DSClientSession.ConnectionState), State, true));
                        foreach (DSClientSession session in matchedSessions)
                            if (!selectedSessions.Contains(session))
                                selectedSessions.Add(session);
                    }

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(OperatingSystem)))
                    {
                        foreach (string os in OperatingSystem)
                        {
                            IEnumerable<DSClientSession> matchedSessions = _sessions.Where(s => s.OperatingSystem == os);
                            foreach (DSClientSession session in matchedSessions)
                                if (!selectedSessions.Contains(session))
                                    selectedSessions.Add(session);
                        }
                    }
                }

                foreach (DSClientSession session in selectedSessions)
                    ProcessDSClientSessionAction(session);
            }
        }
    }
}