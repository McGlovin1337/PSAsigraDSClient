using System;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientBackupSetValidation")]
    [CmdletBinding(DefaultParameterSetName = "Selective")]

    public class StartDSClientBackupSetValidation: DSClientCmdlet
    {
        [Parameter(Position = 0, ParameterSetName = "Selective", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the items to validate")]
        [ValidateNotNullOrEmpty]
        public string[] Path { get; set; }

        [Parameter(Position = 1, ParameterSetName = "Selective", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the items to validate by ItemId")]
        public long[] ItemId { get; set; }

        [Parameter(ParameterSetName = "Full", HelpMessage = "Specify to Validate All Backup Set Data")]
        public SwitchParameter FullValidation { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Check for a Backup Set Validation View stored in Session State
            WriteVerbose("Checking for DS-Client Validation View Session...");
            BackupSetValidationView validationSession = SessionState.PSVariable.GetValue("ValidateView", null) as BackupSetValidationView;

            if (validationSession == null)
                throw new Exception("There is no Backup Set Validation View Session, use Initialize-DSClientBackupSetValidation Cmdlet to create a Validation Session");

            List<long> selectedItems = new List<long>();

            if (!FullValidation)
            {
                // Get the ItemId for specified Path items
                if (Path != null)
                {
                    foreach (string item in Path)
                    {
                        try
                        {
                            SelectableItem itemInfo = validationSession.getItem(item);
                            selectedItems.Add(itemInfo.id);
                        }
                        catch
                        {
                            WriteWarning("Unable to select item: " + item);
                            continue;
                        }
                    }
                }

                if (MyInvocation.BoundParameters.ContainsKey("ItemId"))
                    selectedItems.AddRange(ItemId);
            }

            ValidationActivityInitiator validationActivityInitiator = validationSession.prepareValidation(selectedItems.ToArray());
            GenericActivity validationActivity;

            if (!FullValidation)
                validationActivity = validationActivityInitiator.start(true);
            else
                validationActivity = validationActivityInitiator.start(false);

            WriteObject("Started Backup Set Validation Activity with ActivityId " + validationActivity.getID());

            validationSession.Dispose();
        }
    }
}