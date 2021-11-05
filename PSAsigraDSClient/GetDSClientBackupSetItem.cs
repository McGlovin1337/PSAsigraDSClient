using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientBackupSetItem")]
    [OutputType(typeof(DSClientBackupSetItem))]

    sealed public class GetDSClientBackupSetItem: BaseDSClientBackupSet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set to query")]
        public int BackupSetId { get; set; }

        protected override void DSClientProcessRecord()
        {
            BackupSet backupSet = null;

            // Get the specified Backup Set
            try
            {
                WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId: {BackupSetId}");
                backupSet = DSClientSession.backup_set(BackupSetId);
            }
            catch (APIException e)
            {
                ErrorRecord errorRecord = new ErrorRecord(
                    e,
                    "APIException",
                    ErrorCategory.ObjectNotFound,
                    null);
                WriteError(errorRecord);
            }

            if (backupSet != null)
            {
                // Get the Data Type this Backup Set contains
                EBackupDataType backupDataType = backupSet.getDataType();

                // Get the Backup Set Items specified in the Backup Set
                BackupSetItem[] backupSetItems = backupSet.items();

                List<DSClientBackupSetItem> dSClientBackupSetItems = new List<DSClientBackupSetItem>();

                foreach (BackupSetItem item in backupSetItems)
                    dSClientBackupSetItems.Add(new DSClientBackupSetItem(item, backupDataType, DSClientSessionInfo.OperatingSystem));

                dSClientBackupSetItems.ForEach(WriteObject);

                backupSet.Dispose();
            }
        }
    }
}