using System.Management.Automation;

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
    }
}