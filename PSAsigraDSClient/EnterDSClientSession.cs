﻿using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Enter, "DSClientSession")]
    public class EnterDSClientSession: BaseDSClientSessionCleanup
    {
        [Parameter(Position = 0, HelpMessage = "Specify the DS-Client Host to connect to")]
        [ValidateNotNullOrEmpty]
        public new string Host { get; set; }

        [Parameter(Position = 1, HelpMessage = "Specify the TCP port to connect to")]
        [ValidateRange(1, 65535)]
        public UInt16 Port { get; set; } = 4411;

        [Parameter(Position = 3, HelpMessage = "Specify to NOT establish an SSL Connection")]
        public SwitchParameter NoSSL { get; set; }

        [Parameter(Position = 4, HelpMessage = "Specify the Asigra DS-Client API Version to use")]
        public string APIVersion { get; set; } = "13.0.0.0";

        [Parameter(Position = 5, HelpMessage = "Specify Credentials to use to connect to DSClient")]
        [ValidateNotNullOrEmpty]
        public PSCredential Credential { get; set; }

        protected override void ProcessCleanSession()
        {
            string user = Credential.UserName;
            string pwd = Credential.GetNetworkCredential().Password;

            WriteVerbose("Performing Action: Establish DS-Client Session");
            ClientConnection DSClientSession = ConnectSession(Host, Port, NoSSL, APIVersion, user, pwd);

            SessionState.PSVariable.Set("DSClientSession", DSClientSession);

            // Get and store the DSClient Operating System Type into SessionState
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();
            EOSFlavour DSClientOSFlavour = DSClientConfigMgr.getClientOSType();
            DSClientOSType DSClientOSType = new DSClientOSType(DSClientOSFlavour);
            SessionState.PSVariable.Set("DSClientOSType", DSClientOSType);

            DSClientConfigMgr.Dispose();

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