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

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Time Value this Time Retention Option is Valid For")]
        public new int ValidForValue { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Time Unit this Time Retention Option is Valid For")]
        [ValidateSet("Hours", "Days", "Weeks", "Months", "Years")]
        public new string ValidForUnit { get; set; }

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

                RetentionRule.addTimeRetentionOption(IntervalTimeRetentionRule(intervalTimeRetention, IntervalTimeValue, IntervalTimeUnit, ValidForValue, ValidForUnit));
            }

            // Weekly based Time Retention
            if (WeeklyRetentionDay != null)
            {
                WriteVerbose("Performing Action: Add Weekly Time Based Retention Rule");
                WeeklyTimeRetentionOption weeklyTimeRetention = DSClientRetentionRuleMgr.createWeeklyTimeRetention();

                RetentionRule.addTimeRetentionOption(WeeklyTimeRetentionRule(weeklyTimeRetention, WeeklyRetentionDay, RetentionTime, ValidForValue, ValidForUnit));
            }

            // Monthly based Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("RetentionDayOfMonth"))
            {
                WriteVerbose("Performing Action: Add Monthly Time Based Retention Rule");
                MonthlyTimeRetentionOption monthlyTimeRetention = DSClientRetentionRuleMgr.createMonthlyTimeRetention();

                RetentionRule.addTimeRetentionOption(MonthlyTimeRetentionRule(monthlyTimeRetention, RetentionDayOfMonth, RetentionTime, ValidForValue, ValidForUnit));
            }

            // Yearly based Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("YearlyRetentionMonth"))
            {
                WriteVerbose("Performing Action: Add Yearly Time Based Retention Rule");
                YearlyTimeRetentionOption yearlyTimeRetention = DSClientRetentionRuleMgr.createYearlyTimeRetention();

                RetentionRule.addTimeRetentionOption(YearlyTimeRetentionRule(yearlyTimeRetention, RetentionDayOfMonth, YearlyRetentionMonth, RetentionTime, ValidForValue, ValidForUnit));
            }

            RetentionRule.Dispose();
            DSClientRetentionRuleMgr.Dispose();
        }

        protected override void DSClientProcessRecord()
        {
            if ((MyInvocation.BoundParameters.ContainsKey("IntervalTimeValue") && IntervalTimeUnit == null) || (!MyInvocation.BoundParameters.ContainsKey("IntervalTimeValue") && IntervalTimeUnit != null))
                throw new ParameterBindingException("IntervalTimeValue and IntervalTimeUnit must be specified together");

            base.DSClientProcessRecord();
        }
    }
}