using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientInitialBackupPath")]

    public class NewDSClientInitialBackupPath : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Intial Backup Storage Path")]
        [ValidateNotNullOrEmpty]
        public string Path { get; set; }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Credentials for the Storage Path")]
        public PSCredential Credential { get; set; }

        [Parameter(HelpMessage = "Specify the Encryption Type")]
        [ValidateSet("None", "ClientPrivateKey", "SpecifiedKey")]
        public string EncryptionType { get; set; }

        [Parameter(HelpMessage = "Specify an Encryption Key to use (setting this implies EncryptionType as 'SpecifiedKey'")]
        public string EncryptionKey { get; set; } = "";

        protected override void DSClientProcessRecord()
        {
            // Create a Data Source Browser
            DataSourceBrowser dataSourceBrowser = DSClientSession.createBrowser(EBackupDataType.EBackupDataType__FileSystem);

            // Set the Credentials
            BackupSetCredentials pathCredentials = null;
            if (DSClientSessionInfo.OperatingSystem == "Windows" && Path[0] == '\\' && Path[1] == '\\')
            {
                string path = Path.TrimStart('\\');
                string computer = path.Split('\\').First();

                computer = dataSourceBrowser.expandToFullPath(computer);
                WriteVerbose($"Notice: Computer resolved to '{computer}'");

                pathCredentials = dataSourceBrowser.neededCredentials(computer);

                Win32FS_Generic_BackupSetCredentials win32PathCredentials = Win32FS_Generic_BackupSetCredentials.from(pathCredentials);

                win32PathCredentials.setCredentials(Credential.UserName, Credential.GetNetworkCredential().Password);
            }
            else
            {
                string localhost = DSClientSession.getAboutInfo()
                                                    .Single(hostname => hostname.info_type == EAboutInfoType.EAboutInfoType__HostName)
                                                    .inf_value;

                pathCredentials = dataSourceBrowser.neededCredentials(localhost);

                pathCredentials.setUsingClientCredentials(true);
            }

            dataSourceBrowser.Dispose();

            // If EncryptionKey is specified, force the EncryptionType to SpecifiedKey
            EInitBackupDataEncType encryptionType = EInitBackupDataEncType.EInitBackupDataEncType__UNDEFINED;
            if (EncryptionKey != null || EncryptionKey != "")
                encryptionType = EInitBackupDataEncType.EInitBackupDataEncType__SpecifiedKey;
            else
                encryptionType = StringToEnum<EInitBackupDataEncType>(EncryptionType);

            // Create the new Initial Backup Path object
            init_backup_path_info newInitBackupPath = new init_backup_path_info
            {
                path = Path,
                encType = encryptionType
            };

            if (pathCredentials != null)
                newInitBackupPath.credentials = pathCredentials;

            if (EncryptionKey != null)
                newInitBackupPath.encKey = EncryptionKey;

            InitialBackupManager initialBackupManager = DSClientSession.getInitialBackupManager();

            WriteVerbose("Performing Action: Add new Initial Backup Path to DS-Client");
            initialBackupManager.addPath(newInitBackupPath);

            pathCredentials.Dispose();
            initialBackupManager.Dispose();
        }
    }
}