using System;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientTimeRetentionOption")]
    [OutputType(typeof(void))]

    sealed public class AddDSClientTimeRetentionOption: BaseDSClientTimeRetentionOption
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

            // Retrieve Current Time Retention Options for Comparison
            TimeRetentionOption[] timeRetentionOptions = RetentionRule.getTimeRetentions();
            TimeRetentionOptionComparer comparer = new TimeRetentionOptionComparer();

            // Interval based Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("IntervalTimeValue"))
            {
                WriteVerbose("Performing Action: Add Interval Time Based Retention Rule");
                IntervalTimeRetentionOption intervalTimeRetention = DSClientRetentionRuleMgr.createIntervalTimeRetention();
                intervalTimeRetention = IntervalTimeRetentionRule(intervalTimeRetention, IntervalTimeValue, IntervalTimeUnit, ValidForValue, ValidForUnit);

                ErrorRecord intervalErrorRecord = null;
                foreach (TimeRetentionOption option in timeRetentionOptions)
                {                    
                    if (comparer.Equals(option, intervalTimeRetention))
                    {
                        intervalErrorRecord = new ErrorRecord(
                            new Exception("An Identical Time Retention Option already exists"),
                            "Exception",
                            ErrorCategory.ResourceExists,
                            option);
                        WriteError(intervalErrorRecord);

                        option.Dispose();
                        intervalTimeRetention.Dispose();
                    }
                }

                if (intervalErrorRecord == null)
                    RetentionRule.addTimeRetentionOption(intervalTimeRetention);
            }

            // Weekly based Time Retention
            if (WeeklyRetentionDay != null)
            {
                WriteVerbose("Performing Action: Add Weekly Time Based Retention Rule");
                WeeklyTimeRetentionOption weeklyTimeRetention = DSClientRetentionRuleMgr.createWeeklyTimeRetention();
                weeklyTimeRetention = WeeklyTimeRetentionRule(weeklyTimeRetention, WeeklyRetentionDay, RetentionTime, ValidForValue, ValidForUnit);

                ErrorRecord weeklyErrorRecord = null;
                foreach (TimeRetentionOption option in timeRetentionOptions)
                {
                    if (comparer.Equals(option, weeklyTimeRetention))
                    {
                        weeklyErrorRecord = new ErrorRecord(
                            new Exception("An Identical Time Retention Option already exists"),
                            "Exception",
                            ErrorCategory.ResourceExists,
                            option);
                        WriteError(weeklyErrorRecord);

                        RetentionRule.Dispose();
                        DSClientRetentionRuleMgr.Dispose();
                    }
                }

                if (weeklyErrorRecord == null)
                    RetentionRule.addTimeRetentionOption(weeklyTimeRetention);
            }

            // Monthly based Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("RetentionDayOfMonth"))
            {
                WriteVerbose("Performing Action: Add Monthly Time Based Retention Rule");
                MonthlyTimeRetentionOption monthlyTimeRetention = DSClientRetentionRuleMgr.createMonthlyTimeRetention();
                monthlyTimeRetention = MonthlyTimeRetentionRule(monthlyTimeRetention, RetentionDayOfMonth, RetentionTime, ValidForValue, ValidForUnit);

                ErrorRecord monthlyErrorRecord = null;
                foreach (TimeRetentionOption option in timeRetentionOptions)
                {
                    if (comparer.Equals(option, monthlyTimeRetention))
                    {
                        monthlyErrorRecord = new ErrorRecord(
                            new Exception("An Identical Time Retention Option already exists"),
                            "Exception",
                            ErrorCategory.ResourceExists,
                            option);
                        WriteError(monthlyErrorRecord);

                        RetentionRule.Dispose();
                        DSClientRetentionRuleMgr.Dispose();
                    }
                }

                if (monthlyErrorRecord == null)
                    RetentionRule.addTimeRetentionOption(monthlyTimeRetention);
            }

            // Yearly based Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("YearlyRetentionMonth"))
            {
                WriteVerbose("Performing Action: Add Yearly Time Based Retention Rule");
                YearlyTimeRetentionOption yearlyTimeRetention = DSClientRetentionRuleMgr.createYearlyTimeRetention();
                yearlyTimeRetention = YearlyTimeRetentionRule(yearlyTimeRetention, RetentionDayOfMonth, YearlyRetentionMonth, RetentionTime, ValidForValue, ValidForUnit);

                ErrorRecord yearlyErrorRecord = null;
                foreach (TimeRetentionOption option in timeRetentionOptions)
                {
                    if (comparer.Equals(option, yearlyTimeRetention))
                    {
                        yearlyErrorRecord = new ErrorRecord(
                            new Exception("An Identical Time Retention Option already exists"),
                            "Exception",
                            ErrorCategory.ResourceExists,
                            option);
                        WriteError(yearlyErrorRecord);

                        RetentionRule.Dispose();
                        DSClientRetentionRuleMgr.Dispose();
                    }
                }

                if (yearlyErrorRecord == null)
                    RetentionRule.addTimeRetentionOption(yearlyTimeRetention);
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