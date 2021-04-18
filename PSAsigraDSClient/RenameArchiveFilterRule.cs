using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Rename, "DSClientArchiveFilterRule", SupportsShouldProcess = true)]

    public class RenameArchiveFilterRule : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Name of the Archive Filter Rule to be Renamed")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "Specify a new Name for the Archive Filter Rule")]
        [ValidateNotNullOrEmpty]
        public string NewName { get; set; }

        protected override void DSClientProcessRecord()
        {
            RetentionRuleManager retentionRuleManager = DSClientSession.getRetentionRuleManager();

            WriteVerbose("Performing Action: Retrieve Archive Filter Rule");
            ArchiveFilterRule archiveFilterRule = retentionRuleManager.definedArchiveFilterRules().Single(rule => rule.getName() == Name);

            if (ShouldProcess($"{Name}", $"Rename Archive Filter Rule to '{NewName}'"))
                archiveFilterRule.setName(NewName);

            archiveFilterRule.Dispose();
            retentionRuleManager.Dispose();
        }
    }
}