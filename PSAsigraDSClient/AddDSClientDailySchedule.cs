using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientDailySchedule")]

    public class AddDSClientDailySchedule: BaseDSClientScheduleDetail
    {
        [Parameter(Position = 1, HelpMessage = "Set the Repeat Frequency in Days")]
        [ValidateNotNullOrEmpty]
        public int RepeatDays { get; set; } = 1;

        [Parameter(Position = 2, HelpMessage = "Set the Start Date for this Schedule Detail")]
        [ValidateNotNullOrEmpty]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Parameter(Position = 3, HelpMessage = "Set the End Date for this Schedule Detail")]
        [ValidateNotNullOrEmpty]
        public DateTime EndDate { get; set; }

        protected override ScheduleDetail ProcessScheduleDetail(ScheduleManager dsClientScheduleMgr)
        {
            // Create a new Daily Schedule
            DailyScheduleDetail newDailyDetail = dsClientScheduleMgr.createDailyDetail();

            // Set the Repeat Days
            newDailyDetail.setRepeatDays(RepeatDays);

            // Set the Start Date
            newDailyDetail.setPeriodStartDate(DateTimeToUnixEpoch(StartDate));

            // Set the End Date if specified
            if (MyInvocation.BoundParameters.ContainsKey("EndDate"))
                newDailyDetail.setPeriodEndDate(DateTimeToUnixEpoch(EndDate));
            else
                newDailyDetail.setPeriodEndDate(0);

            return newDailyDetail;
        }
    }
}