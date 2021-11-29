using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientWinFsBackupSet")]
    [OutputType(typeof(DSClientBackupSetBasicProps))]

    sealed public class NewDSClientWinFsBackupSet: BaseDSClientWinFsBackupSet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The name of the Backup Set")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Computer the Backup Set will be assigned to")]
        [ValidateNotNullOrEmpty]
        public string Computer { get; set; }

        [Parameter(Position = 3, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Backup Set Type")]
        [ValidateSet("Offsite", "Statistical", "SelfContained", "LocalOnly")]
        public string SetType { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Items to Include in Backup Set")]
        public string[] IncludeItem { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Max Number of Generations for Included Items")]
        [ValidateRange(1, 9999)]
        public int MaxGenerations { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Items to Exclude from Backup Set")]
        public string[] ExcludeItem { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Regex Item Exclusion Patterns")]
        [ValidateNotNullOrEmpty]
        public string[] RegexExcludePattern { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Path Regex Exclusion Pattern applies to")]
        [ValidateNotNullOrEmpty]
        public string RegexExclusionPath { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to also Exclude Directories with Regex pattern")]
        public SwitchParameter RegexMatchDirectory { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify if Regex Exclusions Items are case insensitive")]
        public SwitchParameter RegexCaseInsensitive { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to exclude Sub-Directories")]
        public SwitchParameter ExcludeSubDirs { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Notification Method")]
        [ValidateSet("Email", "Page", "Broadcast", "Event")]
        public string NotificationMethod { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Notification Recipient")]
        public string NotificationRecipient { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Completion Status to Notify on")]
        [ValidateSet("Incomplete", "CompletedWithErrors", "Successful", "CompletedWithWarnings")]
        public string[] NotificationCompletion { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Email Notification Options")]
        [ValidateSet("DetailedInfo", "AttachDetailedLog", "CompressAttachment", "HtmlFormat")]
        public string[] NotificationEmailOptions { get; set; }

        protected override void ProcessWinFsBackupSet()
        {
            // Create a Data Source Browser
            DataSourceBrowser dataSourceBrowser = DSClientSession.createBrowser(EBackupDataType.EBackupDataType__FileSystem);

            // Try to resolve the supplied Computer
            string computer = ResolveWinComputer(Computer);
            computer = dataSourceBrowser.expandToFullPath(computer);
            WriteVerbose($"Notice: Specified Computer resolved to: {computer}");

            // Create the Backup Set Object
            DataBrowserWithSetCreation setCreation = DataBrowserWithSetCreation.from(dataSourceBrowser);
            BackupSet newBackupSet = setCreation.createBackupSet(computer);
            setCreation.Dispose();

            // Process the Common Backup Set Parameters
            newBackupSet = ProcessBaseBackupSetParams(MyInvocation.BoundParameters, newBackupSet);

            // Process Inclusion & Exclusion Items
            if (IncludeItem != null || ExcludeItem != null)
            {
                List<BackupSetItem> backupSetItems = new List<BackupSetItem>();

                if (ExcludeItem != null)
                    backupSetItems.AddRange(ProcessExclusionItems(DSClientSessionInfo.OperatingSystem, dataSourceBrowser, computer, ExcludeItem, ExcludeSubDirs));

                if (RegexExcludePattern != null)
                    backupSetItems.AddRange(ProcessRegexExclusionItems(dataSourceBrowser, computer, RegexExclusionPath, RegexMatchDirectory, RegexCaseInsensitive, RegexExcludePattern));

                if (IncludeItem != null)
                    backupSetItems.AddRange(ProcessWin32FSInclusionItems(dataSourceBrowser, computer, IncludeItem, MaxGenerations, ExcludeAltDataStreams, ExcludePermissions, ExcludeSubDirs));

                newBackupSet.setItems(backupSetItems.ToArray());
            }
            dataSourceBrowser.Dispose();

            // Set the Schedule and Retention Rules
            if (MyInvocation.BoundParameters.ContainsKey("ScheduleId"))
            {
                ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();
                Schedule schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);
                newBackupSet.setSchedule(schedule);
            }

            if (MyInvocation.BoundParameters.ContainsKey("RetentionRuleId"))
            {
                RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();
                RetentionRule[] retentionRules = DSClientRetentionRuleMgr.definedRules();
                RetentionRule retentionRule = retentionRules.Single(rule => rule.getID() == RetentionRuleId);
                newBackupSet.setRetentionRule(retentionRule);
            }

            // Process this Cmdlets specific configuration
            Win32FS_BackupSet newWin32BackupSet = ProcessWinFsBackupSetParams(MyInvocation.BoundParameters, Win32FS_BackupSet.from(newBackupSet));

            // Add the Backup Set to the DS-Client
            WriteVerbose("Performing Action: Add Backup Set Object to DS-Client");
            DSClientSession.addBackupSet(newWin32BackupSet);
            WriteVerbose($"Notice: Backup Set Created with BackupSetId: {newWin32BackupSet.getID()}");

            if (PassThru)
                WriteObject(new DSClientBackupSetBasicProps(newWin32BackupSet));

            newWin32BackupSet.Dispose();
        }
    }
}