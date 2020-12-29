using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientArchiveFilterRule")]
    [OutputType(typeof(DSClientArchiveFilterRule))]

    public class GetDSClientArchiveFilterRule: BaseDSClientArchiveFilterRule
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Archive Filter Rule Name")]
        [SupportsWildcards]
        public string Name { get; set; }

        protected override void ProcessArchiveFilterRules(ArchiveFilterRule[] archiveFilterRules)
        {
            List<DSClientArchiveFilterRule> DSClientArchiveFilterRules = new List<DSClientArchiveFilterRule>();

            if (Name != null)
            {
                WildcardOptions wcOptions = WildcardOptions.IgnoreCase |
                                            WildcardOptions.Compiled;

                WildcardPattern wcPattern = new WildcardPattern(Name, wcOptions);

                archiveFilterRules = archiveFilterRules.Where(rule => wcPattern.IsMatch(rule.getName())).ToArray();
            }

            foreach (ArchiveFilterRule rule in archiveFilterRules)
            {
                DSClientArchiveFilterRule dSClientArchiveFilterRule = new DSClientArchiveFilterRule(rule);
                DSClientArchiveFilterRules.Add(dSClientArchiveFilterRule);
            }

            WriteVerbose($"Notice: Yielded {DSClientArchiveFilterRules.Count()} Archive Filter Rules");

            DSClientArchiveFilterRules.ForEach(WriteObject);
        }
    }
}