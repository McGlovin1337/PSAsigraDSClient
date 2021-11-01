using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientQuota")]
    [OutputType(typeof(DSClientQuota))]

    sealed public class GetDSClientQuota: DSClientCmdlet
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
            public DSClientStorageUnit Used { get; private set; }
            public DSClientStorageUnit Limit { get; private set; }
            public decimal PercentUsed { get; private set; }
            public int GracePercentage { get; private set; }

            public DSClientQuota(EQuotaOn assigned, quota_information quotaInfo)
            {
                QuotaType = EnumToString(assigned);
                QuotaMeasure = EnumToString(quotaInfo.type);
                Used = new DSClientStorageUnit(quotaInfo.current_use);
                Limit = new DSClientStorageUnit(quotaInfo.use_limit);
                PercentUsed = (quotaInfo.use_limit > 0) ? decimal.Round(((decimal)quotaInfo.current_use / (decimal)quotaInfo.use_limit) * 100, 2) : 0;
                GracePercentage = quotaInfo.grace_percentage;
            }
        }
    }
}