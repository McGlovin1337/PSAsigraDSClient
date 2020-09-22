using System;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Register, "DSClient")]

    public class RegisterDSClient: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Initial", HelpMessage = "Perform Initial DS-Client Registration")]
        public SwitchParameter Initial { get; set; }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "ReRegister", HelpMessage = "Re-register DS-Client following Hardware Change")]
        public SwitchParameter ReRegister { get; set; }

        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            if (Initial == true)
            {
                WriteVerbose("Performing Initial DS-Client Registration with DS-System...");
                DSClientConfigMgr.init_register();
                WriteObject("DS-Client Initial Registration Complete");
            }
            else if (ReRegister == true)
            {
                WriteVerbose("Performing Re-registration of DS-Client with DS-System...");
                DSClientConfigMgr.re_register();
                WriteObject("DS-Client Re-registration Complete");
            }
            else
            {
                DSClientConfigMgr.Dispose();
                throw new Exception("Initial or ReRegister Parameter must be true");
            }

            DSClientConfigMgr.Dispose();
        }
    }
}