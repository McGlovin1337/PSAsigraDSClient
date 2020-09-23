/* To-Do:
 * Add further Time Based Retention period Parameters (Monthly, Yearly)
 * Add support for Archive Rules
 */

using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientRetentionRule")]

    public class AddDSClientRetentionRule: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Retention Rule Name")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to Cleanup Files Deleted from Source")]
        public SwitchParameter CleanupDeletedFiles { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Cleanup Files Deleted from Source after Time period")]
        [ValidateNotNullOrEmpty]
        public int CleanupDeletedAfterValue { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Cleanup Files Deleted from Source after Time period")]
        [ValidateNotNullOrEmpty]
        [ValidateSet("Minutes", "Hours", "Days", "Weeks", "Months", "Years")]
        public string CleanupDeletedAfterUnit { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Generations to keep of Files Deleted from Source")]
        public int CleanupDeletedKeepGens { get; set; } = 0;

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to Delete All Generations prior to Stub")]
        public SwitchParameter DeleteGensPriorToStub { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to Delete Non-stub Generations prior to Stub")]
        public SwitchParameter DeleteNonStubGens { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the number of most recent Generations to keep")]
        public int KeepLastGens { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Time Period to keep ALL Generations")]
        public int KeepAllGensTimeValue { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Time Period Unit for keeping ALL Generations")]
        [ValidateSet("Minutes", "Hours", "Days")]
        public string KeepAllGensTimeUnit { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Interval Retention Time Value")]
        public int IntervalTimeValue { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Interval Retention Time Unit")]
        [ValidateSet("Minutes", "Hours", "Days", "Weeks", "Months", "Years")]
        public string IntervalTimeUnit { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Time Value Interval is Valid For")]
        public int IntervalValidForValue { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Time Unit Interval is Valid For")]
        [ValidateSet("Hours", "Days", "Weeks", "Months", "Years")]
        public string IntervalValidForUnit { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Weekday for Weekly Time Retention")]
        [ValidateSet("Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday")]
        public string WeeklyRetentionDay { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Weekly Retention Time Hour")]
        [ValidateRange(0, 23)]
        public int WeeklyRetentionHour { get; set; } = 23;

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Weekly Retention Time Minute")]
        [ValidateRange(0, 59)]
        public int WeeklyRetentionMinute { get; set; } = 59;

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Time Value Weekly Time Retention is Valid for")]
        public int WeeklyValidForValue { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Time Unit Weekly Time Retention is Valid for")]
        [ValidateSet("Hours", "Days", "Weeks", "Months", "Years")]
        public string WeeklyValidForUnit { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to Delete Obsolete Data")]
        public SwitchParameter DeleteObsoleteData { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to Move Obsolete Data to BLM")]
        public SwitchParameter MoveObsoleteData { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to create new BLM Packages when moving to BLM")]
        public SwitchParameter CreateNewBLMPackage { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Local Storage Retention Time Value")]
        public int LSRetentionTimeValue { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Local Storage Retention Time Unit")]
        [ValidateSet("Minutes", "Hours", "Days", "Weeks", "Months", "Years")]
        public string LSRetentionTimeUnit { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to Cleanup Files Deleted from Source on Local Storage")]
        public SwitchParameter LSCleanupDeletedFiles { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Cleanup Files Deleted from Source after Time period on Local Storage")]
        public int LSCleanupDeletedAfterValue {get; set;}

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Cleanup Files Deleted from Source after Time period on Local Storage")]
        [ValidateSet("Minutes", "Hours", "Days", "Weeks", "Months", "Years")]
        public string LSCleanupDeletedAfterUnit { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Generations to keep of Files Deleted from Source")]
        public int LSCleanupDeletedKeepGens { get; set; } = 0;

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to Delete Unreferenced Files")]
        public SwitchParameter DeleteUnreferencedFiles { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to Delete Incomplete Components")]
        public SwitchParameter DeleteIncompleteComponents { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Perform Parameter Validation
            if (CleanupDeletedFiles == true)
                if ((MyInvocation.BoundParameters.ContainsKey("CleanupDeletedAfterValue") && CleanupDeletedAfterUnit == null) || (!MyInvocation.BoundParameters.ContainsKey("CleanupDeletedAfterValue") && CleanupDeletedAfterUnit != null))
                    throw new ParameterBindingException("CleanupDeletedAfterValue and CleanupDeletedAfterUnit must be specified when CleanupDeletedFiles specified");

            if (MyInvocation.BoundParameters.ContainsKey("DeleteGensPriorToStub") && MyInvocation.BoundParameters.ContainsKey("DeleteNonStubGens"))
                throw new ParameterBindingException("DeleteGensPriorToStub cannot be specified with DeleteNonStubGens");

            if ((MyInvocation.BoundParameters.ContainsKey("KeepAllGensTimeValue") && KeepAllGensTimeUnit == null) || (!MyInvocation.BoundParameters.ContainsKey("KeepAllGensTimeValue") && KeepAllGensTimeUnit != null))
                throw new ParameterBindingException("KeepAllGensTimeValue and KeepAllGensTimeUnit must be specified together");

            if ((MyInvocation.BoundParameters.ContainsKey("IntervalTimeValue") && IntervalTimeUnit == null) || (!MyInvocation.BoundParameters.ContainsKey("IntervalTimeValue") && IntervalTimeUnit != null))
                throw new ParameterBindingException("IntervalTimeValue and IntervalTimeUnit must be specified together");

            if ((MyInvocation.BoundParameters.ContainsKey("IntervalValidForValue") && IntervalValidForUnit == null) || (!MyInvocation.BoundParameters.ContainsKey("IntervalValidForValue") && IntervalValidForUnit != null))
                throw new ParameterBindingException("IntervalValidForValue and IntervalValidForUnit must be specified together");

            if ((MyInvocation.BoundParameters.ContainsKey("IntervalTimeValue") && !MyInvocation.BoundParameters.ContainsKey("IntervalValidForValue")) || (!MyInvocation.BoundParameters.ContainsKey("IntervalTimeValue") && MyInvocation.BoundParameters.ContainsKey("IntervalValidForValue")))
                throw new ParameterBindingException("IntervalTimeValue and IntervalTimeUnit must be specified with IntervalValidForValue and IntervalValidForUnit");

            if ((WeeklyRetentionDay != null && (!MyInvocation.BoundParameters.ContainsKey("WeeklyValidForValue") || WeeklyValidForUnit == null)))
                throw new ParameterBindingException("WeeklyValidForValue and WeeklyValidForUnit must be specified with WeeklyRetentionDay");

            if (MyInvocation.BoundParameters.ContainsKey("MoveObsoleteData") && MyInvocation.BoundParameters.ContainsKey("DeleteObsoleteData"))
                throw new ParameterBindingException("MoveObsoleteData cannot be specified with DeleteObsoleteData");

            /* API appears to error when creating or editing most Retention Rule settings unless a 2FA Verification code has been set
             * So we send a Dummy validation code, after which we can successfully add and change Retention Rule configuration */
            TFAManager tFAManager = DSClientSession.getTFAManager();
            try
            {
                tFAManager.validateCode("bleh", ERequestCodeEmailType.ERequestCodeEmailType__UNDEFINED);
            }
            catch
            {
                //Do nothing
            }
            tFAManager.Dispose();

            WriteVerbose("Building new Retention Rule...");
            RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();
            RetentionRule NewRetentionRule = DSClientRetentionRuleMgr.createRule();

            // Set Retention Rule Name
            NewRetentionRule.setName(Name);

            // Set Cleanup of Deleted Files from Source
            if (CleanupDeletedFiles == true)
            {
                WriteVerbose("Setting Cleanup of Deleted Files...");
                NewRetentionRule.setCleanupRemovedFiles(CleanupDeletedFiles);

                retention_time_span timeSpan = new retention_time_span
                {
                    period = CleanupDeletedAfterValue,
                    unit = StringToRetentionTimeUnit(CleanupDeletedAfterUnit)
                };
                WriteVerbose("Setting Time Span for Deleted File Cleanup...");
                NewRetentionRule.setCleanupRemovedAfter(timeSpan);

                NewRetentionRule.setCleanupRemovedKeep(CleanupDeletedKeepGens);
            }

            // Set Stub Cleanup
            if (DeleteGensPriorToStub == true)
            {
                NewRetentionRule.setDeleteStub(true);
                NewRetentionRule.setDeleteStubAllGens(true);
            }

            if (DeleteNonStubGens == true)
            {
                NewRetentionRule.setDeleteStub(true);
                NewRetentionRule.setDeleteStubAllGens(false);
            }

            // Set Time Based Retention
            if (MyInvocation.BoundParameters.ContainsKey("KeepLastGens"))
                NewRetentionRule.setKeepLastGenerations(KeepLastGens);

            if (MyInvocation.BoundParameters.ContainsKey("KeepAllGensTimeValue"))
            {
                NewRetentionRule.setKeepGenerationsByPeriod(true);

                retention_time_span timeSpan = new retention_time_span
                {
                    period = KeepAllGensTimeValue,
                    unit = StringToRetentionTimeUnit(KeepAllGensTimeUnit)
                };
                NewRetentionRule.setKeepPeriodTimeSpan(timeSpan);
            }

            // Interval based Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("IntervalTimeValue"))
            {
                IntervalTimeRetentionOption intervalTimeRetention = DSClientRetentionRuleMgr.createIntervalTimeRetention();

                retention_time_span intTimeSpan = new retention_time_span
                {
                    period = IntervalTimeValue,
                    unit = StringToRetentionTimeUnit(IntervalTimeUnit)
                };
                intervalTimeRetention.setRepeatTime(intTimeSpan);

                retention_time_span validTimeSpan = new retention_time_span
                {
                    period = IntervalValidForValue,
                    unit = StringToRetentionTimeUnit(IntervalValidForUnit)
                };
                intervalTimeRetention.setValidFor(validTimeSpan);

                NewRetentionRule.addTimeRetentionOption(intervalTimeRetention);
            }

            // Weekly based Time Retention
            if (WeeklyRetentionDay != null)
            {
                WeeklyTimeRetentionOption weeklyTimeRetention = DSClientRetentionRuleMgr.createWeeklyTimeRetention();

                weeklyTimeRetention.setTriggerDay(StringToEWeekDay(WeeklyRetentionDay));

                time_in_day weeklyTime = new time_in_day
                {
                    hour = WeeklyRetentionHour,
                    minute = WeeklyRetentionMinute,
                    second = 0
                };
                weeklyTimeRetention.setSnapshotTime(weeklyTime);

                retention_time_span validTimeSpan = new retention_time_span
                {
                    period = WeeklyValidForValue,
                    unit = StringToRetentionTimeUnit(WeeklyValidForUnit)
                };
                weeklyTimeRetention.setValidFor(validTimeSpan);

                NewRetentionRule.addTimeRetentionOption(weeklyTimeRetention);
            }

            // Move or Delete Obsolete Data
            if (DeleteObsoleteData == true)
                NewRetentionRule.setMoveObsoleteDataToBLM(false);

            if (MoveObsoleteData == true)
                NewRetentionRule.setMoveObsoleteDataToBLM(true);

            if (MyInvocation.BoundParameters.ContainsKey("CreateNewBLMPackage"))
                NewRetentionRule.setCreateNewBLMPackage(CreateNewBLMPackage);

            // Set Local Storage Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("LSRetentionTimeValue"))
            {
                retention_time_span timeSpan = new retention_time_span
                {
                    period = LSRetentionTimeValue,
                    unit = StringToRetentionTimeUnit(LSRetentionTimeUnit)
                };
                NewRetentionRule.setLocalStorageRetention(timeSpan);
            }

            // Set Local Storage Cleanup of Deleted Files from Source
            if (LSCleanupDeletedFiles == true)
            {
                NewRetentionRule.setLSCleanupRemovedFiles(true);
                NewRetentionRule.setLSCleanupRemovedKeep(LSCleanupDeletedKeepGens);

                retention_time_span timeSpan = new retention_time_span
                {
                    period = LSCleanupDeletedAfterValue,
                    unit = StringToRetentionTimeUnit(LSCleanupDeletedAfterUnit)
                };
                NewRetentionRule.setLSCleanupRemovedAfter(timeSpan);
            }

            // VSS Options
            if (MyInvocation.BoundParameters.ContainsKey("DeleteUnreferencedFiles"))
                NewRetentionRule.setDeleteUnreferencedFiles(DeleteUnreferencedFiles);

            if (MyInvocation.BoundParameters.ContainsKey("DeleteIncompleteComponents"))
                NewRetentionRule.setDeleteIncompleteComponents(DeleteIncompleteComponents);

            // Add the New Retention Rule to DS-Client
            WriteVerbose("Adding new Retention Rule to DS-Client...");
            DSClientRetentionRuleMgr.addRule(NewRetentionRule);

            int NewRuleId = NewRetentionRule.getID();
            WriteObject("Created new Retention Rule with RetentionRuleId " + NewRuleId);

            NewRetentionRule.Dispose();
            DSClientRetentionRuleMgr.Dispose();
        }

        private static EWeekDay StringToEWeekDay(string weekDay)
        {
            EWeekDay WeekDay = EWeekDay.EWeekDay__UNDEFINED;

            switch(weekDay)
            {
                case "Monday":
                    WeekDay = EWeekDay.EWeekDay__Monday;
                    break;
                case "Tuesday":
                    WeekDay = EWeekDay.EWeekDay__Tuesday;
                    break;
                case "Wednesday":
                    WeekDay = EWeekDay.EWeekDay__Wednesday;
                    break;
                case "Thursday":
                    WeekDay = EWeekDay.EWeekDay__Thursday;
                    break;
                case "Friday":
                    WeekDay = EWeekDay.EWeekDay__Friday;
                    break;
                case "Saturday":
                    WeekDay = EWeekDay.EWeekDay__Saturday;
                    break;
                case "Sunday":
                    WeekDay = EWeekDay.EWeekDay__Sunday;
                    break;
            }

            return WeekDay;
        }

        private static RetentionTimeUnit StringToRetentionTimeUnit(string timeUnit)
        {
            RetentionTimeUnit TimeUnit = RetentionTimeUnit.RetentionTimeUnit__UNDEFINED;

            switch(timeUnit)
            {
                case "Seconds":
                    TimeUnit = RetentionTimeUnit.RetentionTimeUnit__Seconds;
                    break;
                case "Minutes":
                    TimeUnit = RetentionTimeUnit.RetentionTimeUnit__Minutes;
                    break;
                case "Hours":
                    TimeUnit = RetentionTimeUnit.RetentionTimeUnit__Hours;
                    break;
                case "Days":
                    TimeUnit = RetentionTimeUnit.RetentionTimeUnit__Days;
                    break;
                case "Weeks":
                    TimeUnit = RetentionTimeUnit.RetentionTimeUnit__Weeks;
                    break;
                case "Months":
                    TimeUnit = RetentionTimeUnit.RetentionTimeUnit__Months;
                    break;
                case "Years":
                    TimeUnit = RetentionTimeUnit.RetentionTimeUnit__Years;
                    break;
            }

            return TimeUnit;
        }
    }
}