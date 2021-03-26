using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientVMwareVADPBackupSet")]

    public class NewDSClientVMwareVADPBackupSet : BaseDSClientVMwareVADPBackupSet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The name of the Backup Set")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Computer the Backup Set will be Assigned To")]
        [ValidateNotNullOrEmpty]
        public string Computer { get; set; }

        [Parameter(Position = 3, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Backup Set Type")]
        [ValidateSet("Offsite", "Statistical", "SelfContained", "LocalOnly")]
        public string SetType { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Items to Include in Backup Set")]
        public string[] IncludeItem { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Max Number of Generations for Included Items")]
        public int MaxGenerations { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Items to Exclude from Backup Set")]
        public string[] ExcludeItem { get; set; }

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

        protected override void ProcessVADPSet()
        {
            // Create a DataSourceBrowser
            DataSourceBrowser dataSourceBrowser = DSClientSession.createBrowser(EBackupDataType.EBackupDataType__VMwareVADP);

            // Attempt Computer Name Resolution
            string computer = dataSourceBrowser.expandToFullPath(Computer);
            WriteVerbose($"Notice: Specified Computer resolved to: {computer}");

            // Set Computer Credentials
            Win32FS_Generic_BackupSetCredentials backupSetCredentials = Win32FS_Generic_BackupSetCredentials.from(dataSourceBrowser.neededCredentials(computer));

            if (Credential != null)
            {
                string user = Credential.UserName;
                string pass = Credential.GetNetworkCredential().Password;
                backupSetCredentials.setCredentials(user, pass);
            }
            else
            {
                WriteVerbose("Notice: Credentials not specified, using DS-Client Credentials");
                backupSetCredentials.setUsingClientCredentials(true);
            }
            dataSourceBrowser.setCurrentCredentials(backupSetCredentials);

            backupSetCredentials.Dispose();

            // Create Backup Set Object
            DataBrowserWithSetCreation setCreation = DataBrowserWithSetCreation.from(dataSourceBrowser);
            BackupSet newBackupSet = setCreation.createBackupSet(computer);
            setCreation.Dispose();

            // Process Common Backup Set Parameters
            newBackupSet = ProcessBaseBackupSetParams(MyInvocation.BoundParameters, newBackupSet);

            VMwareVADP_BackupSet newVMwareVADPBackupSet = VMwareVADP_BackupSet.from(newBackupSet);

            // Process Inclusion & Exclusion Items
            if (IncludeItem != null || ExcludeItem != null)
            {
                List<BackupSetItem> backupSetItems = new List<BackupSetItem>();

                if (ExcludeItem != null)
                    backupSetItems.AddRange(ProcessBasicExclusionItems(dataSourceBrowser, computer, ExcludeItem, ExcludeSubDirs));

                if (IncludeItem != null)
                    backupSetItems.AddRange(ProcessVMwareVADPInclusionItem(dataSourceBrowser, computer, IncludeItem, MaxGenerations, ExcludeSubDirs));

                newVMwareVADPBackupSet.setItems(backupSetItems.ToArray());
            }
            dataSourceBrowser.Dispose();

            // Set the Schedule and Retention Rules
            if (MyInvocation.BoundParameters.ContainsKey("ScheduleId"))
            {
                ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();
                Schedule schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);
                newVMwareVADPBackupSet.setSchedule(schedule);
            }

            if (MyInvocation.BoundParameters.ContainsKey("RetentionRuleId"))
            {
                RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();
                RetentionRule[] retentionRules = DSClientRetentionRuleMgr.definedRules();
                RetentionRule retentionRule = retentionRules.Single(rule => rule.getID() == RetentionRuleId);
                newVMwareVADPBackupSet.setRetentionRule(retentionRule);
            }

            // Process this Cmdlets specific Parameters
            vmware_additional_options vmwareOptions = new vmware_additional_options
            {
                valBackupP2VIncrementally = IncrementalP2VBackup,
                valBackupVMmemory = BackupVMMemory,
                valQuiesceIOBeforeSnap = SnapshotQuiesce,
                valSnapAllVMSameTime = SameTimeSnapshot,
                valUsingCBT = UseCBT,
                valUsingFLR = UseFLR,
                valUsingLocalVDR = UseLocalVDR,
                valVMLibVersion = StringToEnum<EVMwareLibraryVersion>(VMLibraryVersion)
            };
            newVMwareVADPBackupSet.setAdditionalVMwareOptions(vmwareOptions);

            newVMwareVADPBackupSet.setUsingBuffer(UseBuffer);

            // Add the Backup Set to the DS-Client
            WriteVerbose("Performing Action: Add Backup Set Object to DS-Client");
            DSClientSession.addBackupSet(newVMwareVADPBackupSet);
            WriteVerbose($"Notice: Backup Set Created with BackupSetId: {newVMwareVADPBackupSet.getID()}");

            if (PassThru)
                WriteObject(new DSClientBackupSetBasicProps(newVMwareVADPBackupSet));

            newVMwareVADPBackupSet.Dispose();
        }
    }
}