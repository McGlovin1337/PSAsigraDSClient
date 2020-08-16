using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientSchedule")]
    [OutputType(typeof(DSClientScheduleInfo))]
    public class GetDSClientSchedule: BaseDSClientSchedule
    {
        protected override void ProcessScheduleDetail()
        {
            ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();

            List<DSClientScheduleInfo> ScheduleInfo = new List<DSClientScheduleInfo>();

            var Schedules = DSClientScheduleMgr.definedSchedulesInfo();

            foreach (var schedule in Schedules)
            {
                ScheduleInfo.Add(new DSClientScheduleInfo
                {
                    Active = schedule.active,
                    AdminOnly = schedule.administratorsOnly,
                    CPUThrottle = schedule.backupCPUThrottle,
                    ConcurrentBackupSets = schedule.concurrentBackupSets,
                    ScheduleId = schedule.id,
                    Name = schedule.name,
                    ShortName = schedule.shortName,
                    NetworkDetection = schedule.usingNetworkDetection
                });
            }

            ScheduleInfo.ForEach(WriteObject);

            DSClientScheduleMgr.Dispose();
        }
    }
}