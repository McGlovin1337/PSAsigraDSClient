using System;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientGridLog")]
    [OutputType(typeof(DSClientGridLog))]

    sealed public class GetDSClientGridLog : DSClientCmdlet
    {
        [Parameter(HelpMessage = "Specify From Date")]
        public DateTime From { get; set; } = DateTime.Parse("1/1/1970");

        [Parameter(HelpMessage = "Specify To Date")]
        public DateTime To { get; set; } = DateTime.Now;

        protected override void DSClientProcessRecord()
        {
            GridClientConnection gridClient = GridClientConnection.from(DSClientSession);

            grid_log_info[] gridLogInfo = gridClient.getGridLog(DateTimeToUnixEpoch(From), DateTimeToUnixEpoch(To));

            List<DSClientGridLog> gridLog = new List<DSClientGridLog>();

            foreach (grid_log_info log in gridLogInfo)
                gridLog.Add(new DSClientGridLog(log));

            gridLog.ForEach(WriteObject);
        }

        private class DSClientGridLog
        {
            public int EventId { get; private set; }
            public DateTime DateTime { get; private set; }
            public string EventType { get; private set; }
            public string Description { get; private set; }
            public int NodeId { get; private set; }
            public string NodeAddress { get; private set; }

            public DSClientGridLog(grid_log_info gridLog)
            {
                EventId = gridLog.event_id;
                DateTime = UnixEpochToDateTime(gridLog.time);
                EventType = EnumToString(gridLog.type);
                Description = gridLog.text;
                NodeId = gridLog.node_id;
                NodeAddress = gridLog.ip;
            }
        }
    }
}