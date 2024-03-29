﻿using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientRetentionRule", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class SetDSClientRetentionRule: BaseDSClientRetentionRuleParams
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the RetentionRuleId to Modify")]
        public int RetentionRuleId { get; set; }

        [Parameter(HelpMessage = "Specify a New Name for the Retention Rule")]
        [ValidateNotNullOrEmpty]
        public string NewName { get; set; }

        [Parameter(HelpMessage = "Enable Keeping All Generations by Period")]
        public SwitchParameter KeepGensByPeriod { get; set; }

        protected override void ProcessRetentionRule(RetentionRule[] retentionRules)
        {
            // Perform Parameter Validation
            if (CleanupDeletedFiles)
                if ((MyInvocation.BoundParameters.ContainsKey("CleanupDeletedAfterValue") && CleanupDeletedAfterUnit == null) || (!MyInvocation.BoundParameters.ContainsKey("CleanupDeletedAfterValue") && CleanupDeletedAfterUnit != null))
                    throw new ParameterBindingException("CleanupDeletedAfterValue and CleanupDeletedAfterUnit must be specified when CleanupDeletedFiles specified");

            if (MyInvocation.BoundParameters.ContainsKey("DeleteGensPriorToStub") && MyInvocation.BoundParameters.ContainsKey("DeleteNonStubGens"))
                throw new ParameterBindingException("DeleteGensPriorToStub cannot be specified with DeleteNonStubGens");

            if ((MyInvocation.BoundParameters.ContainsKey("KeepAllGensTimeValue") && KeepAllGensTimeUnit == null) || (!MyInvocation.BoundParameters.ContainsKey("KeepAllGensTimeValue") && KeepAllGensTimeUnit != null))
                throw new ParameterBindingException("KeepAllGensTimeValue and KeepAllGensTimeUnit must be specified together");

            if ((MyInvocation.BoundParameters.ContainsKey("KeepGensByPeriod") && !KeepGensByPeriod) && MyInvocation.BoundParameters.ContainsKey("KeepAllGensTimeValue"))
                throw new ParameterBindingException("Specifying KeepAllGensTimeValue implies KeepGensByPeriod as True, but KeepGensByPeriod is False");

            if (MyInvocation.BoundParameters.ContainsKey("MoveObsoleteData") && MyInvocation.BoundParameters.ContainsKey("DeleteObsoleteData"))
            throw new ParameterBindingException("MoveObsoleteData cannot be specified with DeleteObsoleteData");

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
            string retentionRuleName = retentionRule.getName();

            // Apply changes here
            if (NewName != null)
                if (ShouldProcess($"Retention Rule '{retentionRuleName}'", $"Set Retention Rule Name '{NewName}'"))
                    retentionRule.setName(NewName);

            // Set Cleanup of Deleted Files from Source
            if (CleanupDeletedFiles || retentionRule.getCleanupRemovedFiles())
            {
                if (ShouldProcess($"{retentionRuleName}", "Enable Cleanup of Deleted Files"))
                {
                    WriteVerbose("Performing Action: Set Cleanup of Deleted Files");
                    retentionRule.setCleanupRemovedFiles(CleanupDeletedFiles);
                }

                if (ShouldProcess($"{retentionRuleName}", "Set Time Span for Cleanup of Deleted Files"))
                {
                    retention_time_span timeSpan = new retention_time_span
                    {
                        period = CleanupDeletedAfterValue,
                        unit = StringToEnum<RetentionTimeUnit>(CleanupDeletedAfterUnit)
                    };
                    WriteVerbose("Performing Action: Set Time Span for Deleted File Cleanup");
                    retentionRule.setCleanupRemovedAfter(timeSpan);
                }

                if (ShouldProcess($"{retentionRuleName}", "Set Number of Generations of Deleted Files to Keep"))
                    retentionRule.setCleanupRemovedKeep(CleanupDeletedKeepGens);
            }

            // Set Stub Cleanup
            if (DeleteGensPriorToStub)
            {
                if (ShouldProcess($"{retentionRuleName}", "Set Delete Generations prior to Stub File"))
                {
                    retentionRule.setDeleteStub(true);
                    retentionRule.setDeleteStubAllGens(true);
                }
            }
            else if (DeleteNonStubGens)
            {
                if (ShouldProcess($"{retentionRuleName}", "Set Delete Non Stub File Generations"))
                {
                    retentionRule.setDeleteStub(true);
                    retentionRule.setDeleteStubAllGens(false);
                }
            }

            // Set Local Storage Time Retention
            if (MyInvocation.BoundParameters.ContainsKey("LSRetentionTimeValue"))
            {
                if (ShouldProcess($"{retentionRuleName}", $"Set Local Storage Retention Time Span to '{LSRetentionTimeValue} {LSRetentionTimeUnit}'"))
                {
                    retention_time_span timeSpan = new retention_time_span
                    {
                        period = LSRetentionTimeValue,
                        unit = StringToEnum<RetentionTimeUnit>(LSRetentionTimeUnit)
                    };
                    retentionRule.setLocalStorageRetention(timeSpan);
                }
            }

            // Set Local Storage Cleanup of Deleted Files from Source
            if (LSCleanupDeletedFiles)
            {
                if (ShouldProcess($"{retentionRuleName}", "Set Cleanup of Deleted Files from Local Storage"))
                    retentionRule.setLSCleanupRemovedFiles(true);

                if (ShouldProcess($"{retentionRuleName}", "Set Generations to Keep for Deleted Files on Local Storage"))
                    retentionRule.setLSCleanupRemovedKeep(LSCleanupDeletedKeepGens);

                if (ShouldProcess($"{retentionRuleName}", $"Set Time Span for Cleanup of Deleted Files on Local Storage to '{LSCleanupDeletedAfterValue} {LSCleanupDeletedAfterUnit}'"))
                {
                    retention_time_span timeSpan = new retention_time_span
                    {
                        period = LSCleanupDeletedAfterValue,
                        unit = StringToEnum<RetentionTimeUnit>(LSCleanupDeletedAfterUnit)
                    };
                    retentionRule.setLSCleanupRemovedAfter(timeSpan);
                }
            }

            // VSS Options
            if (MyInvocation.BoundParameters.ContainsKey("DeleteUnreferencedFiles"))
                if (ShouldProcess($"{retentionRuleName}", $"Set Delete Unreferenced Files to '{DeleteUnreferencedFiles}'"))
                    retentionRule.setDeleteUnreferencedFiles(DeleteUnreferencedFiles);

            if (MyInvocation.BoundParameters.ContainsKey("DeleteIncompleteComponents"))
                if (ShouldProcess($"{retentionRuleName}", $"Set Delete Incomplete Components to '{DeleteIncompleteComponents}'"))
                    retentionRule.setDeleteIncompleteComponents(DeleteIncompleteComponents);

            // Time Retention Config
            if (MyInvocation.BoundParameters.ContainsKey("KeepGensByPeriod"))
                if (ShouldProcess($"{retentionRuleName}", $"Set Keep All Generations within Period to '{KeepGensByPeriod}'"))
                    retentionRule.setKeepGenerationsByPeriod(KeepGensByPeriod);

            if (MyInvocation.BoundParameters.ContainsKey("KeepLastGens"))
                if (ShouldProcess($"{retentionRuleName}", $"Set Keep Number of Most Recent Generations to '{KeepLastGens}'"))
                    retentionRule.setKeepLastGenerations(KeepLastGens);

            if (MyInvocation.BoundParameters.ContainsKey("KeepAllGensTimeValue"))
                if (ShouldProcess($"{retentionRuleName}", $"Set Keeping All Generations for Period of '{KeepAllGensTimeValue} {KeepAllGensTimeUnit}'"))
                    KeepAllGenerationsRule(retentionRule, KeepAllGensTimeValue, KeepAllGensTimeUnit);

            // Move or Delete Obsolete Data
            if (DeleteObsoleteData)
                if (ShouldProcess($"{retentionRuleName}", $"Set Deletion of Obsolete Data to '{DeleteObsoleteData}'"))
                    retentionRule.setMoveObsoleteDataToBLM(false);

            if (MoveObsoleteData)
                if (ShouldProcess($"{retentionRuleName}", $"Set Move Obsolete Data to BLM to '{MoveObsoleteData}'"))
                    retentionRule.setMoveObsoleteDataToBLM(true);

            if (MyInvocation.BoundParameters.ContainsKey("CreateNewBLMPackage"))
                if (ShouldProcess($"{retentionRuleName}", $"Set Create New BLM Package to '{CreateNewBLMPackage}'"))
                    retentionRule.setCreateNewBLMPackage(CreateNewBLMPackage);

            WriteVerbose("Notice: Retention Rule Settings applied");
            // Done

            tFAManager.Dispose();
            retentionRule.Dispose();
        }
    }
}