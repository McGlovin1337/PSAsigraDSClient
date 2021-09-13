using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Exit, "DSClientSession")]
    [OutputType(typeof(string))]

    sealed public class ExitDSClientSession: BaseDSClientSessionCleanup
    {
        protected override void ProcessCleanSession()
        {
            // Nothing required here, Base Class Completes all actions
        }
    }
}