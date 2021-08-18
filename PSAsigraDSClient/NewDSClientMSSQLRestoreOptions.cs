using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientMSSQLRestoreOptions")]
    [OutputType(typeof(RestoreOptions_MSSQLServer))]

    sealed public class NewDSClientMSSQLRestoreOptions : BaseDSClientRestoreOptions
    {
        [Parameter(HelpMessage = "Specify the Dump Method")]
        [ValidateSet("DumpToPipe", "DumpToClientBuffer", "DumpToSQLPath")]
        public string DumpMethod { get; set; }

        [Parameter(HelpMessage = "If using DumpToSQLPath, specify the path")]
        public string DumpPath { get; set; }

        [Parameter(HelpMessage = "Specify to ONLY Restore the Dump File")]
        public SwitchParameter RestoreDumpOnly { get; set; }

        [Parameter(HelpMessage = "Leave the Restored Database in Restoring Mode once restore is complete")]
        public SwitchParameter LeaveRestoringMode { get; set; }

        [Parameter(HelpMessage = "Preserve the Original Databases File Locations")]
        public SwitchParameter PreserveOriginalLocation { get; set; }

        protected override void ProcessRecord()
        {
            RestoreOptions_MSSQLServer restoreOptions = new RestoreOptions_MSSQLServer();

            if (MyInvocation.BoundParameters.ContainsKey(nameof(UseDetailedLog)))
                restoreOptions.SetUseDetailedLog(UseDetailedLog);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(LocalStorageMethod)))
                restoreOptions.SetLocalStorageMethod(LocalStorageMethod);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(DSSystemReadThreads)))
                restoreOptions.SetDSSystemReadThreads(DSSystemReadThreads);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(MaxPendingAsyncIO)))
                restoreOptions.SetMaxPendingIO(MaxPendingAsyncIO);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(DumpMethod)))
                restoreOptions.SetDumpMethod(DumpMethod);

            if (DumpMethod == "DumpToSQLPath" && !MyInvocation.BoundParameters.ContainsKey(nameof(DumpPath)))
                throw new System.Exception("Dump Path Must be Specified when DumpMethod is DumpToSQLPath");

            if (MyInvocation.BoundParameters.ContainsKey(nameof(DumpPath)))
                restoreOptions.SetDumpPath(DumpPath);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(RestoreDumpOnly)))
                restoreOptions.SetRestoreDumpOnly(RestoreDumpOnly);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(LeaveRestoringMode)))
                restoreOptions.SetLeaveRestoringMode(LeaveRestoringMode);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(PreserveOriginalLocation)))
                restoreOptions.SetPreserveOriginalLocation(PreserveOriginalLocation);

            WriteObject(restoreOptions);
        }
    }
}