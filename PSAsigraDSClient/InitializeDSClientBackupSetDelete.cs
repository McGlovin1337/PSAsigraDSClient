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

            WriteVerbose("Retrieving Backup Set from DS-Client...");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            WriteVerbose("Creating a new Backup Set Delete View...");
            WriteVerbose("View Start Date: " + DateFrom);
            WriteVerbose("View End Date: " + DateTo);
            BackupSetDeleteView backupSetDeleteView = backupSet.prepare_delete(DateTimeToUnixEpoch(DateFrom), DateTimeToUnixEpoch(DateTo), DateTimeToUnixEpoch(DeletedDate), KeepGenerations, deleteArchiveOption);

            // Check for a previous Backup Set Validation View stored in Session State
            WriteVerbose("Checking for previous DS-Client Delete View Sessions...");
            BackupSetDeleteView previousDeleteSession = SessionState.PSVariable.GetValue("DeleteView", null) as BackupSetDeleteView;

            // If a previous session is found, remove it
            if (previousDeleteSession != null)
            {
                WriteVerbose("Previous Delete View found, attempting to Dispose...");
                try
                {
                    previousDeleteSession.Dispose();
                }
                catch
                {
                    WriteVerbose("Previous Session failed to Dispose, deleting session...");
                }
                SessionState.PSVariable.Remove("DeleteView");
            }

            // Add new Delete View to SessionState
            WriteVerbose("Storing new Backup Set Delete View into SessionState...");
            SessionState.PSVariable.Set("DeleteView", backupSetDeleteView);

            backupSet.Dispose();
        }
    }
}