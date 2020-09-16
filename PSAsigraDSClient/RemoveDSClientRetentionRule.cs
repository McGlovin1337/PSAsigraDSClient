using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientRetentionRule")]

    public class RemoveDSClientRetentionRule: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Retention Rule Id")]
        public int RetentionRuleId { get; set; }

        protected override void DSClientProcessRecord()
        {
            RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();

            WriteVerbose("Retrieving Retention Rules from DS-Client...");
            RetentionRule[] retentionRules = DSClientRetentionRuleMgr.definedRules();

            RetentionRule retentionRule = retentionRules.Single(rule => rule.getID() == RetentionRuleId);

            DSClientRetentionRuleMgr.removeRule(retentionRule);
            WriteObject("Retention Rule removed");

            DSClientRetentionRuleMgr.Dispose();
        }
    }
}