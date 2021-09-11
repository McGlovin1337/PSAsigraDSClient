using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient.PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientUnixCredential")]
    [OutputType(typeof(DSClientCredential), typeof(DSClientSSHCredential))]

    sealed public class NewDSClientUnixCredential : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify PSCredentials")]
        public PSCredential Credential { get; set; }

        [Parameter(HelpMessage = "Specify Sudo User Credentials")]
        public PSCredential SudoCredential { get; set; }

        [Parameter(HelpMessage = "Specify Path to SSH Key File on DS-Client")]
        public string SSHKeyFile { get; set; }

        [Parameter(HelpMessage = "Specify the SSH Accessor Type")]
        [ValidateSet("Direct", "Python", "Perl")]
        public string SSHAccessType { get; set; }

        [Parameter(HelpMessage = "Specify a Path to the SSH Interpreter")]
        public string SSHInterpreterPath { get; set; }

        protected override void DSClientProcessRecord()
        {
            // SSHAccessType and SSHInterpreterPath MUST both be specified if either Parameter is used
            if ((MyInvocation.BoundParameters.ContainsKey(nameof(SSHAccessType)) &&
                !MyInvocation.BoundParameters.ContainsKey(nameof(SSHInterpreterPath))) ||
                (!MyInvocation.BoundParameters.ContainsKey(nameof(SSHAccessType)) &&
                MyInvocation.BoundParameters.ContainsKey(nameof(SSHInterpreterPath))))
            {
                throw new ParameterBindingException("SSHAccessType and SSHInterpreterPath must be specified together");
            }

            WriteVerbose("Performing Action: Create Credential Object");
            BackupSetCredentials newCredentials = DSClientSessionInfo.GetClientConnection()
                .createBrowser(EBackupDataType.EBackupDataType__FileSystem)
                .neededCredentials("UNIX-SSH\\localhost");
            UnixFS_SSH_BackupSetCredentials credentials = UnixFS_SSH_BackupSetCredentials.from(newCredentials);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(SSHKeyFile)))
                credentials.setCredentialsViaKeyFile(Credential.UserName, SSHKeyFile, Credential.GetNetworkCredential().Password);
            else
                credentials.setCredentials(Credential.UserName, Credential.GetNetworkCredential().Password);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(SSHAccessType)))
                credentials.setSSHAccessType(StringToEnum<SSHAccesorType>(SSHAccessType), SSHInterpreterPath);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(SudoCredential)))
                credentials.setSudoAs(SudoCredential.UserName, SudoCredential.GetNetworkCredential().Password);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(SSHKeyFile)))
            {
                DSClientSSHCredential dsClientSSHCredential = new DSClientSSHCredential(credentials, Credential.UserName, SSHKeyFile);
                WriteObject(dsClientSSHCredential);
            }
            else
            {
                DSClientCredential dsClientCredential = new DSClientCredential(credentials, Credential.UserName);
                WriteObject(dsClientCredential);
            }
        }
    }
}