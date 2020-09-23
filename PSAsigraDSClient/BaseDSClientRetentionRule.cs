/* To-Do:
 * Improve DSClientTimeRetention Class Data (account for Interval, Weekly, Monthly)
 */

using System.Collections.Generic;
using System.Linq;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientRetentionRule: DSClientCmdlet
    {
        protected abstract void ProcessRetentionRule(IEnumerable<DSClientRetentionRule> retentionRules);
        protected override void DSClientProcessRecord()
        {
            RetentionRuleManager DSClientRetentionMgr = DSClientSession.getRetentionRuleManager();

            WriteVerbose("Getting defined Retention Rules from DS-Client...");
            RetentionRule[] RetentionRules = DSClientRetentionMgr.definedRules();
            WriteVerbose("Yielded " + RetentionRules.Count() + " Retention Rules");

            List<DSClientRetentionRule> DSClientRetentionRules = new List<DSClientRetentionRule>();

            foreach (var rule in RetentionRules)
            {
                DSClientRetentionRule dSClientRetentionRule = new DSClientRetentionRule(rule);
                DSClientRetentionRules.Add(dSClientRetentionRule);
                rule.Dispose();
            }           

            ProcessRetentionRule(DSClientRetentionRules);

            DSClientRetentionMgr.Dispose();
        }

        public class DSClientRetentionRule
        {
            public int RetentionRuleId { get; set; }
            public string Name { get; set; }
            public int[] BackupSets { get; set; }
            public DSClientTimeRetention[] TimeRetention { get; set; }
            public DSClientArchiveRule[] ArchiveRule { get; set; }
            public bool ArchiveSpecialFiles { get; set; }
            public bool ArchiveLatestSpecialFiles { get; set; }
            public bool CleanupRemovedFiles { get; set; }
            public DSClientRetentionTimeSpan CleanupRemovedAfter { get; set; }
            public int CleanupRemovedKeep { get; set; }
            public bool NewBLMPackage { get; set; }
            public bool DeleteIncompleteComponenets { get; set; }
            public bool DeleteStub { get; set; }
            public bool DeleteStubAllGens { get; set; }
            public bool DeleteUnreferencedFiles { get; set; }
            public bool KeepGensByPeriod { get; set; }
            public int KeepLastGens { get; set; }
            public DSClientRetentionTimeSpan KeepPeriodTimeSpan { get; set; }
            public DSClientRetentionTimeSpan LocalStorageRetention { get; set; }
            public bool LSCleanupRemovedFiles { get; set; }
            public DSClientRetentionTimeSpan LSCleanupRemovedafter { get; set; }
            public int LSCleanupRemovedKeep { get; set; }
            public bool MoveObsoleteToBLM { get; set; }
            public bool OnlyCompareBackupTime { get; set; }

            public DSClientRetentionRule(RetentionRule retentionRule)
            {
                // Get the Time Retention Configuration
                TimeRetentionOption[] timeRetentionOptions = retentionRule.getTimeRetentions();
                List<DSClientTimeRetention> dSClientTimeRetentions = new List<DSClientTimeRetention>();

                foreach (var timeRetentionOption in timeRetentionOptions)
                {
                    DSClientTimeRetention timeRetention = new DSClientTimeRetention(timeRetentionOption);
                    dSClientTimeRetentions.Add(timeRetention);
                }

                // Get the Archive Rules
                ArchiveRule[] archiveRules = retentionRule.getArchiveRules();
                List<DSClientArchiveRule> dSClientArchiveRules = new List<DSClientArchiveRule>();

                foreach (var archiveRule in archiveRules)
                {
                    DSClientArchiveRule dSClientArchiveRule = new DSClientArchiveRule(archiveRule);
                    dSClientArchiveRules.Add(dSClientArchiveRule);
                }

                // Assign Property Values
                RetentionRuleId = retentionRule.getID();
                Name = retentionRule.getName();
                BackupSets = retentionRule.getAssignedBackupSets();
                TimeRetention = dSClientTimeRetentions.ToArray();
                ArchiveRule = dSClientArchiveRules.ToArray();
                ArchiveSpecialFiles = retentionRule.getArchiveSpecialFiles();
                ArchiveLatestSpecialFiles = retentionRule.getArchiveLatestSpecialFiles();
                CleanupRemovedFiles = retentionRule.getCleanupRemovedFiles();
                CleanupRemovedAfter = new DSClientRetentionTimeSpan(retentionRule.getCleanupRemovedAfter());
                CleanupRemovedKeep = retentionRule.getCleanupRemovedKeep();
                NewBLMPackage = retentionRule.getCreateNewBLMPackage();
                DeleteIncompleteComponenets = retentionRule.getDeleteIncompleteComponents();
                DeleteStub = retentionRule.getDeleteStub();
                DeleteStubAllGens = retentionRule.getDeleteStubAllGens();
                DeleteUnreferencedFiles = retentionRule.getDeleteUnreferencedFiles();
                KeepGensByPeriod = retentionRule.getKeepGenerationsByPeriod();
                KeepLastGens = retentionRule.getKeepLastGenerations();
                KeepPeriodTimeSpan = new DSClientRetentionTimeSpan(retentionRule.getKeepPeriodTimeSpan());
                LocalStorageRetention = new DSClientRetentionTimeSpan(retentionRule.getLocalStorageRetention());
                LSCleanupRemovedFiles = retentionRule.getLSCleanupRemovedFiles();
                LSCleanupRemovedafter = new DSClientRetentionTimeSpan(retentionRule.getLSCleanupRemovedAfter());
                LSCleanupRemovedKeep = retentionRule.getLSCleanupRemovedKeep();
                MoveObsoleteToBLM = retentionRule.getMoveObsoleteDataToBLM();
                OnlyCompareBackupTime = retentionRule.getOnlyCompareBackupTime();
            }

            public override string ToString()
            {
                return Name;
            }
        }

        public class DSClientTimeRetention
        {
            public string Type { get; set; }
            public DSClientRetentionTimeSpan TimePeriod { get; set; }

            public DSClientTimeRetention(TimeRetentionOption timeRetention)
            {
                ETimeRetentionType type = timeRetention.getType();
                retention_time_span period = timeRetention.getValidFor();

                switch (type)
                {
                    case ETimeRetentionType.ETimeRetentionType__Interval:
                        Type = "Interval";
                        break;
                    case ETimeRetentionType.ETimeRetentionType__Weekly:
                        Type = "Weekly";
                        break;
                    case ETimeRetentionType.ETimeRetentionType__Monthly:
                        Type = "Monthly";
                        break;
                    case ETimeRetentionType.ETimeRetentionType__Yearly:
                        Type = "Yearly";
                        break;
                    case ETimeRetentionType.ETimeRetentionType__UNDEFINED:
                        Type = "Undefined";
                        break;
                }

                TimePeriod = new DSClientRetentionTimeSpan(period);
            }

            public override string ToString()
            {
                return Type;
            }
        }

        public class DSClientArchiveRule
        {
            public DSClientRetentionTimeSpan TimeSpan { get; set; }
            public DSClientArchiveFilterRule FilterRule { get; set; }

            public DSClientArchiveRule(ArchiveRule archiveRule)
            {
                retention_time_span timeSpan = archiveRule.getTimeSpan();
                ArchiveFilterRule archiveFilterRule = archiveRule.getFilterRule();

                TimeSpan = new DSClientRetentionTimeSpan(timeSpan);
            }

        }

        public class DSClientArchiveFilterRule
        {
            public string Name { get; set; }
            public DSClientArchiveFilter[] ArchiveFilter { get; set; }

            public DSClientArchiveFilterRule(ArchiveFilterRule filterRule)
            {
                ArchiveFilter[] filterList = filterRule.getFilterList();
                List<DSClientArchiveFilter> dSClientArchiveFilters = new List<DSClientArchiveFilter>();

                foreach (var filter in filterList)
                {
                    DSClientArchiveFilter dSClientArchiveFilter = new DSClientArchiveFilter(filter);
                    dSClientArchiveFilters.Add(dSClientArchiveFilter);
                }

                Name = filterRule.getName();
                ArchiveFilter = dSClientArchiveFilters.ToArray();
            }
        }

        public class DSClientArchiveFilter
        {
            public string Type { get; set; }
            public string Pattern { get; set; }
            public bool Inclusion { get; set; }

            public DSClientArchiveFilter(ArchiveFilter archiveFilter)
            {
                EArchiveFilterRuleType type = archiveFilter.getType();

                Type = ArchiveFilterTypeToString(type);
                Pattern = archiveFilter.getPattern();
                Inclusion = archiveFilter.isInclusion();
            }

            private string ArchiveFilterTypeToString(EArchiveFilterRuleType filterRuleType)
            {
                string filterType = null;

                switch(filterRuleType)
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

            public override string ToString()
            {
                return Type;
            }
        }

        public class DSClientRetentionTimeSpan
        {
            public int Period { get; set; }
            public string Unit { get; set; }

            public DSClientRetentionTimeSpan(retention_time_span timeSpan)
            {
                Period = timeSpan.period;
                Unit = RetentionTimeUnitToString(timeSpan.unit);
            }

            private string RetentionTimeUnitToString(RetentionTimeUnit unit)
            {
                string newUnit = null;

                switch (unit)
                {
                    case RetentionTimeUnit.RetentionTimeUnit__Seconds:
                        newUnit = "Seconds";
                        break;
                    case RetentionTimeUnit.RetentionTimeUnit__Minutes:
                        newUnit = "Minutes";
                        break;
                    case RetentionTimeUnit.RetentionTimeUnit__Hours:
                        newUnit = "Hours";
                        break;
                    case RetentionTimeUnit.RetentionTimeUnit__Days:
                        newUnit = "Days";
                        break;
                    case RetentionTimeUnit.RetentionTimeUnit__Weeks:
                        newUnit = "Weeks";
                        break;
                    case RetentionTimeUnit.RetentionTimeUnit__Months:
                        newUnit = "Months";
                        break;
                    case RetentionTimeUnit.RetentionTimeUnit__Years:
                        newUnit = "Years";
                        break;
                    case RetentionTimeUnit.RetentionTimeUnit__UNDEFINED:
                        newUnit = "Undefined";
                        break;
                }

                return newUnit;
            }

            public override string ToString()
            {
                return Period.ToString() + " " + Unit;
            }
        }
    }
}