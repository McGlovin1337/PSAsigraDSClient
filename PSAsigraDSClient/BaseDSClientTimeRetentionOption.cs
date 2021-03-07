using System.Management.Automation;

namespace PSAsigraDSClient
{
    public class BaseDSClientTimeRetentionOption: BaseDSClientRetentionRule
    {
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

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Day of Month for Monthly Time Retention")]
        [ValidateRange(1, 28)]
        public int MonthlyRetentionDay { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Monthly Retention Time Hour")]
        [ValidateRange(0, 23)]
        public int MonthlyRetentionHour { get; set; } = 23;

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Monthly Retention Time Minute")]
        [ValidateRange(0, 59)]
        public int MonthlyRetentionMinute { get; set; } = 59;

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Time Value Monthly Time Retention is Valid for")]
        public int MonthlyValidForValue { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Time Unit Monthly Time Retention is Valid for")]
        [ValidateSet("Hours", "Days", "Weeks", "Months", "Years")]
        public string MonthlyValidForUnit { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Day of Month for Yearly Time Retention")]
        [ValidateRange(1, 28)]
        public int YearlyRetentionMonthDay { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Month for Yearly Time Retention")]
        [ValidateSet("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December")]
        public string YearlyRetentionMonth { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Yearly Retention Time Hour")]
        [ValidateRange(0, 23)]
        public int YearlyRetentionHour { get; set; } = 23;

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Yearly Retention Time Minute")]
        [ValidateRange(0, 59)]
        public int YearlyRetentionMinute { get; set; } = 59;

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Time Value Yearly Time Retention is Valid for")]
        public int YearlyValidForValue { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Time Unit Yearly Time Retention is Valid for")]
        [ValidateSet("Hours", "Days", "Weeks", "Months", "Years")]
        public string YearlyValidForUnit { get; set; }

        protected override void DSClientProcessRecord()
        {


            base.DSClientProcessRecord();
        }
    }
}