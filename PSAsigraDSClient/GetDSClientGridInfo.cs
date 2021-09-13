using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientGridInfo")]
    [OutputType(typeof(DSClientGridInfo))]

    sealed public class GetDSClientGridInfo : DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Performing Action: Retrieve DS-Client Grid Information");
            GridClientConnection gridClient = GridClientConnection.from(DSClientSession);

            grid_client_info[] gridInfo = gridClient.getGridInfo();

            List<DSClientGridInfo> dsClientGridInfo = new List<DSClientGridInfo>();

            foreach (grid_client_info client in gridInfo)
                dsClientGridInfo.Add(new DSClientGridInfo(client));

            dsClientGridInfo.ForEach(WriteObject);
        }

        private class DSClientGridInfo
        {
            public int NodeId { get; private set; }
            public string Name { get; private set; }
            public string Status { get; private set; }
            public string Type { get; private set; }
            public string InternalAddress { get; private set; }
            public string ExternalAddress { get; private set; }
            public int Activities { get; private set; }
            public int PercentCPU { get; private set; }
            public int PercentMem { get; private set; }
            public int MemoryCommit { get; private set; }
            public int NetSend { get; private set; }
            public int NetReceive { get; private set; }
            public int TaskLimit { get; private set; }

            public DSClientGridInfo(grid_client_info gridInfo)
            {
                NodeId = gridInfo.id;
                Name = gridInfo.name;
                Status = EnumToString(gridInfo.node_status);
                Type = EnumToString(gridInfo.node_type);
                InternalAddress = gridInfo.internal_ip;
                ExternalAddress = gridInfo.external_ip;
                Activities = gridInfo.activities;
                PercentCPU = gridInfo.cpu_usage;
                PercentMem = gridInfo.memory_load;
                MemoryCommit = gridInfo.memory_commit;
                NetSend = gridInfo.send;
                NetReceive = gridInfo.receive;
                TaskLimit = gridInfo.task_limit;
            }
        }
    }
}