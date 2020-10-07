using System.Management.Automation;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientBackupSetParams: BaseDSClientBackupSet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The name of the Backup Set")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Computer the Backup Set will be assigned to")]
        [ValidateNotNullOrEmpty]
        public string Computer { get; set; }

        [Parameter(Position = 2, HelpMessage = "Credentials to use")]
        public PSCredential Credential { get; set; }

        [Parameter(Position = 3, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Backup Set Type")]
        [ValidateSet("Offsite", "Statistical", "SelfContained", "LocalOnly")]
        public string SetType { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Items to Include in Backup Set")]
        public string[] IncludeItem { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Max Number of Generations for Included Items")]
        public int MaxGenerations { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Items to Exclude from Backup Set")]
        public string[] ExcludeItem { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Regex Item Exclusion Patterns")]
        [ValidateNotNullOrEmpty]
        public string[] RegexExcludeItem { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Path for Regex Exclusion Item")]
        [ValidateNotNullOrEmpty]
        public string RegexExclusionPath { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to also Exclude Directories with Regex pattern")]
        public SwitchParameter RegexExcludeDirectory { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify if Regex Exclusions Items are case insensitive")]
        public SwitchParameter RegexCaseInsensitive { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, HelpMessage = "Set the Compression Method to use")]
        [ValidateSet("None", "ZLIB", "LZOP", "ZLIB_LO", "ZLIB_MED", "ZLIB_HI")]
        public string Compression { get; set; }

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