using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsData.Publish, "DSClientEncryptionKeys")]

    public class PublishDSClientEncryptionKeys: DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            WriteVerbose("Setting Encryption Key Forwarding to True...");
            DSClientConfigMgr.setForwardingEncryptionKey(true);

            WriteVerbose("Forwarding DS-Client Encryption Keys to DS-System...");
            DSClientConfigMgr.forwardEncryptionKeyToDSSystem();

            DSClientConfigMgr.Dispose();
        }
    }
}