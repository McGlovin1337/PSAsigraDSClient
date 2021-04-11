using System;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientDailySchedule", SupportsShouldProcess = true)]

    public class SetDSClientDailySchedule : BaseDSClientSetScheduleDetail
    {
        [Parameter(Position = 1, HelpMessage = "Set the Repeat Frequency in Days")]
        [ValidateNotNullOrEmpty]
        public int RepeatDays { get; set; }

        protected override void CheckScheduleDetailType(ScheduleDetail scheduleDetail)
        {
            if (scheduleDetail.getType() != EScheduleDetailType.EScheduleDetailType__Daily)
                throw new Exception("Selected Schedule Detail is not of Type Daily");
        }

        protected override void ProcessScheduleDetail(ScheduleDetail scheduleDetail)
        {
            DailyScheduleDetail dailyScheduleDetail = DailyScheduleDetail.from(scheduleDetail);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(RepeatDays)))
                if (ShouldProcess($"Daily Schedule Detail Id: {DetailId}", $"Set Repeat Every {RepeatDays} Days"))
                    dailyScheduleDetail.setRepeatDays(RepeatDays);

            dailyScheduleDetail.Dispose();
        }
    }
}

/* 
 * To-do:
 * - Create Cmdlet Help
 * - Add to Module Manifest
 */