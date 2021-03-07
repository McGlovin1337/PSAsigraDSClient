using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientTimeRetentionOption")]

    public class AddDSClientTimeRetentionOption: BaseDSClientTimeRetentionOption
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify RetentionRuleId to Apply Time Retention to")]
        public int RetentionRuleId { get; set; }

        protected override void ProcessRetentionRule(RetentionRule[] retentionRules)
        {
            RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();

            WriteVerbose($"Performing Action: Retrieve Retention Rule with RetentionRuleId: {RetentionRuleId}");
            RetentionRule RetentionRule = retentionRules.Single(rule => rule.getID() == RetentionRuleId);

            // Interval based Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("IntervalTimeValue"))
            {
                WriteVerbose("Performing Action: Add Interval Time Based Retention Rule");
                IntervalTimeRetentionOption intervalTimeRetention = DSClientRetentionRuleMgr.createIntervalTimeRetention();

                RetentionRule.addTimeRetentionOption(IntervalTimeRetentionRule(intervalTimeRetention, IntervalTimeValue, IntervalTimeUnit, IntervalValidForValue, IntervalValidForUnit));
            }

            // Weekly based Time Retention
            if (WeeklyRetentionDay != null)
            {
                WriteVerbose("Performing Action: Add Weekly Time Based Retention Rule");
                WeeklyTimeRetentionOption weeklyTimeRetention = DSClientRetentionRuleMgr.createWeeklyTimeRetention();

                RetentionRule.addTimeRetentionOption(WeeklyTimeRetentionRule(weeklyTimeRetention, WeeklyRetentionDay, WeeklyRetentionHour, WeeklyRetentionMinute, WeeklyValidForValue, WeeklyValidForUnit));
            }

            // Monthly based Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("MonthlyRetentionDay"))
            {
                WriteVerbose("Performing Action: Add Monthly Time Based Retention Rule");
                MonthlyTimeRetentionOption monthlyTimeRetention = DSClientRetentionRuleMgr.createMonthlyTimeRetention();

                RetentionRule.addTimeRetentionOption(MonthlyTimeRetentionRule(monthlyTimeRetention, MonthlyRetentionDay, MonthlyRetentionHour, MonthlyRetentionMinute, MonthlyValidForValue, MonthlyValidForUnit));
            }

            // Yearly based Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("YearlyRetentionMonthDay"))
            {
                WriteVerbose("Performing Action: Add Yearly Time Based Retention Rule");
                YearlyTimeRetentionOption yearlyTimeRetention = DSClientRetentionRuleMgr.createYearlyTimeRetention();

                RetentionRule.addTimeRetentionOption(YearlyTimeRetentionRule(yearlyTimeRetention, YearlyRetentionMonthDay, YearlyRetentionMonth, YearlyRetentionHour, YearlyRetentionMinute, YearlyValidForValue, YearlyValidForUnit));
            }

            RetentionRule.Dispose();
            DSClientRetentionRuleMgr.Dispose();
        }

        protected override void DSClientProcessRecord()
        {
            if ((MyInvocation.BoundParameters.ContainsKey("IntervalTimeValue") && IntervalTimeUnit == null) || (!MyInvocation.BoundParameters.ContainsKey("IntervalTimeValue") && IntervalTimeUnit != null))
                throw new ParameterBindingException("IntervalTimeValue and IntervalTimeUnit must be specified together");

            if ((MyInvocation.BoundParameters.ContainsKey("IntervalValidForValue") && IntervalValidForUnit == null) || (!MyInvocation.BoundParameters.ContainsKey("IntervalValidForValue") && IntervalValidForUnit != null))
                throw new ParameterBindingException("IntervalValidForValue and IntervalValidForUnit must be specified together");

            if ((MyInvocation.BoundParameters.ContainsKey("IntervalTimeValue") && !MyInvocation.BoundParameters.ContainsKey("IntervalValidForValue")) || (!MyInvocation.BoundParameters.ContainsKey("IntervalTimeValue") && MyInvocation.BoundParameters.ContainsKey("IntervalValidForValue")))
                throw new ParameterBindingException("IntervalTimeValue and IntervalTimeUnit must be specified with IntervalValidForValue and IntervalValidForUnit");

            if (WeeklyRetentionDay != null && (!MyInvocation.BoundParameters.ContainsKey("WeeklyValidForValue") || WeeklyValidForUnit == null))
                throw new ParameterBindingException("WeeklyValidForValue and WeeklyValidForUnit must be specified with WeeklyRetentionDay");

            if (MyInvocation.BoundParameters.ContainsKey("MonthlyRetentionDay") && (!MyInvocation.BoundParameters.ContainsKey("MonthlyValidForValue") || MonthlyValidForUnit == null))
                throw new ParameterBindingException("MonthlyValidForValue and MonthlyValidForUnit must be specified with MonthlyRetentionDay");

            if (MyInvocation.BoundParameters.ContainsKey("YearlyRetentionMonthDay") && (!MyInvocation.BoundParameters.ContainsKey("YearlyValidForValue") || YearlyValidForUnit == null))
                throw new ParameterBindingException("YearlyValidForValue and YearlyValidForUnit must be specified with YearlyRetentionMonthDay and YearlyRetentionMonth");

            if ((MyInvocation.BoundParameters.ContainsKey("YearlyRetentionMonthDay") && YearlyRetentionMonth == null) || (!MyInvocation.BoundParameters.ContainsKey("YearlyRetentionMonthDay") && YearlyRetentionMonth != null))
                throw new ParameterBindingException("YearlyRetentionMonthDay and YearlyRetentionMonth must both be specified together");

            base.DSClientProcessRecord();
        }
    }
}