using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientRegistrationInfo", SupportsShouldProcess = true)]

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
        [ValidateSet("DES", "AES128", "AES192", "AES256", "AES128_IV", "AES192_IV", "AES256_IV")]
        public string AccountEncryption { get; set; } = "AES128";

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Private Key")]
        [ValidateNotNullOrEmpty]
        public string PrivateKey { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Encryption for Private Key")]
        [ValidateSet("DES", "AES128", "AES192", "AES256", "AES128_IV", "AES192_IV", "AES256_IV")]
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
                if (ShouldProcess("Customer Account Key", $"Set Value '{AccountKey}'"))
                    regInfo.account_key = AccountKey;

            if (AccountEncryption != null)
                if (ShouldProcess("Customer Account Key Encryption", $"Set Value '{AccountEncryption}'"))
                    regInfo.account_key_enc = StringToEnum<EEncryptionType>(AccountEncryption);

            if (AccountNumber != null)
                if (ShouldProcess("Customer Account Number", $"Set Value '{AccountNumber}'"))
                    regInfo.account_num = AccountNumber;

            if (ClientNumber != null)
                if (ShouldProcess("DS-Client Number", $"Set Value '{ClientNumber}'"))
                    regInfo.client_num = ClientNumber;

            if (CustomerName != null)
                if (ShouldProcess("Customer Name", $"Set Value '{CustomerName}'"))
                    regInfo.customer_name = CustomerName;

            if (DsSystemAddress != null)
                if (ShouldProcess("DS-System Address", $"Set Value '{DsSystemAddress}'"))
                    regInfo.dssys_ip_addr = DsSystemAddress;

            if (MyInvocation.BoundParameters.ContainsKey("EscrowKeys"))
                if (ShouldProcess("Encryption Key Forwarding", $"Set Value '{EscrowKeys}'"))
                    regInfo.escrow_enc_key = EscrowKeys;

            if (PrivateKey != null)
                if (ShouldProcess("DS-Client Private Key", $"Set Value '{PrivateKey}'"))
                    regInfo.private_key = PrivateKey;

            if (PrivateKeyEncryption != null)
                if (ShouldProcess("DS-Client Private Key Encryption", $"Set Value '{PrivateKeyEncryption}'"))
                    regInfo.private_key_enc = StringToEnum<EEncryptionType>(PrivateKeyEncryption);

            // Retrieve the current 'End User' Info
            UserInfoConfiguration DSClientUserInfoConfig = DSClientConfgMgr.getUserInfoConfiguration();

            user_info userInfo = DSClientUserInfoConfig.getUserInfo();

            // Set the 'End User' Info
            if (MyInvocation.BoundParameters.ContainsKey("CountryCode"))
                if (ShouldProcess("Customer Country Code", $"Set Value '{CountryCode}'"))
                    userInfo.country_code = CountryCode;

            if (Industry != null)
                if (ShouldProcess("Customer Industry Vertical", $"Set Value '{Industry}'"))
                    userInfo.industry_code = StringToEnum<EIndustryVertical>(Industry);

            if (MyInvocation.BoundParameters.ContainsKey("Employees"))
                if (ShouldProcess("Customer Number of Employees", $"Set Value '{Employees}'"))
                    userInfo.num_of_employees = IntToENumberOfEmployees(Employees);

            if (ShouldProcess("DS-Client", "Update DS-Client Registration"))
            {
                WriteVerbose("Setting DS-Client Registration...");
                DSClientConfgMgr.setDSCRegistrationInfo(regInfo);
                DSClientUserInfoConfig.setUserInfo(userInfo);
            }

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
    }
}