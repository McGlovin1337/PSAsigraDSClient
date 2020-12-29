using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsData.Initialize, "DSClientBackupSetDelete")]

    public class InitializeDSClientBackupSetDelete: BaseDSClientInitializeBackupSetDataBrowser
    {
        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the number of Generations to keep online")]
        public int KeepGenerations { get; set; }

        [Parameter(ParameterSetName = "Exclude", HelpMessage = "Specify to Exclude Archive Placeholders in Delete")]
        public SwitchParameter ExcludeArchivePlaceholder { get; set; }

        [Parameter(ParameterSetName = "Only", HelpMessage = "Specify to Only Delete Archive Placeholders")]
        public SwitchParameter OnlyArchivePlaceholder { get; set; }

        protected override void DSClientProcessRecord()
        {
            DeleteArchiveOptions deleteArchiveOption = DeleteArchiveOptions.DeleteArchiveOptions__UNDEFINED;

            if (ExcludeArchivePlaceholder)
                deleteArchiveOption = DeleteArchiveOptions.DeleteArchiveOptions__Exclude;
            else if (OnlyArchivePlaceholder)
                deleteArchiveOption = DeleteArchiveOptions.DeleteArchiveOptions__Only;
            else
                deleteArchiveOption = DeleteArchiveOptions.DeleteArchiveOptions__Include;

            WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId: {BackupSetId}");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            WriteVerbose("Performing Action: Create Backup Set Delete View");
            WriteVerbose($"Notice: View Start Date: {DateFrom}");
            WriteVerbose($"Notice: View End Date: {DateTo}");
            BackupSetDeleteView backupSetDeleteView = backupSet.prepare_delete(DateTimeToUnixEpoch(DateFrom), DateTimeToUnixEpoch(DateTo), DateTimeToUnixEpoch(DeletedDate), KeepGenerations, deleteArchiveOption);

            // Check for a previous Backup Set Validation View stored in Session State
            WriteVerbose("Performing Action: Check for previous DS-Client Delete View Sessions");
            BackupSetDeleteView previousDeleteSession = SessionState.PSVariable.GetValue("DeleteView", null) as BackupSetDeleteView;

            // If a previous session is found, remove it
            if (previousDeleteSession != null)
            {
                WriteVerbose("Notice: Previous Delete View found");
                try
                {
                    WriteVerbose("Performing Action: Dispose Delete View");
                    previousDeleteSession.Dispose();
                }
                catch
                {
                    WriteVerbose("Notice: Previous Session failed to Dispose");
                }
                WriteVerbose("Performing Action: Remove on Delete View Session");
                SessionState.PSVariable.Remove("DeleteView");
            }

            // Add new Delete View to SessionState
            WriteVerbose("Performing Action: Store Backup Set Delete View into SessionState");
            SessionState.PSVariable.Set("DeleteView", backupSetDeleteView);

            backupSet.Dispose();
        }
    }
}