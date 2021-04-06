using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientMSSqlServerBackupSetItem")]

    public class AddDSClientMSSqlServerBackupSetItem: BaseDSClientBackupSetItemParams
    {
        [Parameter(ParameterSetName = "inclusion", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Item Filter")]
        [Parameter(ParameterSetName = "exclusion")]
        [Parameter(Mandatory = true, ParameterSetName = "regex")]
        [ValidateNotNull]
        [Alias("Expression", "Item")]
        public new string Filter { get; set; }

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

            if (Filter == null)
                Filter = "";

            if (Exclusion)
                backupSetItems.Add(CreateExclusionItem(dataSourceBrowser, computer, Path, Filter, ExcludeSubDirs));
            else if (RegexExclusion)
                backupSetItems.Add(CreateRegexExclusion(dataSourceBrowser, computer, Path, Filter, RegexMatchDirectory, RegexCaseInsensitive));
            else if (Inclusion)
                backupSetItems.Add(CreateMSSqlBackupSetItem(dataSourceBrowser, computer, Path, Filter, MaxGenerations, BackupLog, RunDBCC, DBCCErrorStop, ExcludeSubDirs));


            // Get the existing specified items and store in the list
            backupSetItems.AddRange(backupSet.items());

            // Strip any duplicates from the list, duplicates cause an error and wipes all the items from the Backup Set
            backupSetItems = backupSetItems.Distinct(new BackupSetItemComparer()).ToList();

            // Add all the items to the Backup Set
            WriteVerbose("Performing Action: Add Items to Backup Set");
            backupSet.setItems(backupSetItems.ToArray());

            foreach (BackupSetItem item in backupSetItems)
                item.Dispose();

            dataSourceBrowser.Dispose();
            backupSet.Dispose();
        }
    }
}