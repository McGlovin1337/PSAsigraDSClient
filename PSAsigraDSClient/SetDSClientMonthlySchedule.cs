using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientMonthlySchedule", SupportsShouldProcess = true)]

    public class SetDSClientMonthlySchedule : BaseDSClientSetScheduleDetail
    {
        [Parameter(Position = 1, HelpMessage = "Set the Repeat Frequency in Months")]
        [ValidateNotNullOrEmpty]
        public int RepeatMonths { get; set; }

        [Parameter(Position = 2, HelpMessage = "Set the Day of the Month the Schedule Executes on")]
        [ValidateNotNullOrEmpty]
        public int ScheduleDay { get; set; }

        [Parameter(Position = 3, HelpMessage = "Set the Monthly Start Day")]
        [ValidateSet("DayOfMonth", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday", "Day", "WeekDay", "WeekEndDay")]
        public string MonthlyStartDay { get; set; }

        protected override void CheckScheduleDetailType(ScheduleDetail scheduleDetail)
        {
            if (scheduleDetail.getType() != EScheduleDetailType.EScheduleDetailType__Monthly)
                throw new Exception("Selected Schedule Detail is not of Type Monthly");
        }

        protected override void ProcessScheduleDetail(ScheduleDetail scheduleDetail)
        {
            MonthlyScheduleDetail monthlyScheduleDetail = MonthlyScheduleDetail.from(scheduleDetail);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(RepeatMonths)))
                if (ShouldProcess($"Schedule Detail Id: {DetailId}", $"Set Repeat Every {RepeatMonths} Months"))
                    monthlyScheduleDetail.setRepeatMonths(RepeatMonths);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(ScheduleDay)))
                if (ShouldProcess($"Schedule Detail Id: {DetailId}", $"Set Day of Month to '{ScheduleDay}'"))
                    monthlyScheduleDetail.setScheduleDay(ScheduleDay);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(MonthlyStartDay)))
                if (ShouldProcess($"Schedule Detail Id: {DetailId}", $"Set Monthly Start Day to '{MonthlyStartDay}'"))
                    monthlyScheduleDetail.setScheduleWhen(StringToEnum<EScheduleMonthlyStartDay>(MonthlyStartDay));

            monthlyScheduleDetail.Dispose();
        }
    }
}