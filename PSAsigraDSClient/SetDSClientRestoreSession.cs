﻿using System;
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

        [Parameter(ParameterSetName = "default", HelpMessage = "Specify Destination Computer Credentials")]
        public DSClientCredential ComputerCredential { get; set; }

        [Parameter(ParameterSetName = "default", HelpMessage = "Specify MSSQL Database Credentials")]
        public DSClientCredential DatabaseCredential { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Reason for the Restore")]
        [ValidateSet("UserErrorDataDeletion", "MaliciousIntent", "DeviceLostOrStolen", "HardwareMalfunction", "SoftwareMalfunction", "DataStolen", "DataCorruption", "NaturalDisasters", "PowerOutages", "OtherDisaster", "PreviousGeneration", "DeviceDamaged")]
        public string RestoreReason { get; set; }

        [Parameter(ParameterSetName = "default", ValueFromPipelineByPropertyName = true, HelpMessage = "Restore Classification")]
        [ValidateSet("Production", "Drill", "ProductionDrill")]
        public string RestoreClassification { get; set; }

        [Parameter(ParameterSetName = "default", HelpMessage = "Apply New Restore Options")]
        public RestoreOptions_Base Options { get; set; }

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "sharemapping", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify a Destination Path Id to modify")]
        public int DestinationId { get; set; }

        [Parameter(ParameterSetName = "sharemapping", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Destination Path for the restore")]
        public string DestinationPath { get; set; }

        [Parameter(ParameterSetName = "sharemapping", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the amount to truncate the original path by")]
        public int TruncateAmount { get; set; }

        protected override void DSClientProcessRecord()
        {
            DSClientRestoreSession restoreSession = DSClientSessionInfo.GetRestoreSession(RestoreId);

            if (restoreSession != null)
            {
                if (MyInvocation.BoundParameters.ContainsKey(nameof(Options)))
                    if (ShouldProcess($"Restore Session with RestoreId: {RestoreId}", "Set Restore Options"))
                        restoreSession.SetRestoreOptions(Options);

                if (MyInvocation.BoundParameters.ContainsKey(nameof(Computer)))
                    if (ShouldProcess($"Restore Session with RestoreId: {RestoreId}", $"Set Destination Computer to '{Computer}'"))
                        restoreSession.SetComputer(Computer);

                if (MyInvocation.BoundParameters.ContainsKey(nameof(ComputerCredential)))
                    if (ShouldProcess($"Restore Session with RestoreId: {RestoreId}", $"Set Restore Credentials to '{ComputerCredential.UserName}"))
                        restoreSession.SetCredentials(ComputerCredential);

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
            else
            {
                throw new Exception("Restore Session Not Found");
            }
        }
    }
}