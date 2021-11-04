using AsigraDSClientApi;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientWinFsBackupSet: BaseDSClientBackupSetParams
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

        protected abstract void ProcessWinFsBackupSet();

        protected override void DSClientProcessRecord()
        {
            // Check DS-Client is Windows
            if (DSClientSessionInfo.OperatingSystem != "Windows")
            {
                ErrorRecord errorRecord = new ErrorRecord(
                    new PlatformNotSupportedException("Windows FileSystem Backup Sets can only be created on a Windows DS-Client"),
                    "PlatformNotSupportedException",
                    ErrorCategory.InvalidOperation,
                    null);
                WriteError(errorRecord);
            }
            else
            {
                // Validate the Common Base Parameters
                BaseBackupSetParamValidation(MyInvocation.BoundParameters);

                // Validate Parameters specific to this Cmdlet
                if (MyInvocation.BoundParameters.ContainsKey("ExcludeOldFilesByDate") && ExcludeOldFilesDate == null)
                    throw new ParameterBindingException("A Date for ExcludeOldFilesDate must be specified when ExcludeOldFilesByDate is enabled");

                if (MyInvocation.BoundParameters.ContainsKey("ExcludeOldFilesByTimeSpan") && (ExcludeOldFilesTimeSpan == null || ExcludeOldFilesTimeSpanValue < 1))
                    throw new ParameterBindingException("A Time Span and Time Span Value must be specified when ExcludeOldFilesByTimeSpan is enabled");

                ProcessWinFsBackupSet();
            }
        }

        protected static Win32FS_BackupSet ProcessWinFsBackupSetParams(Dictionary<string, object> winfsParams, Win32FS_BackupSet backupSet)
        {
            winfsParams.TryGetValue("BackupRemoteStorage", out object BackupRemoteStorage);
            if (BackupRemoteStorage != null)
                backupSet.setBackupRemoteStorage(Convert.ToBoolean(BackupRemoteStorage.ToString()));

            winfsParams.TryGetValue("BackupSingleInstanceStore", out object BackupSingleInstanceStore);
            if (BackupSingleInstanceStore != null)
                backupSet.setBackupSingleInstanceStore(Convert.ToBoolean(BackupSingleInstanceStore.ToString()));

            winfsParams.TryGetValue("CheckCommonFiles", out object CheckCommonFiles);
            if (CheckCommonFiles != null)
                backupSet.setCheckingCommonFiles(Convert.ToBoolean(CheckCommonFiles.ToString()));

            winfsParams.TryGetValue("UseVSS", out object UseVSS);
            if (UseVSS != null)
                backupSet.setUseVSS(Convert.ToBoolean(UseVSS.ToString()));

            winfsParams.TryGetValue("ExcludeVSSComponents", out object ExcludeVSSComponents);
            if (ExcludeVSSComponents != null)
                backupSet.setExcludeVSSComponents(Convert.ToBoolean(ExcludeVSSComponents.ToString()));

            winfsParams.TryGetValue("IgnoreVSSComponents", out object IgnoreVSSComponents);
            if (IgnoreVSSComponents != null)
                backupSet.setIgnoreVSSComponents(Convert.ToBoolean(IgnoreVSSComponents.ToString()));

            winfsParams.TryGetValue("IgnoreVSSWriters", out object IgnoreVSSWriters);
            if (IgnoreVSSWriters != null)
                backupSet.setIgnoreVSSWriters(Convert.ToBoolean(IgnoreVSSWriters.ToString()));

            winfsParams.TryGetValue("FollowJunctionPoints", out object FollowJunctionPoints);
            if (FollowJunctionPoints != null)
                backupSet.setFollowJunctionPoint(Convert.ToBoolean(FollowJunctionPoints.ToString()));

            winfsParams.TryGetValue("NoAutoFileFilter", out object NoAutoFileFilter);
            if (NoAutoFileFilter != null)
                backupSet.setNoAutomaticFileFilter(Convert.ToBoolean(NoAutoFileFilter.ToString()));

            winfsParams.TryGetValue("UseBuffer", out object UseBuffer);
            if (UseBuffer != null)
                backupSet.setUsingBuffer(Convert.ToBoolean(UseBuffer.ToString()));

            winfsParams.TryGetValue("ExcludeOldFilesByDate", out object ExcludeOldFilesByDate);
            if (ExcludeOldFilesByDate != null)
            {
                old_file_exclusion_config exclusionConfig = new old_file_exclusion_config
                {
                    type = EOldFileExclusionType.EOldFileExclusionType__Date,
                    value = DateTimeToUnixEpoch(DateTime.Parse(ExcludeOldFilesByDate.ToString()))
                };

                backupSet.setOldFileExclusionOption(exclusionConfig);
            }

            winfsParams.TryGetValue("ExcludeOldFilesByTimeSpan", out object ExcludeOldFilesByTimeSpan);
            if (ExcludeOldFilesByTimeSpan != null)
            {
                winfsParams.TryGetValue("ExcludeOldFilesTimeSpan", out object ExcludeOldFilesTimeSpan);
                winfsParams.TryGetValue("ExcludeOldFilesTimeSpanValue", out object ExcludeOldFilesTimeSpanValue);

                old_file_exclusion_config exclusionConfig = new old_file_exclusion_config
                {
                    type = EOldFileExclusionType.EOldFileExclusionType__TimeSpan,
                    unit = StringToEnum<ETimeUnit>(ExcludeOldFilesTimeSpan as string),
                    value = (ExcludeOldFilesTimeSpanValue as int?).GetValueOrDefault()
                };

                backupSet.setOldFileExclusionOption(exclusionConfig);
            }

            winfsParams.TryGetValue("CDPInterval", out object CDPInterval);
            if (CDPInterval != null)
            {
                winfsParams.TryGetValue("CDPStoppedChangingForInterval", out object CDPStoppedChangingForInterval);
                winfsParams.TryGetValue("CDPStopForRetention", out object CDPStopForRetention);
                bool cdpRetention = Convert.ToBoolean(CDPStopForRetention.ToString());
                winfsParams.TryGetValue("CDPStopForBLM", out object CDPStopForBLM);
                bool cdpBLM = Convert.ToBoolean(CDPStopForBLM.ToString());
                winfsParams.TryGetValue("CDPStopForValidation", out object CDPStopForValidation);
                bool cdpValidation = Convert.ToBoolean(CDPStopForValidation.ToString());

                CDP_settings cdpSettings = new CDP_settings
                {
                    backup_check_interval = (CDPInterval as int?).GetValueOrDefault(),
                    backup_strategy = (Convert.ToBoolean(CDPStoppedChangingForInterval.ToString())) ? ECDPBackupStrategy.ECDPBackupStrategy__BackupStopChangingFor : ECDPBackupStrategy.ECDPBackupStrategy__BackupNotOftenThan,
                    file_change_detection_type = ECDPFileChangeDetectionType.ECDPFileChangeDetectionType__WinBuiltInMonitor,
                    suspendable_activities = SwitchParamsToECDPSuspendableScheduledActivityInt(cdpRetention, cdpBLM, cdpValidation)
                };
                backupSet.setCDPSettings(cdpSettings);

                backupSet.setContinuousDataProtection(true);
            }

            return backupSet;
        }
    }
}