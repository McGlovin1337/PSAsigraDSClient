using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientRegistrationInfo")]
    [OutputType(typeof(DSClientRegistrationInfo))]

    sealed public class GetDSClientRegistrationInfo: DSClientCmdlet
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
                AccountKeyEncryption = EnumToString(regInfo.account_key_enc);
                PrivateKey = regInfo.private_key;
                PrivateKeyEncryption = EnumToString(regInfo.private_key_enc);
                EscrowKeys = regInfo.escrow_enc_key;
                CountryCode = userInfo.country_code;
                Industry = EnumToString(userInfo.industry_code);
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
        }
    }
}