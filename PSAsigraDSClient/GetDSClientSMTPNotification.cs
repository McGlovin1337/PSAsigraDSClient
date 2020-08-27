using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientSMTPNotification")]
    [OutputType(typeof(DSClientSMTPConfig))]

    public class GetDSClientSMTPNotification: BaseDSClientSMTPNotification
    {
        protected override void ProcessSMTPConfig(DSClientSMTPConfig dSClientSMTPConfig)
        {
            WriteObject(dSClientSMTPConfig);
        }
    }
}