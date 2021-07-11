using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientRestoreItem")]

    public class AddDSClientRestoreItem : DSClientCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Restore Session to Select Items for")]
        public int RestoreId { get; set; }

        [Parameter(HelpMessage = "Specify Items for Restore by ItemId")]
        public long[] ItemId { get; set; }

        [Parameter(HelpMessage = "Specify Items for Restore by Name")]
        public string[] Item { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Retrieve the Restore Session from SessionState

            if (!(SessionState.PSVariable.GetValue("RestoreSessions", null) is List<DSClientRestoreSession> restoreSessions))
                throw new Exception("No Restore Sessions Found");

            bool found = false;
            for (int i = 0; i < restoreSessions.Count; i++)
            {
                if (restoreSessions[i].RestoreId == RestoreId)
                {
                    DSClientRestoreSession restoreSession = restoreSessions[i];

                    // Process ItemId's, these should already exist in the sessions browsed items list
                    if (ItemId.Length > 0)
                        restoreSession.AddSelectedItems(ItemId);

                    found = true;
                }
            }

            if (!found)
                throw new Exception("Specified Restore Session Not Found");
        }
    }
}