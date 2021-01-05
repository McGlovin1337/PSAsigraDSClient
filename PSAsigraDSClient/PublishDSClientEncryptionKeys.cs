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

            WriteVerbose("Performing Action: Set Encryption Key Forwarding to True");
            DSClientConfigMgr.setForwardingEncryptionKey(true);

            WriteVerbose("Performing Action: Forward DS-Client Encryption Keys to DS-System");
            DSClientConfigMgr.forwardEncryptionKeyToDSSystem();

            DSClientConfigMgr.Dispose();
        }
    }
}