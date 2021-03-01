using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientTimeRetentionOption")]
    [OutputType(typeof(TimeRetentionOverview))]

    public class GetDSClientTimeRetentionOption : BaseDSClientRetentionRule
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Retention Rule")]
        public int RetentionRuleId { get; set; }

        protected override void ProcessRetentionRule(RetentionRule[] retentionRules)
        {
            // Select the required Retention Rule
            RetentionRule retentionRule = retentionRules.Single(rule => rule.getID() == RetentionRuleId);

            // Get all the Time Retention Options
            WriteVerbose("Performing Action: Retrieve Time Retention Options");
            TimeRetentionOption[] timeRetentionOptions = retentionRule.getTimeRetentions();

            List<TimeRetentionOverview> timeRetentions = new List<TimeRetentionOverview>();

            int id = 1;
            foreach (TimeRetentionOption timeRetention in timeRetentionOptions)
            {
                TimeRetentionOverview timeRetentionOverview = new TimeRetentionOverview(id, timeRetention);
                timeRetentions.Add(timeRetentionOverview);
                id++;
                WriteDebug($"TimeRetentionOverview Hash: {timeRetentionOverview.GetHashCode()}");
            }

            // Store the retrieved rules into Session State, which will ensure correct rules are selected in case of removal
            WriteVerbose("Performing Action: Store Time Retention Options in PS Session State");
            Dictionary<int, int> retentionHash = new Dictionary<int, int>();
            int ruleNameHash = retentionRule.getName().GetHashCode();
            int ruleIdHash = retentionRule.getID().GetHashCode();

            foreach (TimeRetentionOverview timeRetention in timeRetentions)
            {
                int timeRetentionHash = timeRetention.GetHashCode() * ruleNameHash * ruleIdHash;
                WriteDebug($"TimeRetentionOption Final Hash: {timeRetentionHash}");
                retentionHash.Add(timeRetentionHash, timeRetention.TimeRetentionId);
            }
            SessionState.PSVariable.Remove("TimeRetention");
            SessionState.PSVariable.Set("TimeRetention", retentionHash);
            
            retentionRule.Dispose();

            timeRetentions.ForEach(WriteObject);
        }
    }
}