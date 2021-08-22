using System.Management.Automation;
using static PSAsigraDSClient.DSClientRestoreOptions;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientFSRestoreOptions")]
    [OutputType(typeof(RestoreOptions_FileSystem), typeof(RestoreOptions_Win32FileSystem))]

    sealed public class NewDSClientFSRestoreOptions : BaseDSClientRestoreOptions
    {
        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the File System Overwrite Option")]
        [ValidateSet("RestoreAll", "RestoreNewer", "RestoreOlder", "RestoreDifferent", "SkipExisting")]
        public string FileOverwriteOption { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the File Restore Method")]
        [ValidateSet("Fast", "Save", "UseBuffer")]
        public string RestoreMethod { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify whether to Restore File Permissions")]
        [ValidateSet("Yes", "Skip", "Only")]
        public string RestorePermissions { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify if this is an Authoritative Restore (Windows Only)")]
        public SwitchParameter AuthoritativeRestore { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify whether to Overwrite Junction Points (Windows Only)")]
        public SwitchParameter OverwriteJunctionPoint { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify whether to Skip Restoring Offline Files")]
        public SwitchParameter SkipOfflineFiles { get; set; }

        [Parameter(ParameterSetName = "filesys", HelpMessage = "Create a Default File System Restore Option Object")]
        public SwitchParameter DefaultFileSystemOptions { get; set; }

        [Parameter(ParameterSetName = "winfilesys", HelpMessage = "Create a Default Windows File System Restore Option Object")]
        public SwitchParameter DefaultWinFileSystemOptions { get; set; }

        protected override void ProcessRecord()
        {
            RestoreOptions_FileSystem restoreOptions = new RestoreOptions_FileSystem();

            if (!DefaultFileSystemOptions && !DefaultWinFileSystemOptions)
            {
                if (MyInvocation.BoundParameters.ContainsKey(nameof(UseDetailedLog)))
                    restoreOptions.SetUseDetailedLog(UseDetailedLog);

                if (MyInvocation.BoundParameters.ContainsKey(nameof(LocalStorageMethod)))
                    restoreOptions.SetLocalStorageMethod(LocalStorageMethod);

                if (MyInvocation.BoundParameters.ContainsKey(nameof(DSSystemReadThreads)))
                    restoreOptions.SetDSSystemReadThreads(DSSystemReadThreads);

                if (MyInvocation.BoundParameters.ContainsKey(nameof(MaxPendingAsyncIO)))
                    restoreOptions.SetMaxPendingIO(MaxPendingAsyncIO);

                if (MyInvocation.BoundParameters.ContainsKey(nameof(FileOverwriteOption)))
                    restoreOptions.SetOverwriteOption(FileOverwriteOption);

                if (MyInvocation.BoundParameters.ContainsKey(nameof(RestoreMethod)))
                    restoreOptions.SetRestoreMethod(RestoreMethod);

                if (MyInvocation.BoundParameters.ContainsKey(nameof(RestorePermissions)))
                    restoreOptions.SetRestorePermissions(RestorePermissions);

                if (MyInvocation.BoundParameters.ContainsKey(nameof(AuthoritativeRestore)) ||
                    MyInvocation.BoundParameters.ContainsKey(nameof(OverwriteJunctionPoint)) ||
                    MyInvocation.BoundParameters.ContainsKey(nameof(SkipOfflineFiles)))
                {
                    RestoreOptions_Win32FileSystem win32restoreOptions = RestoreOptions_Win32FileSystem.From(restoreOptions);

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(AuthoritativeRestore)))
                        win32restoreOptions.SetAuthoritative(AuthoritativeRestore);

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(OverwriteJunctionPoint)))
                        win32restoreOptions.SetOverwriteJunctionPoint(OverwriteJunctionPoint);

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(SkipOfflineFiles)))
                        win32restoreOptions.SetSkipOfflineFile(SkipOfflineFiles);

                    WriteObject(win32restoreOptions);
                }
                else
                {
                    WriteObject(restoreOptions);
                }
            }
            else if (DefaultWinFileSystemOptions)
            {
                WriteObject(new RestoreOptions_Win32FileSystem());
            }
            else
            {
                WriteObject(restoreOptions);
            }
        }
    }
}