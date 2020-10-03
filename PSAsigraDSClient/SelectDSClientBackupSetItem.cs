using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Select, "DSClientBackupSetItem")]

    public class SelectDSClientBackupSetItem: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Selected Items")]
        [ValidateNotNullOrEmpty]
        [Alias("Path")]
        public string[] Item { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "DeleteSession", HelpMessage = "Specify to select from existing Delete View in SessionState")]
        public SwitchParameter DeleteSession { get; set; }

        /* Not yet in use
        [Parameter(Mandatory = true, ParameterSetName = "RestoreSession", HelpMessage = "Specify to select from existing Restore View in SessionState")]
        public SwitchParameter RestoreSession { get; set; }
        */

        [Parameter(Mandatory = true, ParameterSetName = "ValidateSession", HelpMessage = "Specify to select from existing Validation View in SessionState")]
        public SwitchParameter ValidateSession { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Check for previous Selected Items stored in Session State
            WriteVerbose("Checking for previous Selected Items Session...");
            BackedUpDataView selectedItemSession = SessionState.PSVariable.GetValue("SelectedItems", null) as BackedUpDataView;

            // If a previous session is found, remove it
            if (selectedItemSession != null)
            {
                WriteVerbose("Previous Selected Items Session found, attempting to Dispose...");
                try
                {
                    selectedItemSession.Dispose();
                }
                catch
                {
                    WriteVerbose("Previous Session failed to Dispose, deleting session...");
                }
                SessionState.PSVariable.Remove("SelectedItems");
            }

            BackedUpDataView backedUpDataView = null;

            if (DeleteSession)
            {
                // Get Delete View stored in SessionState
                backedUpDataView = SessionState.PSVariable.GetValue("DeleteView", null) as BackedUpDataView;

                if (backedUpDataView == null)
                    throw new Exception("There is no Delete View stored in SessionState, use Initialize-DSClientBackupSetDelete Cmdlet");
            }
            else if (ValidateSession)
            {
                // Get the Validation View stored in SessionState
                backedUpDataView = SessionState.PSVariable.GetValue("ValidateView", null) as BackedUpDataView;

                if (backedUpDataView == null)
                    throw new Exception("There is no Validation View stored in SessionState, use Initialize-DSClientBackupSetValidation Cmdlet");
            }
            else
            {
                throw new Exception("There is no BackedUp Data View");
            }

            // Setup a new List for holding the selected ItemIds
            List<long> ItemIds = new List<long>();

            WriteVerbose("Retrieving specified items...");
            foreach (string item in Item)
            {
                try
                {
                    ItemIds.Add(backedUpDataView.getItem(item).id);
                }
                catch
                {
                    WriteWarning("Failed to select item: " + item);
                }
            }

            WriteVerbose("Number of selected items: " + ItemIds.Count());

            // Store the selected Items in SessionState
            WriteVerbose("Storing selected items into SessionState...");
            SessionState.PSVariable.Set("SelectedItems", ItemIds);
        }
    }
}