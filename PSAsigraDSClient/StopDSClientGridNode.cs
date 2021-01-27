using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Stop, "DSClientGridNode", SupportsShouldProcess = true)]
    [OutputType(typeof(DSClientGridNodeStatus))]

    public class StopDSClientGridNode : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Grid NodeId")]
        public int NodeId { get; set; }

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "stop", HelpMessage = "Stop DS-Client Service")]
        public SwitchParameter StopService { get; set; }

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "shutdown", HelpMessage = "Shutdown DS-Client Computer")]
        public SwitchParameter Shutdown { get; set; }

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "restart", HelpMessage = "Restart DS-Client Computer")]
        [Alias("Reboot")]
        public SwitchParameter Restart { get; set; }

        [Parameter(Position = 2, HelpMessage = "Specify Number of Minutes to Wait for Running Activities to Stop (0 = Infinate)")]
        public int Wait { get; set; } = 0;

        [Parameter(HelpMessage = "Specify to Output Object")]
        public SwitchParameter PassThru { get; set; }

        protected override void DSClientProcessRecord()
        {
            GridClientConnection gridClient = GridClientConnection.from(DSClientSession);

            string nodeName = gridClient.getGridInfo()
                                .Single(node => node.id == NodeId)
                                .name;

            EGridClientStopType stopType = EGridClientStopType.EGridClientStopType__UNDEFINED;

            if (StopService)
                stopType = EGridClientStopType.EGridClientStopType__StopServiceOnly;
            else if (Shutdown)
                stopType = EGridClientStopType.EGridClientStopType__ShutDownComputer;
            else if (Restart)
                stopType = EGridClientStopType.EGridClientStopType__RebootComputer;

            grid_client_stop_option stopOption = new grid_client_stop_option
            {
                type = stopType,
                wait_minutes = Wait
            };

            if (ShouldProcess($"DS-Client '{nodeName}'", $"{EnumToString(stopType)}"))
            {
                WriteVerbose($"Performing Operation: {EnumToString(stopType)} on DS-Client Node {nodeName}");
                gridClient.stopNode(NodeId, stopOption);

                if (PassThru)
                    WriteObject(new DSClientGridNodeStatus(gridClient.getGridInfo()
                                                                    .Single(node => node.id == NodeId)));
            }
        }

        private class DSClientGridNodeStatus
        {
            public int NodeId { get; private set; }
            public string Name { get; private set; }
            public int Activities { get; private set; }
            public string Status { get; private set; }

            public DSClientGridNodeStatus(grid_client_info gridInfo)
            {
                NodeId = gridInfo.id;
                Name = gridInfo.name;
                Activities = gridInfo.activities;
                Status = EnumToString(gridInfo.node_status);
            }
        }
    }
}