using System;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientWeeklySchedule", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class SetDSClientWeeklySchedule : BaseDSClientSetScheduleDetail
    {
        [Parameter(Position = 1, HelpMessage = "Set the Repeat Frequency in Weeks")]
        [ValidateNotNullOrEmpty]
        public int RepeatWeeks { get; set; }

        [Parameter(Position = 2, HelpMessage = "Set the Days of Week the Schedule Executes on")]
        [ValidateSet("Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday")]
        [ValidateNotNullOrEmpty]
        public string[] ScheduleDays { get; set; }

        protected override void CheckScheduleDetailType(ScheduleDetail scheduleDetail)
        {
            if (scheduleDetail.getType() != EScheduleDetailType.EScheduleDetailType__Weekly)
                throw new Exception("Selected Schedule Detail is not of Type Weekly");
        }

        protected override void ProcessScheduleDetail(ScheduleDetail scheduleDetail)
        {
            WeeklyScheduleDetail weeklyScheduleDetail = WeeklyScheduleDetail.from(scheduleDetail);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(RepeatWeeks)))
                if (ShouldProcess($"Schedule Detail Id: {DetailId}", $"Set Repeat Every {RepeatWeeks} Weeks"))
                    weeklyScheduleDetail.setRepeatWeeks(RepeatWeeks);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(ScheduleDays)))
            {
                int weekdays = 0;
                string days = null;
                foreach (string day in ScheduleDays)
                {
                    weekdays += (int)StringToEnum<EScheduleWeekDays>(day);
                    if (day != ScheduleDays.Last())
                        days += $"{day},";
                    else
                        days += day;
                }

                if (ShouldProcess($"Schedule Detail Id: {DetailId}", $"Set Selected Week Days to '{days}'"))
                    weeklyScheduleDetail.setScheduleDays(weekdays);
            }

            weeklyScheduleDetail.Dispose();
        }
    }
}