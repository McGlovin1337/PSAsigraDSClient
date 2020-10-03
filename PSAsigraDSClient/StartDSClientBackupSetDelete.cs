using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientBackupSetDelete")]

    public class StartDSClientBackupSetDelete: DSClientCmdlet
    {
        [Parameter(HelpMessage = "Specify to Move Data to BLM rather than Delete")]
        public SwitchParameter MoveToBLM { get; set; }

        [Parameter(HelpMessage = "Specify BLM Label")]
        [ValidateNotNullOrEmpty]
        public string BLMLabel { get; set; }

        [Parameter(HelpMessage = "Specify to Create a new BLM Archive Package")]
        public SwitchParameter NewPackage { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Parameter Validation
            if (MoveToBLM && BLMLabel == null)
                throw new Exception("BLMLabel must be specified when Moving to BLM selected");

            // Get the Delete View for SessionState
            BackupSetDeleteView backupSetDeleteView = SessionState.PSVariable.GetValue("DeleteView", null) as BackupSetDeleteView;

            if (backupSetDeleteView == null)
                throw new Exception("There is no Delete View stored in SessionState, use Initialize-DSClientBackupSetDelete Cmdlet");

            // Get the Selected Items from SessionState
            IEnumerable<long> ItemIds = SessionState.PSVariable.GetValue("SelectedItems", null) as IEnumerable<long>;

            if (ItemIds == null)
                throw new Exception("There are no Selected Items stored in SessionState, use Select-DSClientBackupSetItem Cmdlet");

            DeleteActivityInitiator deleteActivityInitiator = backupSetDeleteView.prepareDelete(ItemIds.ToArray());

            if (MoveToBLM)
                deleteActivityInitiator.enableBLMMove(BLMLabel, NewPackage);
            else
                deleteActivityInitiator.disableBLMMove();

            GenericActivity deleteActivity = deleteActivityInitiator.start();

            WriteObject("Started Backup Set Item Delete with ActivityId " + deleteActivity.getID());

            deleteActivity.Dispose();
            deleteActivityInitiator.Dispose();
            backupSetDeleteView.Dispose();

            SessionState.PSVariable.Remove("SelectedItems");
            SessionState.PSVariable.Remove("DeleteView");
        }
    }
}