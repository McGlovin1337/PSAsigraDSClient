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
            public string AccountNumber { get; private set; }
            public string CustomerName { get; private set; }
            public string ClientNumber { get; private set; }
            public string DsSystemAddress { get; private set; }
            public string AccountKey { get; private set; }
            public string AccountKeyEncryption { get; private set; }
            public string PrivateKey { get; private set; }
            public string PrivateKeyEncryption { get; private set; }
            public bool EscrowKeys { get; private set; }
            public int CountryCode { get; private set; }
            public string Industry { get; private set; }
            public string Employees { get; private set; }

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
                switch(employees)
                {
                    case ENumberOfEmployees.ENumberOfEmployees__Size1:
                        return "1-50";
                    case ENumberOfEmployees.ENumberOfEmployees__Size2:
                        return "51-200";
                    case ENumberOfEmployees.ENumberOfEmployees__Size3:
                        return "201-500";
                    case ENumberOfEmployees.ENumberOfEmployees__Size4:
                        return "501-1000";
                    case ENumberOfEmployees.ENumberOfEmployees__Size5:
                        return "1001-5000";
                    case ENumberOfEmployees.ENumberOfEmployees__Size6:
                        return "5001-10000";
                    case ENumberOfEmployees.ENumberOfEmployees__Size7:
                        return "10001+";
                    default:
                        return null;
                }
            }

            private static string EIndustryVerticalToString(EIndustryVertical industry)
            {
                switch(industry)
                {
                    case EIndustryVertical.EIndustryVertical__ManufacturingAndEnergy:
                        return "ManufacturingAndEnergy";
                    case EIndustryVertical.EIndustryVertical__RetailAndWholesaleTrade:
                        return "RetailAndWholesaleTrade";
                    case EIndustryVertical.EIndustryVertical__UtilitiesAndTelecommunications:
                        return "UtilitiesAndTelecommunications";
                    case EIndustryVertical.EIndustryVertical__ComputersHardwareSoftware:
                        return "ComputersHardwareSoftware";
                    case EIndustryVertical.EIndustryVertical__BusinessServicesAndConstruction:
                        return "BusinessServicesAndConstruction";
                    case EIndustryVertical.EIndustryVertical__MediaEntertainmentAndLeisure:
                        return "MediaEntertainmentAndLeisure";
                    case EIndustryVertical.EIndustryVertical__FinancialServicesAndInsurance:
                        return "FinancialServicesAndInsurance";
                    case EIndustryVertical.EIndustryVertical__PublicSectorAndHealthcare:
                        return "PublicSectorAndHealthcare";
                    default:
                        return null;
                }
            }

            private static string EEncryptionTypeToString(EEncryptionType encryptionType)
            {
                switch(encryptionType)
                {
                    case EEncryptionType.EEncryptionType__NONE:
                        return "None";
                    case EEncryptionType.EEncryptionType__DES:
                        return "DES";
                    case EEncryptionType.EEncryptionType__AES128:
                        return "AES128";
                    case EEncryptionType.EEncryptionType__AES192:
                        return "AES192";
                    case EEncryptionType.EEncryptionType__AES256:
                        return "AES256";
                    case EEncryptionType.EEncryptionType__AES128_IV:
                        return "AES128IV";
                    case EEncryptionType.EEncryptionType__AES192_IV:
                        return "AES192IV";
                    case EEncryptionType.EEncryptionType__AES256_IV:
                        return "AES256IV";
                    default:
                        return null;
                }
            }
        }
    }
}