using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientMSSqlServerBackupSetItem")]

    public class AddDSClientMSSqlServerBackupSetItem: BaseDSClientBackupSet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set to Add items to")]
        public int BackupSetId { get; set; }

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

        [Parameter(HelpMessage = "Specify to run Database Consistency Check DBCC")]
        public SwitchParameter RunDBCC { get; set; }

        [Parameter(HelpMessage = "Specify to Stop on DBCC Errors")]
        public SwitchParameter DBCCErrorStop { get; set; }

        [Parameter(HelpMessage = "Specify to Backup Transaction Log")]
        public SwitchParameter BackupLog { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Check DS-Client is Windows
            if (DSClientOSType.OsType != "Windows")
                throw new Exception("MS SQL Server Backup Sets can only be created on a Windows DS-Client");

            // Get the requested Backup Set from DS-Client
            WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId: {BackupSetId}");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);
            string computer = backupSet.getComputerName();

            // Create a Data Source Browser
            DataSourceBrowser dataSourceBrowser = backupSet.dataBrowser();

            // Create a List of Items
            List<BackupSetItem> backupSetItems = new List<BackupSetItem>();

            // Process any Exclusion Items
            if (ExcludeItem != null)
                backupSetItems.AddRange(ProcessBasicExclusionItems(dataSourceBrowser, computer, ExcludeItem));

            if (RegexExcludeItem != null)
                backupSetItems.AddRange(ProcessRegexExclusionItems(dataSourceBrowser, computer, RegexExclusionPath, RegexExcludeDirectory, RegexCaseInsensitive, RegexExcludeItem));

            // Process any Inclusion Items
            if (IncludeItem != null)
                backupSetItems.AddRange(ProcessMsSqlInclusionItems(dataSourceBrowser, computer, IncludeItem, MaxGenerations, BackupLog, RunDBCC, DBCCErrorStop));

            // Get the existing specified items and store in the list
            backupSetItems.AddRange(backupSet.items());

            // Strip any duplicates from the list, duplicates cause an error and wipes all the items from the Backup Set
            backupSetItems = backupSetItems.Distinct(new BackupSetItemComparer()).ToList();

            // Add all the items to the Backup Set
            WriteVerbose("Performing Action: Add Items to Backup Set");
            backupSet.setItems(backupSetItems.ToArray());

            dataSourceBrowser.Dispose();
            backupSet.Dispose();
        }
    }
}