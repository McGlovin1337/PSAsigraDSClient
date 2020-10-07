using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientWinFsBackupSet")]

    public class NewDSClientWinFsBackupSet: BaseDSClientBackupSetParams
    {
        [Parameter(HelpMessage = "Specify to Backup Remote Storage")]
        public SwitchParameter BackupRemoteStorage { get; set; }

        [Parameter(HelpMessage = "Specify to Backup Single Instance Store")]
        public SwitchParameter BackupSingleInstanceStore { get; set; }

        [Parameter(HelpMessage = "Specify Common File Elimination")]
        public SwitchParameter CheckCommonFiles { get; set; }

        [Parameter(HelpMessage = "Use Volume Shadow Copies (VSS)")]
        public SwitchParameter UseVSS { get; set; }

        [Parameter(HelpMessage = "Exclude VSS Components")]
        public SwitchParameter ExcludeVSSComponents { get; set; }

        [Parameter(HelpMessage = "Ignore VSS Components")]
        public SwitchParameter IgnoreVSSComponents { get; set; }

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

        protected override void DSClientProcessRecord()
        {
            // Check DS-Client is Windows
            if (DSClientOSType.OsType != "Windows")
                throw new Exception("Windows FileSystem Backup Sets can only be created on a Windows DS-Client");

            // Validate the Common Base Parameters
            BaseBackupSetParamValidation(MyInvocation.BoundParameters);

            // Validate Parameters specific to this Cmdlet
            if (MyInvocation.BoundParameters.ContainsKey("ExcludeOldFilesByDate") && ExcludeOldFilesDate == null)
                throw new ParameterBindingException("A Date for ExcludeOldFilesDate must be specified when ExcludeOldFilesByDate is enabled");

            if (MyInvocation.BoundParameters.ContainsKey("ExcludeOldFilesByTimeSpan") && (ExcludeOldFilesTimeSpan == null || ExcludeOldFilesTimeSpanValue < 1))
                throw new ParameterBindingException("A Time Span and Time Span Value must be specified when ExcludeOldFilesByTimeSpan is enabled");

            // Create a Data Source Browser
            DataSourceBrowser dataSourceBrowser = DSClientSession.createBrowser(EBackupDataType.EBackupDataType__FileSystem);

            // Set the Credentials
            Win32FS_Generic_BackupSetCredentials backupSetCredentials = Win32FS_Generic_BackupSetCredentials.from(dataSourceBrowser.neededCredentials(Computer));            

            if (Credential != null)
            {
                string user = Credential.UserName;
                string pass = Credential.GetNetworkCredential().Password;
                backupSetCredentials.setCredentials(user, pass);
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
                    backupSetItems.AddRange(ProcessExclusionItems(dataSourceBrowser, Computer, ExcludeItem));

                if (RegexExcludeItem != null)
                    backupSetItems.AddRange(ProcessRegexExclusionItems(dataSourceBrowser, Computer, RegexExclusionPath, RegexExcludeDirectory, RegexCaseInsensitive, RegexExcludeItem));

                if (IncludeItem != null)
                    backupSetItems.AddRange(ProcessWin32FSInclusionItems(dataSourceBrowser, Computer, IncludeItem, MaxGenerations, ExcludeAltDataStreams, ExcludePermissions));

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
            Win32FS_BackupSet newWin32BackupSet = Win32FS_BackupSet.from(newBackupSet);

            if (MyInvocation.BoundParameters.ContainsKey("BackupRemoteStorage"))
                newWin32BackupSet.setBackupRemoteStorage(BackupRemoteStorage);

            if (MyInvocation.BoundParameters.ContainsKey("BackupSingleInstanceStore"))
                newWin32BackupSet.setBackupSingleInstanceStore(BackupSingleInstanceStore);

            if (MyInvocation.BoundParameters.ContainsKey("CheckCommonFiles"))
                newWin32BackupSet.setCheckingCommonFiles(CheckCommonFiles);

            if (MyInvocation.BoundParameters.ContainsKey("UseVSS"))
                newWin32BackupSet.setUseVSS(UseVSS);

            if (MyInvocation.BoundParameters.ContainsKey("ExcludeVSSComponents"))
                newWin32BackupSet.setExcludeVSSComponents(ExcludeVSSComponents);

            if (MyInvocation.BoundParameters.ContainsKey("IgnoreVSSComponents"))
                newWin32BackupSet.setIgnoreVSSComponents(IgnoreVSSComponents);

            if (MyInvocation.BoundParameters.ContainsKey("IgnoreVSSWriters"))
                newWin32BackupSet.setIgnoreVSSWriters(IgnoreVSSWriters);

            if (MyInvocation.BoundParameters.ContainsKey("FollowJunctionPoints"))
                newWin32BackupSet.setFollowJunctionPoint(FollowJunctionPoints);

            if (MyInvocation.BoundParameters.ContainsKey("NoAutoFileFilter"))
                newWin32BackupSet.setNoAutomaticFileFilter(NoAutoFileFilter);

            if (MyInvocation.BoundParameters.ContainsKey("ExcludeOldFilesByDate"))
            {
                old_file_exclusion_config exclusionConfig = new old_file_exclusion_config
                {
                    type = EOldFileExclusionType.EOldFileExclusionType__Date,
                    value = DateTimeToUnixEpoch(ExcludeOldFilesDate)
                };

                newWin32BackupSet.setOldFileExclusionOption(exclusionConfig);
            }

            if (MyInvocation.BoundParameters.ContainsKey("ExcludeOldFilesByTimeSpan"))
            {
                old_file_exclusion_config exclusionConfig = new old_file_exclusion_config
                {
                    type = EOldFileExclusionType.EOldFileExclusionType__TimeSpan,
                    unit = StringToETimeUnit(ExcludeOldFilesTimeSpan),
                    value = ExcludeOldFilesTimeSpanValue
                };

                newWin32BackupSet.setOldFileExclusionOption(exclusionConfig);
            }

            if (MyInvocation.BoundParameters.ContainsKey("UseBuffer"))
                newWin32BackupSet.setUsingBuffer(UseBuffer);

            // CDP Configuration
            if (MyInvocation.BoundParameters.ContainsKey("CDPInterval"))
            {
                CDP_settings cdpSettings = new CDP_settings
                {
                    backup_check_interval = CDPInterval,
                    backup_strategy = (CDPStoppedChangingForInterval) ? ECDPBackupStrategy.ECDPBackupStrategy__BackupStopChangingFor : ECDPBackupStrategy.ECDPBackupStrategy__BackupNotOftenThan,
                    file_change_detection_type = ECDPFileChangeDetectionType.ECDPFileChangeDetectionType__WinBuiltInMonitor,
                    suspendable_activities = SwitchParamsToECDPSuspendableScheduledActivityInt(CDPStopForRetention, CDPStopForBLM, CDPStopForValidation)
                };
                newWin32BackupSet.setCDPSettings(cdpSettings);

                newWin32BackupSet.setContinuousDataProtection(true);
            }

            // Add the Backup Set to the DS-Client
            WriteVerbose("Adding the new Backup Set Object to DS-Client...");
            DSClientSession.addBackupSet(newWin32BackupSet);
            WriteObject("Backup Set Created with BackupSetId: " + newWin32BackupSet.getID());

            newWin32BackupSet.Dispose();
            dataSourceBrowser.Dispose();
        }
    }
}