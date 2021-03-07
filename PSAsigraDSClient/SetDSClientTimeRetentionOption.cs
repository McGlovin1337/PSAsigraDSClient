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
            Dictionary<int, int> retentionHash = SessionState.PSVariable.GetValue("TimeRetention", null) as Dictionary<int, int>;

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

                        if (MyInvocation.BoundParameters.ContainsKey("IntervalValidForValue"))
                        {
                            retention_time_span timeSpan = intervalTimeRetention.getValidFor();
                            timeSpan.period = IntervalValidForValue;
                            intervalTimeRetention.setValidFor(timeSpan);
                        }

                        if (MyInvocation.BoundParameters.ContainsKey("IntervalValidForUnit"))
                        {
                            retention_time_span timeSpan = intervalTimeRetention.getValidFor();
                            timeSpan.unit = StringToEnum<RetentionTimeUnit>(IntervalValidForUnit);
                            intervalTimeRetention.setValidFor(timeSpan);
                        }
                    }                    
                    else if (timeRetentionType == ETimeRetentionType.ETimeRetentionType__Weekly)
                    {
                        // Weekly based Time Retention
                        WriteVerbose("Performing Action: Set Weekly Time Based Retention Rule");
                        WeeklyTimeRetentionOption weeklyTimeRetention = WeeklyTimeRetentionOption.from(timeRetentionOption);

                        if (MyInvocation.BoundParameters.ContainsKey("WeeklyRetentionDay"))
                            weeklyTimeRetention.setTriggerDay(StringToEnum<EWeekDay>(WeeklyRetentionDay));

                        if (MyInvocation.BoundParameters.ContainsKey("WeeklyRetentionHour") ||
                            MyInvocation.BoundParameters.ContainsKey("WeeklyRetentionMinute"))
                        {
                            time_in_day time = weeklyTimeRetention.getSnapshotTime();

                            if (MyInvocation.BoundParameters.ContainsKey("WeeklyRetentionHour"))
                                time.hour = WeeklyRetentionHour;

                            if (MyInvocation.BoundParameters.ContainsKey("WeeklyRetentionMinute"))
                                time.minute = WeeklyRetentionMinute;

                            weeklyTimeRetention.setSnapshotTime(time);
                        }

                        if (MyInvocation.BoundParameters.ContainsKey("WeeklyValidForValue"))
                        {
                            retention_time_span timeSpan = weeklyTimeRetention.getValidFor();
                            timeSpan.period = WeeklyValidForValue;
                            weeklyTimeRetention.setValidFor(timeSpan);
                        }

                        if (MyInvocation.BoundParameters.ContainsKey("WeeklyValidForUnit"))
                        {
                            retention_time_span timeSpan = weeklyTimeRetention.getValidFor();
                            timeSpan.unit = StringToEnum<RetentionTimeUnit>(WeeklyValidForUnit);
                            weeklyTimeRetention.setValidFor(timeSpan);
                        }
                    }                    
                    else if (timeRetentionType == ETimeRetentionType.ETimeRetentionType__Monthly)
                    {
                        // Monthly based Time Retention
                        WriteVerbose("Performing Action: Set Monthly Time Based Retention Rule");
                        MonthlyTimeRetentionOption monthlyTimeRetention = MonthlyTimeRetentionOption.from(timeRetentionOption);

                        if (MyInvocation.BoundParameters.ContainsKey("MonthlyRetentionDay"))
                            monthlyTimeRetention.setDayOfMonth(MonthlyRetentionDay);

                        if (MyInvocation.BoundParameters.ContainsKey("MonthlyRetentionHour") ||
                            MyInvocation.BoundParameters.ContainsKey("MonthlyRetentionMinute"))
                        {
                            time_in_day time = monthlyTimeRetention.getSnapshotTime();

                            if (MyInvocation.BoundParameters.ContainsKey("MonthlyRetentionHour"))
                                time.hour = MonthlyRetentionHour;

                            if (MyInvocation.BoundParameters.ContainsKey("MonthlyRetentionMinute"))
                                time.minute = MonthlyRetentionMinute;

                            monthlyTimeRetention.setSnapshotTime(time);
                        }

                        if (MyInvocation.BoundParameters.ContainsKey("MonthlyValidForValue"))
                        {
                            retention_time_span timeSpan = monthlyTimeRetention.getValidFor();
                            timeSpan.period = MonthlyValidForValue;
                            monthlyTimeRetention.setValidFor(timeSpan);
                        }

                        if (MyInvocation.BoundParameters.ContainsKey("MonthlyValidForUnit"))
                        {
                            retention_time_span timeSpan = monthlyTimeRetention.getValidFor();
                            timeSpan.unit = StringToEnum<RetentionTimeUnit>(MonthlyValidForUnit);
                            monthlyTimeRetention.setValidFor(timeSpan);
                        }
                    }                    
                    else if (timeRetentionType == ETimeRetentionType.ETimeRetentionType__Yearly)
                    {
                        // Yearly based Time Retention
                        WriteVerbose("Performing Action: Set Yearly Time Based Retention Rule");
                        YearlyTimeRetentionOption yearlyTimeRetention = YearlyTimeRetentionOption.from(timeRetentionOption);

                        if (MyInvocation.BoundParameters.ContainsKey("YearlyRetentionMonth"))
                            yearlyTimeRetention.setTriggerMonth(StringToEnum<EMonth>(YearlyRetentionMonth));

                        if (MyInvocation.BoundParameters.ContainsKey("YearlyRetentionMonthDay"))
                            yearlyTimeRetention.setDayOfMonth(YearlyRetentionMonthDay);

                        if (MyInvocation.BoundParameters.ContainsKey("YearlyRetentionHour"))
                        {
                            time_in_day time = yearlyTimeRetention.getSnapshotTime();
                            time.hour = YearlyRetentionHour;
                            yearlyTimeRetention.setSnapshotTime(time);
                        }

                        if (MyInvocation.BoundParameters.ContainsKey("YearlyRetentionMinute"))
                        {
                            time_in_day time = yearlyTimeRetention.getSnapshotTime();
                            time.minute = YearlyRetentionMinute;
                            yearlyTimeRetention.setSnapshotTime(time);
                        }

                        if (MyInvocation.BoundParameters.ContainsKey("YearlyValidForValue"))
                        {
                            retention_time_span timeSpan = yearlyTimeRetention.getValidFor();
                            timeSpan.period = YearlyValidForValue;
                            yearlyTimeRetention.setValidFor(timeSpan);
                        }

                        if (MyInvocation.BoundParameters.ContainsKey("YearlyValidForUnit"))
                        {
                            retention_time_span timeSpan = yearlyTimeRetention.getValidFor();
                            timeSpan.unit = StringToEnum<RetentionTimeUnit>(YearlyValidForUnit);
                            yearlyTimeRetention.setValidFor(timeSpan);
                        }
                    }

                    DSClientRetentionRuleMgr.Dispose();
                }
            }

            retentionRule.Dispose();
        }
    }
}