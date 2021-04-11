using System;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientSetScheduleDetail : BaseDSClientScheduleDetailParams
    {
        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Schedule Detail Id to modify (use Get-DSClientScheduleDetail)")]
        public int DetailId { get; set; }

        protected abstract void CheckScheduleDetailType(ScheduleDetail scheduleDetail);

        protected abstract void ProcessScheduleDetail(ScheduleDetail scheduleDetail);

        protected override void DSClientProcessRecord()
        {
            Dictionary<string, int> detailHashes = SessionState.PSVariable.GetValue("ScheduleDetail", null) as Dictionary<string, int>;
            if (detailHashes == null)
                throw new Exception("No Schedule Details found in Session State, use Get-DSClientScheduleDetail Cmdlet");

            ScheduleManager scheduleManager = DSClientSession.getScheduleManager();

            WriteVerbose($"Performing Action: Retrieve Schedule with ScheduleId: {ScheduleId}");
            Schedule schedule = scheduleManager.definedSchedule(ScheduleId);

            WriteVerbose($"Performing Action: Retrieve Schedule Detail with DetailId: {DetailId}");
            (ScheduleDetail scheduleDetail, string detailHash) = SelectScheduleDetail(schedule, DetailId, detailHashes);
            CheckScheduleDetailType(scheduleDetail);

            if (MyInvocation.BoundParameters.ContainsKey(nameof(StartDate)))
                if (ShouldProcess($"Schedule Detail Id: {DetailId}", $"Set Start Date to '{StartDate}'"))
                    scheduleDetail.setPeriodStartDate(DateTimeToUnixEpoch(StartDate));

            if (MyInvocation.BoundParameters.ContainsKey(nameof(EndDate)))
                if (ShouldProcess($"Schedule Detail Id: {DetailId}", $"Set End Date to '{EndDate}'"))
                    scheduleDetail.setPeriodEndDate(DateTimeToUnixEpoch(EndDate));

            if (MyInvocation.BoundParameters.ContainsKey(nameof(StartTime)))
                if (ShouldProcess($"Schedule Detail Id: {DetailId}", $"Set Start Time to '{StartTime}'"))
                    scheduleDetail.setStartTime(StringTotime_in_day(StartTime));

            if (MyInvocation.BoundParameters.ContainsKey(nameof(EndTime)))
                if (ShouldProcess($"Schedule Detail Id: {DetailId}", $"Set End Time to '{EndTime}'"))
                    scheduleDetail.setEndTime(StringTotime_in_day(EndTime));

            if (MyInvocation.BoundParameters.ContainsKey(nameof(NoEndTime)))
                if (ShouldProcess($"Schedule Detail Id: {DetailId}", "Set No End Time"))
                    scheduleDetail.setNoEndTime();

            if (MyInvocation.BoundParameters.ContainsKey(nameof(HourlyFrequency)))
                if (ShouldProcess($"Schedule Detail Id: {DetailId}", $"Set Hourly Frequency to '{HourlyFrequency}'"))
                    scheduleDetail.setHourlyFrequency(HourlyFrequency);

            #region EnabledTasks
            // Configure the Tasks for this Schedule Detail
            int origEnabledTasks = scheduleDetail.getTasks();
            int enabledTasks = origEnabledTasks;

            if (MyInvocation.BoundParameters.ContainsKey(nameof(Backup)))
            {
                bool backupTask = ((int)ETaskToRun.ETaskToRun__Backup & enabledTasks) > 0;
                if (backupTask != Backup)
                    _ = backupTask ? enabledTasks += (int)ETaskToRun.ETaskToRun__Backup : enabledTasks -= (int)ETaskToRun.ETaskToRun__Backup;
            }

            if (MyInvocation.BoundParameters.ContainsKey(nameof(Retention)))
            {
                bool retentionTask = ((int)ETaskToRun.ETaskToRun__Retention & enabledTasks) > 0;
                if (retentionTask != Retention)
                    _ = retentionTask ? enabledTasks += (int)ETaskToRun.ETaskToRun__Retention : enabledTasks -= (int)ETaskToRun.ETaskToRun__Retention;
            }

            if (MyInvocation.BoundParameters.ContainsKey(nameof(Validation)))
            {
                bool validationTask = ((int)ETaskToRun.ETaskToRun__Validation & enabledTasks) > 0;
                if (validationTask != Validation)
                    _ = validationTask ? enabledTasks += (int)ETaskToRun.ETaskToRun__Validation : enabledTasks -= (int)ETaskToRun.ETaskToRun__Validation;
            }

            if (MyInvocation.BoundParameters.ContainsKey(nameof(BLM)))
            {
                bool blmTask = ((int)ETaskToRun.ETaskToRun__BLM & enabledTasks) > 0;
                if (blmTask != BLM)
                    _ = blmTask ? enabledTasks += (int)ETaskToRun.ETaskToRun__BLM : enabledTasks -= (int)ETaskToRun.ETaskToRun__BLM;
            }

            if (MyInvocation.BoundParameters.ContainsKey(nameof(LANScan)))
            {
                bool lanscanTask = ((int)ETaskToRun.ETaskToRun__LANScan & enabledTasks) > 0;
                if (lanscanTask != LANScan)
                    _ = lanscanTask ? enabledTasks += (int)ETaskToRun.ETaskToRun__LANScan : enabledTasks -= (int)ETaskToRun.ETaskToRun__LANScan;
            }

            if (MyInvocation.BoundParameters.ContainsKey(nameof(CleanTrash)))
            {
                bool trashTask = ((int)ETaskToRun.ETaskToRun__CleanTrash & enabledTasks) > 0;
                if (trashTask != CleanTrash)
                    _ = trashTask ? enabledTasks += (int)ETaskToRun.ETaskToRun__CleanTrash : enabledTasks -= (int)ETaskToRun.ETaskToRun__CleanTrash;
            }

            if (origEnabledTasks != enabledTasks)
                if (ShouldProcess($"Schedule Detail Id: {DetailId}", "Update Enabled Tasks"))
                    scheduleDetail.setTasks(enabledTasks);
            #endregion EnabledTasks

            #region ValidationOptions
            // Configure the Validation Task Options
            int origValidationOptions = scheduleDetail.getValidationOptions();
            int validationOptions = origValidationOptions;

            if (MyInvocation.BoundParameters.ContainsKey(nameof(LastGenOnly)))
            {
                bool lastGen = ((int)EScheduleValidationOption.EScheduleValidationOption__LastGen & validationOptions) > 0;
                if (lastGen != LastGenOnly)
                    _ = lastGen ? validationOptions += (int)EScheduleValidationOption.EScheduleValidationOption__LastGen : validationOptions -= (int)EScheduleValidationOption.EScheduleValidationOption__LastGen;
            }

            if (MyInvocation.BoundParameters.ContainsKey(nameof(ExcludeDeleted)))
            {
                bool excludeDeleted = ((int)EScheduleValidationOption.EScheduleValidationOption__ExcludeDeletedFiles & validationOptions) > 0;
                if (excludeDeleted != ExcludeDeleted)
                    _ = excludeDeleted ? validationOptions += (int)EScheduleValidationOption.EScheduleValidationOption__ExcludeDeletedFiles : validationOptions -= (int)EScheduleValidationOption.EScheduleValidationOption__ExcludeDeletedFiles;
            }

            if (MyInvocation.BoundParameters.ContainsKey(nameof(Resume)))
            {
                bool resume = ((int)EScheduleValidationOption.EScheduleValidationOption__Resume & validationOptions) > 0;
                if (resume != Resume)
                    _ = resume ? validationOptions += (int)EScheduleValidationOption.EScheduleValidationOption__Resume : validationOptions -= (int)EScheduleValidationOption.EScheduleValidationOption__Resume;
            }

            if (origValidationOptions != validationOptions)
                if (ShouldProcess($"Schedule Detail Id: {DetailId}", "Update Validation Options"))
                    scheduleDetail.setValidationOptions(validationOptions);
            #endregion ValidationOptions

            #region BLMOptions
            // Configure the BLM Archive Options
            blm_schedule_options blmOptions = scheduleDetail.getBLMOptions();
            bool updated = false;

            if (MyInvocation.BoundParameters.ContainsKey(nameof(IncludeAllGenerations)))
            {
                blmOptions.include_all_generations = IncludeAllGenerations;
                updated = true;
            }

            if (MyInvocation.BoundParameters.ContainsKey(nameof(BackReference)))
            {
                blmOptions.use_back_reference = BackReference;
                updated = true;
            }

            if (MyInvocation.BoundParameters.ContainsKey(nameof(PackageClosing)))
            {
                EActivePackageClosing packageClosing = StringToEnum<EActivePackageClosing>(PackageClosing);
                if (packageClosing != blmOptions.package_close)
                    blmOptions.package_close = packageClosing;

                updated = true;
            }

            if (updated)
                if (ShouldProcess($"Schedule Detail Id: {DetailId}", "Update BLM Options"))
                    scheduleDetail.setBLMOptions(blmOptions);
            #endregion BLMOptions

            // Process Cmdlet Specific Options
            ProcessScheduleDetail(scheduleDetail);

            // Update the Schedule Detail Hash and update it in SessionState Dictionary
            string newHash = ScheduleDetailHash(schedule, scheduleDetail);
            detailHashes.Remove(detailHash);
            detailHashes.Add(newHash, DetailId);
            SessionState.PSVariable.Set("ScheduleDetail", detailHashes);

            scheduleDetail.Dispose();
            schedule.Dispose();
            scheduleManager.Dispose();
        }
    }
}