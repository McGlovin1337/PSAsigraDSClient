using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientRegistrationInfo")]
    [OutputType(typeof(DSClientRegistrationInfo))]
    public class GetDSClientRegistrationInfo: DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfig = DSClientSession.getConfigurationManager();

            DSClientRegistrationInfo DSCregInfo = new DSClientRegistrationInfo(DSClientConfig.getDSCRegistrationInfo());

            WriteObject(DSCregInfo);

            DSClientConfig.Dispose();
        }

        private class DSClientRegistrationInfo
        {
            public string AccountNumber { get; set; }
            public string CustomerName { get; set; }
            public string ClientNumber { get; set; }
            public string DsSystemAddress { get; set; }
            public string AccountKey { get; set; }
            public string AccountKeyEncryption { get; set; }
            public string PrivateKey { get; set; }
            public string PrivateKeyEncryption { get; set; }
            public bool EscrowKeys { get; set; }

            public DSClientRegistrationInfo(dsc_reg_info regInfo)
            {
                AccountNumber = regInfo.account_num;
                CustomerName = regInfo.customer_name;
                ClientNumber = regInfo.client_num;
                DsSystemAddress = regInfo.dssys_ip_addr;
                AccountKey = regInfo.account_key;
                AccountKeyEncryption = EEncryptionTypeToString(regInfo.account_key_enc);
                PrivateKey = regInfo.private_key;
                PrivateKeyEncryption = EEncryptionTypeToString(regInfo.private_key_enc);
                EscrowKeys = regInfo.escrow_enc_key;
            }

            private static string EEncryptionTypeToString(EEncryptionType encryptionType)
            {
                string EncryptionType = null;

                switch(encryptionType)
                {
                    case EEncryptionType.EEncryptionType__NONE:
                        EncryptionType = "None";
                        break;
                    case EEncryptionType.EEncryptionType__DES:
                        EncryptionType = "DES";
                        break;
                    case EEncryptionType.EEncryptionType__AES128:
                        EncryptionType = "AES128";
                        break;
                    case EEncryptionType.EEncryptionType__AES192:
                        EncryptionType = "AES192";
                        break;
                    case EEncryptionType.EEncryptionType__AES256:
                        EncryptionType = "AES256";
                        break;
                    case EEncryptionType.EEncryptionType__AES128_IV:
                        EncryptionType = "AES128IV";
                        break;
                    case EEncryptionType.EEncryptionType__AES192_IV:
                        EncryptionType = "AES192IV";
                        break;
                    case EEncryptionType.EEncryptionType__AES256_IV:
                        EncryptionType = "AES256IV";
                        break;
                }

                return EncryptionType;
            }
        }
    }
}