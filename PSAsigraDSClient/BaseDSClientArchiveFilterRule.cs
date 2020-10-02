using System.Collections.Generic;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientArchiveFilterRule: DSClientCmdlet
    {
        protected abstract void ProcessArchiveFilterRules(ArchiveFilterRule[] archiveFilterRules);

        protected override void DSClientProcessRecord()
        {
            RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();

            WriteVerbose("Retrieving Archive Filter Rules from DS-Client...");
            ArchiveFilterRule[] archiveFilterRules = DSClientRetentionRuleMgr.definedArchiveFilterRules();

            ProcessArchiveFilterRules(archiveFilterRules);

            DSClientRetentionRuleMgr.Dispose();
        }

        public class DSClientArchiveFilterRule
        {
            public string Name { get; set; }
            public DSClientArchiveFilter[] Filter { get; set; }

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
            public string FilterType { get; set; }
            public string Type { get; set; }
            public string Pattern { get; set; }
            public bool IncludeSubDirs { get; set; }
            public bool CaseSensitive { get; set; }
            public bool MatchDirectories { get; set; }
            public bool NegateRegex { get; set; }

            public DSClientArchiveFilter(ArchiveFilter archiveFilter)
            {
                EArchiveFilterRuleType filterType = archiveFilter.getType();

                FilterType = ArchiveFilterTypeToString(filterType);
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

            private string ArchiveFilterTypeToString(EArchiveFilterRuleType filterRuleType)
            {
                string filterType = null;

                switch (filterRuleType)
                {
                    case EArchiveFilterRuleType.EArchiveFilterRuleType__FileFilter:
                        filterType = "FileFilter";
                        break;
                    case EArchiveFilterRuleType.EArchiveFilterRuleType__Regex:
                        filterType = "Regex";
                        break;
                    case EArchiveFilterRuleType.EArchiveFilterRuleType__UNDEFINED:
                        filterType = "Undefined";
                        break;
                }

                return filterType;
            }
        }
    }
}