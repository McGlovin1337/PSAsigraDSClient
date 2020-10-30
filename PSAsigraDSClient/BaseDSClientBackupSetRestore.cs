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

        [Parameter(HelpMessage = "Specify to use Detailed Log")]
        public SwitchParameter UseDetailedLog { get; set; }

        [Parameter(HelpMessage = "Specify Local Storage Handling")]
        [ValidateSet("None", "ConnectDsSysFirst", "ConnectDsSysNeeded", "ContinueWithoutDsSys")]
        public string LocalStorageMethod { get; set; } = "ConnectDsSysNeeded";

        protected ERestoreReason StringToERestoreReason(string reason)
        {
            ERestoreReason Reason;

            switch (reason.ToLower())
            {
                case "accidentaldeletion":
                    Reason = ERestoreReason.ERestoreReason__UserErrorDataDeletion;
                    break;
                case "maliciousintent":
                    Reason = ERestoreReason.ERestoreReason__MaliciousIntent;
                    break;
                case "devicelostorstolen":
                    Reason = ERestoreReason.ERestoreReason__DeviceLostOrStolen;
                    break;
                case "hardwarefault":
                    Reason = ERestoreReason.ERestoreReason__HardwareMalfunction;
                    break;
                case "softwarefault":
                    Reason = ERestoreReason.ERestoreReason__SoftwareMalfunction;
                    break;
                case "datastolen":
                    Reason = ERestoreReason.ERestoreReason__DataStolen;
                    break;
                case "datacorruption":
                    Reason = ERestoreReason.ERestoreReason__DataCorruption;
                    break;
                case "naturaldisaster":
                    Reason = ERestoreReason.ERestoreReason__NaturalDisasters;
                    break;
                case "poweroutage":
                    Reason = ERestoreReason.ERestoreReason__PowerOutages;
                    break;
                case "otherdisaster":
                    Reason = ERestoreReason.ERestoreReason__OtherDisaster;
                    break;
                case "previousgeneration":
                    Reason = ERestoreReason.ERestoreReason__PreviousGeneration;
                    break;
                case "devicedamaged":
                    Reason = ERestoreReason.ERestoreReason__DeviceDamaged;
                    break;
                default:
                    Reason = ERestoreReason.ERestoreReason__UNDEFINED;
                    break;
            }

            return Reason;
        }

        protected ERestoreClassification StringToERestoreClassification(string classification)
        {
            ERestoreClassification Classification;

            switch (classification.ToLower())
            {
                case "production":
                    Classification = ERestoreClassification.ERestoreClassification__Production;
                    break;
                case "drill":
                    Classification = ERestoreClassification.ERestoreClassification__StopAfterDrillUsedOut;
                    break;
                case "productiondrill":
                    Classification = ERestoreClassification.ERestoreClassification__ProductionAfterDrillUsedOut;
                    break;
                default:
                    Classification = ERestoreClassification.ERestoreClassification__UNDEFINED;
                    break;
            }

            return Classification;
        }

        protected ERestoreLocalStorageHandling StringToERestoreLocalStorageHandling(string method)
        {
            ERestoreLocalStorageHandling Method;

            switch (method.ToLower())
            {
                case "none":
                    Method = ERestoreLocalStorageHandling.ERestoreLocalStorageHandling__None;
                    break;
                case "connectdssysfirst":
                    Method = ERestoreLocalStorageHandling.ERestoreLocalStorageHandling__ConnectFirst;
                    break;
                case "connectdssysneeded":
                    Method = ERestoreLocalStorageHandling.ERestoreLocalStorageHandling__ConnectIfNeeded;
                    break;
                case "continuewithoutdssys":
                    Method = ERestoreLocalStorageHandling.ERestoreLocalStorageHandling__ContinueIfDisconnect;
                    break;
                default:
                    Method = ERestoreLocalStorageHandling.ERestoreLocalStorageHandling__UNDEFINED;
                    break;
            }

            return Method;
        }
    }
}