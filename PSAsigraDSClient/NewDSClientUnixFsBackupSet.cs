using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientUnixFsBackupSet")]
    [OutputType(typeof(DSClientBackupSetBasicProps))]

    public class NewDSClientUnixFsBackupSet: BaseDSClientUnixFsBackupSet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The name of the Backup Set")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Computer the Backup Set will be assigned to")]
        [ValidateNotNullOrEmpty]
        public string Computer { get; set; }

        [Parameter(Position = 2, HelpMessage = "Credentials to use")]
        public PSCredential Credential { get; set; }

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

        protected override void ProcessUnixFsBackupSet()
        {
            // Create a Data Source Browser
            DataSourceBrowser dataSourceBrowser = DSClientSession.createBrowser(EBackupDataType.EBackupDataType__FileSystem);

            // Try to resolve the supplied Computer
            string computer = dataSourceBrowser.expandToFullPath(Computer);
            WriteVerbose("Specified Computer resolved to: " + computer);

            // Set the Credentials
            UnixFS_Generic_BackupSetCredentials backupSetCredentials = UnixFS_Generic_BackupSetCredentials.from(dataSourceBrowser.neededCredentials(computer));
            if (Credential != null)
                backupSetCredentials.setCredentials(Credential.UserName, Credential.GetNetworkCredential().Password);
            else
            {
                WriteVerbose("Credentials not specified, using DS-Client Credentials...");
                backupSetCredentials.setUsingClientCredentials(true);
            }
            dataSourceBrowser.setCurrentCredentials(backupSetCredentials);

            if (SSHKeyFile != null || SudoCredential != null || SSHInterpreter != null)
            {
                try
                {
                    UnixFS_SSH_BackupSetCredentials sshBackupSetCredentials = UnixFS_SSH_BackupSetCredentials.from(backupSetCredentials);

                    if (SSHInterpreter != null)
                    {
                        SSHAccesorType sshAccessType = StringToSSHAccesorType(SSHInterpreter);

                        sshBackupSetCredentials.setSSHAccessType(sshAccessType, SSHInterpreterPath);
                    }

                    if (SudoCredential != null)
                        sshBackupSetCredentials.setSudoAs(SudoCredential.UserName, SudoCredential.GetNetworkCredential().Password);

                    if (SSHKeyFile != null)
                        sshBackupSetCredentials.setCredentialsViaKeyFile(Credential.UserName, SSHKeyFile, Credential.GetNetworkCredential().Password);

                    dataSourceBrowser.setCurrentCredentials(sshBackupSetCredentials);

                    sshBackupSetCredentials.Dispose();
                }
                catch
                {
                    WriteWarning("Unable to set SSH Credential Options");
                }
            }
            else
                backupSetCredentials.Dispose();

            // Create the Backup Set Object
            DataBrowserWithSetCreation setCreation = DataBrowserWithSetCreation.from(dataSourceBrowser);
            BackupSet newBackupSet = setCreation.createBackupSet(computer);

            // Process the Common Backup Set Parameters
            newBackupSet = ProcessBaseBackupSetParams(MyInvocation.BoundParameters, newBackupSet);

            // Process Inclusion & Exclusion Items
            if (IncludeItem != null || ExcludeItem != null)
            {
                List<BackupSetItem> backupSetItems = new List<BackupSetItem>();

                if (ExcludeItem != null)
                    backupSetItems.AddRange(ProcessExclusionItems(DSClientOSType, dataSourceBrowser, computer, ExcludeItem));

                if (RegexExcludeItem != null)
                    backupSetItems.AddRange(ProcessRegexExclusionItems(dataSourceBrowser, computer, RegexExclusionPath, RegexExcludeDirectory, RegexCaseInsensitive, RegexExcludeItem));

                if (IncludeItem != null)
                {
                    foreach (string item in IncludeItem)
                    {
                        UnixFS_BackupSetInclusionItem inclusionItem = UnixFS_BackupSetInclusionItem.from(dataSourceBrowser.createInclusionItem(computer, item, MaxGenerations));

                        if (MyInvocation.BoundParameters.ContainsKey("ExcludeACLs"))
                            inclusionItem.setIncludingACL(false);
                        else
                            inclusionItem.setIncludingACL(true);

                        if (computer.Split('\\').First() == "Local File System")
                        {
                            if (MyInvocation.BoundParameters.ContainsKey("ExcludePosixACLs"))
                            {
                                UnixFS_LinuxLFS_BackupSetInclusionItem linuxInclusionItem = UnixFS_LinuxLFS_BackupSetInclusionItem.from(inclusionItem);
                                linuxInclusionItem.setIncludingPosixACL(false);
                            }
                            else
                            {
                                UnixFS_LinuxLFS_BackupSetInclusionItem linuxInclusionItem = UnixFS_LinuxLFS_BackupSetInclusionItem.from(inclusionItem);
                                linuxInclusionItem.setIncludingPosixACL(true);
                            }
                        }

                        backupSetItems.Add(inclusionItem);
                    }
                }

                newBackupSet.setItems(backupSetItems.ToArray());
            }

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
            UnixFS_Generic_BackupSet newUnixBackupSet = ProcessUnixFsBackupSetParams(MyInvocation.BoundParameters, UnixFS_Generic_BackupSet.from(newBackupSet));            

            // Add the Backup Set to the DS-Client
            WriteVerbose("Adding the new Backup Set Object to DS-Client...");
            DSClientSession.addBackupSet(newUnixBackupSet);
            WriteVerbose("Backup Set Created with BackupSetId: " + newUnixBackupSet.getID());

            if (PassThru)
                WriteObject(new DSClientBackupSetBasicProps(newUnixBackupSet));

            newUnixBackupSet.Dispose();
            dataSourceBrowser.Dispose();
        }
    }
}