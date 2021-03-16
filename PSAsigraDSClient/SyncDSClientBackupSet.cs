using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsData.Sync, "DSClientBackupSet")]
    [OutputType(typeof(GenericBackupSetActivity))]

    public class StartDSClientBackupSetSync: BaseDSClientStartBackupSetActivity
    {
        [Parameter(HelpMessage = "Specify Sync should be DS-System based")]
        public SwitchParameter DSSystemBased { get; set; }

        protected override void ProcessBackupSet(BackupSet backupSet)
        {
            GenericActivity syncActivity;

            WriteVerbose("Performing Action: Start Backup Synchronization Activity");
            if (MyInvocation.BoundParameters.ContainsKey("DSSystemBased"))
                syncActivity = backupSet.start_sync(DSSystemBased);
            else
                syncActivity = backupSet.start_sync(false);

            if (PassThru)
                WriteObject(new GenericBackupSetActivity(syncActivity));

            syncActivity.Dispose();
        }

        protected override void ProcessBackupSets(BackupSet[] backupSets)
        {
            List<GenericBackupSetActivity> startActivity = new List<GenericBackupSetActivity>();

            foreach (BackupSet set in backupSets)
            {
                WriteVerbose("Performing Action: Start Backup Set Synchronization Activity");
                if (MyInvocation.BoundParameters.ContainsKey("DSSystemBased"))
                {
                    try
                    {
                        GenericActivity syncActivity = set.start_sync(DSSystemBased);

                        startActivity.Add(new GenericBackupSetActivity(syncActivity));

                        syncActivity.Dispose();
                    }
                    catch (APIException e)
                    {
                        WriteWarning(e.Message);

                        continue;
                    }
                }
                else
                {
                    try
                    {
                        GenericActivity syncActivity = set.start_sync(false);

                        startActivity.Add(new GenericBackupSetActivity(syncActivity));

                        syncActivity.Dispose();
                    }
                    catch (APIException e)
                    {
                        WriteWarning(e.Message);

                        continue;
                    }
                }
            }

            if (PassThru)
                startActivity.ForEach(WriteObject);
        }
    }
}