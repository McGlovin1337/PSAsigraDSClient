using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientVMwareVADPBackupSet", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class SetDSClientVMwareVADPBackupSet : BaseDSClientVMwareVADPBackupSet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set to Modify")]
        public int BackupSetId { get; set; }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Rename the Backup Set")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Compression Method to use")]
        [ValidateSet("None", "ZLIB", "LZOP", "ZLIB_LO", "ZLIB_MED", "ZLIB_HI")]
        public new string Compression { get; set; }

        protected override void ProcessVADPSet()
        {
            // Get the Backup Set from DS-Client
            WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId: {BackupSetId}");
            BackupSet backupSet = null;
            try
            {
                backupSet = DSClientSession.backup_set(BackupSetId);
            }
            catch (APIException e)
            {
                ErrorRecord errorRecord = new ErrorRecord(
                    e,
                    "APIException",
                    ErrorCategory.ObjectNotFound,
                    null);
                WriteError(errorRecord);
            }

            if (backupSet != null)
            {
                string backupSetName = backupSet.getName();

                // Process the Common Backup Set Parameters
                backupSet = ProcessBaseBackupSetParams(MyInvocation.BoundParameters, backupSet);

                // Set the Schedule and Retention Rules
                if (MyInvocation.BoundParameters.ContainsKey("ScheduleId"))
                {
                    ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();
                    Schedule schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);
                    if (ShouldProcess($"Backup Set: '{backupSetName}'", $"Set Schedule {schedule.getName()}"))
                        backupSet.setSchedule(schedule);
                }

                if (MyInvocation.BoundParameters.ContainsKey("RetentionRuleId"))
                {
                    RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();
                    RetentionRule[] retentionRules = DSClientRetentionRuleMgr.definedRules();
                    RetentionRule retentionRule = retentionRules.Single(rule => rule.getID() == RetentionRuleId);
                    if (ShouldProcess($"Backup Set: '{backupSetName}'", $"Set Retention Rule '{retentionRule.getName()}'"))
                        backupSet.setRetentionRule(retentionRule);
                }

                // Process this Cmdlets specific Parameters
                VMwareVADP_BackupSet vmwareVADPBackupSet = VMwareVADP_BackupSet.from(backupSet);

                vmware_additional_options vmwareOptions = vmwareVADPBackupSet.getAdditionalVMwareOptions();

                if (MyInvocation.BoundParameters.ContainsKey("IncrementalP2VBackup"))
                    if (ShouldProcess("Incremental P2V Backups", $"Set Value '{IncrementalP2VBackup}'"))
                        vmwareOptions.valBackupP2VIncrementally = IncrementalP2VBackup;

                if (MyInvocation.BoundParameters.ContainsKey("BackupVMMemory"))
                    if (ShouldProcess("Backup Virtual Machine Memory", $"Set Value '{BackupVMMemory}'"))
                        vmwareOptions.valBackupVMmemory = BackupVMMemory;

                if (MyInvocation.BoundParameters.ContainsKey("SnapshotQuiesce"))
                    if (ShouldProcess("Quiesce Virtual Machine prior to Snapshot", $"Set Value '{SnapshotQuiesce}'"))
                        vmwareOptions.valQuiesceIOBeforeSnap = SnapshotQuiesce;

                if (MyInvocation.BoundParameters.ContainsKey("SameTimeSnapshot"))
                    if (ShouldProcess("Snapshot All Virtual Machines at Same Time", $"Set Value '{SameTimeSnapshot}'"))
                        vmwareOptions.valSnapAllVMSameTime = SameTimeSnapshot;

                if (MyInvocation.BoundParameters.ContainsKey("UseCBT"))
                    if (ShouldProcess("Use Change Block Tracking (CBT)", $"Set Value '{UseCBT}'"))
                        vmwareOptions.valUsingCBT = UseCBT;

                if (MyInvocation.BoundParameters.ContainsKey("UseFLR"))
                    if (ShouldProcess("Use File Level Restore (FLR)", $"Set Value '{UseFLR}'"))
                        vmwareOptions.valUsingFLR = UseFLR;

                if (MyInvocation.BoundParameters.ContainsKey("UseLocalVDR"))
                    if (ShouldProcess("Use Local Virtual Disaster Recovery (VDR)", $"Set Value '{UseLocalVDR}'"))
                        vmwareOptions.valUsingLocalVDR = UseLocalVDR;

                if (MyInvocation.BoundParameters.ContainsKey("VMLibraryVersion"))
                    if (ShouldProcess("VMware Library Version", $"Set Value '{VMLibraryVersion}'"))
                        vmwareOptions.valVMLibVersion = StringToEnum<EVMwareLibraryVersion>(VMLibraryVersion);

                if (ShouldProcess($"Backup Set: '{backupSetName}'", "Update Additional VMware Options"))
                    vmwareVADPBackupSet.setAdditionalVMwareOptions(vmwareOptions);

                if (MyInvocation.BoundParameters.ContainsKey("UseBuffer"))
                    if (ShouldProcess("Use Buffer", $"Set Value '{UseBuffer}'"))
                        vmwareVADPBackupSet.setUsingBuffer(UseBuffer);

                vmwareVADPBackupSet.Dispose();
            }
        }
    }
}