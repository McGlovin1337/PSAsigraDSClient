using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Data;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientRetentionRule")]
    [OutputType(typeof(DSClientRetentionRule))]

    public class GetDSClientRetentionRule: BaseDSClientRetentionRule
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Retention Rule Id")]
        public int RetentionId { get; set; } = 0;

        [Parameter(Position = 1, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Name of the Retention Rule")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        protected override void ProcessRetentionRule(IEnumerable<DSClientRetentionRule> dSClientRetentionRules)
        {
            if (RetentionId > 0 || MyInvocation.BoundParameters.ContainsKey("Name"))
            {
                List<DSClientRetentionRule> filteredRetentionRules = new List<DSClientRetentionRule>();

                if (RetentionId > 0)
                {
                    filteredRetentionRules.Add(dSClientRetentionRules.SingleOrDefault(rule => rule.RetentionId == RetentionId));
                }

                if (MyInvocation.BoundParameters.ContainsKey("Name"))
                {
                    WildcardOptions wcOptions = WildcardOptions.IgnoreCase |
                                                WildcardOptions.Compiled;

                    WildcardPattern wcPattern = new WildcardPattern(Name, wcOptions);

                    filteredRetentionRules = dSClientRetentionRules.Where(rule => wcPattern.IsMatch(rule.Name)).ToList();
                }

                filteredRetentionRules.ForEach(WriteObject);
            }
            else
            {
                dSClientRetentionRules.ToList().ForEach(WriteObject);
            }
        }
    }
}