using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientMonthlySchedule")]

    public class AddDSClientMonthlySchedule: BaseDSClientScheduleDetail
    {
        [Parameter(Position = 1, HelpMessage = "Set the Repeat Frequency in Months")]
        [ValidateNotNullOrEmpty]
        public int RepeatMonths { get; set; } = 1;

        [Parameter(Position = 2, HelpMessage = "Set the Day of the Month the Schedule Executes on")]
        [ValidateNotNullOrEmpty]
        public int ScheduleDay { get; set; } = 1;

        [Parameter(Position = 3, HelpMessage = "Set the Monthly Start Day")]
        [ValidateSet("DayOfMonth", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun", "Day", "WeekDay", "WeekEndDay")]
        public string MonthlyStartDay { get; set; }

        [Parameter(Position = 4, HelpMessage = "Set the Start Date for this Schedule Detail")]
        [ValidateNotNullOrEmpty]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Parameter(Position = 5, HelpMessage = "Set the End Date for this Schedule Detail")]
        [ValidateNotNullOrEmpty]
        public DateTime EndDate { get; set; }

        protected override ScheduleDetail ProcessScheduleDetail(ScheduleManager dsClientScheduleMgr)
        {
            // Create a new Monthly Schedule
            MonthlyScheduleDetail newMonthlyDetail = dsClientScheduleMgr.createMonthlyDetail();

            // Set the Repeat Months
            newMonthlyDetail.setRepeatMonths(RepeatMonths);

            // Set the Schedule Day, NB: When "ScheduleStartDay" Parameter is used, anything >= 4 is Last
            newMonthlyDetail.setScheduleDay(ScheduleDay);

            // Set the Week Day in Month if specified
            if (MonthlyStartDay != null)
                newMonthlyDetail.setScheduleWhen(StringToEScheduleMonthlyStartDay(MonthlyStartDay));

            // Set the Start Date
            newMonthlyDetail.setPeriodStartDate(DateTimeToUnixEpoch(StartDate));

            // Set the End Date if specified
            if (EndDate != null)
                newMonthlyDetail.setPeriodEndDate(DateTimeToUnixEpoch(EndDate));
            else
                newMonthlyDetail.setPeriodEndDate(0);

            return newMonthlyDetail;
        }
    }
}