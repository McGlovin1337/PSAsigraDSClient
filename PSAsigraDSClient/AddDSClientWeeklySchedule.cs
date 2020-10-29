using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientWeeklySchedule")]

    public class AddDSClientWeeklySchedule: BaseDSClientScheduleDetail
    {
        [Parameter(Position = 1, HelpMessage = "Set the Repeat Frequency in Weeks")]
        [ValidateNotNullOrEmpty]
        public int RepeatWeeks { get; set; } = 1;

        [Parameter(Position = 2, HelpMessage = "Set the Days of Week the Schedule Executes on")]
        [ValidateSet("Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun")]
        [ValidateNotNullOrEmpty]
        public string[] ScheduleDays { get; set; } = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

        [Parameter(Position = 3, HelpMessage = "Set the Start Date for this Schedule Detail")]
        [ValidateNotNullOrEmpty]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Parameter(Position = 4, HelpMessage = "Set the End Date for this Schedule Detail")]
        [ValidateNotNullOrEmpty]
        public DateTime EndDate { get; set; }

        protected override ScheduleDetail ProcessScheduleDetail(ScheduleManager dsClientScheduleMgr)
        {
            // Create a new Weekly Schedule
            WeeklyScheduleDetail newWeeklyDetail = dsClientScheduleMgr.createWeeklyDetail();

            // Set the Repeat Weeks
            newWeeklyDetail.setRepeatWeeks(RepeatWeeks);

            // Set the Scheduled Days to run
            int WeekDays = ScheduleWeekDaysToInt(ScheduleDays);
            newWeeklyDetail.setScheduleDays(WeekDays);

            // Set the Start Date
            newWeeklyDetail.setPeriodStartDate(DateTimeToUnixEpoch(StartDate));

            // Set the End Date if specified
            if (MyInvocation.BoundParameters.ContainsKey("EndDate"))
                newWeeklyDetail.setPeriodEndDate(DateTimeToUnixEpoch(EndDate));
            else
                newWeeklyDetail.setPeriodEndDate(0);

            return newWeeklyDetail;
        }
    }
}