using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.BaseDSClientRunningActivity;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientBackupSet")]
    [OutputType(typeof(DSClientRunningActivity))]

    public class RemoveDSClientBackupSet: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Id of Backup Set to remove")]
        [ValidateNotNullOrEmpty]
        public int Id { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Retrieving Backup Set...");
            BackupSet DSClientBackupSet = DSClientSession.backup_set(Id);

            WriteVerbose("Initiating Backup Set Removal...");
            GenericActivity removeActivity = DSClientSession.removeBackupSet(DSClientBackupSet);

            DSClientBackupSet.Dispose();

            int removeId = removeActivity.getID();
            WriteVerbose("Removal Activity Id " + removeId + " created...");

            running_activity_info removalInfo = removeActivity.getCurrentStatus();
            DSClientRunningActivity RemovalActivity = new DSClientRunningActivity(removalInfo);

            WriteObject(RemovalActivity);
        }
    }
}