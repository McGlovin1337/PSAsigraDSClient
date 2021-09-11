using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientArchiveFilterRule", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class RemoveDSClientArchiveFilterRule : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Name of the Archive Filter to Remove")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        protected override void DSClientProcessRecord()
        {
            RetentionRuleManager retentionRuleManager = DSClientSession.getRetentionRuleManager();

            WriteVerbose("Performing Action: Retrieve Archive Filter Rule");
            ArchiveFilterRule archiveFilterRule = retentionRuleManager.definedArchiveFilterRules().Single(rule => rule.getName() == Name);

            if (ShouldProcess($"{Name}", "Remove Archive Filter Rule"))
                retentionRuleManager.removeArchiveFilterRule(archiveFilterRule);

            archiveFilterRule.Dispose();
            retentionRuleManager.Dispose();
        }
    }
}