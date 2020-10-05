using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientRetentionRule")]

    public class NewDSClientRetentionRule: BaseDSClientTimeRetentionRule
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Retention Rule Name")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Archive Rule Time Value")]
        public int ArchiveTimeValue { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Archive Rule Time Unit")]
        [ValidateSet("Minutes", "Hours", "Days", "Weeks", "Months", "Years")]
        public string ArchiveTimeUnit { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify an existing Archive Filter Rule")]
        public string ArchiveFilterRule { get; set; }

        protected override void ProcessRetentionRule()
        {
            // Perform Parameter Validation
            if (CleanupDeletedFiles)
                if ((MyInvocation.BoundParameters.ContainsKey("CleanupDeletedAfterValue") && CleanupDeletedAfterUnit == null) || (!MyInvocation.BoundParameters.ContainsKey("CleanupDeletedAfterValue") && CleanupDeletedAfterUnit != null))
                    throw new ParameterBindingException("CleanupDeletedAfterValue and CleanupDeletedAfterUnit must be specified when CleanupDeletedFiles specified");

            if (MyInvocation.BoundParameters.ContainsKey("DeleteGensPriorToStub") && MyInvocation.BoundParameters.ContainsKey("DeleteNonStubGens"))
                throw new ParameterBindingException("DeleteGensPriorToStub cannot be specified with DeleteNonStubGens");

            if ((MyInvocation.BoundParameters.ContainsKey("ArchiveTimeValue") && ArchiveTimeUnit == null) || (!MyInvocation.BoundParameters.ContainsKey("ArchiveTimeValue") && ArchiveTimeUnit != null))
                throw new ParameterBindingException("ArchiveTimeValue and ArchiveTimeUnit must both be specified together");

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
            if (CleanupDeletedFiles)
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
            if (DeleteGensPriorToStub)
            {
                NewRetentionRule.setDeleteStub(true);
                NewRetentionRule.setDeleteStubAllGens(true);
            }
            else if (DeleteNonStubGens)
            {
                NewRetentionRule.setDeleteStub(true);
                NewRetentionRule.setDeleteStubAllGens(false);
            }

            // Set Time Based Retention
            NewRetentionRule.setKeepLastGenerations(KeepLastGens);

            if (MyInvocation.BoundParameters.ContainsKey("KeepAllGensTimeValue"))
                KeepAllGenerationsRule(NewRetentionRule, KeepAllGensTimeValue, KeepAllGensTimeUnit);

            // Interval based Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("IntervalTimeValue"))
            {
                IntervalTimeRetentionOption intervalTimeRetention = DSClientRetentionRuleMgr.createIntervalTimeRetention();

                NewRetentionRule.addTimeRetentionOption(IntervalTimeRetentionRule(intervalTimeRetention, IntervalTimeValue, IntervalTimeUnit, IntervalValidForValue, IntervalValidForUnit));
            }

            // Weekly based Time Retention
            if (WeeklyRetentionDay != null)
            {
                WeeklyTimeRetentionOption weeklyTimeRetention = DSClientRetentionRuleMgr.createWeeklyTimeRetention();

                NewRetentionRule.addTimeRetentionOption(WeeklyTimeRetentionRule(weeklyTimeRetention, WeeklyRetentionDay, WeeklyRetentionHour, WeeklyRetentionMinute, WeeklyValidForValue, WeeklyValidForUnit));
            }

            // Monthly based Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("MonthlyRetentionDay"))
            {
                MonthlyTimeRetentionOption monthlyTimeRetention = DSClientRetentionRuleMgr.createMonthlyTimeRetention();

                NewRetentionRule.addTimeRetentionOption(MonthlyTimeRetentionRule(monthlyTimeRetention, MonthlyRetentionDay, MonthlyRetentionHour, MonthlyRetentionMinute, MonthlyValidForValue, MonthlyValidForUnit));
            }

            // Yearly based Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("YearlyRetentionMonthDay"))
            {
                YearlyTimeRetentionOption yearlyTimeRetention = DSClientRetentionRuleMgr.createYearlyTimeRetention();

                NewRetentionRule.addTimeRetentionOption(YearlyTimeRetentionRule(yearlyTimeRetention, YearlyRetentionMonthDay, YearlyRetentionMonth, YearlyRetentionHour, YearlyRetentionMinute, YearlyValidForValue, YearlyValidForUnit));
            }

            // Move or Delete Obsolete Data
            if (DeleteObsoleteData)
                NewRetentionRule.setMoveObsoleteDataToBLM(false);

            if (MoveObsoleteData)
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
            if (LSCleanupDeletedFiles)
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

            // Archive Rule Configuration (requires retention rule to be added to DS-Client Database first)
            if (MyInvocation.BoundParameters.ContainsKey("ArchiveTimeValue"))
            {
                WriteVerbose("Setting Archive Rule...");
                ArchiveRule NewArchiveRule = DSClientRetentionRuleMgr.createArchiveRule();

                retention_time_span timeSpan = new retention_time_span
                {
                    period = ArchiveTimeValue,
                    unit = StringToRetentionTimeUnit(ArchiveTimeUnit)
                };
                NewArchiveRule.setTimeSpan(timeSpan);

                if (ArchiveFilterRule != null)
                {
                    ArchiveFilterRule filterRule = DSClientRetentionRuleMgr.definedArchiveFilterRules()
                                            .Single(rule => rule.getName() == ArchiveFilterRule);

                    NewArchiveRule.setFilterRule(filterRule);

                    filterRule.Dispose();
                }

                NewRetentionRule.addArchiveRule(NewArchiveRule);
            }

            int NewRuleId = NewRetentionRule.getID();
            WriteObject("Created new Retention Rule with RetentionRuleId " + NewRuleId);

            NewRetentionRule.Dispose();
            DSClientRetentionRuleMgr.Dispose();
        }
    }
}