using System.Collections.Generic;
using System.Linq;
using AsigraDSClientApi;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientUnixFsRestore")]

    public class StartDSClientUnixFsRestore: BaseDSClientFileSystemRestore
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

        protected override void ProcessFileSystemRestore(string computer, IEnumerable<share_mapping> shareMappings, DataSourceBrowser dataSourceBrowser, FS_RestoreActivityInitiator restoreActivityInitiator)
        {
            if (SSHInterpreter != null || SudoCredential != null || SSHKeyFile != null)
            {
                try
                {
                    UnixFS_SSH_BackupSetCredentials unixFSSSHBackupSetCredentials = UnixFS_SSH_BackupSetCredentials.from(dataSourceBrowser.getCurrentCredentials());

                    if (SSHInterpreter != null)
                    {
                        SSHAccesorType sshAccessType = BaseDSClientBackupSet.StringToSSHAccesorType(SSHInterpreter);

                        unixFSSSHBackupSetCredentials.setSSHAccessType(sshAccessType, SSHInterpreterPath);
                    }

                    if (SudoCredential != null)
                        unixFSSSHBackupSetCredentials.setSudoAs(SudoCredential.UserName, SudoCredential.GetNetworkCredential().Password);

                    if (SSHKeyFile != null)
                        unixFSSSHBackupSetCredentials.setCredentialsViaKeyFile(Credential.UserName, SSHKeyFile, Credential.GetNetworkCredential().Password);

                    dataSourceBrowser.setCurrentCredentials(unixFSSSHBackupSetCredentials);

                    unixFSSSHBackupSetCredentials.Dispose();
                }
                catch
                {
                    WriteWarning("Unable to set SSH Credential Options");
                }
            }

            // Initiate the restore
            WriteVerbose("Initiating the Restore Request...");
            GenericActivity restoreActivity = restoreActivityInitiator.startRestore(dataSourceBrowser, computer, shareMappings.ToArray());

            WriteObject("Started Backup Set Restore Activity with ActivityId: " + restoreActivity.getID());

            restoreActivity.Dispose();
        }
    }
}