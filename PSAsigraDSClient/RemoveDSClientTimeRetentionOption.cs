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

            // Select the required Retention Rule
            RetentionRule retentionRule = retentionRules.Single(rule => rule.getID() == RetentionRuleId);

            // Determine the Hash Codes for the Rule Name and Id
            int ruleNameHash = retentionRule.getName().GetHashCode();
            int ruleIdHash = retentionRule.getID().GetHashCode();

            // Get Time Retention Option Hash Dictionary from Session State
            Dictionary<int, int> retentionHash = SessionState.PSVariable.GetValue("TimeRetention", null) as Dictionary<int, int>;

            if (retentionHash == null)
                WriteWarning("There are no Time Retention Options Stored in Session State, use Get-DSClientTimeRetentionOption to ensure removal of the desired Time Retention Option");

            // Get all the Time Retention Options in this Retention Rule
            TimeRetentionOption[] timeRetentions = retentionRule.getTimeRetentions();

            foreach (TimeRetentionOption timeRetention in timeRetentions)
            {
                TimeRetentionOverview timeRetentionOverview = new TimeRetentionOverview(timeRetention);

                int optionHash = timeRetentionOverview.GetHashCode();

                int completeHash = optionHash * ruleNameHash * ruleIdHash;

                retentionHash.TryGetValue(completeHash, out int optionId);

                if (optionId == TimeRetentionId)
                {
                    if (ShouldProcess($"Retention Rule: '{retentionRule.getName()}'", $"Remove Time Retention Option with Id '{optionId}'"))
                    {
                        retentionRule.removeTimeRetentionOption(timeRetention);
                        retentionHash.Remove(completeHash);
                    }
                }
            }

            retentionRule.Dispose();
        }
    }
}