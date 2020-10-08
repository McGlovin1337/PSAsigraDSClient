using System.Management.Automation;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientBackupSetItemParams: BaseDSClientBackupSet
    {
        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Items to Include in Backup Set")]
        public string[] IncludeItem { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Max Number of Generations for Included Items")]
        public int MaxGenerations { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Items to Exclude from Backup Set")]
        public string[] ExcludeItem { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Regex Item Exclusion Patterns")]
        [ValidateNotNullOrEmpty]
        public string[] RegexExcludeItem { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Path for Regex Exclusion Item")]
        [ValidateNotNullOrEmpty]
        public string RegexExclusionPath { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify to also Exclude Directories with Regex pattern")]
        public SwitchParameter RegexExcludeDirectory { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify if Regex Exclusions Items are case insensitive")]
        public SwitchParameter RegexCaseInsensitive { get; set; }
    }
}