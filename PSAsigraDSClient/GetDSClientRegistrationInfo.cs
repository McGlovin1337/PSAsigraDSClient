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

            dsc_reg_info dscRegInfo = DSClientConfig.getDSCRegistrationInfo();

            UserInfoConfiguration DSClientUserInfoConfig = DSClientConfig.getUserInfoConfiguration();

            user_info userInfo = DSClientUserInfoConfig.getUserInfo();

            DSClientRegistrationInfo DSCregInfo = new DSClientRegistrationInfo(dscRegInfo, userInfo);

            WriteObject(DSCregInfo);

            DSClientUserInfoConfig.Dispose();
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
            public int CountryCode { get; set; }
            public string Industry { get; set; }
            public string Employees { get; set; }

            public DSClientRegistrationInfo(dsc_reg_info regInfo, user_info userInfo)
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
                CountryCode = userInfo.country_code;
                Industry = EIndustryVerticalToString(userInfo.industry_code);
                Employees = ENumberOfEmployeesToString(userInfo.num_of_employees);
            }

            private static string ENumberOfEmployeesToString(ENumberOfEmployees employees)
            {
                string Employees = null;

                switch(employees)
                {
                    case ENumberOfEmployees.ENumberOfEmployees__Size1:
                        Employees = "1-50";
                        break;
                    case ENumberOfEmployees.ENumberOfEmployees__Size2:
                        Employees = "51-200";
                        break;
                    case ENumberOfEmployees.ENumberOfEmployees__Size3:
                        Employees = "201-500";
                        break;
                    case ENumberOfEmployees.ENumberOfEmployees__Size4:
                        Employees = "501-1000";
                        break;
                    case ENumberOfEmployees.ENumberOfEmployees__Size5:
                        Employees = "1001-5000";
                        break;
                    case ENumberOfEmployees.ENumberOfEmployees__Size6:
                        Employees = "5001-10000";
                        break;
                    case ENumberOfEmployees.ENumberOfEmployees__Size7:
                        Employees = "10001+";
                        break;
                }

                return Employees;
            }

            private static string EIndustryVerticalToString(EIndustryVertical industry)
            {
                string Industry = null;

                switch(industry)
                {
                    case EIndustryVertical.EIndustryVertical__ManufacturingAndEnergy:
                        Industry = "ManufacturingAndEnergy";
                        break;
                    case EIndustryVertical.EIndustryVertical__RetailAndWholesaleTrade:
                        Industry = "RetailAndWholesaleTrade";
                        break;
                    case EIndustryVertical.EIndustryVertical__UtilitiesAndTelecommunications:
                        Industry = "UtilitiesAndTelecommunications";
                        break;
                    case EIndustryVertical.EIndustryVertical__ComputersHardwareSoftware:
                        Industry = "ComputersHardwareSoftware";
                        break;
                    case EIndustryVertical.EIndustryVertical__BusinessServicesAndConstruction:
                        Industry = "BusinessServicesAndConstruction";
                        break;
                    case EIndustryVertical.EIndustryVertical__MediaEntertainmentAndLeisure:
                        Industry = "MediaEntertainmentAndLeisure";
                        break;
                    case EIndustryVertical.EIndustryVertical__FinancialServicesAndInsurance:
                        Industry = "FinancialServicesAndInsurance";
                        break;
                    case EIndustryVertical.EIndustryVertical__PublicSectorAndHealthcare:
                        Industry = "PublicSectorAndHealthcare";
                        break;
                }

                return Industry;
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