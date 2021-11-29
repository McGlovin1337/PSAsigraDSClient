using System.Management.Automation;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientVMwareVADPBackupSet: BaseDSClientBackupSetParams
    {
        [Parameter(HelpMessage = "Backup P2V VMs Incrementally")]
        public SwitchParameter IncrementalP2VBackup { get; set; }

        [Parameter(HelpMessage = "Backup VM Memory")]
        public SwitchParameter BackupVMMemory { get; set; }

        [Parameter(HelpMessage =  "Quiesce IO before Snapshot")]
        public SwitchParameter SnapshotQuiesce { get; set; }

        [Parameter(HelpMessage = "Snapshot all VMs at same time")]
        public SwitchParameter SameTimeSnapshot { get; set; }

        [Parameter(HelpMessage = "Use Change Block Tracking (CBT)")]
        public SwitchParameter UseCBT { get; set; }

        [Parameter(HelpMessage =  "Use File Level Restore (FLR)")]
        public SwitchParameter UseFLR { get; set; }

        [Parameter(HelpMessage = "Use Local Virtual Disaster Recovery (VDR)")]
        public SwitchParameter UseLocalVDR { get; set; }

        [Parameter(HelpMessage = "Specify VM Library Version")]
        [ValidateSet("Latest", "VDDK6_0", "VDDK5_5")]
        public string VMLibraryVersion { get; set; } = "Latest";

        [Parameter(HelpMessage = "Specify to use DS-Client Buffer")]
        public SwitchParameter UseBuffer { get; set; }

        protected abstract void ProcessVADPSet();

        protected override void DSClientProcessRecord()
        {
            // Validate the Common Base Parameters
            BaseBackupSetParamValidation(MyInvocation.BoundParameters);

            ProcessVADPSet();
        }
    }
}