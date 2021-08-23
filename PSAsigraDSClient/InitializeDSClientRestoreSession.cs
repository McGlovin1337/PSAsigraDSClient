using System;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsData.Initialize, "DSClientRestoreSession")]
    [OutputType(typeof(DSClientRestoreSession))]

    sealed public class InitializeDSClientRestoreSession : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set to Initialize a Restore Session for")]
        public int BackupSetId { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Specify Reason for Restore")]
        [ValidateSet("UserErrorDataDeletion", "MaliciousIntent", "DeviceLostOrStolen", "HardwareMalfunction", "SoftwareMalfunction", "DataStolen", "DataCorruption", "NaturalDisasters", "PowerOutages", "OtherDisaster", "PreviousGeneration", "DeviceDamaged")]
        public string RestoreReason { get; set; }

        [Parameter(HelpMessage = "Specify Restore Classification")]
        [ValidateSet("Production", "Drill", "ProductionDrill")]
        public string RestoreClassification { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Get the Backup Set Details
            WriteVerbose("Performing Action: Retrieve Backup Set Details");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            // Check if Backup Set is in use
            if (backupSet.check_lock_status(EActivityType.EActivityType__Restore) == EBackupSetLockStatus.EBackupSetLockStatus__Locked)
            {
                backupSet.Dispose();
                throw new Exception("Backup Set is Currently Locked");
            }

            // Determine the Backup Set Type based on the Data Type
            EBackupDataType dataType = backupSet.getDataType();
            Type setType = typeof(BackupSet);
            if (dataType == EBackupDataType.EBackupDataType__FileSystem)
            {
                if (DSClientSessionInfo.OperatingSystem == "Windows")
                    setType = typeof(Win32FS_BackupSet);
                else
                    setType = typeof(UnixFS_Generic_BackupSet);
            }
            else if (dataType == EBackupDataType.EBackupDataType__SQLServer)
            {
                setType = typeof(MSSQL_BackupSet);
            }
            else if (dataType == EBackupDataType.EBackupDataType__VMwareVADP)
            {
                setType = typeof(VMwareVADP_BackupSet);
            }
            WriteDebug($"Backup Set Type: {setType}");

            WriteVerbose("Performing Action: Create Restore Session");
            DSClientRestoreSession restoreSession = new DSClientRestoreSession(DSClientSessionInfo.GenerateRestoreId(), backupSet, setType);
            WriteVerbose($"Restore Session Id: {restoreSession.RestoreId}");

            if (MyInvocation.BoundParameters.ContainsKey(nameof(RestoreClassification)))
                restoreSession.SetRestoreClassification(RestoreClassification);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(RestoreReason)))
                restoreSession.SetRestoreReason(RestoreReason);

            DSClientSessionInfo.AddRestoreSession(restoreSession);

            WriteObject(restoreSession);
        }
    }
}