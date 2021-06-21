using System;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientSession")]
    [OutputType(typeof(DSClientSession))]

    public class NewDSClientSession : BaseDSClientSession
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

        protected override void ProcessRecord()
        {
            WriteVerbose("Performing Action: Establish DS-Client Session");
            DSClientSession session = new DSClientSession(Host, Port, !NoSSL, APIVersion, Credential);

            WriteObject(session);
        }
    }
}