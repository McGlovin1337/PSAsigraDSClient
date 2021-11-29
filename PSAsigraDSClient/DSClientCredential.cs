using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public class DSClientCredential
    {
        private readonly BackupSetCredentials _credentials;

        public string UserName { get; set; }

        internal DSClientCredential(BackupSetCredentials credentials, string username)
        {
            _credentials = credentials;

            UserName = username;
        }

        internal BackupSetCredentials GetCredentials()
        {
            return _credentials;
        }

        public override string ToString()
        {
            return UserName;
        }
    }

    public class DSClientSSHCredential : DSClientCredential
    {
        public string SudoUser { get; set; }
        public string SSHKeyFile { get; set; }
        public string SSHAccessType { get; set; }
        public string SSHInterpreterPath { get; set; }

        internal DSClientSSHCredential(UnixFS_SSH_BackupSetCredentials credentials, string username) : base(credentials, username)
        {
            SudoUser = credentials.getSudoUser();
            SSHAccessType = EnumToString(credentials.getSSHAccessType());
            SSHInterpreterPath = credentials.getSSHInterpreterPath();
        }

        internal DSClientSSHCredential(UnixFS_SSH_BackupSetCredentials credentials, string username, string sshKey) : this(credentials, username)
        {
            SSHKeyFile = sshKey;
        }
    }

    public class DSClientCredentialSet
    {
        private readonly DSClientCredential _credential;

        public DSClientConnectionOption Type { get; private set; }
        public string Value { get; private set; }

        internal DSClientCredentialSet(DSClientCredential credential, DSClientConnectionOption type)
        {
            _credential = credential;
            Type = type;
            Value = credential.UserName;
        }

        internal DSClientCredential GetCredential()
        {
            return _credential;
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }

    public enum DSClientConnectionOption
    {
        GenericCredential,
        ComputerCredential,
        DatabaseCredential,
        SSHCredential
    }
}