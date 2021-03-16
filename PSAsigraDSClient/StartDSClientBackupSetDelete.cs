using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientBackupSetDelete")]
    [OutputType(typeof(GenericBackupSetActivity))]

    public class StartDSClientBackupSetDelete: BaseDSClientBackupSetDelResVal
    {
        [Parameter(Mandatory = true, ParameterSetName = "BLM", HelpMessage = "Specify to Move Data to BLM rather than Delete")]
        [Parameter(ParameterSetName = "Selective")]
        public SwitchParameter MoveToBLM { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "BLM", HelpMessage = "Specify BLM Label")]
        [Parameter(ParameterSetName = "Selective")]
        [ValidateNotNullOrEmpty]
        public string BLMLabel { get; set; }

        [Parameter(HelpMessage = "Specify to Create a new BLM Archive Package")]
        [Parameter(ParameterSetName = "Selective")]
        public SwitchParameter NewPackage { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Parameter Validation
            if (MoveToBLM && BLMLabel == null)
                throw new Exception("BLMLabel must be specified when Moving to BLM selected");

            // Get the Delete View from SessionState
            BackupSetDeleteView backupSetDeleteView = SessionState.PSVariable.GetValue("DeleteView", null) as BackupSetDeleteView;

            if (backupSetDeleteView == null)
                throw new Exception("There is no Delete View stored in SessionState, use Initialize-DSClientBackupSetDelete Cmdlet");

            List<long> selectedItems = new List<long>();

            if (Items != null)
            {
                foreach (string item in Items)
                {
                    try
                    {
                        selectedItems.Add(backupSetDeleteView.getItem(item).id);
                    }
                    catch
                    {
                        WriteWarning("Unable to select item: " + item);
                        continue;
                    }
                }
            }

            if (ItemId.Count() > 0)
                selectedItems.AddRange(ItemId);

            DeleteActivityInitiator deleteActivityInitiator = backupSetDeleteView.prepareDelete(selectedItems.ToArray());

            if (MoveToBLM)
                deleteActivityInitiator.enableBLMMove(BLMLabel, NewPackage);
            else
                deleteActivityInitiator.disableBLMMove();

            GenericActivity deleteActivity = deleteActivityInitiator.start();

            if (PassThru)
                WriteObject(new GenericBackupSetActivity(deleteActivity));

            deleteActivity.Dispose();
            deleteActivityInitiator.Dispose();
            backupSetDeleteView.Dispose();

            SessionState.PSVariable.Remove("DeleteView");
        }
    }
}