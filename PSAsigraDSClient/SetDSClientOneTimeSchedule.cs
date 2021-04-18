using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientOneTimeSchedule", SupportsShouldProcess = true)]

    public class SetDSClientOneTimeSchedule : BaseDSClientSetScheduleDetail
    {
        protected override void CheckScheduleDetailType(ScheduleDetail scheduleDetail)
        {
            if (scheduleDetail.getType() != EScheduleDetailType.EScheduleDetailType__OneTime)
                throw new Exception("Specified Schedule Detail is not of Type One Time");
        }

        protected override void ProcessScheduleDetail(ScheduleDetail scheduleDetail)
        {
            OneTimeScheduleDetail oneTimeScheduleDetail = OneTimeScheduleDetail.from(scheduleDetail);

            if (ShouldProcess($"Schedule Detail Id: {DetailId}", $"Set Start Date to '{StartDate}'"))
                oneTimeScheduleDetail.set_start_date(DateTimeToUnixEpoch(StartDate));

            oneTimeScheduleDetail.Dispose();
        }
    }
}