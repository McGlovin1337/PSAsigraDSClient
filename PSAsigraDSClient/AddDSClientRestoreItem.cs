using System;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientRestoreItem")]
    [OutputType(typeof(void))]

    sealed public class AddDSClientRestoreItem : DSClientCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Restore Session to Select Items for")]
        public int RestoreId { get; set; }

        [Parameter(HelpMessage = "Specify Items for Restore by ItemId")]
        public long[] ItemId { get; set; }

        [Parameter(HelpMessage = "Specify Items for Restore by Name")]
        public string[] Item { get; set; }

        protected override void DSClientProcessRecord()
        {
            DSClientRestoreSession restoreSession = DSClientSessionInfo.GetRestoreSession(RestoreId);

            if (restoreSession != null)
            {
                // Process ItemId's, these should already exist in the sessions browsed items list
                if (ItemId != null && ItemId.Length > 0)
                    restoreSession.AddSelectedItems(ItemId);

                // Attempt to Find and Add Items by Name
                if (Item != null && Item.Length > 0)
                {
                    BackedUpDataView backedUpDataView = restoreSession.GetRestoreView();

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
                            ErrorRecord errorRecord = new ErrorRecord(
                                new ItemNotFoundException($"Failed to Select Item: {item}"),
                                "ItemNotFoundException",
                                ErrorCategory.ObjectNotFound,
                                item);
                            WriteError(errorRecord);
                        }

                        // If no item was found, on to the next
                        if (selectableItem == null)
                            continue;

                        WriteVerbose($"Performing Action: Add '{item}' to Restore Session '{RestoreId}'");
                        restoreSession.AddBrowsedItem(new DSClientBackupSetItemInfo(item, selectableItem, backedUpDataView.getItemSize(selectableItem.id)));
                        restoreSession.AddSelectedItem(selectableItem.id);
                    }
                }
            }
            else
            {
                throw new Exception("Specified Restore Session Not Found");
            }
        }
    }
}