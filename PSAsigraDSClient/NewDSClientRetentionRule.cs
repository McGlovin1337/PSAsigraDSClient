using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientRetentionRule")]
    [OutputType(typeof(DSClientNewRetentionRule))]

    sealed public class NewDSClientRetentionRule: BaseDSClientRetentionRuleParams
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Retention Rule Name")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Archive Rule Time Value")]
        public int ArchiveTimeValue { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Archive Rule Time Unit")]
        [ValidateSet("Minutes", "Hours", "Days", "Weeks", "Months", "Years")]
        public string ArchiveTimeUnit { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify an existing Archive Filter Rule")]
        public string ArchiveFilterRule { get; set; }

        [Parameter(HelpMessage = "Specify to Output Retention Rule Overview")]
        public SwitchParameter PassThru { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Perform Parameter Validation
            if (CleanupDeletedFiles)
                if ((MyInvocation.BoundParameters.ContainsKey("CleanupDeletedAfterValue") && CleanupDeletedAfterUnit == null) || (!MyInvocation.BoundParameters.ContainsKey("CleanupDeletedAfterValue") && CleanupDeletedAfterUnit != null))
                    throw new ParameterBindingException("CleanupDeletedAfterValue and CleanupDeletedAfterUnit must be specified when CleanupDeletedFiles specified");

            if (MyInvocation.BoundParameters.ContainsKey("DeleteGensPriorToStub") && MyInvocation.BoundParameters.ContainsKey("DeleteNonStubGens"))
                throw new ParameterBindingException("DeleteGensPriorToStub cannot be specified with DeleteNonStubGens");

            if ((MyInvocation.BoundParameters.ContainsKey("ArchiveTimeValue") && ArchiveTimeUnit == null) || (!MyInvocation.BoundParameters.ContainsKey("ArchiveTimeValue") && ArchiveTimeUnit != null))
                throw new ParameterBindingException("ArchiveTimeValue and ArchiveTimeUnit must both be specified together");

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
            tFAManager.Dispose();

            WriteVerbose("Performing Action: Build new Retention Rule object");
            RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();
            RetentionRule NewRetentionRule = DSClientRetentionRuleMgr.createRule();

            // Set Retention Rule Name
            NewRetentionRule.setName(Name);

            // Set Cleanup of Deleted Files from Source
            if (CleanupDeletedFiles)
            {
                WriteVerbose("Performing Action: Set Cleanup of Deleted Files");
                NewRetentionRule.setCleanupRemovedFiles(CleanupDeletedFiles);

                retention_time_span timeSpan = new retention_time_span
                {
                    period = CleanupDeletedAfterValue,
                    unit = StringToEnum<RetentionTimeUnit>(CleanupDeletedAfterUnit)
                };
                WriteVerbose("Performing Action: Set Time Span for Deleted File Cleanup");
                NewRetentionRule.setCleanupRemovedAfter(timeSpan);

                NewRetentionRule.setCleanupRemovedKeep(CleanupDeletedKeepGens);
            }

            // Set Stub Cleanup
            if (DeleteGensPriorToStub)
            {
                NewRetentionRule.setDeleteStub(true);
                NewRetentionRule.setDeleteStubAllGens(true);
            }
            else if (DeleteNonStubGens)
            {
                NewRetentionRule.setDeleteStub(true);
                NewRetentionRule.setDeleteStubAllGens(false);
            }

            // Set Time Based Retention
            if (MyInvocation.BoundParameters.ContainsKey("KeepLastGens"))
                NewRetentionRule.setKeepLastGenerations(KeepLastGens);

            if (MyInvocation.BoundParameters.ContainsKey("KeepAllGensTimeValue"))
                KeepAllGenerationsRule(NewRetentionRule, KeepAllGensTimeValue, KeepAllGensTimeUnit);

            // Move or Delete Obsolete Data
            if (MyInvocation.BoundParameters.ContainsKey("DeleteObsoleteData"))
                NewRetentionRule.setMoveObsoleteDataToBLM(false);

            if (MyInvocation.BoundParameters.ContainsKey("MoveObsoleteData"))
                NewRetentionRule.setMoveObsoleteDataToBLM(true);

            if (MyInvocation.BoundParameters.ContainsKey("CreateNewBLMPackage"))
                NewRetentionRule.setCreateNewBLMPackage(CreateNewBLMPackage);

            // Set Local Storage Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("LSRetentionTimeValue"))
            {
                retention_time_span timeSpan = new retention_time_span
                {
                    period = LSRetentionTimeValue,
                    unit = StringToEnum<RetentionTimeUnit>(LSRetentionTimeUnit)
                };
                NewRetentionRule.setLocalStorageRetention(timeSpan);
            }

            // Set Local Storage Cleanup of Deleted Files from Source
            if (LSCleanupDeletedFiles)
            {
                NewRetentionRule.setLSCleanupRemovedFiles(true);
                NewRetentionRule.setLSCleanupRemovedKeep(LSCleanupDeletedKeepGens);

                retention_time_span timeSpan = new retention_time_span
                {
                    period = LSCleanupDeletedAfterValue,
                    unit = StringToEnum<RetentionTimeUnit>(LSCleanupDeletedAfterUnit)
                };
                NewRetentionRule.setLSCleanupRemovedAfter(timeSpan);
            }

            // VSS Options
            if (MyInvocation.BoundParameters.ContainsKey("DeleteUnreferencedFiles"))
                NewRetentionRule.setDeleteUnreferencedFiles(DeleteUnreferencedFiles);

            if (MyInvocation.BoundParameters.ContainsKey("DeleteIncompleteComponents"))
                NewRetentionRule.setDeleteIncompleteComponents(DeleteIncompleteComponents);

            // Add the New Retention Rule to DS-Client
            WriteVerbose("Performing Action: Add Retention Rule to DS-Client");
            DSClientRetentionRuleMgr.addRule(NewRetentionRule);

            // Archive Rule Configuration (requires retention rule to be added to DS-Client Database first)
            if (MyInvocation.BoundParameters.ContainsKey("ArchiveTimeValue"))
            {
                WriteVerbose("Performing Action: Set Archive Rule");
                ArchiveRule NewArchiveRule = DSClientRetentionRuleMgr.createArchiveRule();

                retention_time_span timeSpan = new retention_time_span
                {
                    period = ArchiveTimeValue,
                    unit = StringToEnum<RetentionTimeUnit>(ArchiveTimeUnit)
                };
                NewArchiveRule.setTimeSpan(timeSpan);

                if (ArchiveFilterRule != null)
                {
                    ArchiveFilterRule filterRule = DSClientRetentionRuleMgr.definedArchiveFilterRules()
                                            .Single(rule => rule.getName() == ArchiveFilterRule);

                    NewArchiveRule.setFilterRule(filterRule);

                    filterRule.Dispose();
                }

                NewRetentionRule.addArchiveRule(NewArchiveRule);
            }

            if (PassThru)
                WriteObject(new DSClientNewRetentionRule(NewRetentionRule));

            NewRetentionRule.Dispose();
            DSClientRetentionRuleMgr.Dispose();
        }

        private class DSClientNewRetentionRule
        {
            public int RetentionRuleId { get; private set; }
            public string Name { get; private set; }

            public DSClientNewRetentionRule(RetentionRule retentionRule)
            {
                RetentionRuleId = retentionRule.getID();
                Name = retentionRule.getName();
            }
        }
    }
}