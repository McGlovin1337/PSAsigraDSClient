using System.Management.Automation;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientBackupSetItemParams: BaseDSClientBackupSet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set to modify")]
        public int BackupSetId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "inclusion", HelpMessage = "Specify that this is an Inclusion Item")]
        public SwitchParameter Inclusion { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "exclusion", HelpMessage = "Specify that this is an Exclusion Item")]
        public SwitchParameter Exclusion { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "regex", HelpMessage = "Specify that this is a Regex Exclusion Item")]
        public SwitchParameter RegexExclusion { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Path to add to Backup Set")]
        [Alias("Folder", "Directory")]
        public string Path { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "inclusion", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Item Filter")]
        [Parameter(Mandatory = true, ParameterSetName = "exclusion")]
        [Parameter(Mandatory = true, ParameterSetName = "regex")]
        [Alias("Expression", "Item")]
        public string Filter { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Exclude Sub-Directories in the specified path")]
        public SwitchParameter ExcludeSubDirs { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "inclusion", ValueFromPipelineByPropertyName = true, HelpMessage = "Max Number of Generations for Included Items")]
        [ValidateRange(1, 9999)]
        public int MaxGenerations { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "regex", HelpMessage = "Specify to also Directory Names with Regex pattern")]
        public SwitchParameter RegexMatchDirectory { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "regex", HelpMessage = "Specify if Regex Exclusions Items are case insensitive")]
        public SwitchParameter RegexCaseInsensitive { get; set; }
    }
}