using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientArchiveFilterRule")]

    public class NewDSClientArchiveFilterRule: BaseDSClientArchiveFilter
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Name for the Archive Filter Rule")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        protected override void ProcessArchiveFilter(ArchiveFilter archiveFilter)
        {
            RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();

            ArchiveFilterRule NewArchiveFilterRule = DSClientRetentionRuleMgr.createArchiveFilterRule();

            NewArchiveFilterRule.setName(Name); 

            // Add the Filter to the Filter Rule
            NewArchiveFilterRule.addFilter(archiveFilter);

            // Add the Filter Rule to the DS-Client Database
            WriteVerbose("Adding Archive Filter Rule to DS-Client Database...");
            DSClientRetentionRuleMgr.addArchiveFilterRule(NewArchiveFilterRule);

            NewArchiveFilterRule.Dispose();
            DSClientRetentionRuleMgr.Dispose();
        }
    }
}