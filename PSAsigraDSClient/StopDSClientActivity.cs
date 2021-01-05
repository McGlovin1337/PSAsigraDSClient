using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Stop, "DSClientActivity")]

    public class StopDSClientActivity: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The Id of the Activity to Stop")]
        [ValidateNotNullOrEmpty]
        public int ActivityId { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose($"Performing Action: Retrieve Activity Details for ActivityId: {ActivityId}");
            GenericActivity GenericActivity = DSClientSession.activity(ActivityId);

            // Send Stop request to the activity
            WriteObject("Sending stop request to Activity...");
            GenericActivity.Stop();

            GenericActivity.Dispose();
        }
    }
}