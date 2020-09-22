using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientRetentionRule")]

    public class SetDSClientRetentionRule: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the RetentionRuleId to Modify")]
        public int RetentionRuleId { get; set; }

        protected override void DSClientProcessRecord()
        {
            /* API appears to error when creating or editing most Retention Rule settings unless a 2FA Verification code has been set
             * So we send a Dummy validation code, after which we can successfully add and change Retention Rule configuration */
            TFAManager tFAManager = DSClientSession.getTFAManager();
            try
            {
                tFAManager.validateCode("bleh", ERequestCodeEmailType.ERequestCodeEmailType__UNDEFINED);
            }
            catch
            {
                //Do nothing
            }

            RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();

            RetentionRule retentionRule = DSClientRetentionRuleMgr.definedRules().Single(rule => rule.getID() == RetentionRuleId);

            // Apply changes here

            tFAManager.Dispose();
            retentionRule.Dispose();
            DSClientRetentionRuleMgr.Dispose();
        }
    }
}