using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientNotification")]
    [OutputType(typeof(DSClientNotification))]

    sealed public class GetDSClientNotification: BaseDSClientNotification
    {
        protected override void ProcessNotification(DSClientNotification dSClientNotification)
        {
            WriteObject(dSClientNotification);
        }
    }
}