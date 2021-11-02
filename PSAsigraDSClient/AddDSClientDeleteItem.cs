using System;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientDeleteItem")]
    [OutputType(typeof(void))]

    sealed public class AddDSClientDeleteItem : DSClientCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Delete Session to Select Items for")]
        public int DeleteId { get; set; }

        [Parameter(HelpMessage = "Specify Items for Deletion by ItemId")]
        public long[] ItemId { get; set; }

        [Parameter(HelpMessage = "Specify Items for Deletion by Name")]
        public string[] Item { get; set; }

        protected override void DSClientProcessRecord()
        {
            DSClientDeleteSession deleteSession = DSClientSessionInfo.GetDeleteSession(DeleteId);

            if (deleteSession != null)
            {
                // Process ItemId's, these should already exist in the sessions browsed items list
                if (ItemId != null && ItemId.Length > 0)
                    deleteSession.AddSelectedItems(ItemId);

                // Attempt to Find and Add Items by Name
                if (Item != null && Item.Length > 0)
                {
                    BackedUpDataView backedUpDataView = deleteSession.GetDeleteView();

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

                        WriteVerbose($"Performing Action: Add '{item}' to Restore Session '{DeleteId}'");
                        deleteSession.AddBrowsedItem(new DSClientBackupSetItemInfo(item, selectableItem, backedUpDataView.getItemSize(selectableItem.id)));
                        deleteSession.AddSelectedItem(selectableItem.id);
                    }
                }
            }
            else
            {
                throw new Exception("Delete Session not found");
            }
        }
    }
}