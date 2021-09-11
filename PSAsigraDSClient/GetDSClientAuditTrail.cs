using System;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientAuditTrail")]
    [OutputType(typeof(DSClientAuditTrail))]

    sealed public class GetDSClientAuditTrail: DSClientCmdlet
    {
        [Parameter(HelpMessage = "From Date and Time")]
        public DateTime From { get; set; } = DateTime.Parse("1/1/1970");

        [Parameter(HelpMessage = "To Date and Time")]
        public DateTime To { get; set; } = DateTime.Now;

        [Parameter(HelpMessage = "Filter by Operation")]
        [ValidateSet("All", "Insert", "Delete", "Update", "Action")]
        public string Operation { get; set; } = "All";

        [Parameter(HelpMessage = "Filter by Table")]
        [ValidateSet("All", "Setup", "BackupSchedule", "Permission", "BackupSet", "BackupItems", "UserId", "Notification", "PrePost", "ScheduleDetail", "GroupId", "Roles", "Retention", "RetentionRule", "Config", "SetOption", "SetAdditionalOpt", "NasFilter", "NasVolume", "NasSchedule", "NasRetention", "NasVault")]
        public string Table { get; set; } = "All";

        [Parameter(HelpMessage = "Filter by User")]
        public string User { get; set; } = "";

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Performing Operation: Retrieve Audit Trail Data");
            audit_trail_info[] auditTrailInfo = DSClientSession.getAuditTrail(
                DateTimeToUnixEpoch(From),
                DateTimeToUnixEpoch(To),
                StringToEnum<EAuditTrailOperation>(Operation),
                StringToEnum<EAuditTrailTable>(Table),
                User);

            List<DSClientAuditTrail> auditTrail = new List<DSClientAuditTrail>();

            foreach (audit_trail_info audit in auditTrailInfo)
                auditTrail.Add(new DSClientAuditTrail(audit));

            auditTrail.ForEach(WriteObject);
        }

        private class DSClientAuditTrail
        {
            public DateTime Timestamp { get; private set; }
            public string Operation { get; private set; }
            public string Table { get; private set; }
            public string Description { get; private set; }
            public string User { get; private set; }

            public DSClientAuditTrail(audit_trail_info auditInfo)
            {
                Timestamp = UnixEpochToDateTime(auditInfo.when);
                Operation = EnumToString(auditInfo.operation);
                Table = EnumToString(auditInfo.table);
                Description = auditInfo.description;
                User = auditInfo.user;
            }
        }
    }
}