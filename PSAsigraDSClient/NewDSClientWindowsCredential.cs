using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientWindowsCredential")]
    [OutputType(typeof(DSClientCredential))]

    sealed public class NewDSClientWindowsCredential : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify PSCredentials")]
        public PSCredential Credential { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Performing Action: Create Credential Object");
            BackupSetCredentials newCredentials = DSClientSessionInfo.GetClientConnection()
                .createBrowser(EBackupDataType.EBackupDataType__FileSystem)
                .neededCredentials("Microsoft Windows Network\\localhost");
            Win32FS_Generic_BackupSetCredentials credentials = Win32FS_Generic_BackupSetCredentials.from(newCredentials);

            credentials.setCredentials(Credential.UserName, Credential.GetNetworkCredential().Password);

            DSClientCredential dsClientCredential = new DSClientCredential(credentials, Credential.UserName);

            WriteObject(dsClientCredential);
        }
    }
}