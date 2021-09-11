using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientConfiguration")]
    [OutputType(typeof(DSClientConfiguration))]

    sealed public class GetDSClientConfiguration: BaseDSClientConfiguration
    {
        protected override void ProcessConfiguration(ClientConfiguration clientConfiguration)
        {
            DSClientConfiguration dSClientConfiguration = new DSClientConfiguration(clientConfiguration);

            WriteObject(dSClientConfiguration);
        }
    }
}