using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientScheduleDetail: BaseDSClientSchedule
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The ScheduleId to add Schedule Detail to")]
        [ValidateNotNullOrEmpty]
        public int ScheduleId { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Specify the Start Time in 24Hr Notation HH:mm:ss")]
        [ValidateNotNullOrEmpty]
        public string StartTime { get; set; }

        [Parameter(HelpMessage = "Specify the End Time in 24Hr Notation HH:mm:ss")]
        [ValidateNotNullOrEmpty]
        public string EndTime { get; set; } = "35:00:00";

        [Parameter(HelpMessage = "Specify No End Time")]
        public SwitchParameter NoEndTime { get; set; }

        [Parameter(HelpMessage = "Set the Hourly Frequency")]
        [ValidateNotNullOrEmpty]
        public int HourlyFrequency { get; set; } = 0;

        [Parameter(HelpMessage = "Enable the Backup Task")]
        public SwitchParameter Backup { get; set; }

        [Parameter(HelpMessage = "Enable the Retention Task")]
        public SwitchParameter Retention { get; set; }

        [Parameter(HelpMessage = "Enable the Validation Task")]
        public SwitchParameter Validation { get; set; }

        [Parameter(HelpMessage = "Enable the BLM Task")]
        public SwitchParameter BLM { get; set; }

        [Parameter(HelpMessage = "Enable the LAN Scan Shares Task")]
        public SwitchParameter LANScan { get; set; }

        [Parameter(HelpMessage = "Enable the Clean Local-only Trash Task")]
        public SwitchParameter CleanTrash { get; set; }

        [Parameter(HelpMessage = "Validation Task Option: Validate Last Generation Only")]
        public SwitchParameter LastGenOnly { get; set; }

        [Parameter(HelpMessage = "Validation Task Option: Exclude Deleted Files from Source")]
        public SwitchParameter ExcludeDeleted { get; set; }

        [Parameter(HelpMessage = "Validation Task Option: Resume from previously interrupted location")]
        public SwitchParameter Resume { get; set; }

        [Parameter(HelpMessage = "BLM Task Option: Specify if to Include All Generations")]
        public SwitchParameter IncludeAllGenerations { get; set; }

        [Parameter(HelpMessage = "BLM Task Option: Specify if to use Back Referencing")]
        public SwitchParameter BackReference { get; set; }

        [Parameter(HelpMessage = "BLM Task Option: Specify how the package should be closed")]
        [ValidateSet("DoNotClose", "CloseAtStart", "CloseAtEnd")]
        public string PackageClosing { get; set; }

        protected abstract ScheduleDetail ProcessScheduleDetail(ScheduleManager dsClientScheduleMgr);

        protected override void DSClientProcessRecord()
        {
            ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();

            WriteVerbose($"Performing Action: Retrieve Schedule with ScheduleId: {ScheduleId}");
            Schedule schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);

            // Define and Process a New Schedule Detail
            WriteVerbose("Performing Action: Build new Schedule Detail");
            ScheduleDetail newScheduleDetail = ProcessScheduleDetail(DSClientScheduleMgr);

            // Format the StartTime and EndTime
            time_in_day startTime = StringTotime_in_day(StartTime);
            time_in_day endTime = StringTotime_in_day(EndTime);

            // Add the StartTime to the Schedule
            newScheduleDetail.setStartTime(startTime);

            // Add the EndTime if NoEndTime Parameter isn't specified
            if (!MyInvocation.BoundParameters.ContainsKey("NoEndTime"))
                newScheduleDetail.setEndTime(endTime);
            else
                newScheduleDetail.setNoEndTime();

            // Add the Hourly Frequency
            newScheduleDetail.setHourlyFrequency(HourlyFrequency);

            // Convert the Enabled Tasks to an int, and add to Schedule
            int enabledTasks = 0;

            if (Backup == true)
                enabledTasks += (int)ETaskToRun.ETaskToRun__Backup;
            if (Retention == true)
                enabledTasks += (int)ETaskToRun.ETaskToRun__Retention;
            if (Validation == true)
                enabledTasks += (int)ETaskToRun.ETaskToRun__Validation;
            if (BLM == true)
                enabledTasks += (int)ETaskToRun.ETaskToRun__BLM;
            if (LANScan == true)
                enabledTasks += (int)ETaskToRun.ETaskToRun__LANScan;
            if (CleanTrash == true)
                enabledTasks += (int)ETaskToRun.ETaskToRun__CleanTrash;

            newScheduleDetail.setTasks(enabledTasks);

            // Convert the Enabled Validation options to int, and add to Schedule
            if ((enabledTasks & (int)ETaskToRun.ETaskToRun__Validation) > 0)
            {
                int enabledValidationOpts = 0;

                if (LastGenOnly == true)
                    enabledValidationOpts += (int)EScheduleValidationOption.EScheduleValidationOption__LastGen;
                if (ExcludeDeleted == true)
                    enabledValidationOpts += (int)EScheduleValidationOption.EScheduleValidationOption__ExcludeDeletedFiles;
                if (Resume == true)
                    enabledValidationOpts += (int)EScheduleValidationOption.EScheduleValidationOption__Resume;

                newScheduleDetail.setValidationOptions(enabledValidationOpts);
            }

            // Set the BLM Options
            if ((enabledTasks & (int)ETaskToRun.ETaskToRun__BLM) > 0)
            {
                blm_schedule_options blmOptions = new blm_schedule_options
                {
                    include_all_generations = IncludeAllGenerations,
                    use_back_reference = BackReference,
                    package_close = (EActivePackageClosing)Enum.Parse(typeof(EActivePackageClosing), $"EActivePackageClosing__{PackageClosing}", true)
                };

                newScheduleDetail.setBLMOptions(blmOptions);
            }

            // Add the Schedule Detail to the Schedule
            WriteVerbose($"Performing Action: Add Schedule Detail to Schedule with ScheduleId: {ScheduleId}");
            schedule.addDetail(newScheduleDetail);

            schedule.Dispose();
            DSClientScheduleMgr.Dispose();
        }
    }
}