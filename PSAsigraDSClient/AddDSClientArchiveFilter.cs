using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientArchiveFilter")]

    public class AddDSClientArchiveFilter: BaseDSClientArchiveFilter
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Archive Filter Rule to add this Filter to")]
        [ValidateNotNullOrEmpty]
        public string ArchiveFilterRule { get; set; }

        protected override void ProcessArchiveFilter(ArchiveFilter archiveFilter)
        {
            RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();

            WriteVerbose("Performing Action: Retrieve Archive filter Rule");
            ArchiveFilterRule filterRule = DSClientRetentionRuleMgr.definedArchiveFilterRules()
                                            .Single(rule => rule.getName() == ArchiveFilterRule);

            WriteVerbose("Performing Action: Add Archive Filter to Archive Filter Rule");
            filterRule.addFilter(archiveFilter);

            filterRule.Dispose();
            DSClientRetentionRuleMgr.Dispose();
        }
    }
}