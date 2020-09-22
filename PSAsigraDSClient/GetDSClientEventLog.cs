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
                filteredEvents.ToList().ForEach(WriteObject);
            else
                DSClientEventLogs.ForEach(WriteObject);
        }

        private class DSClientEventLog
        {
            public int EventId { get; set; }
            public DateTime DateTime { get; set; }
            public string EventType { get; set; }
            public string Description { get; set; }
            public string ExtraInfo { get; set; }
            public string EventCategory { get; set; }
            public int ActivityId { get; set; }
            public int NodeId { get; set; }
            public string User { get; set; }

            public DSClientEventLog(event_log_info eventLogInfo)
            {
                EventId = eventLogInfo.event_id;
                DateTime = UnixEpochToDateTime(eventLogInfo.when);
                EventType = EEventTypeToString(eventLogInfo.type);
                Description = eventLogInfo.description;
                ExtraInfo = eventLogInfo.extra_info;
                EventCategory = EEventCategoryToString(eventLogInfo.category);
                ActivityId = eventLogInfo.activity_id;
                NodeId = eventLogInfo.node_id;
                User = eventLogInfo.user;
            }

            private string EEventTypeToString(EEventType eventType)
            {
                string EventType = null;

                switch(eventType)
                {
                    case EEventType.EEventType__Information:
                        EventType = "Information";
                        break;
                    case EEventType.EEventType__Warning:
                        EventType = "Warning";
                        break;
                    case EEventType.EEventType__Error:
                        EventType = "Error";
                        break;
                }

                return EventType;
            }

            private string EEventCategoryToString(EEventCategory eventCategory)
            {
                string EventCategory = null;

                switch(eventCategory)
                {
                    case EEventCategory.EEventCategory__Application:
                        EventCategory = "Application";
                        break;
                    case EEventCategory.EEventCategory__Socket:
                        EventCategory = "Socket";
                        break;
                    case EEventCategory.EEventCategory__Message:
                        EventCategory = "Message";
                        break;
                    case EEventCategory.EEventCategory__Database:
                        EventCategory = "Database";
                        break;
                    case EEventCategory.EEventCategory__Exception:
                        EventCategory = "Exception";
                        break;
                    case EEventCategory.EEventCategory__IO:
                        EventCategory = "IO";
                        break;
                    case EEventCategory.EEventCategory__System:
                        EventCategory = "System";
                        break;
                    case EEventCategory.EEventCategory__Security:
                        EventCategory = "Security";
                        break;
                    case EEventCategory.EEventCategory__MAPI:
                        EventCategory = "MAPI";
                        break;
                    case EEventCategory.EEventCategory__Novell:
                        EventCategory = "Novell";
                        break;
                    case EEventCategory.EEventCategory__Oracle:
                        EventCategory = "Oracle";
                        break;
                    case EEventCategory.EEventCategory__RMAN:
                        EventCategory = "RMAN";
                        break;
                    case EEventCategory.EEventCategory__XML:
                        EventCategory = "XML";
                        break;
                    case EEventCategory.EEventCategory__DB2:
                        EventCategory = "DB2";
                        break;
                }

                return EventCategory;
            }
        }
    }
}