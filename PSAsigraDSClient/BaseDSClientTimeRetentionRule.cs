using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientTimeRetentionRule: BaseDSClientRetentionRuleParams
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

        protected abstract void ProcessRetentionRule();

        protected override void DSClientProcessRecord()
        {
            // Parameter Validation
            if ((MyInvocation.BoundParameters.ContainsKey("KeepAllGensTimeValue") && KeepAllGensTimeUnit == null) || (!MyInvocation.BoundParameters.ContainsKey("KeepAllGensTimeValue") && KeepAllGensTimeUnit != null))
                throw new ParameterBindingException("KeepAllGensTimeValue and KeepAllGensTimeUnit must be specified together");

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

            if ((MyInvocation.BoundParameters.ContainsKey("YearlyRetentionMonthDay") && YearlyRetentionMonth == null) || (!MyInvocation.BoundParameters.ContainsKey("YearlyRetentionMonthDay") && YearlyRetentionMonth != null))
                throw new ParameterBindingException("YearlyRetentionMonthDay and YearlyRetentionMonth must both be specified together");

            if (MyInvocation.BoundParameters.ContainsKey("YearlyRetentionMonthDay") && (!MyInvocation.BoundParameters.ContainsKey("YearlyValidForValue") || YearlyValidForUnit == null))
                throw new ParameterBindingException("YearlyValidForValue and YearlyValidForUnit must be specified with YearlyRetentionMonthDay and YearlyRetentionMonth");

            if (MyInvocation.BoundParameters.ContainsKey("MoveObsoleteData") && MyInvocation.BoundParameters.ContainsKey("DeleteObsoleteData"))
                throw new ParameterBindingException("MoveObsoleteData cannot be specified with DeleteObsoleteData");

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

            ProcessRetentionRule();
        }


    }
}