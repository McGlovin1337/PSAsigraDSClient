using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public class BaseDSClientTimeRetentionOption: BaseDSClientRetentionRule
    {
        [Parameter(ParameterSetName = "interval", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Interval Retention Time Value")]
        public int IntervalTimeValue { get; set; }

        [Parameter(ParameterSetName = "interval", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Interval Retention Time Unit")]
        [ValidateSet("Minutes", "Hours", "Days", "Weeks", "Months", "Years")]
        public string IntervalTimeUnit { get; set; }

        [Parameter(ParameterSetName = "weekly", ValueFromPipelineByPropertyName  = true, HelpMessage = "Specify Weekday for Weekly Time Retention")]
        [ValidateSet("Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday")]
        public string WeeklyRetentionDay { get; set; }

        [Parameter(ParameterSetName = "monthly", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Day of Month to Keep One Generation on or before")]
        [Parameter(ParameterSetName = "yearly")]
        public int RetentionDayOfMonth { get; set; } = 1;

        [Parameter(ParameterSetName = "weekly", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Time of Day to Keep One Generation on or before")]
        [Parameter(ParameterSetName = "monthly")]
        [Parameter(ParameterSetName = "yearly")]
        public string RetentionTime { get; set; } = "23:59:59";

        [Parameter(Mandatory = true, ParameterSetName = "yearly", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Month for Yearly Time Retention")]
        [ValidateSet("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December")]
        public string YearlyRetentionMonth { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Time Value this Time Retention Option is Valid For")]
        public int ValidForValue { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Time Unit this Time Retention Option is Valid For")]
        [ValidateSet("Hours", "Days", "Weeks", "Months", "Years")]
        public string ValidForUnit { get; set; }

        protected override void DSClientProcessRecord()
        {
            base.DSClientProcessRecord();
        }

        protected class TimeRetentionOptionComparer: EqualityComparer<TimeRetentionOption>
        {
            public override bool Equals(TimeRetentionOption option1, TimeRetentionOption option2)
            {
                // Compare Type
                ETimeRetentionType type1 = option1.getType();
                ETimeRetentionType type2 = option2.getType();
                if (type1 != type2)
                    return false;

                // Compare Validity Period
                retention_time_span timeSpan1 = option1.getValidFor();
                retention_time_span timeSpan2 = option2.getValidFor();
                if (timeSpan1.period != timeSpan2.period && timeSpan1.unit != timeSpan2.unit)
                    return false;

                if (type1 == ETimeRetentionType.ETimeRetentionType__Interval && type2 == ETimeRetentionType.ETimeRetentionType__Interval)
                {
                    IntervalTimeRetentionOption interval1 = IntervalTimeRetentionOption.from(option1);
                    IntervalTimeRetentionOption interval2 = IntervalTimeRetentionOption.from(option2);

                    // Compare Repeat Period
                    retention_time_span repeat1 = interval1.getRepeatTime();
                    retention_time_span repeat2 = interval2.getRepeatTime();
                    if (repeat1.period != repeat2.period && repeat1.unit != repeat2.unit)
                        return false;
                }
                else if (type1 == ETimeRetentionType.ETimeRetentionType__Weekly && type2 == ETimeRetentionType.ETimeRetentionType__Weekly)
                {
                    WeeklyTimeRetentionOption weekly1 = WeeklyTimeRetentionOption.from(option1);
                    WeeklyTimeRetentionOption weekly2 = WeeklyTimeRetentionOption.from(option2);

                    // Compare Trigger Day
                    if (weekly1.getTriggerDay() != weekly2.getTriggerDay())
                        return false;

                    // Compare Snapshot Time
                    time_in_day time1 = weekly1.getSnapshotTime();
                    time_in_day time2 = weekly2.getSnapshotTime();
                    if (
                        time1.hour != time2.hour ||
                        time1.minute != time2.minute ||
                        time1.second != time2.second
                        )
                        return false;
                }
                else if (type1 == ETimeRetentionType.ETimeRetentionType__Monthly && type2 == ETimeRetentionType.ETimeRetentionType__Monthly)
                {
                    MonthlyTimeRetentionOption monthly1 = MonthlyTimeRetentionOption.from(option1);
                    MonthlyTimeRetentionOption monthly2 = MonthlyTimeRetentionOption.from(option2);

                    // Compare Day of Month
                    if (monthly1.getDayOfMonth() != monthly2.getDayOfMonth())
                        return false;

                    // Compare Snapshot Time
                    time_in_day time1 = monthly1.getSnapshotTime();
                    time_in_day time2 = monthly2.getSnapshotTime();
                    if (
                        time1.hour != time2.hour ||
                        time1.minute != time2.minute ||
                        time1.second != time2.second
                        )
                        return false;
                }
                else if (type1 == ETimeRetentionType.ETimeRetentionType__Yearly && type2 == ETimeRetentionType.ETimeRetentionType__Yearly)
                {
                    YearlyTimeRetentionOption yearly1 = YearlyTimeRetentionOption.from(option1);
                    YearlyTimeRetentionOption yearly2 = YearlyTimeRetentionOption.from(option2);

                    // Compare Day of Month
                    if (yearly1.getDayOfMonth() != yearly2.getDayOfMonth())
                        return false;

                    // Compare Trigger Month
                    if (yearly1.getTriggerMonth() != yearly2.getTriggerMonth())
                        return false;

                    // Compare Snapshot Time
                    time_in_day time1 = yearly1.getSnapshotTime();
                    time_in_day time2 = yearly2.getSnapshotTime();
                    if (
                        time1.hour != time2.hour ||
                        time1.minute != time2.minute ||
                        time1.second != time2.second
                        )
                        return false;
                }
                else
                {
                    return false;
                }

                return true;
            }

            public override int GetHashCode(TimeRetentionOption obj)
            {
                ETimeRetentionType type = obj.getType();
                int inttype = (int)type;
                if (inttype == 0)
                    inttype = 7;
                retention_time_span timeSpan = obj.getValidFor();
                int unit = (int)timeSpan.unit;
                if (unit == 0)
                    unit = 7;
                int extended = 7;

                if (type == ETimeRetentionType.ETimeRetentionType__Interval)
                {
                    IntervalTimeRetentionOption interval = IntervalTimeRetentionOption.from(obj);
                    retention_time_span inttimeSpan = interval.getRepeatTime();

                    extended *= inttimeSpan.period * ((int)inttimeSpan.unit + 7);
                }
                else if (type == ETimeRetentionType.ETimeRetentionType__Weekly)
                {
                    WeeklyTimeRetentionOption weekly = WeeklyTimeRetentionOption.from(obj);
                    time_in_day time = weekly.getSnapshotTime();

                    extended *= ((int)weekly.getTriggerDay() + 7) * (time.hour + 7) * (time.minute + 7) * (time.second + 7);
                }
                else if (type == ETimeRetentionType.ETimeRetentionType__Monthly)
                {
                    MonthlyTimeRetentionOption monthly = MonthlyTimeRetentionOption.from(obj);
                    time_in_day time = monthly.getSnapshotTime();

                    extended *= monthly.getDayOfMonth() * (time.hour + 7) * (time.minute + 7) * (time.second + 7);
                }
                else if (type == ETimeRetentionType.ETimeRetentionType__Yearly)
                {
                    YearlyTimeRetentionOption yearly = YearlyTimeRetentionOption.from(obj);
                    time_in_day time = yearly.getSnapshotTime();

                    extended *= yearly.getDayOfMonth() * ((int)yearly.getTriggerMonth() + 7) * (time.hour + 7) * (time.minute + 7) * (time.second + 7);
                }

                return (inttype * timeSpan.period * unit * extended).GetHashCode();
            }
        }
    }
}