﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientScheduleDetail")]
    [OutputType(typeof(DSClientScheduleDetail))]
    public class GetDSClientScheduleDetail: BaseDSClientSchedule
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Schedule Id")]
        [ValidateNotNullOrEmpty]
        public int ScheduleId { get; set; }

        [Parameter(Position = 1, HelpMessage = "Get the Schedule Details for the specified type: OneTime, Daily, Weekly, Monthly, Undefined")]
        [ValidateSet("OneTime", "Daily", "Weekly", "Monthly", "Undefined")]
        public string Type { get; set; }

        protected override void DSClientProcessRecord()
        {
            ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();

            WriteVerbose($"Performing Action: Retrieve Schedule with ScheduleId: {ScheduleId}");
            Schedule Schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);

            WriteVerbose($"Performing Action: Retrieve Schedule Info for ScheduleId: {ScheduleId}");
            schedule_info scheduleInfo = DSClientScheduleMgr.definedScheduleInfo(ScheduleId);

            WriteVerbose($"Performing Action: Retrieve Schedule Details for ScheduleId: {ScheduleId}");
            ScheduleDetail[] ScheduleDetails = Schedule.getDetails();
            WriteVerbose($"Notice: Yielded {ScheduleDetails.Count()} Schedule Details");

            List<DSClientScheduleDetail> DSClientScheduleDetail = new List<DSClientScheduleDetail>();

            // Create a Base ID for each Detail in the Schedule
            int DetailID = 0;

            foreach (ScheduleDetail schedule in ScheduleDetails)
            {
                DetailID += 1;
                WriteVerbose($"Performing Action: Process DetailID {DetailID}");

                ScheduleDetail scheduleDetail = schedule;

                // Get the Schedule Type
                EScheduleDetailType scheduleType = scheduleDetail.getType();
                string ScheduleType = EnumToString(scheduleType);
                if (MyInvocation.BoundParameters.ContainsKey("Type"))
                    if (Type != ScheduleType)
                        continue;

                // Get the Schedule Type Detail
                dynamic ScheduleTypeDetail = 0;
                if (scheduleType == EScheduleDetailType.EScheduleDetailType__OneTime)
                {
                    OneTimeScheduleDetail oneTimeScheduleDetail = OneTimeScheduleDetail.from(schedule);
                    int oneTimeStartDate = oneTimeScheduleDetail.get_start_date();
                    ScheduleTypeDetail = new OneTimeScheduleType(oneTimeStartDate);
                }
                else if (scheduleType == EScheduleDetailType.EScheduleDetailType__Daily)
                {
                    DailyScheduleDetail dailyScheduleDetail = DailyScheduleDetail.from(schedule);
                    int repeatDays = dailyScheduleDetail.getRepeatDays();
                    ScheduleTypeDetail = new DailyScheduleType(repeatDays);
                }
                else if (scheduleType == EScheduleDetailType.EScheduleDetailType__Weekly)
                {
                    WeeklyScheduleDetail weeklyScheduleDetail = WeeklyScheduleDetail.from(schedule);
                    int repeatWeeks = weeklyScheduleDetail.getRepeatWeeks();
                    int scheduleDays = weeklyScheduleDetail.getScheduleDays();
                    ScheduleTypeDetail = new WeeklyScheduleType(repeatWeeks, scheduleDays);
                }
                else if (scheduleType == EScheduleDetailType.EScheduleDetailType__Monthly)
                {
                    MonthlyScheduleDetail monthlyScheduleDetail = MonthlyScheduleDetail.from(schedule);
                    int repeatMonths = monthlyScheduleDetail.getRepeatMonths();
                    int scheduleDay = monthlyScheduleDetail.getScheduleDay();
                    EScheduleMonthlyStartDay monthlyStartDay = monthlyScheduleDetail.getScheduleWhen();
                    ScheduleTypeDetail = new MonthlyScheduleType(repeatMonths, scheduleDay, monthlyStartDay);
                }

                // Get the Validation Schedule Options
                int validationOptValue = scheduleDetail.getValidationOptions();
                DSClientValidationScheduleOptions ValidationOpts = new DSClientValidationScheduleOptions(validationOptValue);

                // Get the BLM Schedule Options
                blm_schedule_options blmScheduleOpts = scheduleDetail.getBLMOptions();
                DSClientBLMScheduleOptions BLMScheduleOpts = new DSClientBLMScheduleOptions(blmScheduleOpts);

                // Get the Schedule Start Time
                time_in_day startTime = scheduleDetail.getStartTime();
                DSClientCommon.TimeInDay StartTime = new DSClientCommon.TimeInDay(startTime);

                // Get the Schedule End Time
                time_in_day endTime = scheduleDetail.getEndTime();
                DSClientCommon.TimeInDay EndTime = new DSClientCommon.TimeInDay(endTime);

                // Get End Time Enabled/Disabled Status
                bool endTimeEnabled = scheduleDetail.hasEndTime();

                // Get the Schedule Start Date
                int startDate = scheduleDetail.getPeriodStartDate();
                DateTime StartDate = DSClientCommon.UnixEpochToDateTime(startDate);

                // Get the Schedule End Date
                int endDate = scheduleDetail.getPeriodEndDate();
                DateTime EndDate = DSClientCommon.UnixEpochToDateTime(endDate);

                // Get the Exclusion Setting value
                bool isExcluded = scheduleDetail.isExcluded();

                // Get the Enabled Tasks
                int enabledTasks = scheduleDetail.getTasks();
                DSClientScheduleTasks EnabledTasks = new DSClientScheduleTasks(enabledTasks);

                scheduleDetail.Dispose();
                DSClientScheduleDetail.Add(new DSClientScheduleDetail
                {
                    DetailId = DetailID,
                    ScheduleId = scheduleInfo.id,
                    ScheduleName = scheduleInfo.name,
                    Type = ScheduleTypeDetail,
                    StartTime = StartTime,
                    EndTime = EndTime,
                    EndTimeEnabled = endTimeEnabled,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    Excluded = isExcluded,
                    EnabledTasks = EnabledTasks,
                    ValidationOptions = ValidationOpts,
                    BLMScheduleOptions = BLMScheduleOpts
                });
            }

            DSClientScheduleDetail.ForEach(WriteObject);

            Schedule.Dispose();
            DSClientScheduleMgr.Dispose();
        }
    }
}