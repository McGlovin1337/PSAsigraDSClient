using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientRestoreSession")]

    public class SetDSClientRestoreSession : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Restore Session Id")]
        public int RestoreId { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Destination Computer")]
        [ValidateNotNullOrEmpty]
        public string Computer { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Computer Credentials")]
        public PSCredential Credential { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Reason for the Restore")]
        [ValidateSet("UserErrorDataDeletion", "MaliciousIntent", "DeviceLostOrStolen", "HardwareMalfunction", "SoftwareMalfunction", "DataStolen", "DataCorruption", "NaturalDisasters", "PowerOutages", "OtherDisaster", "PreviousGeneration", "DeviceDamaged")]
        public string RestoreReason { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Restore Classification")]
        [ValidateSet("Production", "Drill", "ProductionDrill")]
        public string RestoreClassification { get; set; }

        [Parameter(ParameterSetName = "default", HelpMessage = "Specify to use Detailed Log")]
        public SwitchParameter UseDetailedLog { get; set; }

        [Parameter(ParameterSetName = "default", HelpMessage = "Specify Local Storage Handling")]
        [ValidateSet("None", "ConnectFirst", "ConnectIfNeeded", "ContinueIfDisconnect")]
        public string LocalStorageMethod { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Set the number of DS-System Read Threads")]
        public int DSSystemReadThreads { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Set the maximum pending Asynchronious IO")]
        public int MaxPendingAsyncIO { get; set; }

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "sharemapping", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify a Destination Path Id to modify")]
        public int DestinationId { get; set; }

        [Parameter(ParameterSetName = "sharemapping", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Destination Path for the restore")]
        public string DestinationPath { get; set; }

        [Parameter(ParameterSetName = "sharemapping", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the amount to truncate the original path by")]
        public int TruncateAmount { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the File System Overwrite Option")]
        [ValidateSet("RestoreAll", "RestoreNewer", "RestoreOlder", "RestoreDifferent", "SkipExisting")]
        public string FileOverwriteOption { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the File Restore Method")]
        [ValidateSet("Fast", "Save", "UseBuffer")]
        public string RestoreMethod { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify whether to Restore File Permissions")]
        [ValidateSet("Yes", "Skip", "Only")]
        public string RestorePermissions { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify if this is an Authoritative Restore (Windows Only)")]
        public SwitchParameter AuthoritativeRestore { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify whether to Overwrite Junction Points (Windows Only)")]
        public SwitchParameter OverwriteJunctionPoint { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify whether to Skip Restoring Offline Files")]
        public SwitchParameter SkipOfflineFiles { get; set; }

        protected override void DSClientProcessRecord()
        {
            List<DSClientRestoreSession> restoreSessions = SessionState.PSVariable.GetValue("RestoreSessions", null) as List<DSClientRestoreSession>;

            if (restoreSessions == null)
                throw new Exception("No Restore Sessions Found");

            bool found = false;
            for (int i = 0; i < restoreSessions.Count; i++)
            {
                if (restoreSessions[i].RestoreId == RestoreId)
                {
                    DSClientRestoreSession restoreSession = restoreSessions[i];

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(Computer)))
                        restoreSession.SetComputer(Computer);

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(Credential)))
                        restoreSession.SetCredentials(Credential);

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(RestoreReason)))
                        restoreSession.SetRestoreReason(RestoreReason);

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(RestoreClassification)))
                        restoreSession.SetRestoreClassification(RestoreClassification);

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(UseDetailedLog)))
                        restoreSession.SetUseDetailedLog(UseDetailedLog);

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(LocalStorageMethod)))
                        restoreSession.SetLocalStorageMethod(LocalStorageMethod);

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(DSSystemReadThreads)))
                        restoreSession.SetDSSystemReadThreads(DSSystemReadThreads);

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(MaxPendingAsyncIO)))
                        restoreSession.SetMaxPendingAsyncIO(MaxPendingAsyncIO);

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(DestinationId)))
                    {
                        for (int x = 0; x < restoreSession.DestinationPaths.Length; x++)
                        {
                            if (restoreSession.DestinationPaths[x].DestinationId == DestinationId)
                            {
                                DSClientRestoreSession.RestoreDestination destination = restoreSession.DestinationPaths[x];
                                if (MyInvocation.BoundParameters.ContainsKey(nameof(DestinationPath)))
                                    destination.SetDestination(DestinationPath);

                                if (MyInvocation.BoundParameters.ContainsKey(nameof(TruncateAmount)))
                                    destination.SetTruncateLevel(TruncateAmount);

                                restoreSession.DestinationPaths[x] = destination;
                                break;
                            }
                        }
                    }

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(FileOverwriteOption)))
                        restoreSession.FileSystemOptions.SetOverwriteOption(FileOverwriteOption);

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(RestoreMethod)))
                        restoreSession.FileSystemOptions.SetRestoreMethod(RestoreMethod);

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(RestorePermissions)))
                        restoreSession.FileSystemOptions.SetRestorePermissions(RestorePermissions);

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(AuthoritativeRestore)))
                        restoreSession.FileSystemOptions.WinFSOptions.SetAuthoritative(AuthoritativeRestore);

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(OverwriteJunctionPoint)))
                        restoreSession.FileSystemOptions.WinFSOptions.SetOverwriteJunctionPoint(OverwriteJunctionPoint);

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(SkipOfflineFiles)))
                        restoreSession.FileSystemOptions.WinFSOptions.SetSkipOfflineFile(SkipOfflineFiles);

                    found = true;
                    break;
                }
            }

            if (!found)
                throw new Exception("Restore Session Not Found");
        }
    }
}