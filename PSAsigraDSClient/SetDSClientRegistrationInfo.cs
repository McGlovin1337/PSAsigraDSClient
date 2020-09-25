using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientRegistrationInfo")]

    public class SetDSClientRegistrationInfo: DSClientCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the DS-System Address")]
        [ValidateNotNullOrEmpty]
        public string DsSystemAddress { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Account Number")]
        [ValidateNotNullOrEmpty]
        public string AccountNumber { get; set; }

        [Parameter(Position = 2, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Customer Name")]
        [ValidateNotNullOrEmpty]
        public string CustomerName { get; set; }

        [Parameter(Position = 3, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Client Number")]
        [ValidateNotNullOrEmpty]
        public string ClientNumber { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Account Key")]
        [ValidateNotNullOrEmpty]
        public string AccountKey { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Encryption for Account Key")]
        [ValidateSet("DES", "AES128", "AES192", "AES256", "AES128IV", "AES192IV", "AES256IV")]
        public string AccountEncryption { get; set; } = "AES128";

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Private Key")]
        [ValidateNotNullOrEmpty]
        public string PrivateKey { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Encryption for Private Key")]
        [ValidateSet("DES", "AES128", "AES192", "AES256", "AES128IV", "AES192IV", "AES256IV")]
        public string PrivateKeyEncryption { get; set; }

        [Parameter(HelpMessage = "Specify if to Forward Encryption Keys to DS-System")]
        public SwitchParameter EscrowKeys { get; set; }

        protected override void DSClientProcessRecord()
        {
            dsc_reg_info RegInfo = new dsc_reg_info
            {
                account_key = AccountKey,
                account_key_enc = StringToEEncryptionType(AccountEncryption),
                account_num = AccountNumber,
                client_num = ClientNumber,
                customer_name = CustomerName,
                dssys_ip_addr = DsSystemAddress,
                escrow_enc_key = EscrowKeys,
                private_key = PrivateKey,
                private_key_enc = StringToEEncryptionType(PrivateKeyEncryption)
            };

            WriteVerbose("Setting DS-Client Registration...");
            ClientConfiguration DSClientConfgMgr = DSClientSession.getConfigurationManager();

            DSClientConfgMgr.setDSCRegistrationInfo(RegInfo);

            DSClientConfgMgr.Dispose();
        }

        private static EEncryptionType StringToEEncryptionType(string encryptionType)
        {
            EEncryptionType EncryptionType = EEncryptionType.EEncryptionType__UNDEFINED;

            switch(encryptionType)
            {
                case "DES":
                    EncryptionType = EEncryptionType.EEncryptionType__DES;
                    break;
                case "AES128":
                    EncryptionType = EEncryptionType.EEncryptionType__AES128;
                    break;
                case "AES192":
                    EncryptionType = EEncryptionType.EEncryptionType__AES192;
                    break;
                case "AES256":
                    EncryptionType = EEncryptionType.EEncryptionType__AES256;
                    break;
                case "AES128IV":
                    EncryptionType = EEncryptionType.EEncryptionType__AES128_IV;
                    break;
                case "AES192IV":
                    EncryptionType = EEncryptionType.EEncryptionType__AES192_IV;
                    break;
                case "AES256IV":
                    EncryptionType = EEncryptionType.EEncryptionType__AES256_IV;
                    break;
            }

            return EncryptionType;
        }
    }
}