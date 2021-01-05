using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsData.Initialize, "DSClientBackupSetRestore")]

    public class InitializeDSClientBackupSetRestore: BaseDSClientInitializeBackupSetDataBrowser
    {
        [Parameter(HelpMessage = "Specify to Hide Files Deleted from Source")]
        public SwitchParameter HideDeleted { get; set; }

        [Parameter(HelpMessage = "Specify to Only Show Deleted files from Source")]
        public SwitchParameter ShowOnlyDeleted { get; set; }

        [Parameter(HelpMessage = "For VSS Backup Sets, select if this will be a Component Level Restore")]
        public SwitchParameter VssComponentRestore { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId: {BackupSetId}");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            WriteVerbose("Performing Action: Create Backup Set Restore View");
            WriteVerbose($"Notice: View Start Date: {DateFrom}");
            WriteVerbose($"Notice: View End Date: {DateTo}");
            BackupSetRestoreView backupSetRestoreView = backupSet.prepare_restore(DateTimeToUnixEpoch(DateFrom), DateTimeToUnixEpoch(DateTo), 0);

            // Set the Deleted Items View
            if (HideDeleted == true)
                backupSetRestoreView.setDeletedFileFilterType(EDeleteFilterType.EDeleteFilterType__HideDeleted, DateTimeToUnixEpoch(DeletedDate));
            else if (ShowOnlyDeleted == true)
                backupSetRestoreView.setDeletedFileFilterType(EDeleteFilterType.EDeleteFilterType__ShowDeletedOnly, DateTimeToUnixEpoch(DeletedDate));
            else
                backupSetRestoreView.setDeletedFileFilterType(EDeleteFilterType.EDeleteFilterType__ShowAll, 0);

            // Set the View Type based on the Backup Set Data Type and specified Parameter
            EBackupDataType dataType = backupSet.getDataType();
            if (dataType == EBackupDataType.EBackupDataType__ClusteredHyperV ||
                dataType == EBackupDataType.EBackupDataType__HyperV ||
                dataType == EBackupDataType.EBackupDataType__VSSSQLServer ||
                dataType == EBackupDataType.EBackupDataType__VSSExchange ||
                dataType == EBackupDataType.EBackupDataType__VSSSharePoint)
            {
                if (VssComponentRestore)
                    backupSetRestoreView.setViewType(ERestoreViewType.ERestoreViewType__Component);
                else
                    backupSetRestoreView.setViewType(ERestoreViewType.ERestoreViewType__File);
            }
            else
            {
                backupSetRestoreView.setViewType(ERestoreViewType.ERestoreViewType__Default);
            }

            // Check for a previous Backup Set Restore View stored in Session State
            WriteVerbose("Performing Action: Check for previous DS-Client Restore View Sessions");
            BackupSetRestoreView previousRestoreSession = SessionState.PSVariable.GetValue("RestoreView", null) as BackupSetRestoreView;

            // If a previous session is found, remove it
            if (previousRestoreSession != null)
            {
                WriteVerbose("Notice: Previous Restore View found");
                try
                {
                    WriteVerbose("Performing Action: Dispose Restore View");
                    previousRestoreSession.Dispose();
                }
                catch
                {
                    WriteVerbose("Previous Session failed to Dispose");
                }
                WriteVerbose("Performing Action: Remove on Restore View Session");
                SessionState.PSVariable.Remove("RestoreView");
            }

            // Remove any previous DataType from SessionState and store the current Backup Set DataType
            SessionState.PSVariable.Remove("RestoreType");
            SessionState.PSVariable.Set("RestoreType", dataType);

            // Add new Restore View to SessionState
            WriteVerbose("Performing Action: Store Backup Set Restore View into SessionState");
            SessionState.PSVariable.Set("RestoreView", backupSetRestoreView);

            backupSet.Dispose();
        }
    }
}