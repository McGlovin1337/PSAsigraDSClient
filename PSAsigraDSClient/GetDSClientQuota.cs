﻿using System.Collections.Generic;
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

            WriteVerbose("Performing Action: Retreive Quota Information");
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
                switch(quotaOn)
                {
                    case EQuotaOn.EQuotaOn__OnlineClient:
                        return "DSClientOnlineQuota";
                    case EQuotaOn.EQuotaOn__OnlineCustomer:
                        return "CustomerOnlineQuota";
                    case EQuotaOn.EQuotaOn__LocalOnly:
                        return "LocalOnlyQuota";
                    default:
                        return null;
                }
            }

            private string EQuotaTypeToString(EQuotaType quotaType)
            {
                switch(quotaType)
                {
                    case EQuotaType.EQuotaType__None:
                        return "None";
                    case EQuotaType.EQuotaType__Protected:
                        return "ProtectedSize";
                    case EQuotaType.EQuotaType__Stored:
                        return "StoredSize";
                    case EQuotaType.EQuotaType__Native:
                        return "NativeSize";
                    default:
                        return null;
                }
            }
        }
    }
}