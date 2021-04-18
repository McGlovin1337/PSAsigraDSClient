using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientTimeRetentionOption", SupportsShouldProcess = true)]

    public class RemoveDSClientTimeRetentionOption: BaseDSClientRetentionRule
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
            Dictionary<string, int> retentionHash = SessionState.PSVariable.GetValue("TimeRetention", null) as Dictionary<string, int>;

            if (retentionHash == null)
                throw new Exception("There are no Time Retention Options Stored in Session State, use Get-DSClientTimeRetentionOption");

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