using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsData.Initialize, "DSClientBackupSetValidation")]

    public class InitializeDSClientBackupSetValidation: BaseDSClientInitializeBackupSetDataBrowser
    {
        [Parameter(HelpMessage = "Specify to Hide Files Deleted from Source")]
        public SwitchParameter HideDeleted { get; set; }

        [Parameter(HelpMessage = "Specify to Only Show Deleted files from Source")]
        public SwitchParameter ShowOnlyDeleted { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId: {BackupSetId}");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            WriteVerbose("Performing Action: Create Backup Set Validation View");
            WriteVerbose($"Notice: View Start Date: {DateFrom}");
            WriteVerbose($"Notice: View End Date: {DateTo}");
            BackupSetValidationView backupSetValidationView = backupSet.prepare_validation(DateTimeToUnixEpoch(DateFrom), DateTimeToUnixEpoch(DateTo), 0);

            // Set the Deleted Items View
            if (HideDeleted == true)
                backupSetValidationView.setDeletedFileFilterType(EDeleteFilterType.EDeleteFilterType__HideDeleted, DateTimeToUnixEpoch(DeletedDate));
            else if (ShowOnlyDeleted == true)
                backupSetValidationView.setDeletedFileFilterType(EDeleteFilterType.EDeleteFilterType__ShowDeletedOnly, DateTimeToUnixEpoch(DeletedDate));
            else
                backupSetValidationView.setDeletedFileFilterType(EDeleteFilterType.EDeleteFilterType__ShowAll, 0);

            // Check for a previous Backup Set Validation View stored in Session State
            WriteVerbose("Performing Action: Check for previous DS-Client Validation View Sessions");
            BackupSetValidationView previousValidationSession = SessionState.PSVariable.GetValue("ValidateView", null) as BackupSetValidationView;

            // If a previous session is found, remove it
            if (previousValidationSession != null)
            {
                WriteVerbose("Notice: Previous Validation View found");
                try
                {
                    WriteVerbose("Performing Action: Dispose Validation View");
                    previousValidationSession.Dispose();
                }
                catch
                {
                    WriteVerbose("Notice: Previous Session failed to Dispose");
                }
                WriteVerbose("Performing Action: Remove on Validation View Session");
                SessionState.PSVariable.Remove("ValidateView");
            }

            // Add new Validation View to SessionState
            WriteVerbose("Performing Action: Store Backup Set Validation View into SessionState");
            SessionState.PSVariable.Set("ValidateView", backupSetValidationView);

            backupSet.Dispose();
        }
    }
}