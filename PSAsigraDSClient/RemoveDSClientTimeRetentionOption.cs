using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientTimeRetentionOption", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class RemoveDSClientTimeRetentionOption: BaseDSClientRetentionRule
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Retention Rule Id")]
        public int RetentionRuleId { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Time Retention Option Id to Remove")]
        public int TimeRetentionId { get; set; }

        protected override void ProcessRetentionRule(RetentionRule[] retentionRules)
        {
            // Select the required Retention Rule
            RetentionRule retentionRule = retentionRules.Single(rule => rule.getID() == RetentionRuleId);

            // Get Time Retention Option Hash Dictionary from Session State
            if (!(SessionState.PSVariable.GetValue("TimeRetention", null) is Dictionary<string, int> retentionHash))
            {
                ErrorRecord errorRecord = new ErrorRecord(
                    new Exception("There are no Time Retention Options Stored in Session State, use Get-DSClientTimeRetentionOption"),
                    "Exception",
                    ErrorCategory.ObjectNotFound,
                    null);
                WriteError(errorRecord);
            }
            else
            {
                // Select the Time Retention Option
                TimeRetentionOption timeRetentionOption = SelectTimeRetentionOption(retentionRule, TimeRetentionId, retentionHash);

                if (timeRetentionOption != null)
                    if (ShouldProcess($"Retention Rule: '{retentionRule.getName()}'", $"Remove Time Retention Option with Id '{TimeRetentionId}'"))
                        retentionRule.removeTimeRetentionOption(timeRetentionOption);

                timeRetentionOption.Dispose();
                retentionRule.Dispose();
            }
        }
    }
}