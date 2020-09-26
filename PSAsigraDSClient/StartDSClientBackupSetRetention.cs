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
            WriteVerbose("Starting Backup Set Retention Activity...");
            GenericActivity retentionActivity = backupSet.enforceRetention();

            DSClientStartBackupSetActivity startActivity = new DSClientStartBackupSetActivity
            {
                ActivityId = retentionActivity.getID(),
                BackupSetId = backupSet.getID(),
                Name = backupSet.getName()
            };

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
                    WriteVerbose("Starting a Backup Set Retention Activity...");
                    GenericActivity retentionActivity = set.enforceRetention();
                    
                    startActivity.Add( new DSClientStartBackupSetActivity
                    {
                        ActivityId = retentionActivity.getID(),
                        BackupSetId = set.getID(),
                        Name = set.getName()
                    });

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