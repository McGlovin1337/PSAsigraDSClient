using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Stop, "DSClientActivity", SupportsShouldProcess = true)]

    public class StopDSClientActivity: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The Id of the Activity to Stop")]
        [ValidateNotNullOrEmpty]
        public int ActivityId { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose($"Performing Action: Retrieve Activity Details for ActivityId: {ActivityId}");
            GenericActivity genericActivity = DSClientSession.activity(ActivityId);

            if (ShouldProcess($"Activity '{genericActivity.getID()}'", "Stop Running Activity"))
            {
                // Send Stop request to the activity
                WriteObject("Sending stop request to Activity...");
                genericActivity.Stop();
            }

            genericActivity.Dispose();
        }
    }
}