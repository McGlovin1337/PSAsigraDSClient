using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientRestoreItem", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    public class RemoveDSClientRestoreItem : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Restore Session to remove item from")]
        public int RestoreId { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Item(s) to remove by Id")]
        public long[] ItemId { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Performing Action: Retrieve Restore Sessions");
            DSClientRestoreSession restoreSession = DSClientSessionInfo.GetRestoreSession(RestoreId);

            if (restoreSession != null)
                if (ShouldProcess($"Restore Session with Id '{RestoreId}'", "Remove Items for Restore"))
                    restoreSession.RemoveSelectedItems(ItemId);
        }
    }
}