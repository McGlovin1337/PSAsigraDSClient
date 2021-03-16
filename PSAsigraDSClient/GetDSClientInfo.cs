using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientInfo")]
    [OutputType(typeof(DSClientInfo))]
    public class GetDSClientInfo: DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            ds_client_info[] ds_Client_Info = DSClientSession.getDSClientInfo();
            List<DSClientInfo> dsClientInfo = new List<DSClientInfo>();
            foreach (ds_client_info client_info in ds_Client_Info)
                dsClientInfo.Add(new DSClientInfo(client_info));

            dsClientInfo.ForEach(WriteObject);
        }

        private class DSClientInfo
        {
            public string Name { get; private set; }
            public string Value { get; private set; }

            public DSClientInfo(ds_client_info dsclientInfo)
            {
                Name = dsclientInfo.name;
                Value = dsclientInfo.value;
            }
        }
    }
}