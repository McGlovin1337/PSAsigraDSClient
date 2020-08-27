using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientNotification")]
    [OutputType(typeof(DSClientNotification))]

    public class GetDSClientNotification: BaseDSClientNotification
    {
        protected override void ProcessNotification(DSClientNotification dSClientNotification)
        {
            WriteObject(dSClientNotification);
        }
    }
}