using System;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientValidationItem")]
    [OutputType(typeof(void))]

    sealed public class AddDSClientValidationItem : DSClientCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Validation Session to Select Items for")]
        public int ValidationId { get; set; }

        [Parameter(HelpMessage = "Specify Items for Restore by ItemId")]
        public long[] ItemId { get; set; }

        [Parameter(HelpMessage = "Specify Items for Restore by Name")]
        public string[] Item { get; set; }

        protected override void DSClientProcessRecord()
        {
            DSClientValidationSession validationSession = DSClientSessionInfo.GetValidationSession(ValidationId);

            if (validationSession != null)
            {
                // Process ItemId's, these should already exist in the sessions browsed items list
                if (ItemId != null && ItemId.Length > 0)
                    validationSession.AddSelectedItems(ItemId);

                // Attempt to Find and Add Items by Name
                if (Item != null && Item.Length > 0)
                {
                    BackedUpDataView backedUpDataView = validationSession.GetValidationView();

                    foreach (string item in Item)
                    {
                        SelectableItem selectableItem = null;
                        try
                        {
                            WriteVerbose($"Performing Action: Retrieve Item Info for '{item}'");
                            selectableItem = backedUpDataView.getItem(item);
                        }
                        catch
                        {
                            WriteWarning($"Failed to Select Item: {item}");
                        }

                        // If no item was found, on to the next
                        if (selectableItem == null)
                            continue;

                        WriteVerbose($"Performing Action: Add '{item}' to Validation Session '{ValidationId}'");
                        validationSession.AddBrowsedItem(new DSClientBackupSetItemInfo(item, selectableItem, backedUpDataView.getItemSize(selectableItem.id)));
                        validationSession.AddSelectedItem(selectableItem.id);
                    }
                }
            }
            else
            {
                throw new Exception("Specified Validation Session Not Found");
            }
        }
    }
}