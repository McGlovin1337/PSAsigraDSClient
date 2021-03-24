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

            // Get Hash Codes for Previously Identified Time Retention Options from SessionState, or start a new Dictionary if none exists
            WriteVerbose("Performing Action: Compute Hashes to identify Time Retention Options");
            Dictionary<string, int> retentionHashes = SessionState.PSVariable.GetValue("TimeRetention", null) as Dictionary<string, int>;
            if (retentionHashes == null)
                retentionHashes = new Dictionary<string, int>();

            int startingId = 1;
            Dictionary<string, TimeRetentionOption> unidentified = new Dictionary<string, TimeRetentionOption>();

            foreach (TimeRetentionOption timeRetention in timeRetentionOptions)
            {
                string optionHash = TimeRetentionHash(retentionRule, timeRetention);
                WriteDebug($"Computed Hash: {optionHash}");

                retentionHashes.TryGetValue(optionHash, out int id);
                if (id > 0)
                {
                    if (id >= startingId)
                        startingId = id + 1;
                    timeRetentions.Add(new TimeRetentionOverview(id, timeRetention));
                    timeRetention.Dispose();
                }
                else
                    unidentified.Add(optionHash, timeRetention);
            }

            foreach (KeyValuePair<string, TimeRetentionOption> hash in unidentified)
            {                
                timeRetentions.Add(new TimeRetentionOverview(startingId, hash.Value));
                hash.Value.Dispose();
                retentionHashes.Add(hash.Key, startingId);
                startingId++;
            }

            SessionState.PSVariable.Set("TimeRetention", retentionHashes);
            
            retentionRule.Dispose();

            timeRetentions.ForEach(WriteObject);
        }
    }
}