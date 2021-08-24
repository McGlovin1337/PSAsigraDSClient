using System.Management.Automation;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientRestoreOptions : PSCmdlet
    {
        [Parameter(ParameterSetName = "default", HelpMessage = "Specify to use Detailed Log")]
        public SwitchParameter UseDetailedLog { get; set; }

        [Parameter(ParameterSetName = "default", HelpMessage = "Specify Local Storage Handling")]
        [ValidateSet("None", "ConnectFirst", "ConnectIfNeeded", "ContinueIfDisconnect")]
        public string LocalStorageMethod { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Set the number of DS-System Read Threads")]
        public int DSSystemReadThreads { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Set the maximum pending Asynchronious IO")]
        public int MaxPendingAsyncIO { get; set; }
    }
}