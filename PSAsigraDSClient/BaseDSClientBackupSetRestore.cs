using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientBackupSetRestore: DSClientCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the items to validate")]
        [ValidateNotNullOrEmpty]
        [Alias("Path")]
        public string[] Items { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Computer to Restore to")]
        [ValidateNotNullOrEmpty]
        public string Computer { get; set; }

        [Parameter(HelpMessage = "Specify Credentials for Destination Computer")]
        public PSCredential Credential { get; set; }

        [Parameter(HelpMessage = "Specify how many levels to Truncate Source Path")]
        public int TruncateSource { get; set; } = 0;

        [Parameter(Mandatory = true, HelpMessage = "Specify the Restore Reason")]
        [ValidateSet("AccidentalDeletion", "MaliciousIntent", "DeviceLostOrStolen", "HardwareFault", "SoftwareFault", "DataStolen", "DataCorruption", "NaturalDisaster", "PowerOutage", "OtherDisaster", "PreviousGeneration", "DeviceDamaged")]
        public string RestoreReason { get; set; }

        [Parameter(HelpMessage = "Specify Restore Classification")]
        [ValidateSet("Production", "Drill", "ProductionDrill")]
        public string RestoreClassification { get; set; } = "Production";

        [Parameter(HelpMessage = "Specify Max Pending Asynchronous I/O per File")]
        public int MaxPendingAsyncIO { get; set; } = 0;

        [Parameter(HelpMessage = "Specify the number of DS-System Read Threads")]
        public int ReadThreads { get; set; } = 0;

        [Parameter(HelpMessage = "Specify to use Detailed Log")]
        public SwitchParameter UseDetailedLog { get; set; }

        [Parameter(HelpMessage = "Specify Local Storage Handling")]
        [ValidateSet("None", "ConnectDsSysFirst", "ConnectDsSysNeeded", "ContinueWithoutDsSys")]
        public string LocalStorageMethod { get; set; } = "ConnectDsSysNeeded";

        protected ERestoreReason StringToERestoreReason(string reason)
        {
            ERestoreReason Reason = ERestoreReason.ERestoreReason__UNDEFINED;

            switch (reason)
            {
                case "AccidentalDeletion":
                    Reason = ERestoreReason.ERestoreReason__UserErrorDataDeletion;
                    break;
                case "MaliciousIntent":
                    Reason = ERestoreReason.ERestoreReason__MaliciousIntent;
                    break;
                case "DeviceLostOrStolen":
                    Reason = ERestoreReason.ERestoreReason__DeviceLostOrStolen;
                    break;
                case "HardwareFault":
                    Reason = ERestoreReason.ERestoreReason__HardwareMalfunction;
                    break;
                case "SoftwareFault":
                    Reason = ERestoreReason.ERestoreReason__SoftwareMalfunction;
                    break;
                case "DataStolen":
                    Reason = ERestoreReason.ERestoreReason__DataStolen;
                    break;
                case "DataCorruption":
                    Reason = ERestoreReason.ERestoreReason__DataCorruption;
                    break;
                case "NaturalDisaster":
                    Reason = ERestoreReason.ERestoreReason__NaturalDisasters;
                    break;
                case "PowerOutage":
                    Reason = ERestoreReason.ERestoreReason__PowerOutages;
                    break;
                case "OtherDisaster":
                    Reason = ERestoreReason.ERestoreReason__OtherDisaster;
                    break;
                case "PreviousGeneration":
                    Reason = ERestoreReason.ERestoreReason__PreviousGeneration;
                    break;
                case "DeviceDamaged":
                    Reason = ERestoreReason.ERestoreReason__DeviceDamaged;
                    break;
            }

            return Reason;
        }

        protected ERestoreClassification StringToERestoreClassification(string classification)
        {
            ERestoreClassification Classification = ERestoreClassification.ERestoreClassification__UNDEFINED;

            switch (classification)
            {
                case "Production":
                    Classification = ERestoreClassification.ERestoreClassification__Production;
                    break;
                case "Drill":
                    Classification = ERestoreClassification.ERestoreClassification__StopAfterDrillUsedOut;
                    break;
                case "ProductionDrill":
                    Classification = ERestoreClassification.ERestoreClassification__ProductionAfterDrillUsedOut;
                    break;
            }

            return Classification;
        }

        protected ERestoreLocalStorageHandling StringToERestoreLocalStorageHandling(string method)
        {
            ERestoreLocalStorageHandling Method = ERestoreLocalStorageHandling.ERestoreLocalStorageHandling__UNDEFINED;

            switch (method)
            {
                case "None":
                    Method = ERestoreLocalStorageHandling.ERestoreLocalStorageHandling__None;
                    break;
                case "ConnectDsSysFirst":
                    Method = ERestoreLocalStorageHandling.ERestoreLocalStorageHandling__ConnectFirst;
                    break;
                case "ConnectDsSysNeeded":
                    Method = ERestoreLocalStorageHandling.ERestoreLocalStorageHandling__ConnectIfNeeded;
                    break;
                case "ContinueWithoutDsSys":
                    Method = ERestoreLocalStorageHandling.ERestoreLocalStorageHandling__ContinueIfDisconnect;
                    break;
            }

            return Method;
        }
    }
}