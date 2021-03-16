using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientInitialBackupPath")]
    [OutputType(typeof(DSClientInitialBackupPath))]

    public class GetDSClientInitialBackupPath : DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            InitialBackupManager initialBackupManager = DSClientSession.getInitialBackupManager();

            List<DSClientInitialBackupPath> initPaths = new List<DSClientInitialBackupPath>();

            init_backup_path_info[] initPathInfos = initialBackupManager.definedPaths();

            initialBackupManager.Dispose();

            foreach (init_backup_path_info path in initPathInfos)
                initPaths.Add(new DSClientInitialBackupPath(path, DSClientOSType.OsType));

            initPaths.ForEach(WriteObject);
        }

        private class DSClientInitialBackupPath
        {
            public int PathId { get; private set; }
            public string Path { get; private set; }
            public int AvailableSpace { get; private set; }
            public int TotalSpace { get; private set; }
            public string Credentials { get; private set; }
            public string EncryptionKey { get; private set; }
            public string EncryptionType { get; private set; }

            public DSClientInitialBackupPath(init_backup_path_info initBackupPath, string osType)
            {
                PathId = initBackupPath.pathID;
                Path = initBackupPath.path;
                AvailableSpace = initBackupPath.availableSpaces;
                TotalSpace = initBackupPath.totalSpaces;
                Credentials = (osType == "Windows")
                    ? Win32FS_Generic_BackupSetCredentials.from(initBackupPath.credentials).getUserName()
                    : UnixFS_Generic_BackupSetCredentials.from(initBackupPath.credentials).getUserName();
                EncryptionKey = initBackupPath.encKey;
                EncryptionType = EnumToString(initBackupPath.encType);
            }
        }
    }
}