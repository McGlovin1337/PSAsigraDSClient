using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientBackupSetRetention")]
    [OutputType(typeof(void), typeof(GenericBackupSetActivity))]

    sealed public class StartDSClientBackupSetRetention: BaseDSClientStartBackupSetActivity
    {
        protected override void ProcessBackupSet(BackupSet backupSet)
        {
            WriteVerbose("Performing Action: Start Backup Set Retention Activity");
            GenericActivity retentionActivity = backupSet.enforceRetention();

            if (PassThru)
                WriteObject(new GenericBackupSetActivity(retentionActivity));

            retentionActivity.Dispose();
        }

        protected override void ProcessBackupSets(BackupSet[] backupSets)
        {
            List<GenericBackupSetActivity> startActivity = new List<GenericBackupSetActivity>();

            foreach (BackupSet set in backupSets)
            {
                try
                {
                    WriteVerbose("Performing Action: Start Backup Set Retention Activity");
                    GenericActivity retentionActivity = set.enforceRetention();

                    startActivity.Add(new GenericBackupSetActivity(retentionActivity));

                    retentionActivity.Dispose();
                }
                catch (APIException e)
                {
                    WriteWarning(e.Message);

                    continue;
                }
            }

            if (PassThru)
                startActivity.ForEach(WriteObject);
        }
    }
}