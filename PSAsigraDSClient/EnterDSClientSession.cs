using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Enter, "DSClientSession", DefaultParameterSetName = "default")]

    public class EnterDSClientSession: BaseDSClientSessionCleanup
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "default", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the DS-Client Host to connect to")]
        [ValidateNotNullOrEmpty]
        public string HostName { get; set; }

        [Parameter(Position = 1, ParameterSetName = "default", HelpMessage = "Specify the TCP port to connect to")]
        [ValidateRange(1, 65535)]
        public UInt16 Port { get; set; } = 4411;

        [Parameter(ParameterSetName = "default", HelpMessage = "Specify to NOT establish an SSL Connection")]
        public SwitchParameter NoSSL { get; set; }

        [Parameter(ParameterSetName = "default", HelpMessage = "Specify the Asigra DS-Client API Version to use")]
        public string APIVersion { get; set; } = "13.0.0.0";

        [Parameter(ParameterSetName = "default", HelpMessage = "Specify Credentials to use to connect to DSClient")]
        [ValidateNotNullOrEmpty]
        public PSCredential Credential { get; set; }

        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "session", HelpMessage = "Specify an existing Session to use" )]
        public DSClientSession Session { get; set; }

        protected override void ProcessCleanSession()
        {
            DSClientSession currentSessionContext;
            //ClientConnection DSClientSession;

            if (Session != null)
                currentSessionContext = Session;
            else
            {
                string user = Credential.UserName;
                string pwd = Credential.GetNetworkCredential().Password;

                WriteVerbose("Performing Action: Establish DS-Client Session");
                currentSessionContext = new DSClientSession(-1, HostName, Port, NoSSL, APIVersion, Credential, logoutExit: true);
                //DSClientSession = ConnectSession(Host, Port, NoSSL, APIVersion, user, pwd);
            }

            // Confirm the Connection is Alive
            try
            {
                currentSessionContext.GetClientConnection().keepAlive();
            }
            catch
            {
                throw new Exception("DS-Client Connection Failed");
            }

            SessionState.PSVariable.Set("DSClientSession", currentSessionContext);

            // Get and store the DSClient Operating System Type into SessionState
            //ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();
            //EOSFlavour DSClientOSFlavour = DSClientConfigMgr.getClientOSType();
            //DSClientOSType DSClientOSType = new DSClientOSType(DSClientOSFlavour);
            //SessionState.PSVariable.Set("DSClientOSType", DSClientOSType);

            //DSClientConfigMgr.Dispose();

            WriteObject("DS-Client Session Established.");
        }

        private ClientConnection ConnectSession(string Host, UInt16 Port, bool NoSSL, string APIVersion, string User, string Pass)
        {
            string URL;

            if (NoSSL == false)
                URL = @"https://" + Host + ":" + Port + @"/api";
            else
                URL = @"http://" + Host + ":" + Port + @"/api";

            ClientConnection session = ApiFactory.CreateConnection(URL, APIVersion, User, Pass, 0);

            return session;
        }
    }
}