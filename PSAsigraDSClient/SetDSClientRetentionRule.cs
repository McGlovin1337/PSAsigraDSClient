using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;
using static PSAsigraDSClient.BaseDSClientTimeRetentionRule;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientRetentionRule")]

    public class SetDSClientRetentionRule: BaseDSClientRetentionRuleParams
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the RetentionRuleId to Modify")]
        public int RetentionRuleId { get; set; }

        [Parameter(HelpMessage = "Specify a New Name for the Retention Rule")]
        [ValidateNotNullOrEmpty]
        public string NewName { get; set; }

        protected override void ProcessRetentionRule(RetentionRule[] retentionRules)
        {
            // Perform Parameter Validation
            if (CleanupDeletedFiles)
                if ((MyInvocation.BoundParameters.ContainsKey("CleanupDeletedAfterValue") && CleanupDeletedAfterUnit == null) || (!MyInvocation.BoundParameters.ContainsKey("CleanupDeletedAfterValue") && CleanupDeletedAfterUnit != null))
                    throw new ParameterBindingException("CleanupDeletedAfterValue and CleanupDeletedAfterUnit must be specified when CleanupDeletedFiles specified");

            if (MyInvocation.BoundParameters.ContainsKey("DeleteGensPriorToStub") && MyInvocation.BoundParameters.ContainsKey("DeleteNonStubGens"))
                throw new ParameterBindingException("DeleteGensPriorToStub cannot be specified with DeleteNonStubGens");

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

            // Select the specific Retention Rule to modify
            RetentionRule retentionRule = retentionRules.Single(rule => rule.getID() == RetentionRuleId);

            // Apply changes here
            if (NewName != null)
                retentionRule.setName(NewName);

            // Set Cleanup of Deleted Files from Source
            if (CleanupDeletedFiles || retentionRule.getCleanupRemovedFiles())
            {
                WriteVerbose("Performing Action: Set Cleanup of Deleted Files");
                retentionRule.setCleanupRemovedFiles(CleanupDeletedFiles);

                retention_time_span timeSpan = new retention_time_span
                {
                    period = CleanupDeletedAfterValue,
                    unit = StringToEnum<RetentionTimeUnit>(CleanupDeletedAfterUnit)                
                };
                WriteVerbose("Performing Action: Set Time Span for Deleted File Cleanup");
                retentionRule.setCleanupRemovedAfter(timeSpan);

                retentionRule.setCleanupRemovedKeep(CleanupDeletedKeepGens);
            }

            // Set Stub Cleanup
            if (DeleteGensPriorToStub)
            {
                retentionRule.setDeleteStub(true);
                retentionRule.setDeleteStubAllGens(true);
            }
            else if (DeleteNonStubGens)
            {
                retentionRule.setDeleteStub(true);
                retentionRule.setDeleteStubAllGens(false);
            }

            // Set Local Storage Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("LSRetentionTimeValue"))
            {
                retention_time_span timeSpan = new retention_time_span
                {
                    period = LSRetentionTimeValue,
                    unit = StringToEnum<RetentionTimeUnit>(LSRetentionTimeUnit)
                };
                retentionRule.setLocalStorageRetention(timeSpan);
            }

            // Set Local Storage Cleanup of Deleted Files from Source
            if (LSCleanupDeletedFiles)
            {
                retentionRule.setLSCleanupRemovedFiles(true);
                retentionRule.setLSCleanupRemovedKeep(LSCleanupDeletedKeepGens);

                retention_time_span timeSpan = new retention_time_span
                {
                    period = LSCleanupDeletedAfterValue,
                    unit = StringToEnum<RetentionTimeUnit>(LSCleanupDeletedAfterUnit)
                };
                retentionRule.setLSCleanupRemovedAfter(timeSpan);
            }

            // VSS Options
            if (MyInvocation.BoundParameters.ContainsKey("DeleteUnreferencedFiles"))
                retentionRule.setDeleteUnreferencedFiles(DeleteUnreferencedFiles);

            if (MyInvocation.BoundParameters.ContainsKey("DeleteIncompleteComponents"))
                retentionRule.setDeleteIncompleteComponents(DeleteIncompleteComponents);

            WriteVerbose("Notice: Retention Rule Settings applied");
            // Done

            tFAManager.Dispose();
            retentionRule.Dispose();
        }
    }
}