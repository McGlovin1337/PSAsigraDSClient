using System;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientArchiveFilter", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class RemoveDSClientArchiveFilter : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Archive Filter Rule the Filter belongs to")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Pattern of the Archive Filter")]
        [ValidateNotNullOrEmpty]
        public string Pattern { get; set; }

        protected override void DSClientProcessRecord()
        {
            RetentionRuleManager retentionRuleManager = DSClientSession.getRetentionRuleManager();
            ArchiveFilterRule archiveFilterRule = null;
            ArchiveFilter archiveFilter = null;

            try
            {
                WriteVerbose("Performing Action: Retrieve Archive Filter Rule");
                archiveFilterRule = retentionRuleManager.definedArchiveFilterRules().Single(rule => rule.getName() == Name);
            }
            catch
            {
                ErrorRecord errorRecord = new ErrorRecord(
                    new Exception("No matching Archive Filter Rule"),
                    "Exception",
                    ErrorCategory.ObjectNotFound,
                    retentionRuleManager);
                WriteError(errorRecord);
            }

            if (archiveFilterRule != null)
            {
                try
                {
                    WriteVerbose("Performing Action: Retrieve Archive Filter");
                    archiveFilter = archiveFilterRule.getFilterList().Single(filter => filter.getPattern() == Pattern);
                }
                catch
                {
                    ErrorRecord errorRecord = new ErrorRecord(
                        new Exception("No matching Archive Filter Rule"),
                        "Exception",
                        ErrorCategory.ObjectNotFound,
                        retentionRuleManager);
                    WriteError(errorRecord);
                }

                if (archiveFilter != null)
                {
                    if (ShouldProcess($"Archive Filter Rule '{Name}'", $"Remove Archive Filter with Pattern '{Pattern}'"))
                    {
                        archiveFilterRule.removeFilter(archiveFilter);
                    }

                    archiveFilter.Dispose();
                    archiveFilterRule.Dispose();
                }
            }

            retentionRuleManager.Dispose();
        }
    }
}