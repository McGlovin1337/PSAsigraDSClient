using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using AsigraDSClientApi;
using System.Globalization;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientScheduleDetail")]

    public class AddDSClientScheduleDetail: BaseDSClientSchedule, IDynamicParameters
    {
        private OneTimeParams OneTimeParam;
        private DailyParams DailyParam;
        private WeeklyParams WeeklyParam;
        private MonthlyParams MonthlyParam;

        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, HelpMessage = "The ScheduleId to add Schedule Detail to")]
        [ValidateNotNullOrEmpty]
        public int ScheduleId { get; set; }

        [Parameter(Position = 1, ParameterSetName = "DetailType", Mandatory = true, HelpMessage = "The Type of Schedule Detail to Add")]
        [ValidateSet("OneTime", "Daily", "Weekly", "Monthly")]
        public string Type { get; set; }

        [Parameter(Position = 6, Mandatory = true, HelpMessage = "Specify the Start Time in 24Hr Notation HH:mm:ss")]
        [ValidateNotNullOrEmpty]
        public string StartTime { get; set; } = "19:00:00";

        [Parameter(Position = 7, HelpMessage = "Specify the End Time in 24Hr Notation HH:mm:ss")]
        [ValidateNotNullOrEmpty]
        public string EndTime { get; set; } = "35:00:00";

        [Parameter(Position = 8, HelpMessage = "Specify No End Time")]
        public SwitchParameter NoEndTime { get; set; }

        [Parameter(Position = 9, HelpMessage = "Set the Hourly Frequency")]
        [ValidateNotNullOrEmpty]
        public int HourlyFrequency { get; set; } = 0;

        [Parameter(Position = 10, HelpMessage = "Enable the Backup Task")]
        public SwitchParameter Backup { get; set; }

        [Parameter(Position = 11, HelpMessage = "Enable the Retention Task")]
        public SwitchParameter Retention { get; set; }

        [Parameter(Position = 12, HelpMessage = "Enable the Validation Task")]
        public SwitchParameter Validation { get; set; }

        [Parameter(Position = 13, HelpMessage = "Enable the BLM Task")]
        public SwitchParameter BLM { get; set; }

        [Parameter(Position = 14, HelpMessage = "Enable the LAN Scan Shares Task")]
        public SwitchParameter LANScan { get; set; }

        [Parameter(Position = 15, HelpMessage = "Enable the Clean Local-only Trash Task")]
        public SwitchParameter CleanTrash { get; set; }

        [Parameter(Position = 16, HelpMessage = "Validation Task Option: Validate Last Generation Only")]
        public SwitchParameter LastGenOnly { get; set; }

        [Parameter(Position = 17, HelpMessage = "Validation Task Option: Exclude Deleted Files from Source")]
        public SwitchParameter ExcludeDeleted { get; set; }

        [Parameter(Position = 18, HelpMessage = "Validation Task Option: Resume from previously interrupted location")]
        public SwitchParameter Resume { get; set; }

        [Parameter(Position = 19, HelpMessage = "BLM Task Option: Specify if to Include All Generations")]
        public SwitchParameter IncludeAllGenerations { get; set; }

        [Parameter(Position = 20, HelpMessage = "BLM Task Option: Specify if to use Back Referencing")]
        public SwitchParameter BackReference { get; set; }

        [Parameter(Position = 21, HelpMessage = "BLM Task Option: Specify how the package should be closed")]
        [ValidateSet("DoNotClose", "CloseAtStart", "CloseAtEnd")]
        public string PackageClosing { get; set; }

        public object GetDynamicParameters()
        {
            switch(Type)
            {
                case "OneTime":
                    OneTimeParam = new OneTimeParams();
                    return OneTimeParam;
                case "Daily":
                    DailyParam = new DailyParams();
                    return DailyParam;
                case "Weekly":
                    WeeklyParam = new WeeklyParams();
                    return WeeklyParam;
                case "Monthly":
                    MonthlyParam = new MonthlyParams();
                    return MonthlyParam;
                default:
                    return null;
            }
        }

        protected override void ProcessScheduleDetail()
        {
            ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();

            WriteVerbose("Looking up DSClient for ScheduleId " + ScheduleId + "...");
            Schedule Schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);

            // Define a New Schedule
            WriteVerbose("Building a new Schedule Detail...");
            ScheduleDetail NewScheduleDetail = new ScheduleDetail();

            if (Type == "OneTime")
            {
                // Create a new One Time Schedule
                WriteVerbose("Building a new OneTime Schedule Detail...");
                OneTimeScheduleDetail newOneTimeDetail = DSClientScheduleMgr.createOneTimeDetail();

                // Set the Start Date
                WriteVerbose("Setting the Start Date...");
                DateTime startDateTime = DateTime.ParseExact(OneTimeParam.StartDate, "d/M/yyyy", CultureInfo.InvariantCulture);
                int startDate = DSClientCommon.DateTimeToUnixEpoch(startDateTime);
                newOneTimeDetail.set_start_date(startDate);

                NewScheduleDetail = newOneTimeDetail;
            }

            if (Type == "Daily")
            {
                // Create a new Daily Schedule
                WriteVerbose("Building a new Daily Schedule Detail...");
                DailyScheduleDetail newDailyDetail = DSClientScheduleMgr.createDailyDetail();

                // Set the Repeat Days
                WriteVerbose("Setting the Repeat Days...");
                newDailyDetail.setRepeatDays(DailyParam.RepeatDays);

                // Set the Start Date
                WriteVerbose("Setting the Start Date...");
                DateTime startDateTime = DateTime.ParseExact(DailyParam.StartDate, "d/M/yyyy", CultureInfo.InvariantCulture);
                int startDate = DSClientCommon.DateTimeToUnixEpoch(startDateTime);
                newDailyDetail.setPeriodStartDate(startDate);

                // Set the End Date if specified
                DateTime endDateTime;
                if (DailyParam.EndDate != null)
                {
                    WriteVerbose("Setting the End Date...");
                    endDateTime = DateTime.ParseExact(DailyParam.EndDate, "d/M/yyyy", CultureInfo.InvariantCulture);
                    int endDate = DSClientCommon.DateTimeToUnixEpoch(endDateTime);
                    newDailyDetail.setPeriodEndDate(endDate);
                }

                NewScheduleDetail = newDailyDetail;
            }

            if (Type == "Weekly")
            {
                // Create a new Weekly Schedule
                WriteVerbose("Building a new Weekly Schedule Detail...");
                WeeklyScheduleDetail newWeeklyDetail = DSClientScheduleMgr.createWeeklyDetail();

                // Set the Repeat Weeks
                WriteVerbose("Setting the Repeat Weeks...");
                newWeeklyDetail.setRepeatWeeks(WeeklyParam.RepeatWeeks);

                // Set the Scheduled Days to run
                WriteVerbose("Setting the Scheduled Days...");
                int WeekDays = ScheduleWeekDaysToInt(WeeklyParam.ScheduleDays);
                newWeeklyDetail.setScheduleDays(WeekDays);

                // Set the Start Date
                WriteVerbose("Setting the Start Date...");
                DateTime startDateTime = DateTime.ParseExact(WeeklyParam.StartDate, "d/M/yyyy", CultureInfo.InvariantCulture);
                int startDate = DSClientCommon.DateTimeToUnixEpoch(startDateTime);
                newWeeklyDetail.setPeriodStartDate(startDate);

                // Set the End Date if specified
                DateTime endDateTime;
                if (WeeklyParam.EndDate != null)
                {
                    WriteVerbose("Setting the End Date...");
                    endDateTime = DateTime.ParseExact(WeeklyParam.EndDate, "d/M/yyyy", CultureInfo.InvariantCulture);
                    int endDate = DSClientCommon.DateTimeToUnixEpoch(endDateTime);
                    newWeeklyDetail.setPeriodEndDate(endDate);
                }

                NewScheduleDetail = newWeeklyDetail;
            }

            if (Type == "Monthly")
            {
                // Create a new Monthly Schedule
                WriteVerbose("Building a new Monthly Schedule Detail...");
                MonthlyScheduleDetail newMonthlyDetail = DSClientScheduleMgr.createMonthlyDetail();

                // Set the Repeat Months
                WriteVerbose("Setting the Repeat Months...");
                newMonthlyDetail.setRepeatMonths(MonthlyParam.RepeatMonths);

                // Set the Schedule Day, NB: When "ScheduleStartDay" Parameter is used, anything >= 4 is Last
                WriteVerbose("Setting the Scheduled Start Day...");
                newMonthlyDetail.setScheduleDay(MonthlyParam.ScheduleDay);

                // Set the Week Day in Month if specified
                if (MonthlyParam.MonthlyStartDay != null)
                {
                    WriteVerbose("Setting the WeekDay of the Month...");
                    EScheduleMonthlyStartDay monthlyStartDay = StringToEScheduleMonthlyStartDay(MonthlyParam.MonthlyStartDay);
                    newMonthlyDetail.setScheduleWhen(monthlyStartDay);
                }

                // Set the Start Date
                WriteVerbose("Setting the Start Date...");
                DateTime startDateTime = DateTime.ParseExact(MonthlyParam.StartDate, "d/M/yyyy", CultureInfo.InvariantCulture);
                int startDate = DSClientCommon.DateTimeToUnixEpoch(startDateTime);
                newMonthlyDetail.setPeriodStartDate(startDate);

                // Set the End Date if specified
                DateTime endDateTime;
                if (MonthlyParam.EndDate != null)
                {
                    WriteVerbose("Setting the End Date...");
                    endDateTime = DateTime.ParseExact(MonthlyParam.EndDate, "d/M/yyyy", CultureInfo.InvariantCulture);
                    int endDate = DSClientCommon.DateTimeToUnixEpoch(endDateTime);
                    newMonthlyDetail.setPeriodEndDate(endDate);
                }

                NewScheduleDetail = newMonthlyDetail;
            }

            // Format the StartTime and EndTime
            WriteVerbose("Formatting Start and End Times...");
            time_in_day startTime = StringTotime_in_day(StartTime);
            time_in_day endTime = StringTotime_in_day(EndTime);

            // Add the StartTime to the Schedule
            WriteVerbose("Setting the Start Time...");
            NewScheduleDetail.setStartTime(startTime);

            // Add the EndTime if NoEndTime Parameter isn't specified
            WriteVerbose("Setting the End Time...");
            if (!MyInvocation.BoundParameters.ContainsKey("NoEndTime"))
                NewScheduleDetail.setEndTime(endTime);
            else
                NewScheduleDetail.setNoEndTime();

            // Add the Hourly Frequency
            WriteVerbose("Setting the Hourly Frequency...");
            NewScheduleDetail.setHourlyFrequency(HourlyFrequency);

            // Convert the Enabled Tasks to an int, and add to Schedule
            WriteVerbose("Setting the Enabled Tasks...");
            int enabledTasks = 0;

            if (Backup == true)
                enabledTasks += 1;
            if (Retention == true)
                enabledTasks += 2;
            if (Validation == true)
                enabledTasks += 4;
            if (BLM == true)
                enabledTasks += 8;
            if (LANScan == true)
                enabledTasks += 16;
            if (CleanTrash == true)
                enabledTasks += 32;

            WriteVerbose("Enabled Tasks Integer: " + enabledTasks);

            NewScheduleDetail.setTasks(enabledTasks);

            // Convert the Enabled Validation options to int, and add to Schedule
            WriteVerbose("Setting the Validation Task Options...");
            int enabledValidationOpts = 0;

            if (LastGenOnly == true)
                enabledValidationOpts += 1;
            if (ExcludeDeleted == true)
                enabledValidationOpts += 2;
            if (Resume == true)
                enabledValidationOpts += 4;

            WriteVerbose("Validation Task Options Integer is: " + enabledValidationOpts);

            NewScheduleDetail.setValidationOptions(enabledValidationOpts);

            // Set the BLM Options
            WriteVerbose("Setting the BLM Task Options...");
            blm_schedule_options blmOptions = new blm_schedule_options();
            blmOptions.include_all_generations = IncludeAllGenerations;
            blmOptions.use_back_reference = BackReference;
            blmOptions.package_close = StringToEActivePackageClosing(PackageClosing);

            // Add the Schedule Detail to the Schedule
            WriteVerbose("Schedule Detail built, adding to ScheduleId: " + ScheduleId);
            Schedule.addDetail(NewScheduleDetail);
            WriteObject("New " + Type + " Schedule Detail added to ScheduleId " + ScheduleId);

            Schedule.Dispose();
        }

        private class OneTimeParams
        {
            [Parameter(Position = 2, ParameterSetName = "DetailType", Mandatory = true, HelpMessage = "Set the Start Date for this Schedule Detail")]
            [ValidateNotNullOrEmpty]
            public string StartDate { get; set; } = DateTime.Now.ToString();
        }

        private class DailyParams
        {
            [Parameter(Position = 2, ParameterSetName = "DetailType", HelpMessage = "Set the Repeat Frequency in Days")]
            [ValidateNotNullOrEmpty]
            public int RepeatDays { get; set; } = 1;

            [Parameter(Position = 3, ParameterSetName = "DetailType", HelpMessage = "Set the Start Date for this Schedule Detail")]
            [ValidateNotNullOrEmpty]
            public string StartDate { get; set; } = DateTime.Now.ToString();

            [Parameter(Position = 4, ParameterSetName = "DetailType", HelpMessage = "Set the End Date for this Schedule Detail")]
            [ValidateNotNullOrEmpty]
            public string EndDate { get; set; }
        }

        private class WeeklyParams
        {
            [Parameter(Position = 2, ParameterSetName = "DetailType", HelpMessage = "Set the Repeat Frequency in Weeks")]
            [ValidateNotNullOrEmpty]
            public int RepeatWeeks { get; set; } = 1;

            [Parameter(Position = 3, ParameterSetName = "DetailType", HelpMessage = "Set the Days of Week the Schedule Executes on")]
            [ValidateSet("Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun")]
            [ValidateNotNullOrEmpty]
            public string[] ScheduleDays { get; set; } = {"Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

            [Parameter(Position = 4, ParameterSetName = "DetailType", HelpMessage = "Set the Start Date for this Schedule Detail")]
            [ValidateNotNullOrEmpty]
            public string StartDate { get; set; } = DateTime.Now.ToString();

            [Parameter(Position = 5, ParameterSetName = "DetailType", HelpMessage = "Set the End Date for this Schedule Detail")]
            [ValidateNotNullOrEmpty]
            public string EndDate { get; set; }
        }

        private class MonthlyParams
        {
            [Parameter(Position = 2, ParameterSetName = "DetailType", HelpMessage = "Set the Repeat Frequency in Months")]
            [ValidateNotNullOrEmpty]
            public int RepeatMonths { get; set; } = 1;

            [Parameter(Position = 3, ParameterSetName = "DetailType", HelpMessage = "Set the Day of the Month the Schedule Executes on")]
            [ValidateNotNullOrEmpty]
            public int ScheduleDay { get; set; } = 1;

            [Parameter(Position = 4, ParameterSetName = "DetailType", HelpMessage = "Set the Monthly Start Day")]
            [ValidateSet("DayOfMonth", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun", "Day", "WeekDay", "WeekEndDay")]
            public string MonthlyStartDay { get; set; }

            [Parameter(Position = 5, ParameterSetName = "DetailType", HelpMessage = "Set the Start Date for this Schedule Detail")]
            [ValidateNotNullOrEmpty]
            public string StartDate { get; set; } = DateTime.Now.ToString();

            [Parameter(Position = 6, ParameterSetName = "DetailType", HelpMessage = "Set the End Date for this Schedule Detail")]
            [ValidateNotNullOrEmpty]
            public string EndDate { get; set; }
        }
    }
}