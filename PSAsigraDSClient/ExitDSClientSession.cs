using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Exit, "DSClientSession")]
    public class ExitDSClientSession: BaseDSClientSessionCleanup
    {
        protected override void ProcessCleanSession()
        {
            // Nothing required here, Base Class Completes all actions
        }
    }
}