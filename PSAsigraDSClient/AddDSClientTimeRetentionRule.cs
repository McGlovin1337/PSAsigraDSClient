using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientTimeRetentionRule")]

    public class AddDSClientTimeRetentionRule: BaseDSClientTimeRetentionRule
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify RetentionRuleId to Apply Time Retention to")]
        public int RetentionRuleId { get; set; }

        protected override void ProcessRetentionRule()
        {
            RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();

            RetentionRule RetentionRule = DSClientRetentionRuleMgr.definedRules().Single(rule => rule.getID() == RetentionRuleId);

            // Recent Generations
            RetentionRule.setKeepLastGenerations(KeepLastGens);

            if (MyInvocation.BoundParameters.ContainsKey("KeepAllGensTimeValue"))
                KeepAllGenerationsRule(RetentionRule, KeepAllGensTimeValue, KeepAllGensTimeUnit);

            // Interval based Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("IntervalTimeValue"))
            {
                WriteVerbose("Adding Interval Time Based Retention Rule...");
                IntervalTimeRetentionOption intervalTimeRetention = DSClientRetentionRuleMgr.createIntervalTimeRetention();

                RetentionRule.addTimeRetentionOption(IntervalTimeRetentionRule(intervalTimeRetention, IntervalTimeValue, IntervalTimeUnit, IntervalValidForValue, IntervalValidForUnit));
            }

            // Weekly based Time Retention
            if (WeeklyRetentionDay != null)
            {
                WriteVerbose("Adding Weekly Time Based Retention Rule...");
                WeeklyTimeRetentionOption weeklyTimeRetention = DSClientRetentionRuleMgr.createWeeklyTimeRetention();

                RetentionRule.addTimeRetentionOption(WeeklyTimeRetentionRule(weeklyTimeRetention, WeeklyRetentionDay, WeeklyRetentionHour, WeeklyRetentionMinute, WeeklyValidForValue, WeeklyValidForUnit));
            }

            // Monthly based Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("MonthlyRetentionDay"))
            {
                WriteVerbose("Adding Monthly Time Based Retention Rule...");
                MonthlyTimeRetentionOption monthlyTimeRetention = DSClientRetentionRuleMgr.createMonthlyTimeRetention();

                RetentionRule.addTimeRetentionOption(MonthlyTimeRetentionRule(monthlyTimeRetention, MonthlyRetentionDay, MonthlyRetentionHour, MonthlyRetentionMinute, MonthlyValidForValue, MonthlyValidForUnit));
            }

            // Yearly based Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("YearlyRetentionMonthDay"))
            {
                WriteVerbose("Adding Yearly Time Based Retention Rule...");
                YearlyTimeRetentionOption yearlyTimeRetention = DSClientRetentionRuleMgr.createYearlyTimeRetention();

                RetentionRule.addTimeRetentionOption(YearlyTimeRetentionRule(yearlyTimeRetention, YearlyRetentionMonthDay, YearlyRetentionMonth, YearlyRetentionHour, YearlyRetentionMinute, YearlyValidForValue, YearlyValidForUnit));
            }

            // Move or Delete Obsolete Data
            if (DeleteObsoleteData == true)
                RetentionRule.setMoveObsoleteDataToBLM(false);

            if (MoveObsoleteData == true)
                RetentionRule.setMoveObsoleteDataToBLM(true);

            if (MyInvocation.BoundParameters.ContainsKey("CreateNewBLMPackage"))
                RetentionRule.setCreateNewBLMPackage(CreateNewBLMPackage);

            RetentionRule.Dispose();
            DSClientRetentionRuleMgr.Dispose();
        }
    }
}