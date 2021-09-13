using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsData.Unpublish, "DSClientEncryptionKeys", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class UnpublishDSClientEncryptionKeys: DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            if (ShouldProcess("DS-System", "Remove DS-Client Encryption Keys"))
            {
                WriteVerbose("Performing Action: Set Encryption Key Forwarding to False");
                DSClientConfigMgr.setForwardingEncryptionKey(false);

                WriteVerbose("Performing Action: Remove Encryption Keys from DS-System");
                DSClientConfigMgr.deleteEncryptionKeyFromDSSystem();
            }

            DSClientConfigMgr.Dispose();
        }
    }
}