using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientDeleteSession", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class SetDSClientDeleteSession : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Delete Session to Modify")]
        public int DeleteId { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Start Date for Data Selection")]
        public DateTime DateFrom { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify End Date for Data Selection")]
        public DateTime DateEnd { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Number of Generations to Keep for selected items")]
        public int KeepGenerations { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Option for Archive Items")]
        [ValidateSet("Only", "Include", "Exclude")]
        public string ArchiveOption { get; set; }

        [Parameter(HelpMessage = "Specify to Move Items to BLM Archiver")]
        public SwitchParameter MoveToBLM { get; set; }

        [Parameter(HelpMessage = "Specify BLM Label")]
        public string BLMLabel { get; set; }

        [Parameter(HelpMessage = "Create a New BLM Package")]
        public SwitchParameter NewBLMPackage { get; set; }

        protected override void DSClientProcessRecord()
        {
            DSClientDeleteSession deleteSession = DSClientSessionInfo.GetDeleteSession(DeleteId);

            if (deleteSession != null)
            {
                // Set Data Selection Time Range
                if (MyInvocation.BoundParameters.ContainsKey(nameof(DateFrom)) && MyInvocation.BoundParameters.ContainsKey(nameof(DateEnd)))
                {
                    if (ShouldProcess($"Delete Session Id '{DeleteId}'", $"Set Date Time Range From '{DateFrom}' To '{DateEnd}'"))
                        deleteSession.SetDataTimeRange(DateFrom, DateEnd);
                }
                else if (MyInvocation.BoundParameters.ContainsKey(nameof(DateFrom)))
                {
                    if (ShouldProcess($"Delete Session Id '{DeleteId}'", $"Set Date Time Range From '{DateFrom}' To '{deleteSession.DateTo}'"))
                        deleteSession.SetDataTimeRange(DateFrom, deleteSession.DateTo);
                }
                else if (MyInvocation.BoundParameters.ContainsKey(nameof(DateEnd)))
                {
                    if (ShouldProcess($"Delete Session Id '{DeleteId}'", $"Set Date Time Range From '{deleteSession.DateFrom}' To '{DateEnd}'"))
                        deleteSession.SetDataTimeRange(deleteSession.DateFrom, DateEnd);
                }

                if (MyInvocation.BoundParameters.ContainsKey(nameof(KeepGenerations)))
                    if (ShouldProcess($"Delete Session Id '{DeleteId}'", $"Keep '{KeepGenerations}' Generations of Selected Items"))
                        deleteSession.SetKeepGenerations(KeepGenerations);

                if (MyInvocation.BoundParameters.ContainsKey(nameof(ArchiveOption)))
                    if (ShouldProcess($"Delete Session Id '{DeleteId}'", $"Set Archive Option to '{ArchiveOption}'"))
                        deleteSession.SetDeleteArchive(StringToEnum<DeleteArchiveOptions>(ArchiveOption));

                if (MyInvocation.BoundParameters.ContainsKey(nameof(MoveToBLM)))
                    if (ShouldProcess($"Delete Session Id '{DeleteId}'", $"Set Move Items to BLM to '{MoveToBLM}'"))
                        deleteSession.MoveToBLM.SetMoveToBLM(MoveToBLM);

                if (MyInvocation.BoundParameters.ContainsKey(nameof(BLMLabel)))
                    if (ShouldProcess($"Delete Session Id '{DeleteId}'", $"Set BLM Package Label to '{BLMLabel}'"))
                        deleteSession.MoveToBLM.SetLabel(BLMLabel);

                if (MyInvocation.BoundParameters.ContainsKey(nameof(NewBLMPackage)))
                    if (ShouldProcess($"Delete Session Id '{DeleteId}'", $"Set Create New BLM Package to '{NewBLMPackage}'"))
                        deleteSession.MoveToBLM.SetNewPackage(NewBLMPackage);
            }
            else
            {
                ErrorRecord errorRecord = new ErrorRecord(
                    new Exception("Delete Session not found"),
                    "Exception",
                    ErrorCategory.ObjectNotFound,
                    null);
                WriteError(errorRecord);
            }
        }
    }
}