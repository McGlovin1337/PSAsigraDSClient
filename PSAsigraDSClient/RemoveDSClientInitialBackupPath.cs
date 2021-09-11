using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientInitialBackupPath", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class RemoveDSClientInitialBackupPath : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "id", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the PathId to Remove")]
        public int PathId { get; set; }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "path", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Path to Remove")]
        public string Path { get; set; }

        protected override void DSClientProcessRecord()
        {
            InitialBackupManager initialBackupManager = DSClientSession.getInitialBackupManager();

            init_backup_path_info pathInfo = null;

            // Select the specified path from the defined paths
            if (MyInvocation.BoundParameters.ContainsKey("PathId"))
            {
                WriteVerbose($"Performing Action: Select Initial Backup Path with Id '{PathId}'");
                pathInfo = initialBackupManager.definedPaths()
                                                .Single(path => path.pathID == PathId);
            }
            else if (Path != null)
            {
                WriteVerbose($"Performing Action: Select Initial Backup Path with Path '{Path}'");
                pathInfo = initialBackupManager.definedPaths()
                                                .Single(path => path.path == Path);
            }

            // Remove the selected path
            if (pathInfo != null)
            {
                if (ShouldProcess($"{pathInfo.path}", "Remove Initial Backup Path"))
                {
                    initialBackupManager.removePath(pathInfo.path);
                }
            }

            initialBackupManager.Dispose();
        }
    }
}