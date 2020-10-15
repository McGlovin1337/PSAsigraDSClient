using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientSchedule")]
    [OutputType(typeof(DSClientScheduleInfo))]
    public class GetDSClientSchedule: BaseDSClientSchedule
    {
        protected override void DSClientProcessRecord()
        {
            ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();

            List<DSClientScheduleInfo> ScheduleInfo = new List<DSClientScheduleInfo>();

            var Schedules = DSClientScheduleMgr.definedSchedulesInfo();

            foreach (var schedule in Schedules)
            {
                DSClientScheduleInfo scheduleInfo = new DSClientScheduleInfo(schedule);
                ScheduleInfo.Add(scheduleInfo);
            }

            ScheduleInfo.ForEach(WriteObject);

            DSClientScheduleMgr.Dispose();
        }
    }
}