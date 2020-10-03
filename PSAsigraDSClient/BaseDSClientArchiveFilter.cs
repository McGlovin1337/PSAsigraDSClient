using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientArchiveFilter: DSClientCmdlet 
    {
        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Filter Pattern")]
        public string Pattern { get; set; } = "*";

        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify this is an Exclusion Filter")]
        public SwitchParameter Exclusion { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "File", HelpMessage = "Specify to include Sub-Directories in File Filter")]
        public SwitchParameter IncludeSubDirs { get; set; }

        [Parameter(ParameterSetName = "Regex", HelpMessage = "Specify Regex Filter is NOT Case Sensitive")]
        public SwitchParameter NotCaseSensitive { get; set; }

        [Parameter(ParameterSetName = "Regex", HelpMessage = "Specify Regex should Match Directory Names")]
        public SwitchParameter MatchDirectories { get; set; }

        [Parameter(ParameterSetName = "Regex", HelpMessage = "Specify Regex should be Negated")]
        public SwitchParameter Negate { get; set; }

        protected abstract void ProcessArchiveFilter(ArchiveFilter archiveFilter);

        protected override void DSClientProcessRecord()
        {
            RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();

            ArchiveFilter NewArchiveFilter;

            if (IncludeSubDirs)
            {
                // Set File Archive Filter Properties
                WriteVerbose("Creating a File Archive Filter...");
                FileArchiveFilter fileArchiveFilter = DSClientRetentionRuleMgr.createFileArchiveFilter();

                fileArchiveFilter.setIncludeSubDirs(true);

                NewArchiveFilter = fileArchiveFilter;
            }
            else if (MyInvocation.BoundParameters.ContainsKey("NotCaseSensitive") ||
                MyInvocation.BoundParameters.ContainsKey("MatchDirectories") ||
                MyInvocation.BoundParameters.ContainsKey("Negate"))
            {
                //Set Regex Archive Filter Properties
                WriteVerbose("Creating a Regex Archive Filter...");
                RegexArchiveFilter regexArchiveFilter = DSClientRetentionRuleMgr.createRegexArchiveFilter();

                if (NotCaseSensitive)
                    regexArchiveFilter.setCaseSensitive(false);
                else
                    regexArchiveFilter.setCaseSensitive(true);

                regexArchiveFilter.setMatchDirectories(MatchDirectories);

                regexArchiveFilter.setNegate(Negate);

                NewArchiveFilter = regexArchiveFilter;
            }
            else
            {
                // Otherwise just create a File Archive Filter by Default
                WriteVerbose("Creating a File Archive Filter...");
                FileArchiveFilter fileArchiveFilter = DSClientRetentionRuleMgr.createFileArchiveFilter();
                NewArchiveFilter = fileArchiveFilter;
            }

            // Set if this is an Inclusion or Exclusion Filter
            if (Exclusion)
                NewArchiveFilter.setInclusion(false);
            else
                NewArchiveFilter.setInclusion(true);

            // Set the Filter Pattern
            NewArchiveFilter.setPattern(Pattern);

            ProcessArchiveFilter(NewArchiveFilter);

            NewArchiveFilter.Dispose();
            DSClientRetentionRuleMgr.Dispose();
        }
    }
}