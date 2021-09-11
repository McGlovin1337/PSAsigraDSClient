using System;
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
            int optionCount = timeRetentionOptions.Count();

            List<TimeRetentionOverview> timeRetentions = new List<TimeRetentionOverview>();

            // Get Hash Codes for Previously Identified Time Retention Options from SessionState, or start a new Dictionary if none exists
            Dictionary<string, int> retentionHashes = DSClientSessionInfo.GetScheduleOrRetentionDictionary(false);
            if (retentionHashes == null)
                retentionHashes = new Dictionary<string, int>();

            int startingId = 1;
            Dictionary<string, TimeRetentionOption> unidentified = new Dictionary<string, TimeRetentionOption>();

            int optionCounter = 0;
            int hashCounter = 0;

            WriteVerbose("Performing Action: Process Time Retention Options");
            ProgressRecord progressRecord = new ProgressRecord(1, "Process Time Retention Options", $"0 of {optionCount} processed, 0%")
            {
                RecordType = ProgressRecordType.Processing,
                CurrentOperation = "Processing Time Retention Option"
            };
            foreach (TimeRetentionOption timeRetention in timeRetentionOptions)
            {
                progressRecord.PercentComplete = (int)Math.Round((double)(((double)optionCounter + (double)hashCounter) / ((double)optionCount * 2)) * 100);
                progressRecord.StatusDescription = $"{optionCounter} of {optionCount} processed, {progressRecord.PercentComplete}%";
                WriteProgress(progressRecord);

                string optionHash = TimeRetentionHash(retentionRule, timeRetention);
                WriteDebug($"Computed Hash: {optionHash}");
                hashCounter++;
                progressRecord.PercentComplete = (int)Math.Round((double)(((double)optionCounter + (double)hashCounter) / ((double)optionCount * 2)) * 100);
                progressRecord.StatusDescription = $"{optionCounter} of {optionCount} processed, {progressRecord.PercentComplete}%";
                WriteProgress(progressRecord);

                // Check if the Hash is already in the Dictionary and if so fetch the associated Id
                retentionHashes.TryGetValue(optionHash, out int id);
                if (id > 0)
                {
                    WriteDebug("Hash found in SessionState");
                    if (id >= startingId)
                        startingId = id + 1;
                    timeRetentions.Add(new TimeRetentionOverview(id, timeRetention));
                    timeRetention.Dispose();
                    optionCounter++;
                }
                else
                {
                    WriteDebug("Hash not found in SessionState");
                    unidentified.Add(optionHash, timeRetention);
                }
            }

            // Process all the Time Retention Options that were not already in the Dictionary, and add them
            foreach (KeyValuePair<string, TimeRetentionOption> hash in unidentified)
            {
                progressRecord.PercentComplete = (int)Math.Round((double)(((double)optionCounter + (double)hashCounter) / ((double)optionCount * 2)) * 100);
                progressRecord.StatusDescription = $"{optionCounter} of {optionCount} processed, {progressRecord.PercentComplete}%";
                WriteProgress(progressRecord);

                timeRetentions.Add(new TimeRetentionOverview(startingId, hash.Value));
                hash.Value.Dispose();
                retentionHashes.Add(hash.Key, startingId);
                startingId++;
                optionCounter++;
            }

            progressRecord.RecordType = ProgressRecordType.Completed;
            progressRecord.PercentComplete = (int)Math.Round((double)(((double)optionCounter + (double)hashCounter) / ((double)optionCount * 2)) * 100);
            WriteProgress(progressRecord);

            DSClientSessionInfo.SetScheduleOrRetentionDictonary(retentionHashes, false);
            
            retentionRule.Dispose();

            timeRetentions.ForEach(WriteObject);
        }
    }
}