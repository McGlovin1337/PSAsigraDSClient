using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientVMwareVADPRestore")]

    public class StartDSClientVMwareVADPRestore : BaseDSClientBackupSetRestore
    {
        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Select Items by ItemId")]
        public long[] ItemId { get; set; }

        [Parameter(HelpMessage = "Destination Datacenter to Restore to")]
        public string Datacenter { get; set; }

        [Parameter(HelpMessage = "Destination Datastore to Restore to")]
        public string Datastore { get; set; }

        [Parameter(HelpMessage = "Destination Folder to Restore to")]
        public string Folder { get; set; }

        [Parameter(HelpMessage = "Destination Host to Restore to")]
        public string Hostname { get; set; }

        [Parameter(HelpMessage = "Rename the VM")]
        public string VMName { get; set; }

        [Parameter(HelpMessage = "Power-On VM After Restore")]
        public SwitchParameter PowerOn { get; set; }

        [Parameter(HelpMessage = "Add Timestamp to VM Name")]
        public SwitchParameter TimeStampVMName { get; set; }

        [Parameter(HelpMessage = "Unregister VM After Restore")]
        public SwitchParameter Unregister { get; set; }

        [Parameter(HelpMessage = "Use SAN Restore Method")]
        public SwitchParameter SANRestore { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Check for a Backup Set Restore View stored in SessionState
            WriteVerbose("Performing Action: Check for DS-Client Restore View Session");
            BackupSetRestoreView restoreSession = SessionState.PSVariable.GetValue("RestoreView", null) as BackupSetRestoreView;

            if (restoreSession == null)
                throw new Exception("There is no Backup Set Restore View Session, use Initialize-DSClientBackupSetRestore Cmdlet to create a Restore Session");

            // Check for Data Type for the Restore stored in SessionState
            EBackupDataType dataType = (EBackupDataType)SessionState.PSVariable.GetValue("RestoreType", EBackupDataType.EBackupDataType__UNDEFINED);

            if (dataType != EBackupDataType.EBackupDataType__VMwareVADP)
                throw new Exception("Incorrect Data Type for Restore");

            // Get the selected items
            List<long> selectedItems = new List<long>();

            if (Items != null)
            {
                foreach (string item in Items)
                {
                    try
                    {
                        selectedItems.Add(restoreSession.getItem(item).id);
                    }
                    catch
                    {
                        WriteWarning("Unable to select item: " + item);
                        continue;
                    }
                }
            }

            if (ItemId != null)
                selectedItems.AddRange(ItemId);

            RestoreActivityInitiator restoreActivityInitiator = restoreSession.prepareRestore(selectedItems.ToArray());

            DataSourceBrowser dataSourceBrowser = DSClientSession.createBrowser(dataType);

            // Resolve the supplied Computer Name
            string computer = dataSourceBrowser.expandToFullPath(Computer);
            computer = dataSourceBrowser.expandToFullPath(computer);
            WriteVerbose("Notice: Specified Computer resolved to: " + computer);

            // Set the Destination Computer Credentials
            BackupSetCredentials backupSetCredentials = dataSourceBrowser.neededCredentials(computer);

            Win32FS_Generic_BackupSetCredentials win32FSBSCredentials = Win32FS_Generic_BackupSetCredentials.from(dataSourceBrowser.neededCredentials(computer));

            if (Credential != null)
            {
                string user = Credential.UserName;
                string pass = Credential.GetNetworkCredential().Password;
                win32FSBSCredentials.setCredentials(user, pass);
            }
            else
            {
                WriteVerbose("Notice: Credentials not specified, using DS-Client Credentials");
                win32FSBSCredentials.setUsingClientCredentials(true);
            }
            dataSourceBrowser.setCurrentCredentials(win32FSBSCredentials);
            win32FSBSCredentials.Dispose();

            // Get Selected Shares
            selected_shares[] selectedShares = restoreActivityInitiator.selected_shares();

            //selectedShares.ToList().ForEach(WriteObject);



            //// Empty share mapping
            share_mapping[] shareMappings = new share_mapping[0];

            // Set Restore Configuration
            restoreActivityInitiator.setRestoreReason(StringToERestoreReason(RestoreReason));
            restoreActivityInitiator.setRestoreClassification(StringToERestoreClassification(RestoreClassification));
            restoreActivityInitiator.setDetailLogReporting(UseDetailedLog);
            restoreActivityInitiator.setLocalStorageHandling(StringToERestoreLocalStorageHandling(LocalStorageMethod));

            VMwareVADP_RestoreActivityInitiator vadpRestoreActivityInitiator = VMwareVADP_RestoreActivityInitiator.from(restoreActivityInitiator);
            vmware_options[] options = vadpRestoreActivityInitiator.getRestoreOptions("RDG3");
            options.ToList().ForEach(WriteObject);

            vadpRestoreActivityInitiator.Dispose();
            restoreActivityInitiator.Dispose();
            dataSourceBrowser.Dispose();
        }
    }
}