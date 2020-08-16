using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientRegistrationInfo")]
    [OutputType(typeof(dsc_reg_info))]
    public class GetDSClientRegistrationInfo: DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfig = DSClientSession.getConfigurationManager();

            dsc_reg_info DSCregInfo = DSClientConfig.getDSCRegistrationInfo();

            WriteObject(DSCregInfo);

            DSClientConfig.Dispose();
        }
    }
}