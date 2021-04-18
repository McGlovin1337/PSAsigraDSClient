using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientTimeRetentionOption", SupportsShouldProcess = true)]

    public class SetDSClientTimeRetentionOption: BaseDSClientTimeRetentionOption
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify RetentionRuleId to Apply Time Retention to")]
        public int RetentionRuleId { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Time Retention Option Id to Remove")]
        public int TimeRetentionId { get; set; }

        protected override void ProcessRetentionRule(RetentionRule[] retentionRules)
        {
            // Select the required Retention Rule
            RetentionRule retentionRule = retentionRules.Single(rule => rule.getID() == RetentionRuleId);

            // Get Time Retention Option Hash Dictionary from Session State
            Dictionary<string, int> retentionHash = SessionState.PSVariable.GetValue("TimeRetention", null) as Dictionary<string, int>;

            if (retentionHash == null)
                WriteWarning("There are no Time Retention Options Stored in Session State, use Get-DSClientTimeRetentionOption to ensure removal of the desired Time Retention Option");

            TimeRetentionOption timeRetentionOption = SelectTimeRetentionOption(retentionRule, TimeRetentionId, retentionHash);
            ETimeRetentionType timeRetentionType = timeRetentionOption.getType();

            if (timeRetentionOption != null)
            {
                if (ShouldProcess($"{retentionRule.getName()}", "Set Time Retention Options"))
                {
                    RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();

                    int timeRetentionOptionCount = retentionRule.getTimeRetentions().Count();
                    
                    if (timeRetentionType == ETimeRetentionType.ETimeRetentionType__Interval)
                    {
                        // Interval based Time Retention
                        WriteVerbose("Performing Action: Set Interval Time Based Retention Rule");
                        IntervalTimeRetentionOption intervalTimeRetention = IntervalTimeRetentionOption.from(timeRetentionOption);

                        if (MyInvocation.BoundParameters.ContainsKey("IntervalTimeValue"))
                        {
                            retention_time_span timeSpan = intervalTimeRetention.getRepeatTime();
                            timeSpan.period = IntervalTimeValue;
                            intervalTimeRetention.setRepeatTime(timeSpan);
                        }

                        if (MyInvocation.BoundParameters.ContainsKey("IntervalTimeUnit"))
                        {
                            retention_time_span timeSpan = intervalTimeRetention.getRepeatTime();
                            timeSpan.unit = StringToEnum<RetentionTimeUnit>(IntervalTimeUnit);
                            intervalTimeRetention.setRepeatTime(timeSpan);
                        }
                    }                    
                    else if (timeRetentionType == ETimeRetentionType.ETimeRetentionType__Weekly)
                    {
                        // Weekly based Time Retention
                        WriteVerbose("Performing Action: Set Weekly Time Based Retention Rule");
                        WeeklyTimeRetentionOption weeklyTimeRetention = WeeklyTimeRetentionOption.from(timeRetentionOption);

                        if (MyInvocation.BoundParameters.ContainsKey("WeeklyRetentionDay"))
                            weeklyTimeRetention.setTriggerDay(StringToEnum<EWeekDay>(WeeklyRetentionDay));

                        if (MyInvocation.BoundParameters.ContainsKey("RetentionTime"))
                            weeklyTimeRetention.setSnapshotTime(StringTotime_in_day(RetentionTime));
                    }                    
                    else if (timeRetentionType == ETimeRetentionType.ETimeRetentionType__Monthly)
                    {
                        // Monthly based Time Retention
                        WriteVerbose("Performing Action: Set Monthly Time Based Retention Rule");
                        MonthlyTimeRetentionOption monthlyTimeRetention = MonthlyTimeRetentionOption.from(timeRetentionOption);

                        if (MyInvocation.BoundParameters.ContainsKey("MonthlyRetentionDay"))
                            monthlyTimeRetention.setDayOfMonth(RetentionDayOfMonth);

                        if (MyInvocation.BoundParameters.ContainsKey("RetentionTime"))
                            monthlyTimeRetention.setSnapshotTime(StringTotime_in_day(RetentionTime));
                    }                    
                    else if (timeRetentionType == ETimeRetentionType.ETimeRetentionType__Yearly)
                    {
                        // Yearly based Time Retention
                        WriteVerbose("Performing Action: Set Yearly Time Based Retention Rule");
                        YearlyTimeRetentionOption yearlyTimeRetention = YearlyTimeRetentionOption.from(timeRetentionOption);

                        if (MyInvocation.BoundParameters.ContainsKey("YearlyRetentionMonth"))
                            yearlyTimeRetention.setTriggerMonth(StringToEnum<EMonth>(YearlyRetentionMonth));

                        if (MyInvocation.BoundParameters.ContainsKey("YearlyRetentionMonthDay"))
                            yearlyTimeRetention.setDayOfMonth(RetentionDayOfMonth);

                        if (MyInvocation.BoundParameters.ContainsKey("RetentionTime"))
                            yearlyTimeRetention.setSnapshotTime(StringTotime_in_day(RetentionTime));
                    }

                    if (MyInvocation.BoundParameters.ContainsKey("ValidForValue"))
                    {
                        retention_time_span timeSpan = timeRetentionOption.getValidFor();
                        timeSpan.period = ValidForValue;
                        timeRetentionOption.setValidFor(timeSpan);
                    }

                    if (MyInvocation.BoundParameters.ContainsKey("ValidForUnit"))
                    {
                        retention_time_span timeSpan = timeRetentionOption.getValidFor();
                        timeSpan.unit = StringToEnum<RetentionTimeUnit>(ValidForUnit);
                        timeRetentionOption.setValidFor(timeSpan);
                    }

                    timeRetentionOption.Dispose();
                    DSClientRetentionRuleMgr.Dispose();
                }
            }

            retentionRule.Dispose();
        }
    }
}