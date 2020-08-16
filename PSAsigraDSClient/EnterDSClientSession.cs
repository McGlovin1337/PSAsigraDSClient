using System;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Enter, "DSClientSession")]
    public class EnterDSClientSession: PSCmdlet
    {
        [Parameter(Position = 0, HelpMessage = "Specify the DSClient Host to connect to")]
        [ValidateNotNullOrEmpty]
        public string Host { get; set; }

        [Parameter(Position = 1, HelpMessage = "Specify the TCP port to connect to")]
        [ValidateRange(1, 65535)]
        public UInt16 Port { get; set; } = 4411;

        [Parameter(Position = 3, HelpMessage = "Specify to NOT establish an SSL Connection")]
        SwitchParameter NoSSL { get; set; }

        [Parameter(Position = 4, HelpMessage = "Specify the Asigra DSClient API Version to use")]
        string APIVersion { get; set; } = "13.0.0.0";

        [Parameter(Position = 5, HelpMessage = "Specify Credentials to use to connect to DSClient")]
        [ValidateNotNullOrEmpty]
        public PSCredential Credential { get; set; }

        protected override void ProcessRecord()
        {
            string user = Credential.UserName;
            string pwd = Credential.GetNetworkCredential().Password;

            WriteVerbose("Checking for existing DSClient Sessions...");
            ClientConnection previousSession = SessionState.PSVariable.GetValue("DSClientSession", null) as ClientConnection;

            if (previousSession != null)
            {
                WriteVerbose("Previous DSClient Session found, attempting to dispose...");
                try
                {
                    previousSession.logout();
                    previousSession.Dispose();
                }
                catch
                {
                    WriteVerbose("Previous session failed to dispose, removing session...");                    
                }
                SessionState.PSVariable.Remove("DSClientSession");
                WriteObject("DSClient Session removed.");
            }

            WriteVerbose("Establishing a new DSClient Session...");
            ClientConnection DSClientSession = ConnectSession(Host, Port, NoSSL, APIVersion, user, pwd);

            SessionState.PSVariable.Set("DSClientSession", DSClientSession);

            WriteObject("DSClient Session Established.");
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