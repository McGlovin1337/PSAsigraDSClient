using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommunications.Read, "DSClientUnixFsSource")]
    [OutputType(typeof(SourceItemInfo))]

    public class ReadDSClientUnixFsSource: BaseDSClientBackupSource
    {
        [Parameter(HelpMessage = "Path to SSH Key File on DS-Client")]
        public string SSHKeyFile { get; set; }

        [Parameter(HelpMessage = "SSH Interpreter to access the data")]
        [ValidateSet("Perl", "Python", "Direct")]
        public string SSHInterpreter { get; set; }

        [Parameter(HelpMessage = "SSH Interpreter path")]
        public string SSHInterpreterPath { get; set; }

        [Parameter(HelpMessage = "Specify SUDO User Credentials")]
        public PSCredential SudoCredential { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Initialize a Data Source Browser
            DataSourceBrowser dataSourceBrowser = DSClientSession.createBrowser(EBackupDataType.EBackupDataType__FileSystem);

            // Try to resolve the supplied Computer
            string computer = dataSourceBrowser.expandToFullPath(Computer);
            WriteVerbose("Specified Computer resolved to: " + computer);

            // Set the Credentials
            UnixFS_Generic_BackupSetCredentials backupSetCredentials = UnixFS_Generic_BackupSetCredentials.from(dataSourceBrowser.neededCredentials(computer));

            if (Credential != null)
                backupSetCredentials.setCredentials(Credential.UserName, Credential.GetNetworkCredential().Password);
            else
            {
                WriteVerbose("Credentials not specified, using DS-Client Credentials...");
                backupSetCredentials.setUsingClientCredentials(true);
            }
            dataSourceBrowser.setCurrentCredentials(backupSetCredentials);

            if (SSHKeyFile != null || SudoCredential != null || SSHInterpreter != null)
            {
                try
                {
                    UnixFS_SSH_BackupSetCredentials sshBackupSetCredentials = UnixFS_SSH_BackupSetCredentials.from(backupSetCredentials);

                    if (SSHInterpreter != null)
                    {
                        SSHAccesorType sshAccessType = BaseDSClientBackupSet.StringToSSHAccesorType(SSHInterpreter);

                        sshBackupSetCredentials.setSSHAccessType(sshAccessType, SSHInterpreterPath);
                    }

                    if (SudoCredential != null)
                        sshBackupSetCredentials.setSudoAs(SudoCredential.UserName, SudoCredential.GetNetworkCredential().Password);

                    if (SSHKeyFile != null)
                        sshBackupSetCredentials.setCredentialsViaKeyFile(Credential.UserName, SSHKeyFile, Credential.GetNetworkCredential().Password);

                    dataSourceBrowser.setCurrentCredentials(sshBackupSetCredentials);

                    sshBackupSetCredentials.Dispose();
                }
                catch
                {
                    WriteWarning("Unable to set SSH Credential Options");
                }
            }
            else
                backupSetCredentials.Dispose();

            // Set the Starting path
            string path = Path ?? "";

            // Get the items from the specified path
            browse_item_info[] browseItems = dataSourceBrowser.getSubItems(computer, path);

            // If this is a Top Level path, get the Special Items i.e. System State, Service Database etc
            browse_item_info[] specialItems = null;
            if (path == "")
                specialItems = dataSourceBrowser.getSpecialSubItems(computer, path);

            List<SourceItemInfo> sourceItems = new List<SourceItemInfo>();

            if (specialItems != null)
                foreach (browse_item_info item in specialItems)
                    sourceItems.Add(new SourceItemInfo(path, item));

            foreach (browse_item_info item in browseItems)
                sourceItems.Add(new SourceItemInfo(path, item));

            if (Recursive)
            {
                List<ItemPath> newPaths = new List<ItemPath>();

                if (!string.IsNullOrEmpty(path) && path.Last() != '/')
                    path += "/";

                foreach (browse_item_info item in browseItems)
                    if (!item.isfile)
                        newPaths.Add(new ItemPath(path + item.name, 0));

                while (newPaths.Count() > 0)
                {
                    WriteVerbose("Items to enumerate: " + newPaths.Count());
                    // Select the first item in the list
                    ItemPath currentPath = newPaths.ElementAt(0);

                    WriteVerbose("Enumerating Path: " + currentPath.Path + " (Depth: " + currentPath.Depth + ")");

                    // Retrieve all the sub-items for the current selected path
                    try
                    {
                        browse_item_info[] subItems = dataSourceBrowser.getSubItems(computer, currentPath.Path);

                        int subItemDepth = currentPath.Depth + 1;
                        int index = 1;
                        foreach (browse_item_info item in subItems)
                        {
                            sourceItems.Add(new SourceItemInfo(currentPath.Path, item));

                            if (!item.isfile && subItemDepth <= RecursiveDepth)
                            {
                                newPaths.Insert(index, new ItemPath(currentPath.Path + "/" + item.name, subItemDepth));
                                index++;
                            }
                        }
                    }
                    catch (APIException e)
                    {
                        WriteWarning("Failed to Enumerate Path: " + currentPath.Path + " Exception: " + e);
                    }

                    // Remove the Path we've just completed enumerating from the list
                    newPaths.Remove(currentPath);
                }
            }

            sourceItems.ForEach(WriteObject);

            dataSourceBrowser.Dispose();
        }
    }
}