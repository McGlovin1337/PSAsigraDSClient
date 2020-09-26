using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;
using static PSAsigraDSClient.BaseDSClientNotification;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientBackupSet")]

    public class NewDSClientBackupSet: BaseDSClientBackupSet, IDynamicParameters
    {
        private Win32FSBackupSetParams win32FSBackupSetParams = null;
        private UnixFSBackupSetParams unixFSBackupSetParams = null;

        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The name of the Backup Set")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Computer the Backup Set will be assigned to")]
        [ValidateNotNullOrEmpty]
        public string Computer { get; set; }

        [Parameter(Position = 2, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The type of Data this Backup Set will protect")]
        [ValidateSet("WindowsFileSystem", "UnixFileSystem")]
        public string DataType { get; set; }

        [Parameter(Position = 3, ValueFromPipelineByPropertyName = true, HelpMessage = "Credentials to use")]
        public PSCredential Credential { get; set; }

        [Parameter(Position = 4, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Backup Set Type")]
        [ValidateSet("Offsite", "Statistical", "SelfContained", "LocalOnly")]
        public string SetType { get; set; }

        [Parameter(Position = 5, ValueFromPipelineByPropertyName = true, HelpMessage = "Items to Include in Backup Set")]
        public string[] IncludeItem { get; set; }

        [Parameter(Position = 6, ValueFromPipelineByPropertyName = true, HelpMessage = "Max Number of Generations for Included Items")]
        public int MaxGenerations { get; set; }

        [Parameter(Position = 7, ValueFromPipelineByPropertyName = true, HelpMessage = "Items to Exclude from Backup Set")]
        public string[] ExcludeItem { get; set; }

        [Parameter(Position = 8, ValueFromPipelineByPropertyName = true, Mandatory = true, HelpMessage = "Set the Compression Method to use")]
        [ValidateSet("None", "ZLIB", "LZOP", "ZLIB_LO", "ZLIB_MED", "ZLIB_HI")]
        public string Compression { get; set; }

        [Parameter(Position = 9, ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Schedule this Backup Set will use")]
        public int ScheduleId { get; set; }

        [Parameter(Position = 10, ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Retention Rule this Backup Set will use")]
        public int RetentionRuleId { get; set; }

        [Parameter(Position = 11, ValueFromPipelineByPropertyName = true, HelpMessage = "Schedule Priority of Backup Set when assigned to Schedule")]
        public int SchedulePriority { get; set; } = 1;

        [Parameter(Position = 12, HelpMessage = "Force Re-Backup of File even if it hasn't been modified")]
        public SwitchParameter ForceBackup { get; set; }

        [Parameter(Position = 13, HelpMessage = "Set to PreScan before Backup")]
        public SwitchParameter PreScan { get; set; }

        [Parameter(Position = 14, ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Read Buffer Size")]
        public int ReadBufferSize { get; set; } = 0;

        [Parameter(Position = 15, ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Backup Error limit")]
        public int BackupErrorLimit { get; set; } = 0;

        [Parameter(Position = 16, HelpMessage = "Set to use Detailed Log")]
        public SwitchParameter UseDetailedLog { get; set; }

        [Parameter(Position = 17, HelpMessage = "Set to use Infinate BLM Generations")]
        public SwitchParameter InfinateBLMGenerations { get; set; }

        [Parameter(Position = 18, HelpMessage = "Set to use Local Storage")]
        public SwitchParameter UseLocalStorage { get; set; }

        [Parameter(Position = 19, ValueFromPipelineByPropertyName = true, HelpMessage = "Local Storage Path For Local Backups and Cache")]
        public string LocalStoragePath { get; set; }

        [Parameter(Position = 20, HelpMessage = "Set to use Local Transmission Cache for Offsite Backup Sets")]
        public SwitchParameter UseTransmissionCache { get; set; }

        [Parameter(Position = 21, ValueFromPipelineByPropertyName = true, HelpMessage = "Notification Method")]
        [ValidateSet("Email", "Pager", "Broadcast", "Event")]
        public string NotificationMethod { get; set; }

        [Parameter(Position = 22, ValueFromPipelineByPropertyName = true, HelpMessage = "Notification Recipient")]
        public string NotificationRecipient { get; set; }

        [Parameter(Position = 23, ValueFromPipelineByPropertyName = true, HelpMessage = "Completion Status to Notify on")]
        [ValidateSet("Incomplete", "CompletedWithErrors", "Successful", "CompletedWithWarnings")]
        public string[] NotificationCompletion { get; set; }

        [Parameter(Position = 24, ValueFromPipelineByPropertyName = true, HelpMessage = "Email Notification Options")]
        [ValidateSet("DetailedInfo", "AttachDetailedLog", "CompressAttachment", "HtmlFormat")]
        public string[] NotificationEmailOptions { get; set; }

        [Parameter(Position = 25, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Completion Status to send SNMP Traps")]
        [ValidateSet("Incomplete", "CompletedWithErrors", "Successful", "CompletedWithWarnings")]
        public string[] SnmpTrapNotifications { get; set; }

        public object GetDynamicParameters()
        {
            switch(DataType)
            {
                case "WindowsFileSystem":
                    win32FSBackupSetParams = new Win32FSBackupSetParams();
                    return win32FSBackupSetParams;
                case "UnixFileSystem":
                    unixFSBackupSetParams = new UnixFSBackupSetParams();
                    return unixFSBackupSetParams;
            }

            return null;
        }

        private class UnixFSBackupSetParams
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

            [Parameter(HelpMessage = "Enable Continuous Data Protection (CDP)")]
            public SwitchParameter UseCDP { get; set; }

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
        }

        private class Win32FSBackupSetParams
        {
            [Parameter(HelpMessage = "Specify to Backup Remote Storage")]
            public SwitchParameter BackupRemoteStorage { get; set; }

            [Parameter(HelpMessage = "Specify to Backup Single Instance Store")]
            public SwitchParameter BackupSingleInstanceStore { get; set; }

            [Parameter(HelpMessage = "Specify Common File Elimination")]
            public SwitchParameter CheckCommonFiles { get; set; }

            [Parameter(HelpMessage = "Enable Continuous Data Protection (CDP)")]
            public SwitchParameter UseCDP { get; set; }

            [Parameter(HelpMessage = "Use Volume Shadow Copies (VSS)")]
            public SwitchParameter UseVSS { get; set; }

            [Parameter(HelpMessage = "Exclude VSS Components")]
            public SwitchParameter ExcludeVSSComponents { get; set; }

            [Parameter(HelpMessage = "Ignore VSS Components")]
            public SwitchParameter IgnoreVSSComponents {get; set;}

            [Parameter(HelpMessage = "Ignore VSS Writers")]
            public SwitchParameter IgnoreVSSWriters { get; set; }

            [Parameter(HelpMessage = "Specify to follow Junction Points")]
            public SwitchParameter FollowJunctionPoints { get; set; }

            [Parameter(HelpMessage = "No Automatic File Filter")]
            public SwitchParameter NoAutoFileFilter { get; set; }

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

            [Parameter(HelpMessage = "Include Alternate Data Streams for IncludedItems")]
            public SwitchParameter ExcludeAltDataStreams { get; set; }

            [Parameter(HelpMessage = "Include Permissions for IncludedItems")]
            public SwitchParameter ExcludePermissions { get; set; }
        }

        protected override void DSClientProcessRecord()
        {
            // Validate Parameters before creating a new BackupSet object
            if (DataType == "WindowsFileSystem" && DSClientOSType.OsType != "Windows")
                throw new ParameterBindingException("Windows File System Backup Sets can only be created on a Windows DS-Client");

            if (DataType == "UnixFileSystem" && DSClientOSType.OsType != "Linux")
                throw new ParameterBindingException("Unix File System Backup Sets can only be created on a Linux DS-Client");

            if (MyInvocation.BoundParameters.ContainsKey("ExcludeOldFilesByDate") && MyInvocation.BoundParameters.ContainsKey("ExcludeOldFilesByTimeSpan"))
                throw new ParameterBindingException("ExcludeOldFilesByDate and ExcludeOldFilesByTimeSpan cannot both be specified");

            if (MyInvocation.BoundParameters.ContainsKey("ExcludeOldFilesByDate") && win32FSBackupSetParams.ExcludeOldFilesDate == null)
                throw new ParameterBindingException("A Date for ExcludeOldFilesDate must be specified when ExcludeOldFilesByDate is enabled");

            if (MyInvocation.BoundParameters.ContainsKey("ExcludeOldFilesByTimeSpan") && (win32FSBackupSetParams.ExcludeOldFilesTimeSpan == null || win32FSBackupSetParams.ExcludeOldFilesTimeSpanValue < 1))
                throw new ParameterBindingException("A Time Span and Time Span Value must be specified when ExcludeOldFilesByTimeSpan is enabled");

            if ((SetType == "SelfContained" || SetType == "LocalOnly" || UseTransmissionCache == true) && LocalStoragePath == null)
                throw new ParameterBindingException("Local Backups and Transmission Cache require a Local Storage Path");

            if (unixFSBackupSetParams != null)
                if (unixFSBackupSetParams.SSHInterpreter == "Direct" && unixFSBackupSetParams.SSHInterpreterPath == null)
                    throw new ParameterBindingException("Direct SSH Interpretor requires an SSH Interpretor Path");

            WriteVerbose("Building the Backup Set Object...");

            string dataType = DataType;

            if (DataType == "WindowsFileSystem" || DataType == "UnixFileSystem")
                dataType = "FileSystem";

            DataSourceBrowser dataSourceBrowser = DSClientSession.createBrowser(StringToEBackupDataType(dataType));
            BackupSetCredentials backupSetCredentials = dataSourceBrowser.neededCredentials(Computer);

            // Set Credentials
            if (DSClientOSType.OsType == "Windows")
            {                
                Win32FS_Generic_BackupSetCredentials win32FSBSCredentials = Win32FS_Generic_BackupSetCredentials.from(backupSetCredentials);

                if (Credential != null)
                {
                    string user = Credential.UserName;
                    string pass = Credential.GetNetworkCredential().Password;
                    win32FSBSCredentials.setCredentials(user, pass);
                }
                else
                {
                    WriteVerbose("Credentials not specified, using DS-Client Credentials...");
                    win32FSBSCredentials.setUsingClientCredentials(true);
                }

                dataSourceBrowser.setCurrentCredentials(win32FSBSCredentials);
            }

            if (DSClientOSType.OsType == "Linux")
            {
                UnixFS_Generic_BackupSetCredentials unixFSBackupSetCredentials = UnixFS_Generic_BackupSetCredentials.from(backupSetCredentials);

                if (Credential != null)
                {
                    string user = Credential.UserName;
                    string pass = Credential.GetNetworkCredential().Password;

                    if (unixFSBackupSetParams.SSHKeyFile != null)
                    {
                        UnixFS_SSH_BackupSetCredentials unixFSSSHBackupSetCredentials = UnixFS_SSH_BackupSetCredentials.from(unixFSBackupSetCredentials);

                        if (unixFSBackupSetParams.SSHInterpreter != null)
                        {
                            SSHAccesorType sshAccessType = StringToSSHAccesorType(unixFSBackupSetParams.SSHInterpreter);

                            unixFSSSHBackupSetCredentials.setSSHAccessType(sshAccessType, unixFSBackupSetParams.SSHInterpreterPath);
                        }

                        if (unixFSBackupSetParams.SudoCredential != null)
                        {
                            string sudoUser = unixFSBackupSetParams.SudoCredential.UserName;
                            string sudoPass = unixFSBackupSetParams.SudoCredential.GetNetworkCredential().Password;

                            unixFSSSHBackupSetCredentials.setSudoAs(sudoUser, sudoPass);
                        }

                        unixFSSSHBackupSetCredentials.setCredentialsViaKeyFile(user, unixFSBackupSetParams.SSHKeyFile, pass);
                    }
                    else
                    {
                        unixFSBackupSetCredentials.setCredentials(user, pass);
                    }
                }
                else
                {
                    WriteVerbose("Credentials not specified, using DS-Client Credentials...");
                    unixFSBackupSetCredentials.setUsingClientCredentials(true);
                }
            }

            // Create the BackupSet object
            DataBrowserWithSetCreation setCreation = DataBrowserWithSetCreation.from(dataSourceBrowser);
            BackupSet NewBackupSet = setCreation.createBackupSet(Computer);

            NewBackupSet.setName(Name);
            NewBackupSet.setComputerName(Computer);

            EBackupSetType setType = StringToEBackupSetType(SetType);
            NewBackupSet.setSetType(setType);

            ECompressionType compressionType = StringToECompressionType(Compression);
            NewBackupSet.setCompressionType(compressionType);

            if (MyInvocation.BoundParameters.ContainsKey("ScheduleId"))
            {
                ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();
                Schedule schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);
                NewBackupSet.setSchedule(schedule);
            }

            if (MyInvocation.BoundParameters.ContainsKey("RetentionRuleId"))
            {
                RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();
                RetentionRule[] retentionRules = DSClientRetentionRuleMgr.definedRules();
                RetentionRule retentionRule = retentionRules.Single(rule => rule.getID() == RetentionRuleId);
                NewBackupSet.setRetentionRule(retentionRule);
            }

            if (LocalStoragePath != null)
                NewBackupSet.setLocalStoragePath(LocalStoragePath);

            NewBackupSet.setSchedulePriority(SchedulePriority);
            NewBackupSet.setReadBufferSize(ReadBufferSize);
            NewBackupSet.setBackupErrorLimit(BackupErrorLimit);
            NewBackupSet.setForceBackup(ForceBackup);
            NewBackupSet.setPreScanByDefault(PreScan);
            NewBackupSet.setUsingDetailedLog(UseDetailedLog);
            NewBackupSet.setUsingInfBLMGen(InfinateBLMGenerations);
            NewBackupSet.setUsingLocalStorage(UseLocalStorage);
            NewBackupSet.setUsingLocalTransmissionCache(UseTransmissionCache);

            // Backup Set Inclusion Items
            if (IncludeItem != null || ExcludeItem != null)
            {
                List<BackupSetItem> backupSetItems = new List<BackupSetItem>();

                if (IncludeItem != null)
                {
                    foreach (var item in IncludeItem)
                    {
                        BackupSetInclusionItem inclusionItem = dataSourceBrowser.createInclusionItem(Computer, item, MaxGenerations);

                        if (DataType == "WindowsFileSystem")
                        {
                            Win32FS_BackupSetInclusionItem win32InclusionItem = Win32FS_BackupSetInclusionItem.from(inclusionItem);

                            if (MyInvocation.BoundParameters.ContainsKey("ExcludeAltDataStreams"))
                                win32InclusionItem.setIncludingAlternateDataStreams(false);
                            else
                                win32InclusionItem.setIncludingAlternateDataStreams(true);

                            if (MyInvocation.BoundParameters.ContainsKey("ExcludePermissions"))
                                win32InclusionItem.setIncludingPermissions(false);
                            else
                                win32InclusionItem.setIncludingPermissions(true);
                        }

                        if (DataType == "UnixFileSystem")
                        {
                            UnixFS_BackupSetInclusionItem unixInclusionItem = UnixFS_BackupSetInclusionItem.from(inclusionItem);

                            if (MyInvocation.BoundParameters.ContainsKey("ExcludeACLs"))
                                unixInclusionItem.setIncludingACL(false);
                            else
                                unixInclusionItem.setIncludingACL(true);

                            if (MyInvocation.BoundParameters.ContainsKey("ExcludePosixACLs"))
                            {
                                UnixFS_LinuxLFS_BackupSetInclusionItem linuxInclusionItem = UnixFS_LinuxLFS_BackupSetInclusionItem.from(unixInclusionItem);
                                linuxInclusionItem.setIncludingPosixACL(false);
                            }
                            else
                            {
                                UnixFS_LinuxLFS_BackupSetInclusionItem linuxInclusionItem = UnixFS_LinuxLFS_BackupSetInclusionItem.from(unixInclusionItem);
                                linuxInclusionItem.setIncludingPosixACL(true);
                            }
                        }

                        backupSetItems.Add(inclusionItem);
                    }
                }

                // Backup Set Exclusion Items
                if (ExcludeItem != null)
                {
                    foreach (var item in ExcludeItem)
                    {
                        BackupSetFileItem exclusionItem = dataSourceBrowser.createExclusionItem(Computer, item);
                        backupSetItems.Add(exclusionItem);
                    }
                }

                NewBackupSet.setItems(backupSetItems.ToArray());
            }

            // Backup Set Notification Configuration
            if (NotificationMethod != null)
            {
                notification_info notificationInfo = new notification_info
                {
                    completion = ArrayToNotificationCompletionToInt(NotificationCompletion),
                    email_option = (NotificationEmailOptions != null) ? ArrayToEmailOptionsInt(NotificationEmailOptions) : 0,
                    id = 0,
                    method = StringToENotificationMethod(NotificationMethod),
                    recipient = NotificationRecipient
                };

                BackupSetNotification backupSetNotification = NewBackupSet.getNotification();
                backupSetNotification.addOrUpdateNotification(notificationInfo);
                backupSetNotification.Dispose();
            }

            if (SnmpTrapNotifications != null)
                NewBackupSet.setSNMPTrapsConditions(ArrayToNotificationCompletionToInt(SnmpTrapNotifications));

            // Windows File System specific configuration
            if (DataType == "WindowsFileSystem")
            {
                Win32FS_BackupSet NewWin32FSBS = Win32FS_BackupSet.from(NewBackupSet);

                if (MyInvocation.BoundParameters.ContainsKey("BackupRemoteStorage"))
                    NewWin32FSBS.setBackupRemoteStorage(win32FSBackupSetParams.BackupRemoteStorage);

                if (MyInvocation.BoundParameters.ContainsKey("BackupSingleInstanceStore"))
                    NewWin32FSBS.setBackupSingleInstanceStore(win32FSBackupSetParams.BackupSingleInstanceStore);

                if (MyInvocation.BoundParameters.ContainsKey("CheckCommonFiles"))
                    NewWin32FSBS.setCheckingCommonFiles(win32FSBackupSetParams.CheckCommonFiles);

                if (MyInvocation.BoundParameters.ContainsKey("UseCDP"))
                    NewWin32FSBS.setContinuousDataProtection(win32FSBackupSetParams.UseCDP);

                if (MyInvocation.BoundParameters.ContainsKey("UseVSS"))
                    NewWin32FSBS.setUseVSS(win32FSBackupSetParams.UseVSS);

                if (MyInvocation.BoundParameters.ContainsKey("ExcludeVSSComponents"))
                    NewWin32FSBS.setExcludeVSSComponents(win32FSBackupSetParams.ExcludeVSSComponents);

                if (MyInvocation.BoundParameters.ContainsKey("IgnoreVSSComponents"))
                    NewWin32FSBS.setIgnoreVSSComponents(win32FSBackupSetParams.IgnoreVSSComponents);

                if (MyInvocation.BoundParameters.ContainsKey("IgnoreVSSWriters"))
                    NewWin32FSBS.setIgnoreVSSWriters(win32FSBackupSetParams.IgnoreVSSWriters);

                if (MyInvocation.BoundParameters.ContainsKey("FollowJunctionPoints"))
                    NewWin32FSBS.setFollowJunctionPoint(win32FSBackupSetParams.FollowJunctionPoints);

                if (MyInvocation.BoundParameters.ContainsKey("NoAutoFileFilter"))
                    NewWin32FSBS.setNoAutomaticFileFilter(win32FSBackupSetParams.NoAutoFileFilter);

                if (MyInvocation.BoundParameters.ContainsKey("ExcludeOldFilesByDate"))
                {
                    old_file_exclusion_config exclusionConfig = new old_file_exclusion_config
                    {
                        type = EOldFileExclusionType.EOldFileExclusionType__Date,
                        value = DateTimeToUnixEpoch(win32FSBackupSetParams.ExcludeOldFilesDate)
                    };

                    NewWin32FSBS.setOldFileExclusionOption(exclusionConfig);
                }

                if (MyInvocation.BoundParameters.ContainsKey("ExcludeOldFilesByTimeSpan"))
                {
                    old_file_exclusion_config exclusionConfig = new old_file_exclusion_config
                    {
                        type = EOldFileExclusionType.EOldFileExclusionType__TimeSpan,
                        unit = StringToETimeUnit(win32FSBackupSetParams.ExcludeOldFilesTimeSpan),
                        value = win32FSBackupSetParams.ExcludeOldFilesTimeSpanValue
                    };

                    NewWin32FSBS.setOldFileExclusionOption(exclusionConfig);
                }

                if (MyInvocation.BoundParameters.ContainsKey("UseBuffer"))
                    NewWin32FSBS.setUsingBuffer(win32FSBackupSetParams.UseBuffer);
            }

            // Unix/Linux File System specific configuration
            if (DataType == "UnixFileSystem")
            {
                UnixFS_Generic_BackupSet NewUnixFSBS = UnixFS_Generic_BackupSet.from(NewBackupSet);

                if (MyInvocation.BoundParameters.ContainsKey("CheckCommonFiles"))
                    NewUnixFSBS.setCheckingCommonFiles(unixFSBackupSetParams.CheckCommonFiles);

                if (MyInvocation.BoundParameters.ContainsKey("UseCDP"))
                    NewUnixFSBS.setContinuousDataProtection(unixFSBackupSetParams.UseCDP);

                if (MyInvocation.BoundParameters.ContainsKey("ForceBackup"))
                    NewUnixFSBS.setOption(EBackupSetOption.EBackupSetOption__ForceBackup, ForceBackup);

                if (MyInvocation.BoundParameters.ContainsKey("FollowMountPoints"))
                    NewUnixFSBS.setOption(EBackupSetOption.EBackupSetOption__SSHFollowLink, unixFSBackupSetParams.FollowMountPoints);

                if (MyInvocation.BoundParameters.ContainsKey("BackupHardLinks"))
                    NewUnixFSBS.setOption(EBackupSetOption.EBackupSetOption__HardLink, unixFSBackupSetParams.BackupHardLinks);

                if (MyInvocation.BoundParameters.ContainsKey("IgnoreSnapshotFailure"))
                    NewUnixFSBS.setOption(EBackupSetOption.EBackupSetOption__SnapshotFailure, unixFSBackupSetParams.IgnoreSnapshotFailure);

                if (MyInvocation.BoundParameters.ContainsKey("UseSnapDiff"))
                    NewUnixFSBS.setOption(EBackupSetOption.EBackupSetOption__UseSnapDiff, unixFSBackupSetParams.UseSnapDiff);

                if (MyInvocation.BoundParameters.ContainsKey("ExcludeOldFilesByDate"))
                {
                    old_file_exclusion_config exclusionConfig = new old_file_exclusion_config
                    {
                        type = EOldFileExclusionType.EOldFileExclusionType__Date,
                        value = DateTimeToUnixEpoch(unixFSBackupSetParams.ExcludeOldFilesDate)
                    };

                    NewUnixFSBS.setOldFileExclusionOption(exclusionConfig);
                }

                if (MyInvocation.BoundParameters.ContainsKey("ExcludeOldFilesByTimeSpan"))
                {
                    old_file_exclusion_config exclusionConfig = new old_file_exclusion_config
                    {
                        type = EOldFileExclusionType.EOldFileExclusionType__TimeSpan,
                        unit = StringToETimeUnit(unixFSBackupSetParams.ExcludeOldFilesTimeSpan),
                        value = win32FSBackupSetParams.ExcludeOldFilesTimeSpanValue
                    };

                    NewUnixFSBS.setOldFileExclusionOption(exclusionConfig);
                }

                if (MyInvocation.BoundParameters.ContainsKey("UseBuffer"))
                    NewUnixFSBS.setUsingBuffer(unixFSBackupSetParams.UseBuffer);
            }

            // Add the Backup Set to the DS-Client
            WriteVerbose("Adding the new Backup Set Object to DS-Client...");
            DSClientSession.addBackupSet(NewBackupSet);
            WriteObject("Backup Set Created with BackupSetId: " + NewBackupSet.getID());

            NewBackupSet.Dispose();
            dataSourceBrowser.Dispose();
        }
    }
}