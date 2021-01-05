using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientWinFsRestore")]

    public class StartDSClientWinFsRestore: BaseDSClientFileSystemRestore
    {
        [Parameter(HelpMessage = "Specify Max Pending Asynchronous I/O per File")]
        public int MaxPendingAsyncIO { get; set; } = 0;

        [Parameter(HelpMessage = "Specify the number of DS-System Read Threads")]
        public int ReadThreads { get; set; } = 0;

        [Parameter(HelpMessage = "Specify AD (System State) Authoritative Restore")]
        public SwitchParameter AuthoritativeRestore { get; set; }

        [Parameter(HelpMessage = "Specify if to Overwrite Junction Points")]
        public SwitchParameter OverwriteJunctionPoint { get; set; }

        [Parameter(HelpMessage = "Specify if to Skip Offline Files (stubs)")]
        public SwitchParameter SkipOfflineFiles { get; set; }

        protected override void ProcessFileSystemRestore(string computer, IEnumerable<share_mapping> shareMappings, DataSourceBrowser dataSourceBrowser, FS_RestoreActivityInitiator restoreActivityInitiator)
        {
            Win32FS_RestoreActivityInitiator win32FSRestoreActivityInitiator = Win32FS_RestoreActivityInitiator.from(restoreActivityInitiator);

            win32FSRestoreActivityInitiator.setMaxPendingAsyncIO(MaxPendingAsyncIO);
            win32FSRestoreActivityInitiator.setDSSystemReadThreads(ReadThreads);

            if (AuthoritativeRestore || OverwriteJunctionPoint || SkipOfflineFiles)
            {
                win32FSRestoreActivityInitiator.setAuthoritativeRestore(AuthoritativeRestore);
                win32FSRestoreActivityInitiator.setOverwriteJunctionPoint(OverwriteJunctionPoint);
                win32FSRestoreActivityInitiator.setSkipOfflineFiles(SkipOfflineFiles);
            }

            // Initiate the restore
            WriteVerbose("Performing Action: Initiate Restore Request");
            GenericActivity restoreActivity = win32FSRestoreActivityInitiator.startRestore(dataSourceBrowser, computer, shareMappings.ToArray());

            WriteObject($"Started Backup Set Restore Activity with ActivityId: {restoreActivity.getID()}");

            restoreActivity.Dispose();
            win32FSRestoreActivityInitiator.Dispose();
        }
    }
}