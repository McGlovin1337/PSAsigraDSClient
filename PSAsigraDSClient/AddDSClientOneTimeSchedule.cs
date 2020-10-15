using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientOneTimeSchedule")]

    public class AddDSClientOneTimeSchedule: BaseDSClientScheduleDetail
    {
        [Parameter(Position = 1, HelpMessage = "Set the Start Date for this Schedule Detail")]
        [ValidateNotNullOrEmpty]
        public DateTime StartDate { get; set; } = DateTime.Now;

        protected override ScheduleDetail ProcessScheduleDetail(ScheduleManager dsClientScheduleMgr)
        {
            // Create a new One Time Schedule
            OneTimeScheduleDetail newOneTimeDetail = dsClientScheduleMgr.createOneTimeDetail();

            // Set the Start Date
            newOneTimeDetail.set_start_date(DateTimeToUnixEpoch(StartDate));
            
            return newOneTimeDetail;
        }
    }
}