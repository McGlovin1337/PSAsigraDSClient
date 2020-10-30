using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientQuota")]
    [OutputType(typeof(DSClientQuota))]

    public class GetDSClientQuota: DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            List<DSClientQuota> DSClientQuotas = new List<DSClientQuota>();

            WriteVerbose("Retreiving Quota Information...");
            for (int i = 0; i < (int)EQuotaOn.EQuotaOn__UNDEFINED; i++)
            {
                quota_information quotaInfo = DSClientConfigMgr.getQuota((EQuotaOn)i);
                DSClientQuota dSClientQuota = new DSClientQuota((EQuotaOn)i, quotaInfo);
                DSClientQuotas.Add(dSClientQuota);
            }

            DSClientQuotas.ForEach(WriteObject);

            DSClientConfigMgr.Dispose();
        }

        private class DSClientQuota
        {
            public string QuotaType { get; private set; }
            public string QuotaMeasure { get; private set; }
            public long Used { get; private set; }
            public long Limit { get; private set; }
            public decimal PercentUsed { get; private set; }
            public int GracePercentage { get; private set; }

            public DSClientQuota(EQuotaOn assigned, quota_information quotaInfo)
            {
                QuotaType = EQuotaOnToString(assigned);
                QuotaMeasure = EQuotaTypeToString(quotaInfo.type);
                Used = quotaInfo.current_use;
                Limit = quotaInfo.use_limit;
                PercentUsed = (quotaInfo.use_limit > 0) ? decimal.Round(((decimal)quotaInfo.current_use / (decimal)quotaInfo.use_limit) * 100, 2) : 0;
                GracePercentage = quotaInfo.grace_percentage;
            }

            private string EQuotaOnToString(EQuotaOn quotaOn)
            {
                string QuotaOn = null;

                switch(quotaOn)
                {
                    case EQuotaOn.EQuotaOn__OnlineClient:
                        QuotaOn = "DSClientOnlineQuota";
                        break;
                    case EQuotaOn.EQuotaOn__OnlineCustomer:
                        QuotaOn = "CustomerOnlineQuota";
                        break;
                    case EQuotaOn.EQuotaOn__LocalOnly:
                        QuotaOn = "LocalOnlyQuota";
                        break;
                }

                return QuotaOn;
            }

            private string EQuotaTypeToString(EQuotaType quotaType)
            {
                string QuotaType = null;

                switch(quotaType)
                {
                    case EQuotaType.EQuotaType__None:
                        QuotaType = "None";
                        break;
                    case EQuotaType.EQuotaType__Protected:
                        QuotaType = "ProtectedSize";
                        break;
                    case EQuotaType.EQuotaType__Stored:
                        QuotaType = "StoredSize";
                        break;
                    case EQuotaType.EQuotaType__Native:
                        QuotaType = "NativeSize";
                        break;
                }

                return QuotaType;
            }
        }
    }
}