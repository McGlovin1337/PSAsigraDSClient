using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientSession")]
    [OutputType(typeof(DSClientSession))]

    sealed public class NewDSClientSession : BaseDSClientSession
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the DS-Client Host to connect to")]
        [ValidateNotNullOrEmpty]
        public string HostName { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Specify Credentials to use to connect to DSClient")]
        [ValidateNotNullOrEmpty]
        public PSCredential Credential { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify a name for this Session")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(HelpMessage = "Specify the TCP port to connect to")]
        [ValidateRange(1, 65535)]
        public UInt16 Port { get; set; } = 4411;

        [Parameter(HelpMessage = "Specify to NOT establish an SSL Connection")]
        public SwitchParameter NoSSL { get; set; }

        [Parameter(HelpMessage = "Specify the Asigra DS-Client API Version to use")]
        public string APIVersion { get; set; } = "13.0.0.0";

        private List<DSClientSession> _sessions;

        protected override void ProcessDSClientSession(IEnumerable<DSClientSession> sessions)
        {
            if (sessions != null)
                _sessions = sessions.ToList();

            int id = 1;
            if (_sessions.Count() > 0)
            {
                for (int i = 0; i < _sessions.Count; i++)
                {
                    if (_sessions[i].Id >= id)
                        id = _sessions[i].Id + 1;
                }
            }

            bool nossl = false;
            if (MyInvocation.BoundParameters.ContainsKey(nameof(NoSSL)))
                nossl = NoSSL;

            WriteVerbose("Performing Action: Establish DS-Client Session");
            DSClientSession session = new DSClientSession(id, HostName, Port, nossl, APIVersion, Credential, Name);
            _sessions.Add(session);

            SessionState.PSVariable.Set("DSClientSessions", _sessions);

            WriteObject(session);
        }
    }
}