using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientUnixFsBackupSet")]

    public class NewDSClientUnixFsBackupSet: BaseDSClientBackupSetParams
    {
        [Parameter(HelpMessage = "Specify the SSH Iterpreter to access the data")]
        [ValidateSet("Perl", "Python", "Direct")]
        public string SSHInterpreter { get; set; }

        [Parameter(HelpMessage = "Specify SSH Interpreter path")]
        public string SSHInterpreterPath { get; set; }

        [Parameter(HelpMessage = "Specify path to SSH Key File")]
        public string SSHKeyFile { get; set; }

        [Parameter(HelpMessage = "Specify SUDO User Credentials")]
        public PSCredential SudoCredential { get; set; }

        [Parameter(HelpMessage = "Specify Common File Elimination")]
        public SwitchParameter CheckCommonFiles { get; set; }

        [Parameter(HelpMessage = "Specify to Follow Mount Points")]
        public SwitchParameter FollowMountPoints { get; set; }

        [Parameter(HelpMessage = "Backup Hard Links")]
        public SwitchParameter BackupHardLinks { get; set; }

        [Parameter(HelpMessage = "Ignore Snapshot Failures")]
        public SwitchParameter IgnoreSnapshotFailure { get; set; }

        [Parameter(HelpMessage = "For NAS Use Snap Diff feature")]
        public SwitchParameter UseSnapDiff { get; set; }

        [Parameter(HelpMessage = "Old File Exclusions by Date")]
        public SwitchParameter ExcludeOldFilesByDate { get; set; }

        [Parameter(HelpMessage = "Date for Old File Exclusions")]
        public DateTime ExcludeOldFilesDate { get; set; }

        [Parameter(HelpMessage = "Old File Exclusions by TimeSpan")]
        public SwitchParameter ExcludeOldFilesByTimeSpan { get; set; }

        [Parameter(HelpMessage = "TimeSpan Unit to use for Old File Exclusions")]
        [ValidateSet("Seconds", "Minutes", "Hours", "Days", "Weeks", "Months", "Years")]
        public string ExcludeOldFilesTimeSpan { get; set; }

        [Parameter(HelpMessage = "TimeSpan Value to use for Old File Exclusions")]
        public int ExcludeOldFilesTimeSpanValue { get; set; }

        [Parameter(HelpMessage = "Use DS-Client Buffer")]
        public SwitchParameter UseBuffer { get; set; }

        [Parameter(HelpMessage = "Exclude ACLs")]
        public SwitchParameter ExcludeACLs { get; set; }

        [Parameter(HelpMessage = "Exclude POSIX ACLs")]
        public SwitchParameter ExcludePosixACLs { get; set; }

        [Parameter(HelpMessage = "Specify the CDP Backup Interval in Seconds")]
        public int CDPInterval { get; set; }

        [Parameter(HelpMessage = "CDP Backup File When File Stopped Changing for interval duration")]
        public SwitchParameter CDPStoppedChangingForInterval { get; set; }

        [Parameter(HelpMessage = "Specify CDP Backup can be Suspended for Scheduled Retention")]
        public SwitchParameter CDPStopForRetention { get; set; }

        [Parameter(HelpMessage = "Specify CDP Backup can be Suspended for Scheduled BLM")]
        public SwitchParameter CDPStopForBLM { get; set; }

        [Parameter(HelpMessage = "Specify CDP Backup can be Suspended for Scheduled Validation")]
        public SwitchParameter CDPStopForValidation { get; set; }

        [Parameter(HelpMessage = "Specify CDP Linux to use File Alteration Monitor")]
        public SwitchParameter CDPUseFileAlterationMonitor { get; set; }

        protected override void DSClientProcessRecord()
        {
            //Check DS-Client is Linux/Unix
            if (DSClientOSType.OsType != "Linux")
                throw new Exception("Unix FileSystem Backup Sets can only be created on a Unix DS-Client");

            // Validate the Common Base Parameters
            BaseBackupSetParamValidation(MyInvocation.BoundParameters);

            // Validate Parameters specific to this Cmdlet
            if (MyInvocation.BoundParameters.ContainsKey("ExcludeOldFilesByDate") && ExcludeOldFilesDate == null)
                throw new ParameterBindingException("A Date for ExcludeOldFilesDate must be specified when ExcludeOldFilesByDate is enabled");

            if (MyInvocation.BoundParameters.ContainsKey("ExcludeOldFilesByTimeSpan") && (ExcludeOldFilesTimeSpan == null || ExcludeOldFilesTimeSpanValue < 1))
                throw new ParameterBindingException("A Time Span and Time Span Value must be specified when ExcludeOldFilesByTimeSpan is enabled");

            if (SSHInterpreter == "Direct" && SSHInterpreterPath == null)
                throw new ParameterBindingException("Direct SSH Interpretor requires an SSH Interpretor Path");

            // Create a Data Source Browser
            DataSourceBrowser dataSourceBrowser = DSClientSession.createBrowser(EBackupDataType.EBackupDataType__FileSystem);

            // Set the Credentials
            UnixFS_Generic_BackupSetCredentials backupSetCredentials = UnixFS_Generic_BackupSetCredentials.from(dataSourceBrowser.neededCredentials(Computer));

            if (Credential != null)
            {
                string user = Credential.UserName;
                string pass = Credential.GetNetworkCredential().Password;

                if (SSHKeyFile != null)
                {
                    UnixFS_SSH_BackupSetCredentials sshBackupSetCredentials = UnixFS_SSH_BackupSetCredentials.from(backupSetCredentials);

                    if (SSHInterpreter != null)
                    {
                        SSHAccesorType sshAccessType = StringToSSHAccesorType(SSHInterpreter);

                        sshBackupSetCredentials.setSSHAccessType(sshAccessType, SSHInterpreterPath);
                    }

                    if (SudoCredential != null)
                    {
                        string sudoUser = SudoCredential.UserName;
                        string sudoPass = SudoCredential.GetNetworkCredential().Password;

                        sshBackupSetCredentials.setSudoAs(sudoUser, sudoPass);
                    }

                    sshBackupSetCredentials.setCredentialsViaKeyFile(user, SSHKeyFile, pass);
                }
                else
                {
                    backupSetCredentials.setCredentials(user, pass);
                }
            }
            else
            {
                WriteVerbose("Credentials not specified, using DS-Client Credentials...");
                backupSetCredentials.setUsingClientCredentials(true);
            }
            dataSourceBrowser.setCurrentCredentials(backupSetCredentials);

            // Create the Backup Set Object
            DataBrowserWithSetCreation setCreation = DataBrowserWithSetCreation.from(dataSourceBrowser);
            BackupSet newBackupSet = setCreation.createBackupSet(Computer);

            // Process the Common Backup Set Parameters
            newBackupSet = ProcessBaseBackupSetParams(MyInvocation.BoundParameters, newBackupSet);

            // Process Inclusion & Exclusion Items
            if (IncludeItem != null || ExcludeItem != null)
            {
                List<BackupSetItem> backupSetItems = new List<BackupSetItem>();

                if (ExcludeItem != null)
                    backupSetItems.AddRange(ProcessExclusionItems(DSClientOSType, dataSourceBrowser, Computer, ExcludeItem));

                if (RegexExcludeItem != null)
                    backupSetItems.AddRange(ProcessRegexExclusionItems(dataSourceBrowser, Computer, RegexExclusionPath, RegexExcludeDirectory, RegexCaseInsensitive, RegexExcludeItem));

                if (IncludeItem != null)
                {
                    foreach (string item in IncludeItem)
                    {
                        UnixFS_BackupSetInclusionItem inclusionItem = UnixFS_BackupSetInclusionItem.from(dataSourceBrowser.createInclusionItem(Computer, item, MaxGenerations));

                        if (MyInvocation.BoundParameters.ContainsKey("ExcludeACLs"))
                            inclusionItem.setIncludingACL(false);
                        else
                            inclusionItem.setIncludingACL(true);

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
            UnixFS_Generic_BackupSet newUnixBackupSet = UnixFS_Generic_BackupSet.from(newBackupSet);

            if (MyInvocation.BoundParameters.ContainsKey("CheckCommonFiles"))
                newUnixBackupSet.setCheckingCommonFiles(CheckCommonFiles);

            if (MyInvocation.BoundParameters.ContainsKey("ForceBackup"))
                newUnixBackupSet.setOption(EBackupSetOption.EBackupSetOption__ForceBackup, ForceBackup);

            if (MyInvocation.BoundParameters.ContainsKey("FollowMountPoints"))
                newUnixBackupSet.setOption(EBackupSetOption.EBackupSetOption__SSHFollowLink, FollowMountPoints);

            if (MyInvocation.BoundParameters.ContainsKey("BackupHardLinks"))
                newUnixBackupSet.setOption(EBackupSetOption.EBackupSetOption__HardLink, BackupHardLinks);

            if (MyInvocation.BoundParameters.ContainsKey("IgnoreSnapshotFailure"))
                newUnixBackupSet.setOption(EBackupSetOption.EBackupSetOption__SnapshotFailure, IgnoreSnapshotFailure);

            if (MyInvocation.BoundParameters.ContainsKey("UseSnapDiff"))
                newUnixBackupSet.setOption(EBackupSetOption.EBackupSetOption__UseSnapDiff, UseSnapDiff);

            if (MyInvocation.BoundParameters.ContainsKey("ExcludeOldFilesByDate"))
            {
                old_file_exclusion_config exclusionConfig = new old_file_exclusion_config
                {
                    type = EOldFileExclusionType.EOldFileExclusionType__Date,
                    value = DateTimeToUnixEpoch(ExcludeOldFilesDate)
                };

                newUnixBackupSet.setOldFileExclusionOption(exclusionConfig);
            }

            if (MyInvocation.BoundParameters.ContainsKey("ExcludeOldFilesByTimeSpan"))
            {
                old_file_exclusion_config exclusionConfig = new old_file_exclusion_config
                {
                    type = EOldFileExclusionType.EOldFileExclusionType__TimeSpan,
                    unit = StringToETimeUnit(ExcludeOldFilesTimeSpan),
                    value = ExcludeOldFilesTimeSpanValue
                };

                newUnixBackupSet.setOldFileExclusionOption(exclusionConfig);
            }

            if (MyInvocation.BoundParameters.ContainsKey("UseBuffer"))
                newUnixBackupSet.setUsingBuffer(UseBuffer);

            // CDP Configuration
            if (MyInvocation.BoundParameters.ContainsKey("CDPInterval"))
            {
                CDP_settings cdpSettings = new CDP_settings
                {
                    backup_check_interval = CDPInterval,
                    backup_strategy = (CDPStoppedChangingForInterval) ? ECDPBackupStrategy.ECDPBackupStrategy__BackupStopChangingFor : ECDPBackupStrategy.ECDPBackupStrategy__BackupNotOftenThan,
                    file_change_detection_type = (CDPUseFileAlterationMonitor) ? ECDPFileChangeDetectionType.ECDPFileChangeDetectionType__FileAlterationMonitor : ECDPFileChangeDetectionType.ECDPFileChangeDetectionType__GenericScanner,
                    suspendable_activities = SwitchParamsToECDPSuspendableScheduledActivityInt(CDPStopForRetention, CDPStopForBLM, CDPStopForValidation)
                };
                newUnixBackupSet.setCDPSettings(cdpSettings);

                newUnixBackupSet.setContinuousDataProtection(true);
            }

            // Add the Backup Set to the DS-Client
            WriteVerbose("Adding the new Backup Set Object to DS-Client...");
            DSClientSession.addBackupSet(newUnixBackupSet);
            WriteObject("Backup Set Created with BackupSetId: " + newUnixBackupSet.getID());

            newUnixBackupSet.Dispose();
            dataSourceBrowser.Dispose();
        }
    }
}