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

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Account Number")]
        [ValidateNotNullOrEmpty]
        public string AccountNumber { get; set; }

        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Customer Name")]
        [ValidateNotNullOrEmpty]
        public string CustomerName { get; set; }

        [Parameter(Position = 3, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Client Number")]
        [ValidateNotNullOrEmpty]
        public string ClientNumber { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Account Key")]
        [ValidateNotNullOrEmpty]
        public string AccountKey { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Encryption for Account Key")]
        [ValidateSet("DES", "AES128", "AES192", "AES256", "AES128IV", "AES192IV", "AES256IV")]
        public string AccountEncryption { get; set; } = "AES128";

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Private Key")]
        [ValidateNotNullOrEmpty]
        public string PrivateKey { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Encryption for Private Key")]
        [ValidateSet("DES", "AES128", "AES192", "AES256", "AES128IV", "AES192IV", "AES256IV")]
        public string PrivateKeyEncryption { get; set; }

        [Parameter(HelpMessage = "Specify if to Forward Encryption Keys to DS-System")]
        public SwitchParameter EscrowKeys { get; set; }

        [Parameter(HelpMessage = "Set the ISO 3166 Country Code")]
        public int CountryCode { get; set; }

        [Parameter(HelpMessage = "Specify the Industry Vertical of the End User Organisation")]
        [ValidateSet("ManufacturingAndEnergy", "RetailAndWholesaleTrade", "UtilitiesAndTelecommunications", "ComputersHardwareSoftware", "BusinessServicesAndConstruction", "MediaEntertainmentAndLeisure", "FinancialServicesAndInsurance", "PublicSectorAndHealthcare")]
        public string Industry { get; set; }

        [Parameter(HelpMessage = "Specify the Number of Employees of the End User Organisation")]
        public int Employees { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Retrieve the current Registration Info
            ClientConfiguration DSClientConfgMgr = DSClientSession.getConfigurationManager();

            dsc_reg_info regInfo = DSClientConfgMgr.getDSCRegistrationInfo();

            // Set the Registration Info
            if (AccountKey != null)
                regInfo.account_key = AccountKey;

            if (AccountEncryption != null)
                regInfo.account_key_enc = StringToEEncryptionType(AccountEncryption);

            if (AccountNumber != null)
                regInfo.account_num = AccountNumber;

            if (ClientNumber != null)
                regInfo.client_num = ClientNumber;

            if (CustomerName != null)
                regInfo.customer_name = CustomerName;

            if (DsSystemAddress != null)
                regInfo.dssys_ip_addr = DsSystemAddress;

            if (MyInvocation.BoundParameters.ContainsKey("EscrowKeys"))
                regInfo.escrow_enc_key = EscrowKeys;

            if (PrivateKey != null)
                regInfo.private_key = PrivateKey;

            if (PrivateKeyEncryption != null)
                regInfo.private_key_enc = StringToEEncryptionType(PrivateKeyEncryption);

            // Retrieve the current 'End User' Info
            UserInfoConfiguration DSClientUserInfoConfig = DSClientConfgMgr.getUserInfoConfiguration();

            user_info userInfo = DSClientUserInfoConfig.getUserInfo();

            // Set the 'End User' Info
            if (MyInvocation.BoundParameters.ContainsKey("CountryCode"))
                userInfo.country_code = CountryCode;

            if (Industry != null)
                userInfo.industry_code = StringToEIndustryVertical(Industry);

            if (MyInvocation.BoundParameters.ContainsKey("Employees"))
                userInfo.num_of_employees = IntToENumberOfEmployees(Employees);

            WriteVerbose("Setting DS-Client Registration...");
            DSClientConfgMgr.setDSCRegistrationInfo(regInfo);
            DSClientUserInfoConfig.setUserInfo(userInfo);

            DSClientUserInfoConfig.Dispose();
            DSClientConfgMgr.Dispose();
        }

        private static ENumberOfEmployees IntToENumberOfEmployees(int employees)
        {
            ENumberOfEmployees Employees = ENumberOfEmployees.ENumberOfEmployees__UNDEFINED;

            if (employees < 51)
                Employees = ENumberOfEmployees.ENumberOfEmployees__Size1;
            else if (employees > 50 && employees < 201)
                Employees = ENumberOfEmployees.ENumberOfEmployees__Size2;
            else if (employees > 200 && employees < 501)
                Employees = ENumberOfEmployees.ENumberOfEmployees__Size3;
            else if (employees > 500 && employees < 1001)
                Employees = ENumberOfEmployees.ENumberOfEmployees__Size4;
            else if (employees > 1000 && employees < 5001)
                Employees = ENumberOfEmployees.ENumberOfEmployees__Size5;
            else if (employees > 5000 && employees < 10001)
                Employees = ENumberOfEmployees.ENumberOfEmployees__Size6;
            else if (employees > 10000)
                Employees = ENumberOfEmployees.ENumberOfEmployees__Size7;

            return Employees;
        }

        private static EIndustryVertical StringToEIndustryVertical(string industry)
        {
            switch(industry.ToLower())
            {
                case "manufacturingandenergy":
                    return EIndustryVertical.EIndustryVertical__ManufacturingAndEnergy;
                case "retailandwholesaletrade":
                    return EIndustryVertical.EIndustryVertical__RetailAndWholesaleTrade;
                case "utilitiesandtelecommunications":
                    return EIndustryVertical.EIndustryVertical__UtilitiesAndTelecommunications;
                case "computershardwaresoftware":
                    return EIndustryVertical.EIndustryVertical__ComputersHardwareSoftware;
                case "businessservicesandconstruction":
                    return EIndustryVertical.EIndustryVertical__BusinessServicesAndConstruction;
                case "mediaentertainmentandleisure":
                    return EIndustryVertical.EIndustryVertical__MediaEntertainmentAndLeisure;
                case "financialservicesandinsurance":
                    return EIndustryVertical.EIndustryVertical__FinancialServicesAndInsurance;
                case "publicsectorandhealthcare":
                    return EIndustryVertical.EIndustryVertical__PublicSectorAndHealthcare;
                default:
                    return EIndustryVertical.EIndustryVertical__UNDEFINED;
            }
        }

        private static EEncryptionType StringToEEncryptionType(string encryptionType)
        {
            switch(encryptionType.ToLower())
            {
                case "des":
                    return EEncryptionType.EEncryptionType__DES;
                case "aes128":
                    return EEncryptionType.EEncryptionType__AES128;
                case "aes192":
                    return EEncryptionType.EEncryptionType__AES192;
                case "aes256":
                    return EEncryptionType.EEncryptionType__AES256;
                case "aes128iv":
                    return EEncryptionType.EEncryptionType__AES128_IV;
                case "aes192iv":
                    return EEncryptionType.EEncryptionType__AES192_IV;
                case "aes256iv":
                    return EEncryptionType.EEncryptionType__AES256_IV;
                default:
                    return EEncryptionType.EEncryptionType__UNDEFINED;
            }
        }
    }
}