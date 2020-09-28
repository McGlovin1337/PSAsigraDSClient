using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsData.Initialize, "DSClientBackupSetValidation")]

    public class InitializeDSClientBackupSetValidation: BaseDSClientInitializeBackupSetDataBrowser
    {
        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Retrieving Backup Set from DS-Client...");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            WriteVerbose("Creating a new Backup Set Validation View...");
            WriteVerbose("View Start Date: " + DateFrom);
            WriteVerbose("View End Date: " + DateTo);
            BackupSetValidationView backupSetValidationView = backupSet.prepare_validation(DateTimeToUnixEpoch(DateFrom), DateTimeToUnixEpoch(DateTo), 0);

            if (HideDeleted == true)
                backupSetValidationView.setDeletedFileFilterType(EDeleteFilterType.EDeleteFilterType__HideDeleted, DateTimeToUnixEpoch(DeletedDate));
            else if (ShowOnlyDeleted == true)
                backupSetValidationView.setDeletedFileFilterType(EDeleteFilterType.EDeleteFilterType__ShowDeletedOnly, DateTimeToUnixEpoch(DeletedDate));
            else
                backupSetValidationView.setDeletedFileFilterType(EDeleteFilterType.EDeleteFilterType__ShowAll, 0);

            // Check for a previous Backup Set Validation View stored in Session State
            WriteVerbose("Checking for previous DS-Client Validation View Sessions...");
            BackupSetValidationView previousValidationSession = SessionState.PSVariable.GetValue("ValidateView", null) as BackupSetValidationView;

            // If a previous session is found, remove it
            if (previousValidationSession != null)
            {
                WriteVerbose("Previous Validation View found, attempting to Dispose...");
                try
                {
                    previousValidationSession.Dispose();
                }
                catch
                {
                    WriteVerbose("Previous Session failed to Dispose, deleting session...");
                }
                SessionState.PSVariable.Remove("ValidateView");
            }

            WriteVerbose("Storing new Backup Set Validation View into SessionState...");
            SessionState.PSVariable.Set("ValidateView", backupSetValidationView);

            backupSet.Dispose();
        }
    }
}