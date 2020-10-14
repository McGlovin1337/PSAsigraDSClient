using System.Management.Automation;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientBackupSetParams: BaseDSClientBackupSet
    {
        [Parameter(Position = 3, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Backup Set Type")]
        [ValidateSet("Offsite", "Statistical", "SelfContained", "LocalOnly")]
        public string SetType { get; set; }        

        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, HelpMessage = "Set the Compression Method to use")]
        [ValidateSet("None", "ZLIB", "LZOP", "ZLIB_LO", "ZLIB_MED", "ZLIB_HI")]
        public string Compression { get; set; }

        [Parameter(HelpMessage = "Specify this Backup Set should be set to Disabled")]
        public SwitchParameter Disabled { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Schedule this Backup Set will use")]
        public int ScheduleId { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Retention Rule this Backup Set will use")]
        public int RetentionRuleId { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Schedule Priority of Backup Set when assigned to Schedule")]
        public int SchedulePriority { get; set; } = 1;

        [Parameter(HelpMessage = "Force Re-Backup of File even if it hasn't been modified")]
        public SwitchParameter ForceBackup { get; set; }

        [Parameter(HelpMessage = "Set to PreScan before Backup")]
        public SwitchParameter PreScan { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Read Buffer Size")]
        public int ReadBufferSize { get; set; } = 0;

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Backup Error limit")]
        public int BackupErrorLimit { get; set; } = 0;

        [Parameter(HelpMessage = "Set to use Detailed Log")]
        public SwitchParameter UseDetailedLog { get; set; }

        [Parameter(HelpMessage = "Set to use Infinate BLM Generations")]
        public SwitchParameter InfinateBLMGenerations { get; set; }

        [Parameter(HelpMessage = "Set to use Local Storage")]
        public SwitchParameter UseLocalStorage { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Local Storage Path For Local Backups and Cache")]
        public string LocalStoragePath { get; set; }

        [Parameter(HelpMessage = "Set to use Local Transmission Cache for Offsite Backup Sets")]
        public SwitchParameter UseTransmissionCache { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Notification Method")]
        [ValidateSet("Email", "Pager", "Broadcast", "Event")]
        public string NotificationMethod { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Notification Recipient")]
        public string NotificationRecipient { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Completion Status to Notify on")]
        [ValidateSet("Incomplete", "CompletedWithErrors", "Successful", "CompletedWithWarnings")]
        public string[] NotificationCompletion { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Email Notification Options")]
        [ValidateSet("DetailedInfo", "AttachDetailedLog", "CompressAttachment", "HtmlFormat")]
        public string[] NotificationEmailOptions { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Completion Status to send SNMP Traps")]
        [ValidateSet("Incomplete", "CompletedWithErrors", "Successful", "CompletedWithWarnings")]
        public string[] SnmpTrapNotifications { get; set; }
    }
}