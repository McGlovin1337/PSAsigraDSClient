using System.Collections.Generic;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;
using static PSAsigraDSClient.BaseDSClientArchiveFilterRule;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientRetentionRule: DSClientCmdlet
    {
        protected abstract void ProcessRetentionRule(RetentionRule[] retentionRules);
        protected override void DSClientProcessRecord()
        {
            RetentionRuleManager DSClientRetentionMgr = DSClientSession.getRetentionRuleManager();

            WriteVerbose("Getting defined Retention Rules from DS-Client...");
            RetentionRule[] RetentionRules = DSClientRetentionMgr.definedRules();      

            ProcessRetentionRule(RetentionRules);

            DSClientRetentionMgr.Dispose();
        }

        public class DSClientRetentionRule
        {
            public int RetentionRuleId { get; set; }
            public string Name { get; set; }
            public int[] BackupSets { get; set; }
            public DSClientTimeRetention[] TimeRetention { get; set; }
            public string IntervalTimeRetention { get; set; }
            public string WeeklyTimeRetention { get; set; }
            public string MonthlyTimeRetention { get; set; }
            public string YearlyTimeRetention { get; set; }
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

                if (archiveRules != null)
                {
                    foreach (var archiveRule in archiveRules)
                    {
                        DSClientArchiveRule dSClientArchiveRule = new DSClientArchiveRule(archiveRule);
                        dSClientArchiveRules.Add(dSClientArchiveRule);
                    }
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
            public DSClientRetentionTimeSpan ValidFor { get; set; }
            public DSClientRetentionTimeSpan IntervalRepeat { get; set; }
            public TimeInDay SnapshotTime { get; set; }
            public string WeeklyDay { get; set; }            
            public string MonthlyDay { get; set; }
            public string YearlyMonth { get; set; }

            public DSClientTimeRetention(TimeRetentionOption timeRetention)
            {
                ETimeRetentionType type = timeRetention.getType();
                retention_time_span validFor = timeRetention.getValidFor();

                Type = ETimeRetentionTypeToString(type);
                ValidFor = new DSClientRetentionTimeSpan(validFor);

                if (type == ETimeRetentionType.ETimeRetentionType__Interval)
                {
                    IntervalTimeRetentionOption intervalTimeRetention = IntervalTimeRetentionOption.from(timeRetention);
                    IntervalRepeat = new DSClientRetentionTimeSpan(intervalTimeRetention.getRepeatTime());
                }
                else if (type == ETimeRetentionType.ETimeRetentionType__Weekly)
                {
                    WeeklyTimeRetentionOption weeklyTimeRetention = WeeklyTimeRetentionOption.from(timeRetention);
                    SnapshotTime = new TimeInDay(weeklyTimeRetention.getSnapshotTime());
                    WeeklyDay = EWeekDayToString(weeklyTimeRetention.getTriggerDay());
                }
                else if (type == ETimeRetentionType.ETimeRetentionType__Monthly)
                {
                    MonthlyTimeRetentionOption monthlyTimeRetention = MonthlyTimeRetentionOption.from(timeRetention);
                    SnapshotTime = new TimeInDay(monthlyTimeRetention.getSnapshotTime());
                    MonthlyDay = MonthlyDayToString(monthlyTimeRetention.getDayOfMonth());
                }
                else if (type == ETimeRetentionType.ETimeRetentionType__Yearly)
                {
                    YearlyTimeRetentionOption yearlyTimeRetention = YearlyTimeRetentionOption.from(timeRetention);
                    SnapshotTime = new TimeInDay(yearlyTimeRetention.getSnapshotTime());
                    MonthlyDay = MonthlyDayToString(yearlyTimeRetention.getDayOfMonth());
                    YearlyMonth = EMonthToString(yearlyTimeRetention.getTriggerMonth());
                }
            }

            private string MonthlyDayToString(int day)
            {
                string Day = null;

                if (day >= 28)
                    return "Last";
                else if (day > 0)
                    return day.ToString();

                return Day;
            }

            public override string ToString()
            {
                return Type;
            }

            private string ETimeRetentionTypeToString(ETimeRetentionType type)
            {
                string Type = null;

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
                FilterRule = (archiveFilterRule == null) ? new DSClientArchiveFilterRule() : new DSClientArchiveFilterRule(archiveFilterRule);
            }

            public override string ToString()
            {
                return FilterRule.Name;
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