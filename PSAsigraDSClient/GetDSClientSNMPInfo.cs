using System.Collections.Generic;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientSNMPInfo")]
    [OutputType(typeof(DSClientSNMPInfo))]

    public class GetDSClientSNMPInfo: BaseDSClientSNMPConfig
    {
        protected override void ProcessSNMPConfig(DSClientSNMPInfo dSClientSNMPInfo, IEnumerable<DSClientSNMPCommunities> dSClientSNMPCommunities)
        {
            WriteObject(dSClientSNMPInfo);
        }
    }
}