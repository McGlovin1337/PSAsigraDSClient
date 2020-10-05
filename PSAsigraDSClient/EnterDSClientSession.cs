using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

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
        public SwitchParameter NoSSL { get; set; }

        [Parameter(Position = 4, HelpMessage = "Specify the Asigra DSClient API Version to use")]
        public string APIVersion { get; set; } = "13.0.0.0";

        [Parameter(Position = 5, HelpMessage = "Specify Credentials to use to connect to DSClient")]
        [ValidateNotNullOrEmpty]
        public PSCredential Credential { get; set; }

        protected override void ProcessRecord()
        {
            string user = Credential.UserName;
            string pwd = Credential.GetNetworkCredential().Password;

            // Remove any previous DataType from SessionState and store the current Backup Set DataType
            SessionState.PSVariable.Remove("RestoreType");

            // Check for a previous Backup Set Restore View stored in Session State
            WriteVerbose("Checking for previous DS-Client Validation View Sessions...");
            BackupSetRestoreView previousRestoreSession = SessionState.PSVariable.GetValue("RestoreView", null) as BackupSetRestoreView;

            // If a previous session is found, remove it
            if (previousRestoreSession != null)
            {
                WriteVerbose("Previous Restore View found, attempting to Dispose...");
                try
                {
                    previousRestoreSession.Dispose();
                }
                catch
                {
                    WriteVerbose("Previous Session failed to Dispose, deleting session...");
                }
                SessionState.PSVariable.Remove("RestoreView");
            }

            // Check for a previous Backup Set Validation View stored in Session State
            WriteVerbose("Checking for previous DS-Client Delete View Sessions...");
            BackupSetDeleteView previousDeleteSession = SessionState.PSVariable.GetValue("DeleteView", null) as BackupSetDeleteView;

            // If a previous session is found, remove it
            if (previousDeleteSession != null)
            {
                WriteVerbose("Previous Delete View found, attempting to Dispose...");
                try
                {
                    previousDeleteSession.Dispose();
                }
                catch
                {
                    WriteVerbose("Previous Session failed to Dispose, deleting session...");
                }
                SessionState.PSVariable.Remove("DeleteView");
            }

            // Check for a previous Backup Set Validation View stored in Session State
            WriteVerbose("Checking for previous DS-Client Validation View Sessions...");
            BackupSetValidationView previousValidationSession = SessionState.PSVariable.GetValue("ValidateView", null) as BackupSetValidationView;

            // If a previous session is found, remove it
            if (previousValidationSession != null)
            {
                WriteVerbose("Previous Validation View found, attempting to Dispose...");
                try
                {
                    previousValidationSession.Dispose();
                }
                catch
                {
                    WriteVerbose("Previous Session failed to Dispose, deleting session...");
                }
                SessionState.PSVariable.Remove("ValidateView");
            }

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
                SessionState.PSVariable.Remove("DSClientOSType");
                WriteObject("DSClient Session removed.");
            }

            WriteVerbose("Establishing a new DSClient Session...");
            ClientConnection DSClientSession = ConnectSession(Host, Port, NoSSL, APIVersion, user, pwd);

            SessionState.PSVariable.Set("DSClientSession", DSClientSession);

            // Get and store the DSClient Operating System Type into SessionState
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();
            EOSFlavour DSClientOSFlavour = DSClientConfigMgr.getClientOSType();
            DSClientOSType DSClientOSType = new DSClientOSType(DSClientOSFlavour);
            SessionState.PSVariable.Set("DSClientOSType", DSClientOSType);

            DSClientConfigMgr.Dispose();

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