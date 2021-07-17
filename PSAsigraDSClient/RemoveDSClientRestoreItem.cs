using System;
using System.Collections.Generic;
using System.Linq;
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
            if (!(SessionState.PSVariable.GetValue("RestoreSessions", null) is List<DSClientRestoreSession> restoreSessions))
                throw new Exception("No Restore Sessions Found");

            bool found = false;
            for (int i = 0; i < restoreSessions.Count(); i++)
            {
                if (restoreSessions[i].RestoreId == RestoreId)
                {
                    DSClientRestoreSession restoreSession = restoreSessions[i];

                    if(ShouldProcess($"Restore Session with Id '{RestoreId}'", "Remove Items for Restore"))
                        restoreSession.RemoveSelectedItems(ItemId);

                    found = true;
                    break;
                }
            }

            if (!found)
                throw new Exception("Restore Session Not Found");
        }
    }
}