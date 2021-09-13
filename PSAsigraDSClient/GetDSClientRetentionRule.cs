using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Data;
using AsigraDSClientApi;
using System;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientRetentionRule")]
    [OutputType(typeof(DSClientRetentionRule))]

    sealed public class GetDSClientRetentionRule: BaseDSClientRetentionRule
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Retention Rule Id")]
        public int RetentionRuleId { get; set; } = 0;

        [Parameter(Position = 1, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Name of the Retention Rule")]
        [ValidateNotNullOrEmpty, SupportsWildcards]
        public string Name { get; set; }

        protected override void ProcessRetentionRule(RetentionRule[] dSClientRetentionRules)
        {
            List<DSClientRetentionRule> DSClientRetentionRules = new List<DSClientRetentionRule>();

            if (RetentionRuleId > 0 || Name != null)
            {
                if (RetentionRuleId > 0)
                {
                    RetentionRule retentionRule = dSClientRetentionRules.Single(rule => rule.getID() == RetentionRuleId);
                    DSClientRetentionRule dSClientRetentionRule = new DSClientRetentionRule(retentionRule);
                    DSClientRetentionRules.Add(dSClientRetentionRule);
                }

                if (Name != null)
                {
                    WildcardOptions wcOptions = WildcardOptions.IgnoreCase |
                                                WildcardOptions.Compiled;

                    WildcardPattern wcPattern = new WildcardPattern(Name, wcOptions);

                    IEnumerable<RetentionRule> retentionRules = dSClientRetentionRules.Where(rule => wcPattern.IsMatch(rule.getName()));

                    foreach (RetentionRule rule in retentionRules)
                    {
                        DSClientRetentionRule dSClientRetentionRule = new DSClientRetentionRule(rule);
                        DSClientRetentionRules.Add(dSClientRetentionRule);
                    }
                }
            }
            else
            {
                int totalRules = dSClientRetentionRules.Count();
                int processed = 0;
                ProgressRecord progressRecord = new ProgressRecord(1, "Process Retention Rules", $"{processed} of {totalRules} Processed")
                {
                    RecordType = ProgressRecordType.Processing,
                    PercentComplete = 0
                };
                
                foreach (RetentionRule rule in dSClientRetentionRules)
                {
                    WriteVerbose($"Performing Action: Process Retention Rule with RetentionRuleId: {rule.getID()}");
                    progressRecord.CurrentOperation = $"Processing RetentionRuleId: {rule.getID()}";
                    progressRecord.PercentComplete = (int)Math.Round((double)((double)processed / (double)totalRules) * 100);
                    progressRecord.StatusDescription = $"{processed} of {totalRules} Processed, {progressRecord.PercentComplete}%";
                    WriteProgress(progressRecord);

                    DSClientRetentionRules.Add(new DSClientRetentionRule(rule));

                    processed++;
                }
                progressRecord.RecordType = ProgressRecordType.Completed;
            }

            WriteVerbose($"Notice: Yielded {DSClientRetentionRules.Count()} Retention Rules");

            DSClientRetentionRules.ForEach(WriteObject);
        }
    }
}