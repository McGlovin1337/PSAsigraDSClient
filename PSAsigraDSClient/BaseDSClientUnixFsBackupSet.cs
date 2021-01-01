using AsigraDSClientApi;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientUnixFsBackupSet: BaseDSClientBackupSetParams
    {
        [Parameter(HelpMessage = "Specify the SSH Interpreter to access the data")]
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

        protected abstract void ProcessUnixFsBackupSet();

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

            ProcessUnixFsBackupSet();
        }

        protected static UnixFS_Generic_BackupSet ProcessUnixFsBackupSetParams(Dictionary<string, object> unixfsParams, UnixFS_Generic_BackupSet backupSet)
        {
            unixfsParams.TryGetValue("CheckCommonFiles", out object CheckCommonFiles);
            if (CheckCommonFiles != null)
                backupSet.setCheckingCommonFiles(Convert.ToBoolean(CheckCommonFiles.ToString()));

            unixfsParams.TryGetValue("ForceBackup", out object ForceBackup);
            if (ForceBackup != null)
                backupSet.setOption(EBackupSetOption.EBackupSetOption__ForceBackup, Convert.ToBoolean(ForceBackup.ToString()));

            unixfsParams.TryGetValue("FollowMountPoints", out object FollowMountPoints);
            if (FollowMountPoints != null)
                backupSet.setOption(EBackupSetOption.EBackupSetOption__SSHFollowLink, Convert.ToBoolean(FollowMountPoints.ToString()));

            unixfsParams.TryGetValue("BackupHardLinks", out object BackupHardLinks);
            if (BackupHardLinks != null)
                backupSet.setOption(EBackupSetOption.EBackupSetOption__HardLink, Convert.ToBoolean(BackupHardLinks.ToString()));

            unixfsParams.TryGetValue("IgnoreSnapshotFailure", out object IgnoreSnapshotFailure);
            if (IgnoreSnapshotFailure != null)
                backupSet.setOption(EBackupSetOption.EBackupSetOption__SnapshotFailure, Convert.ToBoolean(IgnoreSnapshotFailure.ToString()));

            unixfsParams.TryGetValue("UseSnapDiff", out object UseSnapDiff);
            if (UseSnapDiff != null)
                backupSet.setOption(EBackupSetOption.EBackupSetOption__UseSnapDiff, Convert.ToBoolean(UseSnapDiff.ToString()));

            unixfsParams.TryGetValue("UseBuffer", out object UseBuffer);
            if (UseBuffer != null)
                backupSet.setUsingBuffer(Convert.ToBoolean(UseBuffer.ToString()));

            unixfsParams.TryGetValue("ExcludeOldFilesByDate", out object ExcludeOldFilesByDate);
            if (ExcludeOldFilesByDate != null)
            {
                old_file_exclusion_config exclusionConfig = new old_file_exclusion_config
                {
                    type = EOldFileExclusionType.EOldFileExclusionType__Date,
                    value = DateTimeToUnixEpoch(DateTime.Parse(ExcludeOldFilesByDate.ToString()))
                };

                backupSet.setOldFileExclusionOption(exclusionConfig);
            }

            unixfsParams.TryGetValue("ExcludeOldFilesByTimeSpan", out object ExcludeOldFilesByTimeSpan);
            if (ExcludeOldFilesByTimeSpan != null)
            {
                unixfsParams.TryGetValue("ExcludeOldFilesTimeSpan", out object ExcludeOldFilesTimeSpan);
                unixfsParams.TryGetValue("ExcludeOldFilesTimeSpanValue", out object ExcludeOldFilesTimeSpanValue);

                old_file_exclusion_config exclusionConfig = new old_file_exclusion_config
                {
                    type = EOldFileExclusionType.EOldFileExclusionType__TimeSpan,
                    unit = StringToEnum<ETimeUnit>(ExcludeOldFilesTimeSpan as string),
                    value = (ExcludeOldFilesTimeSpanValue as int?).GetValueOrDefault()
                };

                backupSet.setOldFileExclusionOption(exclusionConfig);
            }

            unixfsParams.TryGetValue("CDPInterval", out object CDPInterval);
            if (CDPInterval != null)
            {
                unixfsParams.TryGetValue("CDPStoppedChangingForInterval", out object CDPStoppedChangingForInterval);
                unixfsParams.TryGetValue("CDPStopForRetention", out object CDPStopForRetention);
                bool cdpRetention = Convert.ToBoolean(CDPStopForRetention.ToString());
                unixfsParams.TryGetValue("CDPStopForBLM", out object CDPStopForBLM);
                bool cdpBLM = Convert.ToBoolean(CDPStopForBLM.ToString());
                unixfsParams.TryGetValue("CDPStopForValidation", out object CDPStopForValidation);
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