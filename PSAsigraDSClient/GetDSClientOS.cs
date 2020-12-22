using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient.PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientOS")]
    [OutputType(typeof(DSClientOperatingSystem))]

    public class GetDSClientOS: DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            WriteVerbose("Retrieving DS-Client Operating System Type...");
            EOSFlavour osType = DSClientConfigMgr.getClientOSType();

            DSClientOperatingSystem dSclientOperatingSystem = new DSClientOperatingSystem(osType);

            WriteObject(dSclientOperatingSystem);

            DSClientConfigMgr.Dispose();
        }

        private class DSClientOperatingSystem
        {
            public string OperatingSystem { get; private set; }

            public DSClientOperatingSystem(EOSFlavour osType)
            {
                OperatingSystem = osType.ToString()
                                        .Split('_')
                                        .Last();
            }
        }
    }
}