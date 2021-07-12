using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientRestoreSession", SupportsShouldProcess = true)]

    sealed public class SetDSClientRestoreSession : DSClientCmdlet
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

        [Parameter(ParameterSetName = "options", HelpMessage = "Apply New Restore Options")]
        public PSObject Options { get; set; }

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "sharemapping", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify a Destination Path Id to modify")]
        public int DestinationId { get; set; }

        [Parameter(ParameterSetName = "sharemapping", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Destination Path for the restore")]
        public string DestinationPath { get; set; }

        [Parameter(ParameterSetName = "sharemapping", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the amount to truncate the original path by")]
        public int TruncateAmount { get; set; }

        protected override void DSClientProcessRecord()
        {
            // If the Options Parameter is specified, validate it is of an accepted object type
            if (MyInvocation.BoundParameters.ContainsKey(nameof(Options)))
            {
                WriteDebug($"Immediate Base Type: {Options.ImmediateBaseObject.GetType()}");                
                
                if (!(Options.ImmediateBaseObject.GetType() == typeof(RestoreOptions_FileSystem) ||
                    Options.ImmediateBaseObject.GetType() == typeof(RestoreOptions_Win32FileSystem)))
                {
                    throw new ParameterBindingException("Invalid Options Data Type");
                }
            }

            WriteVerbose("Performing Action: Retrieve Restore Sessions");
            if (!(SessionState.PSVariable.GetValue("RestoreSessions", null) is List<DSClientRestoreSession> restoreSessions))
                throw new Exception("No Restore Sessions Found");

            bool found = false;
            for (int i = 0; i < restoreSessions.Count; i++)
            {
                if (restoreSessions[i].RestoreId == RestoreId)
                {
                    DSClientRestoreSession restoreSession = restoreSessions[i];

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(Options)))
                    {
                        if (Options.ImmediateBaseObject.GetType() == typeof(RestoreOptions_FileSystem) && restoreSession.GetRestoreOptionsType() == typeof(RestoreOptions_Win32FileSystem))
                        {
                            if (ShouldProcess($"Restore Session with RestoreId: {RestoreId}", "Set Restore Options"))
                                restoreSession.SetRestoreOptions(RestoreOptions_Win32FileSystem.From(new RestoreOptions_FileSystem(Options)));
                        }
                        else if (Options.ImmediateBaseObject.GetType() == typeof(RestoreOptions_FileSystem))
                        {
                            if (ShouldProcess($"Restore Session with RestoreId: {RestoreId}", "Set Restore Options"))
                                restoreSession.SetRestoreOptions(new RestoreOptions_FileSystem(Options));
                        }
                        else if (Options.ImmediateBaseObject.GetType() == typeof(RestoreOptions_Win32FileSystem))
                        {
                            if (ShouldProcess($"Restore Session with RestoreId: {RestoreId}", "Set Restore Options"))
                                restoreSession.SetRestoreOptions(new RestoreOptions_Win32FileSystem(Options));
                        }
                    }
                    else
                    {
                        if (MyInvocation.BoundParameters.ContainsKey(nameof(Computer)))
                            if (ShouldProcess($"Restore Session with RestoreId: {RestoreId}", $"Set Destination Computer to '{Computer}'"))
                                restoreSession.SetComputer(Computer);

                        if (MyInvocation.BoundParameters.ContainsKey(nameof(Credential)))
                            if (ShouldProcess($"Restore Session with RestoreId: {RestoreId}", $"Set Restore Credentials to '{Credential.UserName}'"))
                                restoreSession.SetCredentials(Credential);

                        if (MyInvocation.BoundParameters.ContainsKey(nameof(RestoreReason)))
                            if (ShouldProcess($"Restore Session with RestoreId: {RestoreId}", $"Set Restore Reason to '{RestoreReason}'"))
                                restoreSession.SetRestoreReason(RestoreReason);

                        if (MyInvocation.BoundParameters.ContainsKey(nameof(RestoreClassification)))
                            if (ShouldProcess($"Restore Session with RestoreId: {RestoreId}", $"Set Restore Classification to '{RestoreClassification}'"))
                                restoreSession.SetRestoreClassification(RestoreClassification);

                        if (MyInvocation.BoundParameters.ContainsKey(nameof(DestinationId)))
                        {
                            for (int x = 0; x < restoreSession.DestinationPaths.Length; x++)
                            {
                                if (restoreSession.DestinationPaths[x].DestinationId == DestinationId)
                                {
                                    DSClientRestoreSession.RestoreDestination destination = restoreSession.DestinationPaths[x];
                                    if (MyInvocation.BoundParameters.ContainsKey(nameof(DestinationPath)))
                                        if (ShouldProcess($"DestinationId '{DestinationId}' on Restore Session '{RestoreId}'", $"Set Restore Destination Path to '{DestinationPath}'"))
                                            destination.SetDestination(DestinationPath);

                                    if (MyInvocation.BoundParameters.ContainsKey(nameof(TruncateAmount)))
                                        if (ShouldProcess($"DestinationId '{DestinationId}' on Restore Session '{RestoreId}'", $"Truncate Source Path by '{TruncateAmount}'"))
                                            destination.SetTruncateLevel(TruncateAmount);

                                    restoreSession.DestinationPaths[x] = destination;
                                    break;
                                }
                            }
                        }
                    }

                    found = true;
                    break;
                }
            }

            if (!found)
                throw new Exception("Restore Session Not Found");
        }
    }
}