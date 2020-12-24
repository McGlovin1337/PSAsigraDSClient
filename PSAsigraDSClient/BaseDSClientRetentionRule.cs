using System.Collections.Generic;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;
using static PSAsigraDSClient.BaseDSClientArchiveFilterRule;
using System;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientRetentionRule: DSClientCmdlet
    {
        protected virtual void ProcessRetentionRule(RetentionRule[] retentionRules)
        {
            throw new NotImplementedException("ProcessRetentionRule method should be overridden");
        }

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
            public int RetentionRuleId { get; private set; }
            public string Name { get; private set; }
            public int[] BackupSets { get; private set; }
            public DSClientTimeRetention[] TimeRetention { get; private set; }
            public string IntervalTimeRetention { get; private set; }
            public string WeeklyTimeRetention { get; private set; }
            public string MonthlyTimeRetention { get; private set; }
            public string YearlyTimeRetention { get; private set; }
            public DSClientArchiveRule[] ArchiveRule { get; private set; }
            public bool ArchiveSpecialFiles { get; private set; }
            public bool ArchiveLatestSpecialFiles { get; private set; }
            public bool CleanupRemovedFiles { get; private set; }
            public DSClientRetentionTimeSpan CleanupRemovedAfter { get; private set; }
            public int CleanupRemovedKeep { get; private set; }
            public bool NewBLMPackage { get; private set; }
            public bool DeleteIncompleteComponenets { get; private set; }
            public bool DeleteStub { get; private set; }
            public bool DeleteStubAllGens { get; private set; }
            public bool DeleteUnreferencedFiles { get; private set; }
            public bool KeepGensByPeriod { get; private set; }
            public int KeepLastGens { get; private set; }
            public DSClientRetentionTimeSpan KeepPeriodTimeSpan { get; private set; }
            public DSClientRetentionTimeSpan LocalStorageRetention { get; private set; }
            public bool LSCleanupRemovedFiles { get; private set; }
            public DSClientRetentionTimeSpan LSCleanupRemovedafter { get; private set; }
            public int LSCleanupRemovedKeep { get; private set; }
            public bool MoveObsoleteToBLM { get; private set; }
            public bool OnlyCompareBackupTime { get; private set; }

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
            public string Type { get; private set; }
            public DSClientRetentionTimeSpan ValidFor { get; private set; }
            public DSClientRetentionTimeSpan IntervalRepeat { get; private set; }
            public TimeInDay SnapshotTime { get; private set; }
            public string WeeklyDay { get; private set; }            
            public string MonthlyDay { get; private set; }
            public string YearlyMonth { get; private set; }

            public DSClientTimeRetention(TimeRetentionOption timeRetention)
            {
                ETimeRetentionType type = timeRetention.getType();
                retention_time_span validFor = timeRetention.getValidFor();

                Type = EnumToString(type);
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
        }

        public class DSClientArchiveRule
        {
            public DSClientRetentionTimeSpan TimeSpan { get; private set; }
            public DSClientArchiveFilterRule FilterRule { get; private set; }

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
            public int Period { get; private set; }
            public string Unit { get; private set; }

            public DSClientRetentionTimeSpan(retention_time_span timeSpan)
            {
                Period = timeSpan.period;
                Unit = EnumToString(timeSpan.unit);
            }

            public override string ToString()
            {
                return Period.ToString() + " " + Unit;
            }
        }
    }
}