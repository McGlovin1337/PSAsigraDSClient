using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientBackupSetRetention")]
    [OutputType(typeof(DSClientStartBackupSetActivity))]

    public class StartDSClientBackupSetRetention: BaseDSClientStartBackupSetActivity
    {
        protected override void ProcessBackupSet(BackupSet backupSet)
        {
            WriteVerbose("Performing Action: Start Backup Set Retention Activity");
            GenericActivity retentionActivity = backupSet.enforceRetention();

            DSClientStartBackupSetActivity startActivity = new DSClientStartBackupSetActivity(retentionActivity.getID(), backupSet.getID(), backupSet.getName());

            WriteObject(startActivity);

            retentionActivity.Dispose();
        }

        protected override void ProcessBackupSets(BackupSet[] backupSets)
        {
            List<DSClientStartBackupSetActivity> startActivity = new List<DSClientStartBackupSetActivity>();

            foreach (BackupSet set in backupSets)
            {
                try
                {
                    WriteVerbose("Performing Action: Start Backup Set Retention Activity");
                    GenericActivity retentionActivity = set.enforceRetention();

                    startActivity.Add(new DSClientStartBackupSetActivity(retentionActivity.getID(), set.getID(), set.getName()));

                    retentionActivity.Dispose();
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