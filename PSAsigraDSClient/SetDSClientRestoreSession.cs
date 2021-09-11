using System;
using System.Management.Automation;
using static PSAsigraDSClient.DSClientRestoreOptions;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientRestoreSession", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

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

        [Parameter(ParameterSetName = "default", HelpMessage = "Filter Data Selection From Date")]
        public DateTime DateFrom { get; set; } = DateTime.Parse("1/1/1970");

        [Parameter(ParameterSetName = "default", HelpMessage = "Filter Data Selection To Date")]
        public DateTime DateTo { get; set; } = DateTime.Now;

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "sharemapping", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify a Destination Path Id to modify")]
        public int DestinationId { get; set; }

        [Parameter(ParameterSetName = "sharemapping", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Destination Path for the restore")]
        public string DestinationPath { get; set; }

        [Parameter(ParameterSetName = "sharemapping", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the amount to truncate the original path by")]
        public int TruncateAmount { get; set; }

        [Parameter(ParameterSetName = "sharemapping", ValueFromPipelineByPropertyName = true, HelpMessage = "Update Database Mapping(s)")]
        public MSSQLDatabaseMap[] DatabaseMapping { get; set; }

        protected override void DSClientProcessRecord()
        {
            DSClientRestoreSession restoreSession = DSClientSessionInfo.GetRestoreSession(RestoreId);

            if (restoreSession != null)
            {
                if (MyInvocation.BoundParameters.ContainsKey(nameof(DateFrom)) || MyInvocation.BoundParameters.ContainsKey(nameof(DateTo)))
                    if (ShouldProcess($"Restore Session with RestoreId: {RestoreId}", $"Set Data Selection From '{DateFrom}' to '{DateTo}'"))
                        restoreSession.SetDataTimeRange(DateFrom, DateTo);

                if (MyInvocation.BoundParameters.ContainsKey(nameof(Options)))
                    if (ShouldProcess($"Restore Session with RestoreId: {RestoreId}", "Set Restore Options"))
                        restoreSession.SetRestoreOptions(Options);

                if (MyInvocation.BoundParameters.ContainsKey(nameof(Computer)))
                    if (ShouldProcess($"Restore Session with RestoreId: {RestoreId}", $"Set Destination Computer to '{Computer}'"))
                        restoreSession.SetComputer(Computer);

                if (MyInvocation.BoundParameters.ContainsKey(nameof(ComputerCredential)))
                    if (ShouldProcess($"Restore Session with RestoreId: {RestoreId}", $"Set Restore Credentials to '{ComputerCredential.UserName}'"))
                        restoreSession.SetCredentials(ComputerCredential);

                if (MyInvocation.BoundParameters.ContainsKey(nameof(DatabaseCredential)))
                    if (ShouldProcess($"Restore Session with RestoreId: {RestoreId}", $"Set Database Credentials to '{DatabaseCredential.UserName}'"))
                        restoreSession.SetDatabaseCredentials(DatabaseCredential);

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

                            if (MyInvocation.BoundParameters.ContainsKey(nameof(DatabaseMapping)))
                                if (ShouldProcess($"DestinationId '{DestinationId}' on Restore Session '{RestoreId}'", "Update Database Mappings"))
                                    foreach (MSSQLDatabaseMap map in DatabaseMapping)
                                        restoreSession.UpdateMSSQLDatabaseRestoreMap(DestinationId, map);                                    

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