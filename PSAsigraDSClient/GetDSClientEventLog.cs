using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientEventLog")]
    [OutputType(typeof(DSClientEventLog))]

    public class GetDSClientEventLog: DSClientCmdlet
    {
        [Parameter(Position = 0, HelpMessage = "Specify Date and Time to Search from")]
        [ValidateNotNullOrEmpty]
        public DateTime DateStart { get; set; }

        [Parameter(Position = 1, HelpMessage = "Specify Date and Time to Search to")]
        [ValidateNotNullOrEmpty]
        public DateTime DateEnd { get; set; }

        [Parameter(Position = 2, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify a specific ActivityId")]
        public int ActivityId { get; set; } = 0;

        [Parameter(Position = 3, ValueFromPipelineByPropertyName = true, HelpMessage = "Filter for specific Event Types")]
        [ValidateSet("Information", "Warning", "Error")]
        [ValidateNotNullOrEmpty]
        public string[] EventType { get; set; }

        [Parameter(Position = 4, ValueFromPipelineByPropertyName = true, HelpMessage = "Filter for specific Event Categories")]
        [ValidateSet("Application", "Socket", "Message", "Database", "Exception", "IO", "System", "Security", "MAPI", "Novell", "Oracle", "RMAN", "XML", "DB2")]
        [ValidateNotNullOrEmpty]
        public string[] EventCategory { get; set; }

        [Parameter(Position = 5, ValueFromPipelineByPropertyName = true, HelpMessage = "Filter for specific User events")]
        public string User { get; set; }

        [Parameter(Position = 6, ValueFromPipelineByPropertyName = true, HelpMessage = "Filter to specifc DS-Client NodeId")]
        public int NodeId { get; set; }

        protected override void DSClientProcessRecord()
        {
            int dateStart = 0;
            int dateEnd = 0;

            if (MyInvocation.BoundParameters.ContainsKey("DateStart"))
                dateStart = DateTimeToUnixEpoch(DateStart);

            if (MyInvocation.BoundParameters.ContainsKey("DateEnd"))
                dateEnd = DateTimeToUnixEpoch(DateEnd);

            event_log_info[] eventLogInfo = DSClientSession.event_log(dateStart, dateEnd, ActivityId);

            List<DSClientEventLog> DSClientEventLogs = new List<DSClientEventLog>();

            foreach (var eventLog in eventLogInfo)
            {
                DSClientEventLog dSClientEventLog = new DSClientEventLog(eventLog);
                DSClientEventLogs.Add(dSClientEventLog);
            }

            IEnumerable<DSClientEventLog> filteredEvents = null;

            if (MyInvocation.BoundParameters.ContainsKey("EventType"))
                filteredEvents = DSClientEventLogs.Where(log => EventType.Contains(log.EventType));

            if (MyInvocation.BoundParameters.ContainsKey("EventCategory"))
                filteredEvents = (filteredEvents == null) ? DSClientEventLogs.Where(log => EventCategory.Contains(log.EventCategory)) : filteredEvents.Where(log => EventCategory.Contains(log.EventCategory));

            if (User != null)
                filteredEvents = (filteredEvents == null) ? DSClientEventLogs.Where(log => log.User == User) : filteredEvents.Where(log => log.User == User);

            if (MyInvocation.BoundParameters.ContainsKey("NodeId"))
                filteredEvents = (filteredEvents == null) ? DSClientEventLogs.Where(log => log.NodeId == NodeId) : filteredEvents.Where(log => log.NodeId == NodeId);

            if (filteredEvents != null)
            {
                filteredEvents.ToList().Sort((x, y) => x.DateTime.CompareTo(y.DateTime));
                filteredEvents.ToList().ForEach(WriteObject);
            }
            else
            {
                DSClientEventLogs.Sort((x, y) => x.DateTime.CompareTo(y.DateTime));
                DSClientEventLogs.ForEach(WriteObject);
            }
        }

        private class DSClientEventLog
        {
            public int EventId { get; private set; }
            public DateTime DateTime { get; private set; }
            public string EventType { get; private set; }
            public string Description { get; private set; }
            public string ExtraInfo { get; private set; }
            public string EventCategory { get; private set; }
            public int ActivityId { get; private set; }
            public int NodeId { get; private set; }
            public string User { get; private set; }

            public DSClientEventLog(event_log_info eventLogInfo)
            {
                EventId = eventLogInfo.event_id;
                DateTime = UnixEpochToDateTime(eventLogInfo.when);
                EventType = EnumToString(eventLogInfo.type);
                Description = eventLogInfo.description;
                ExtraInfo = eventLogInfo.extra_info;
                EventCategory = EnumToString(eventLogInfo.category);
                ActivityId = eventLogInfo.activity_id;
                NodeId = eventLogInfo.node_id;
                User = eventLogInfo.user;
            }
        }
    }
}