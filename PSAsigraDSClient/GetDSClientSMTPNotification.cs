using AsigraDSClientApi;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientSMTPNotification")]
    [OutputType(typeof(DSClientSMTPConfig))]

    public class GetDSClientSMTPNotification: BaseDSClientSMTPNotification
    {
        protected override void ProcessSMTPConfig(smtp_email_notification_info smtpInfo)
        {
            DSClientSMTPConfig smtpConfig = new DSClientSMTPConfig(smtpInfo);

            WriteObject(smtpConfig);
        }
    }
}