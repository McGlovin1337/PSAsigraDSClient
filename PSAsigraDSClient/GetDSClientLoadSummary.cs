using System;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientLoadSummary")]
    [OutputType(typeof(DSClientLoadSummary))]

    sealed public class GetDSClientLoadSummary: DSClientCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "DS-Client Node Id")]
        public int NodeId { get; set; } = 0;

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Performing Action: Retrieve DS-Client Current Load Summary");
            dsclient_load[] dsclientLoad = DSClientSession.getLoadSummaryCurrent(NodeId);

            List<DSClientLoadSummary> DSClientLoad = new List<DSClientLoadSummary>();

            foreach (var loadSummary in dsclientLoad)
            {
                DSClientLoadSummary summary = new DSClientLoadSummary(loadSummary);
                DSClientLoad.Add(summary);
            }

            DSClientLoad.ForEach(WriteObject);
        }

        private class DSClientLoadSummary
        {
            public int NodeId { get; private set; }
            public int Activities { get; private set; }
            public int CpuLoad { get; private set; }
            public int MemoryLoad { get; private set; }
            public int MemoryCommit { get; private set; }
            public int NetworkReceive { get; private set; }
            public int NetworkSend { get; private set; }
            public DateTime TimeStamp { get; private set; }

            public DSClientLoadSummary(dsclient_load dsclientLoad)
            {
                NodeId = dsclientLoad.node_id;
                Activities = dsclientLoad.activities;
                CpuLoad = dsclientLoad.cpu_usage;
                MemoryLoad = dsclientLoad.memory_load;
                MemoryCommit = dsclientLoad.memory_commit;
                NetworkReceive = dsclientLoad.receive;
                NetworkSend = dsclientLoad.send;
                TimeStamp = UnixEpochToDateTime(dsclientLoad.timestamp);
            }
        }
    }
}