using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientSession")]
    [OutputType(typeof(DSClientSession))]

    public class NewDSClientSession : BaseDSClientSession
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


        protected override void ProcessRecord()
        {
            List<DSClientSession> sessions = SessionState.PSVariable.GetValue("DSClientSessions", null) as List<DSClientSession>;

            int id = 1;
            if (sessions != null)
            {
                for (int i = 0; i < sessions.Count; i++)
                {
                    if (sessions[i].Id >= id)
                        id = sessions[i].Id + 1;
                }
            }
            else
                sessions = new List<DSClientSession>();

            bool nossl = false;
            if (MyInvocation.BoundParameters.ContainsKey(nameof(NoSSL)))
                nossl = NoSSL;

            WriteVerbose("Performing Action: Establish DS-Client Session");
            DSClientSession session = new DSClientSession(id, HostName, Port, nossl, APIVersion, Credential, Name);

            sessions.Add(session);
            SessionState.PSVariable.Set("DSClientSessions", sessions);

            WriteObject(session);
        }
    }
}