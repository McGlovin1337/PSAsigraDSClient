using System.Collections.Generic;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientArchiveFilterRule: DSClientCmdlet
    {
        protected abstract void ProcessArchiveFilterRules(ArchiveFilterRule[] archiveFilterRules);

        protected override void DSClientProcessRecord()
        {
            RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();

            WriteVerbose("Performing Action: Retrieve Archive Filter Rules");
            ArchiveFilterRule[] archiveFilterRules = DSClientRetentionRuleMgr.definedArchiveFilterRules();

            ProcessArchiveFilterRules(archiveFilterRules);

            DSClientRetentionRuleMgr.Dispose();
        }

        public class DSClientArchiveFilterRule
        {
            public string Name { get; private set; }
            public DSClientArchiveFilter[] Filter { get; private set; }

            public DSClientArchiveFilterRule()
            {
                Name = "AllFiles";
            }

            public DSClientArchiveFilterRule(ArchiveFilterRule archiveFilterRule)
            {
                Name = archiveFilterRule.getName();

                ArchiveFilter[] archiveFilters = archiveFilterRule.getFilterList();

                List<DSClientArchiveFilter> dSClientArchiveFilters = new List<DSClientArchiveFilter>();

                foreach (ArchiveFilter filter in archiveFilters)
                {
                    DSClientArchiveFilter dSClientArchiveFilter = new DSClientArchiveFilter(filter);
                    dSClientArchiveFilters.Add(dSClientArchiveFilter);
                }

                Filter = dSClientArchiveFilters.ToArray();
            }

            public override string ToString()
            {
                return Name;
            }
        }

        public class DSClientArchiveFilter
        {
            public string FilterType { get; private set; }
            public string Type { get; private set; }
            public string Pattern { get; private set; }
            public bool IncludeSubDirs { get; private set; }
            public bool CaseSensitive { get; private set; }
            public bool MatchDirectories { get; private set; }
            public bool NegateRegex { get; private set; }

            public DSClientArchiveFilter(ArchiveFilter archiveFilter)
            {
                EArchiveFilterRuleType filterType = archiveFilter.getType();

                FilterType = EnumToString(filterType);
                Type = (archiveFilter.isInclusion() == true) ? "Inclusion" : "Exclusion";
                Pattern = archiveFilter.getPattern();

                if (filterType == EArchiveFilterRuleType.EArchiveFilterRuleType__FileFilter)
                {
                    FileArchiveFilter fileArchiveFilter = FileArchiveFilter.from(archiveFilter);

                    IncludeSubDirs = fileArchiveFilter.getIncludeSubDirs();
                }
                else if (filterType == EArchiveFilterRuleType.EArchiveFilterRuleType__Regex)
                {
                    RegexArchiveFilter regexArchiveFilter = RegexArchiveFilter.from(archiveFilter);

                    CaseSensitive = regexArchiveFilter.getCaseSensitive();
                    MatchDirectories = regexArchiveFilter.getMatchDirectories();
                    NegateRegex = regexArchiveFilter.getNegate();
                }
            }

            public override string ToString()
            {
                return Type;
            }
        }
    }
}