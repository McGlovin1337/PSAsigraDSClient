using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientDeleteItem", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class RemoveDSClientDeleteItem : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Delete Session to remove item from")]
        public int DeleteId { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Item(s) to remove by Id")]
        public long[] ItemId { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Performing Action: Retrieve Delete Session");
            DSClientDeleteSession deleteSession = DSClientSessionInfo.GetDeleteSession(DeleteId);

            if (deleteSession != null)
                if (ShouldProcess($"Delete Session with Id '{DeleteId}'", "Remove Items for Deletion"))
                    deleteSession.RemoveSelectedItems(ItemId);
        }
    }
}