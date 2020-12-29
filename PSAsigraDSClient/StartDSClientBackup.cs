using System;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientBackup")]
    [OutputType(typeof(DSClientStartBackupSetActivity))]

    public class StartDSClientBackup: BaseDSClientStartBackupSetActivity
    {
        [Parameter(HelpMessage = "Specify to Run Retention After Backup")]
        public SwitchParameter PerformRetention { get; set; }

        [Parameter(HelpMessage = "Specify to Prescan Backup Source")]
        public SwitchParameter PreScan { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Stop Backup Error Limit")]
        public int ErrorLimit { get; set; } = 0;

        [Parameter(HelpMessage = "Specify to use Detailed Log")]
        public SwitchParameter UseDetailedLog { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Start Time")]
        public DateTime StartTime { get; set; }

        protected override void ProcessBackupSet(BackupSet backupSet)
        {
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

            WriteVerbose("Performing Action: Submit Backup Start Activity");
            GenericActivity backupActivity = backupSet.start_backup(backupSetItems, startParams);

            DSClientStartBackupSetActivity startActivity = new DSClientStartBackupSetActivity
            {
                ActivityId = backupActivity.getID(),
                BackupSetId = backupSet.getID(),
                Name = backupSet.getName()
            };

            WriteObject(startActivity);

            backupActivity.Dispose();
            startParams.Dispose();
        }

        protected override void ProcessBackupSets(BackupSet[] backupSets)
        {
            List<DSClientStartBackupSetActivity> startActivity = new List<DSClientStartBackupSetActivity>();

            foreach (BackupSet backupSet in backupSets)
            {
                try
                {
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

                    startActivity.Add(new DSClientStartBackupSetActivity
                    {
                        ActivityId = backupActivity.getID(),
                        BackupSetId = backupSet.getID(),
                        Name = backupSet.getName()
                    });

                    backupActivity.Dispose();
                    startParams.Dispose();
                }
                catch (APIException e)
                {
                    WriteWarning(e.Message);

                    continue;
                }
            }

            startActivity.ForEach(WriteObject);
        }
    }
}