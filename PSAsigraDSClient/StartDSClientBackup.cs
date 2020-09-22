using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;
using static PSAsigraDSClient.BaseDSClientRunningActivity;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientBackup")]
    [OutputType(typeof(DSClientRunningActivity))]

    public class StartDSClientBackup: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set Id")]
        public int BackupSetId { get; set; }

        [Parameter(Position = 1, HelpMessage = "Specify to Run Retention After Backup")]
        public SwitchParameter PerformRetention { get; set; }

        [Parameter(Position = 2, HelpMessage = "Specify to Prescan Backup Source")]
        public SwitchParameter PreScan { get; set; }

        [Parameter(Position = 3, ValueFromPipelineByPropertyName = true, HelpMessage = "Stop Backup Error Limit")]
        public int ErrorLimit { get; set; } = 0;

        [Parameter(Position = 4, HelpMessage = "Specify to use Detailed Log")]
        public SwitchParameter UseDetailedLog { get; set; }

        [Parameter(Position = 5, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Start Time")]
        public DateTime StartTime { get; set; }

        protected override void DSClientProcessRecord()
        {
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            BackupSetItem[] backupSetItems = backupSet.items();

            BackupStartParameters startParams = backupSet.createDefaultBackupParameters();

            if (MyInvocation.BoundParameters.ContainsKey("PerformRetention"))
                startParams.setDoRetentionAfter(PerformRetention);

            if (MyInvocation.BoundParameters.ContainsKey("PreScan"))
                startParams.setPreScanSource(PreScan);

            startParams.setErrorStop(ErrorLimit);

            if (MyInvocation.BoundParameters.ContainsKey("UseDetailedLog"))
                startParams.setUsesDetailedLog(UseDetailedLog);

            if (MyInvocation.BoundParameters.ContainsKey("StartTime"))
                startParams.setStartTime(DateTimeToUnixEpoch(StartTime));

            WriteVerbose("Submitting Backup Start Activity...");
            GenericActivity backupActivity = backupSet.start_backup(backupSetItems, startParams);

            int backupActivityId = backupActivity.getID();
            WriteVerbose("Backup Activity with ID " + backupActivityId + " submitted");

            running_activity_info backupActivityInfo = backupActivity.getCurrentStatus();
            DSClientRunningActivity runningBackup = new DSClientRunningActivity(backupActivityInfo);

            WriteObject(runningBackup);

            backupActivity.Dispose();
            startParams.Dispose();
            backupSet.Dispose();
        }
    }
}