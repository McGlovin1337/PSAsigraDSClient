using System;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.BaseDSClientBackupSet;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientBackupSetRestore")]

    public class StartDSClientBackupSetRestore: BaseDSClientBackupSetDelResVal
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Computer to Restore to")]
        [ValidateNotNullOrEmpty]
        public string Computer { get; set; }

        [Parameter(HelpMessage = "Specify Credentials for Destination Computer")]
        public PSCredential Credential { get; set; }

        [Parameter(HelpMessage = "Specify the SSH Iterpreter to access the data")]
        [ValidateSet("Perl", "Python", "Direct")]
        public string SSHInterpreter { get; set; }

        [Parameter(HelpMessage = "Specify SSH Interpreter path")]
        public string SSHInterpreterPath { get; set; }

        [Parameter(HelpMessage = "Specify path to SSH Key File")]
        public string SSHKeyFile { get; set; }

        [Parameter(HelpMessage = "Specify SUDO User Credentials")]
        public PSCredential SudoCredential { get; set; }

        [Parameter(HelpMessage = "Specify Destination Path to Restore Items to")]
        public string DestinationPath { get; set; }

        [Parameter(HelpMessage = "Specify how many levels to Truncate Source Path")]
        public int TruncateSource { get; set; } = 0;

        [Parameter(Mandatory = true, HelpMessage = "Specify the Restore Reason")]
        [ValidateSet("AccidentalDeletion", "MaliciousIntent", "DeviceLostOrStolen", "HardwareFault", "SoftwareFault", "DataStolen", "DataCorruption", "NaturalDisaster", "PowerOutage", "OtherDisaster", "PreviousGeneration", "DeviceDamaged")]
        public string RestoreReason { get; set; }

        [Parameter(HelpMessage = "Specify Restore Classification")]
        [ValidateSet("Production", "Drill", "ProductionDrill")]
        public string RestoreClassification { get; set; } = "Production";

        [Parameter(HelpMessage = "Specify Max Pending Asynchronous I/O per File")]
        public int MaxPendingAsyncIO { get; set; } = 0;

        [Parameter(HelpMessage = "Specify the number of DS-System Read Threads")]
        public int ReadThreads { get; set; } = 0;

        [Parameter(HelpMessage = "Specify to use Detailed Log")]
        public SwitchParameter UseDetailedLog { get; set; }

        [Parameter(HelpMessage = "Specify Local Storage Handling")]
        [ValidateSet("None", "ConnectDsSysFirst", "ConnectDsSysNeeded", "ContinueWithoutDsSys")]
        public string LocalStorageMethod { get; set; } = "ConnectDsSysNeeded";

        [Parameter(Mandatory = true, ParameterSetName = "FileSystem", HelpMessage = "Specify the File Overwrite Option")]
        [Parameter(ParameterSetName = "Selective")]
        [ValidateSet("RestoreAll", "RestoreNewer", "RestoreOlder", "RestoreDifferent", "SkipExisting")]
        public string OverwriteOption { get; set; }

        [Parameter(ParameterSetName = "FileSystem", HelpMessage = "Specify the File Restore Method")]
        [Parameter(ParameterSetName = "Selective")]
        [ValidateSet("Save", "Fast", "UseBuffer")]
        public string RestoreMethod { get; set; } = "Save";

        [Parameter(ParameterSetName = "FileSystem", HelpMessage = "Specify Restoration of Permissions")]
        [Parameter(ParameterSetName = "Selective")]
        [ValidateSet("Yes", "Skip", "Only")]
        public string RestorePermissions { get; set; } = "Yes";

        [Parameter(ParameterSetName = "WinFileSystem", HelpMessage = "Specify AD (System State) Authoritative Restore")]
        [Parameter(ParameterSetName = "Selective")]
        [Parameter(ParameterSetName = "FileSystem")]
        public SwitchParameter AuthoritativeRestore { get; set; }

        [Parameter(ParameterSetName = "WinFileSystem", HelpMessage = "Specify if to Overwrite Junction Points")]
        [Parameter(ParameterSetName = "Selective")]
        [Parameter(ParameterSetName = "FileSystem")]
        public SwitchParameter OverwriteJunctionPoint { get; set; }

        [Parameter(ParameterSetName = "WinFileSystem", HelpMessage = "Specify if to Skip Offline Files (stubs)")]
        [Parameter(ParameterSetName = "Selective")]
        [Parameter(ParameterSetName = "FileSystem")]
        public SwitchParameter SkipOfflineFiles { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Check for a Backup Set Restore View stored in SessionState
            WriteVerbose("Checking for DS-Client Restore View Session...");
            BackupSetRestoreView restoreSession = SessionState.PSVariable.GetValue("RestoreView", null) as BackupSetRestoreView;

            if (restoreSession == null)
                throw new Exception("There is no Backup Set Restore View Session, use Initialize-DSClientBackupSetRestore Cmdlet to create a Restore Session");

            // Check for Data Type for the Restore stored in SessionState
            EBackupDataType dataType = (EBackupDataType)SessionState.PSVariable.GetValue("RestoreType", EBackupDataType.EBackupDataType__UNDEFINED);

            if (dataType == EBackupDataType.EBackupDataType__UNDEFINED)
                throw new Exception("Undefined Data Type for Restore");

            // Get the selected items
            List<long> selectedItems = new List<long>();

            if (Items != null)
            {
                foreach (string item in Items)
                {
                    try
                    {
                        selectedItems.Add(restoreSession.getItem(item).id);
                    }
                    catch
                    {
                        WriteWarning("Unable to select item: " + item);
                        continue;
                    }
                }
            }

            if (ItemId != null)
                selectedItems.AddRange(ItemId);

            RestoreActivityInitiator restoreActivityInitiator = restoreSession.prepareRestore(selectedItems.ToArray());

            DataSourceBrowser dataSourceBrowser = DSClientSession.createBrowser(dataType);

            // Set the Credentials for the Destination Computer
            BackupSetCredentials backupSetCredentials = dataSourceBrowser.neededCredentials(Computer);

            if (dataType == EBackupDataType.EBackupDataType__DB2)
            {
                /*DB2_BackupSetCredentials db2BSCredentials = DB2_BackupSetCredentials.from(backupSetCredentials);

                Not yet implemented

                */
                throw new NotImplementedException("Restore Type Not Implemented");
            }
            else if (DSClientOSType.OsType == "Windows")
            {
                Win32FS_Generic_BackupSetCredentials win32FSBSCredentials = Win32FS_Generic_BackupSetCredentials.from(backupSetCredentials);

                if (Credential != null)
                {
                    string user = Credential.UserName;
                    string pass = Credential.GetNetworkCredential().Password;
                    win32FSBSCredentials.setCredentials(user, pass);
                }
                else
                {
                    WriteVerbose("Credentials not specified, using DS-Client Credentials...");
                    win32FSBSCredentials.setUsingClientCredentials(true);
                }

                dataSourceBrowser.setCurrentCredentials(win32FSBSCredentials);
            }
            else if (DSClientOSType.OsType == "Linux")
            {
                UnixFS_Generic_BackupSetCredentials unixFSBackupSetCredentials = UnixFS_Generic_BackupSetCredentials.from(backupSetCredentials);

                string user = Credential.UserName;
                string pass = Credential.GetNetworkCredential().Password;

                if (SSHKeyFile != null)
                {
                    UnixFS_SSH_BackupSetCredentials unixFSSSHBackupSetCredentials = UnixFS_SSH_BackupSetCredentials.from(unixFSBackupSetCredentials);

                    if (SSHInterpreter != null)
                    {
                        SSHAccesorType sshAccessType = StringToSSHAccesorType(SSHInterpreter);

                        unixFSSSHBackupSetCredentials.setSSHAccessType(sshAccessType, SSHInterpreterPath);
                    }

                    if (SudoCredential != null)
                    {
                        string sudoUser = SudoCredential.UserName;
                        string sudoPass = SudoCredential.GetNetworkCredential().Password;

                        unixFSSSHBackupSetCredentials.setSudoAs(sudoUser, sudoPass);
                    }

                    unixFSSSHBackupSetCredentials.setCredentialsViaKeyFile(user, SSHKeyFile, pass);
                }
                else
                {
                    unixFSBackupSetCredentials.setCredentials(user, pass);
                }
            }
            else
            {
                backupSetCredentials.setUsingClientCredentials(true);
            }

            // Get the selected shares
            selected_shares[] selectedShares = restoreActivityInitiator.selected_shares();

            // Create a share mapping for the selected shares if DestinationPath has been specified
            List<share_mapping> shareMappings = new List<share_mapping>();

            if (DestinationPath != null)
            {
                foreach (selected_shares share in selectedShares)
                {
                    share_mapping shareMap = new share_mapping
                    {
                        destination_path = DestinationPath,
                        source_share = share.share_name,
                        truncate_level = TruncateSource
                    };

                    shareMappings.Add(shareMap);
                }
            }

            // Set Restore Configuration
            restoreActivityInitiator.setRestoreReason(StringToERestoreReason(RestoreReason));
            restoreActivityInitiator.setRestoreClassification(StringToERestoreClassification(RestoreClassification));
            restoreActivityInitiator.setMaxPendingAsyncIO(MaxPendingAsyncIO);
            restoreActivityInitiator.setDSSystemReadThreads(ReadThreads);
            restoreActivityInitiator.setDetailLogReporting(UseDetailedLog);
            restoreActivityInitiator.setLocalStorageHandling(StringToERestoreLocalStorageHandling(LocalStorageMethod));

            if (dataType == EBackupDataType.EBackupDataType__FileSystem)
            {
                FS_RestoreActivityInitiator fsRestoreActivityInitiator = FS_RestoreActivityInitiator.from(restoreActivityInitiator);

                fsRestoreActivityInitiator.setFileOverwriteOption(StringToEFileOverwriteOption(OverwriteOption));
                fsRestoreActivityInitiator.setRestoreMethod(StringToERestoreMethod(RestoreMethod));
                fsRestoreActivityInitiator.setRestorePermission(StringToERestorePermission(RestorePermissions));

                if ((AuthoritativeRestore || OverwriteJunctionPoint || SkipOfflineFiles) && DSClientOSType.OsType == "Windows")
                {
                    Win32FS_RestoreActivityInitiator win32FSRestoreActivityInitiator = Win32FS_RestoreActivityInitiator.from(fsRestoreActivityInitiator);

                    win32FSRestoreActivityInitiator.setAuthoritativeRestore(AuthoritativeRestore);
                    win32FSRestoreActivityInitiator.setOverwriteJunctionPoint(OverwriteJunctionPoint);
                    win32FSRestoreActivityInitiator.setSkipOfflineFiles(SkipOfflineFiles);
                }
            }
            else if (dataType == EBackupDataType.EBackupDataType__SQLServer)
            {
                // SQL Server Options not yet implemented
                throw new NotImplementedException("MSSQL Database Restore not implemented");
            }
            else if (dataType == EBackupDataType.EBackupDataType__VSSExchange)
            {
                // VSS Exchange Restore Options not yet implemented
                throw new NotImplementedException("VSS Exchange Restore not implemented");
            }
            else if (dataType == EBackupDataType.EBackupDataType__VSSSQLServer)
            {
                // VSS SQL Server Restore Options not yet implemented
                throw new NotImplementedException("VSS SQL Restore not implemented");
            }
            else if (dataType == EBackupDataType.EBackupDataType__VMwareVADP)
            {
                // VMware VADP Restore Options not yet implemented
                throw new NotImplementedException("VMware VADP Restore Options not implemented");
            }
            else if (dataType == EBackupDataType.EBackupDataType__DB2)
            {
                // DB2 Restore Options not yet implemented
                throw new NotImplementedException("DB2 Restore Options not implemented");
            }

            // Initiate the restore
            WriteVerbose("Initiating the Restore Request...");
            GenericActivity restoreActivity = restoreActivityInitiator.startRestore(dataSourceBrowser, Computer, shareMappings.ToArray());

            WriteObject("Started Backup Set Restore Activity with ActivityId: " + restoreActivity.getID());

            restoreActivity.Dispose();
            dataSourceBrowser.Dispose();
            restoreActivityInitiator.Dispose();
            restoreSession.Dispose();

            SessionState.PSVariable.Remove("RestoreView");
        }

        private static EFileOverwriteOption StringToEFileOverwriteOption(string overwriteOption)
        {
            EFileOverwriteOption OverwriteOption = EFileOverwriteOption.EFileOverwriteOption__UNDEFINED;

            switch(overwriteOption)
            {
                case "RestoreAll":
                    OverwriteOption = EFileOverwriteOption.EFileOverwriteOption__RestoreAll;
                    break;
                case "RestoreNewer":
                    OverwriteOption = EFileOverwriteOption.EFileOverwriteOption__RestoreNewer;
                    break;
                case "RestoreOlder":
                    OverwriteOption = EFileOverwriteOption.EFileOverwriteOption__RestoreOlder;
                    break;
                case "RestoreDifferent":
                    OverwriteOption = EFileOverwriteOption.EFileOverwriteOption__RestoreDifferent;
                    break;
                case "SkipExisting":
                    OverwriteOption = EFileOverwriteOption.EFileOverwriteOption__SkipExisting;
                    break;
            }

            return OverwriteOption;
        }

        private static ERestoreMethod StringToERestoreMethod(string restoreMethod)
        {
            ERestoreMethod RestoreMethod = ERestoreMethod.ERestoreMethod__UNDEFINED;

            switch(restoreMethod)
            {
                case "Save":
                    RestoreMethod = ERestoreMethod.ERestoreMethod__Save;
                    break;
                case "Fast":
                    RestoreMethod = ERestoreMethod.ERestoreMethod__Fast;
                    break;
                case "UseBuffer":
                    RestoreMethod = ERestoreMethod.ERestoreMethod__UseBuffer;
                    break;
            }

            return RestoreMethod;
        }

        private static ERestorePermission StringToERestorePermission(string restorePermission)
        {
            ERestorePermission RestorePermission = ERestorePermission.ERestorePermission__UNDEFINED;

            switch(restorePermission)
            {
                case "Yes":
                    RestorePermission = ERestorePermission.ERestorePermission__Yes;
                    break;
                case "Skip":
                    RestorePermission = ERestorePermission.ERestorePermission__Skip;
                    break;
                case "Only":
                    RestorePermission = ERestorePermission.ERestorePermission__Only;
                    break;
            }

            return RestorePermission;
        }

        private ERestoreReason StringToERestoreReason(string reason)
        {
            ERestoreReason Reason = ERestoreReason.ERestoreReason__UNDEFINED;

            switch(reason)
            {
                case "AccidentalDeletion":
                    Reason = ERestoreReason.ERestoreReason__UserErrorDataDeletion;
                    break;
                case "MaliciousIntent":
                    Reason = ERestoreReason.ERestoreReason__MaliciousIntent;
                    break;
                case "DeviceLostOrStolen":
                    Reason = ERestoreReason.ERestoreReason__DeviceLostOrStolen;
                    break;
                case "HardwareFault":
                    Reason = ERestoreReason.ERestoreReason__HardwareMalfunction;
                    break;
                case "SoftwareFault":
                    Reason = ERestoreReason.ERestoreReason__SoftwareMalfunction;
                    break;
                case "DataStolen":
                    Reason = ERestoreReason.ERestoreReason__DataStolen;
                    break;
                case "DataCorruption":
                    Reason = ERestoreReason.ERestoreReason__DataCorruption;
                    break;
                case "NaturalDisaster":
                    Reason = ERestoreReason.ERestoreReason__NaturalDisasters;
                    break;
                case "PowerOutage":
                    Reason = ERestoreReason.ERestoreReason__PowerOutages;
                    break;
                case "OtherDisaster":
                    Reason = ERestoreReason.ERestoreReason__OtherDisaster;
                    break;
                case "PreviousGeneration":
                    Reason = ERestoreReason.ERestoreReason__PreviousGeneration;
                    break;
                case "DeviceDamaged":
                    Reason = ERestoreReason.ERestoreReason__DeviceDamaged;
                    break;
            }

            return Reason;
        }

        private ERestoreClassification StringToERestoreClassification(string classification)
        {
            ERestoreClassification Classification = ERestoreClassification.ERestoreClassification__UNDEFINED;

            switch(classification)
            {
                case "Production":
                    Classification = ERestoreClassification.ERestoreClassification__Production;
                    break;
                case "Drill":
                    Classification = ERestoreClassification.ERestoreClassification__StopAfterDrillUsedOut;
                    break;
                case "ProductionDrill":
                    Classification = ERestoreClassification.ERestoreClassification__ProductionAfterDrillUsedOut;
                    break;
            }

            return Classification;
        }

        private ERestoreLocalStorageHandling StringToERestoreLocalStorageHandling(string method)
        {
            ERestoreLocalStorageHandling Method = ERestoreLocalStorageHandling.ERestoreLocalStorageHandling__UNDEFINED;

            switch(method)
            {
                case "None":
                    Method = ERestoreLocalStorageHandling.ERestoreLocalStorageHandling__None;
                    break;
                case "ConnectDsSysFirst":
                    Method = ERestoreLocalStorageHandling.ERestoreLocalStorageHandling__ConnectFirst;
                    break;
                case "ConnectDsSysNeeded":
                    Method = ERestoreLocalStorageHandling.ERestoreLocalStorageHandling__ConnectIfNeeded;
                    break;
                case "ContinueWithoutDsSys":
                    Method = ERestoreLocalStorageHandling.ERestoreLocalStorageHandling__ContinueIfDisconnect;
                    break;
            }

            return Method;
        }
    }
}