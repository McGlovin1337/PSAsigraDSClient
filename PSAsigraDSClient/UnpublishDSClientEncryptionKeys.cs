using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsData.Unpublish, "DSClientEncryptionKeys")]

    public class UnpublishDSClientEncryptionKeys: DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            WriteVerbose("Setting Encryption Key Forwarding to False...");
            DSClientConfigMgr.setForwardingEncryptionKey(false);

            WriteVerbose("Removing Encryption Keys from DS-System...");
            DSClientConfigMgr.deleteEncryptionKeyFromDSSystem();

            DSClientConfigMgr.Dispose();
        }
    }
}