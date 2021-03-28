using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.BaseDSClientRunningActivity;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientBackupSet", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(DSClientRunningActivity))]

    public class RemoveDSClientBackupSet: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Id of Backup Set to remove")]
        [ValidateNotNullOrEmpty]
        public int BackupSetId { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId {BackupSetId}");
            BackupSet DSClientBackupSet = DSClientSession.backup_set(BackupSetId);

            if (ShouldProcess($"'{DSClientBackupSet.getName()}' with ID: {DSClientBackupSet.getID()}"))
            {
                WriteVerbose("Performing Action: Initiate Backup Set Removal");
                GenericActivity removeActivity = DSClientSession.removeBackupSet(DSClientBackupSet);

                int removeId = removeActivity.getID();
                WriteVerbose($"Notice: Removal Activity Id {removeId} created");

                running_activity_info removalInfo = removeActivity.getCurrentStatus();
                DSClientRunningActivity RemovalActivity = new DSClientRunningActivity(removalInfo);

                WriteObject(RemovalActivity);
            }

            DSClientBackupSet.Dispose();
        }
    }
}