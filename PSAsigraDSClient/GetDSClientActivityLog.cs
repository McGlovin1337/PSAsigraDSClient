/* To-do:
 * Add ActivityId Parameter
 * Add ActivityType Parameter
 * Add Status Parameter
 * Add BackupSetId Parameter
 * Add ScheduleId Parameter
 * Add User Parameter
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;
using static PSAsigraDSClient.BaseDSClientActivityLog;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientActivityLog")]
    [OutputType(typeof(DSClientAcivityLog))]

    public class GetDSClientActivityLog: DSClientCmdlet
    {
        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Start Date & Time")]
        public DateTime StartTime { get; set; } = DateTime.Parse("01/01/1970");

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify End Date & Time")]
        public DateTime EndTime { get; set; } = DateTime.Now;

        protected override void DSClientProcessRecord()
        {
            int epochStart = DateTimeToUnixEpoch(StartTime);
            int epochEnd = DateTimeToUnixEpoch(EndTime);

            WriteVerbose("Retrieving Activity Log Info...");
            activity_log_info[] activityLogs = DSClientSession.activity_log(epochStart, epochEnd);
            WriteVerbose("Yielded " + activityLogs.Count() + " Activities");

            List<DSClientAcivityLog> ActivityLogs = new List<DSClientAcivityLog>();

            foreach(var activity in activityLogs)
            {
                DSClientAcivityLog activityLog = new DSClientAcivityLog(activity);
                ActivityLogs.Add(activityLog);
            }

            ActivityLogs.ForEach(WriteObject);
        }
    }
}