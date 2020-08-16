using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientInfo")]
    [OutputType(typeof(ds_client_info))]
    public class GetDSClientInfo: DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            WriteObject(DSClientSession.getDSClientInfo());
        }
    }
}