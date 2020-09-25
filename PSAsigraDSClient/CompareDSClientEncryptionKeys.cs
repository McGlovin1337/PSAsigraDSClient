using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsData.Compare, "DSClientEncryptionKeys")]

    public class CompareDSClientEncryptionKeys: DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            dsc_reg_info regInfo = DSClientConfigMgr.getDSCRegistrationInfo();

            WriteVerbose("Validating DS-Client Encryption Keys with DS-System...");
            bool KeyValidation = DSClientConfigMgr.validateEncryptionKeys(regInfo);

            WriteObject(KeyValidation);

            DSClientConfigMgr.Dispose();
        }
    }
}