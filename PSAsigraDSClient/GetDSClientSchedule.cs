using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientSchedule")]
    [OutputType(typeof(DSClientScheduleInfo))]

    sealed public class GetDSClientSchedule: BaseDSClientSchedule
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Retention Rule Id")]
        public int[] ScheduleId { get; set; }

        [Parameter(Position = 1, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Name of the Retention Rule")]
        [ValidateNotNullOrEmpty, SupportsWildcards]
        public string[] Name { get; set; }

        protected override void DSClientProcessRecord()
        {
            ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();

            List<DSClientScheduleInfo> ScheduleInfo = new List<DSClientScheduleInfo>();

            List<schedule_info> schedules = DSClientScheduleMgr.definedSchedulesInfo().ToList();

            if (MyInvocation.BoundParameters.ContainsOneOfKeys(new string[] { nameof(ScheduleId), nameof(Name) }))
            {
                // Filter by ScheduleId
                if (MyInvocation.BoundParameters.ContainsKey(nameof(ScheduleId)))
                {
                    foreach (int scheduleId in ScheduleId)
                    {
                        try
                        {
                            schedule_info scheduleInfo = schedules.Single(s => s.id == scheduleId);
                            ScheduleInfo.Add(new DSClientScheduleInfo(scheduleInfo));
                        }
                        catch
                        {
                            ErrorRecord errorRecord = new ErrorRecord(
                                new Exception($"Schedule Id {scheduleId} not found"),
                                "Exception",
                                ErrorCategory.ObjectNotFound,
                                null);
                            WriteError(errorRecord);
                        }
                    }
                }

                // Filter by Schedule Name
                if (MyInvocation.BoundParameters.ContainsKey(nameof(Name)))
                {
                    WildcardOptions wcOptions = WildcardOptions.IgnoreCase |
                                                WildcardOptions.Compiled;

                    foreach (string name in Name)
                    {
                        WildcardPattern wcPattern = new WildcardPattern(name, wcOptions);

                        IEnumerable<schedule_info> matchedSchedules = schedules.Where(s => wcPattern.IsMatch(s.name));

                        foreach (schedule_info scheduleInfo in matchedSchedules)
                        {
                            ScheduleInfo.Add(new DSClientScheduleInfo(scheduleInfo));
                        }
                    }
                }

                ScheduleInfo = ScheduleInfo.Distinct(new DSClientScheduleInfoComparer()).ToList();
            }
            else
            {
                // Select all Schedules
                foreach (schedule_info schedule in schedules)
                {
                    DSClientScheduleInfo scheduleInfo = new DSClientScheduleInfo(schedule);
                    ScheduleInfo.Add(scheduleInfo);
                }
            }

            ScheduleInfo.ForEach(WriteObject);

            DSClientScheduleMgr.Dispose();
        }
    }
}