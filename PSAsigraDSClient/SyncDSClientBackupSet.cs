﻿using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsData.Sync, "DSClientBackupSet")]
    [OutputType(typeof(DSClientStartBackupSetActivity))]

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

            DSClientStartBackupSetActivity startActivity = new DSClientStartBackupSetActivity(syncActivity.getID(), backupSet.getID(), backupSet.getName());

            WriteObject(startActivity);

            syncActivity.Dispose();
        }

        protected override void ProcessBackupSets(BackupSet[] backupSets)
        {
            List<DSClientStartBackupSetActivity> startActivity = new List<DSClientStartBackupSetActivity>();

            foreach (BackupSet set in backupSets)
            {
                WriteVerbose("Performing Action: Start Backup Set Synchronization Activity");
                if (MyInvocation.BoundParameters.ContainsKey("DSSystemBased"))
                {
                    try
                    {
                        GenericActivity syncActivity = set.start_sync(DSSystemBased);

                        startActivity.Add(new DSClientStartBackupSetActivity(syncActivity.getID(), set.getID(), set.getName()));

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

                        startActivity.Add(new DSClientStartBackupSetActivity(syncActivity.getID(), set.getID(), set.getName()));

                        syncActivity.Dispose();
                    }
                    catch (APIException e)
                    {
                        WriteWarning(e.Message);

                        continue;
                    }
                }
            }

            startActivity.ForEach(WriteObject);
        }
    }
}