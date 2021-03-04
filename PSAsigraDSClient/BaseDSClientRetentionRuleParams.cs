using System.Management.Automation;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientRetentionRuleParams: BaseDSClientRetentionRule
    {
        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to Cleanup Files Deleted from Source")]
        public SwitchParameter CleanupDeletedFiles { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Cleanup Files Deleted from Source after Time period")]
        [ValidateNotNullOrEmpty]
        public int CleanupDeletedAfterValue { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Cleanup Files Deleted from Source after Time period")]
        [ValidateNotNullOrEmpty]
        [ValidateSet("Minutes", "Hours", "Days", "Weeks", "Months", "Years")]
        public string CleanupDeletedAfterUnit { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Generations to keep of Files Deleted from Source")]
        public int CleanupDeletedKeepGens { get; set; } = 0;

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to Delete All Generations prior to Stub")]
        public SwitchParameter DeleteGensPriorToStub { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to Delete Non-stub Generations prior to Stub")]
        public SwitchParameter DeleteNonStubGens { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Local Storage Retention Time Value")]
        public int LSRetentionTimeValue { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Local Storage Retention Time Unit")]
        [ValidateSet("Minutes", "Hours", "Days", "Weeks", "Months", "Years")]
        public string LSRetentionTimeUnit { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to Cleanup Files Deleted from Source on Local Storage")]
        public SwitchParameter LSCleanupDeletedFiles { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Cleanup Files Deleted from Source after Time period on Local Storage")]
        public int LSCleanupDeletedAfterValue { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Cleanup Files Deleted from Source after Time period on Local Storage")]
        [ValidateSet("Minutes", "Hours", "Days", "Weeks", "Months", "Years")]
        public string LSCleanupDeletedAfterUnit { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Generations to keep of Files Deleted from Source")]
        public int LSCleanupDeletedKeepGens { get; set; } = 0;

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to Delete Unreferenced Files")]
        public SwitchParameter DeleteUnreferencedFiles { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to Delete Incomplete Components")]
        public SwitchParameter DeleteIncompleteComponents { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the number of most recent Generations to keep")]
        public int KeepLastGens { get; set; } = 1;

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Time Period to keep ALL Generations")]
        public int KeepAllGensTimeValue { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Time Period Unit for keeping ALL Generations")]
        [ValidateSet("Minutes", "Hours", "Days")]
        public string KeepAllGensTimeUnit { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to Delete Obsolete Data")]
        public SwitchParameter DeleteObsoleteData { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to Move Obsolete Data to BLM")]
        public SwitchParameter MoveObsoleteData { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to create new BLM Packages when moving to BLM")]
        public SwitchParameter CreateNewBLMPackage { get; set; }
    }
}