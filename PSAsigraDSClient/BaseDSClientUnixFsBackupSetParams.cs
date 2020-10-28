using System;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientUnixFsBackupSetParams: BaseDSClientBackupSetParams
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
    }
}