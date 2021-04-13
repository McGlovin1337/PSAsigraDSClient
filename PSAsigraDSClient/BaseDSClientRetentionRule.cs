using System.Collections.Generic;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;
using static PSAsigraDSClient.BaseDSClientArchiveFilterRule;
using System;
using System.Security.Cryptography;
using System.Text;

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
            tFAManager.Dispose();

            RetentionRuleManager DSClientRetentionMgr = DSClientSession.getRetentionRuleManager();

            WriteVerbose("Performing Action: Retrieve defined Retention Rules");
            RetentionRule[] RetentionRules = DSClientRetentionMgr.definedRules();      

            ProcessRetentionRule(RetentionRules);

            DSClientRetentionMgr.Dispose();
        }

        public class DSClientRetentionRule
        {
            public int RetentionRuleId { get; private set; }
            public string Name { get; private set; }
            public int[] BackupSets { get; private set; }
            public IntervalTimeRetention[] IntervalTimeRetention { get; private set; }
            public WeeklyTimeRetention[] WeeklyTimeRetention { get; private set; }
            public MonthlyTimeRetention[] MonthlyTimeRetention { get; private set; }
            public YearlyTimeRetention[] YearlyTimeRetention { get; private set; }
            public DSClientArchiveRule[] ArchiveRule { get; private set; }
            public bool ArchiveSpecialFiles { get; private set; }
            public bool ArchiveLatestSpecialFiles { get; private set; }
            public bool CleanupRemovedFiles { get; private set; }
            public DSClientTimeSpan CleanupRemovedAfter { get; private set; }
            public int CleanupRemovedKeep { get; private set; }
            public bool NewBLMPackage { get; private set; }
            public bool DeleteIncompleteComponenets { get; private set; }
            public bool DeleteStub { get; private set; }
            public bool DeleteStubAllGens { get; private set; }
            public bool DeleteUnreferencedFiles { get; private set; }
            public bool KeepGensByPeriod { get; private set; }
            public int KeepLastGens { get; private set; }
            public DSClientTimeSpan KeepPeriodTimeSpan { get; private set; }
            public DSClientTimeSpan LocalStorageRetention { get; private set; }
            public bool LSCleanupRemovedFiles { get; private set; }
            public DSClientTimeSpan LSCleanupRemovedafter { get; private set; }
            public int LSCleanupRemovedKeep { get; private set; }
            public bool DeleteObsoleteData { get; private set; }
            public bool MoveObsoleteToBLM { get; private set; }
            public bool OnlyCompareBackupTime { get; private set; }

            public DSClientRetentionRule(RetentionRule retentionRule)
            {
                // Get the Time Retention Configuration
                TimeRetentionOption[] timeRetentionOptions = retentionRule.getTimeRetentions();

                List<IntervalTimeRetention> intervalTimeRetentions = new List<IntervalTimeRetention>();
                List<WeeklyTimeRetention> weeklyTimeRetentions = new List<WeeklyTimeRetention>();
                List<MonthlyTimeRetention> monthlyTimeRetentions = new List<MonthlyTimeRetention>();
                List<YearlyTimeRetention> yearlyTimeRetentions = new List<YearlyTimeRetention>();

                foreach (var timeRetentionOption in timeRetentionOptions)
                {
                    ETimeRetentionType timeRetentionType = timeRetentionOption.getType();

                    switch (timeRetentionType)
                    {
                        case ETimeRetentionType.ETimeRetentionType__Interval:
                            intervalTimeRetentions.Add(new IntervalTimeRetention(IntervalTimeRetentionOption.from(timeRetentionOption)));
                            break;
                        case ETimeRetentionType.ETimeRetentionType__Weekly:
                            weeklyTimeRetentions.Add(new WeeklyTimeRetention(WeeklyTimeRetentionOption.from(timeRetentionOption)));
                            break;
                        case ETimeRetentionType.ETimeRetentionType__Monthly:
                            monthlyTimeRetentions.Add(new MonthlyTimeRetention(MonthlyTimeRetentionOption.from(timeRetentionOption)));
                            break;
                        case ETimeRetentionType.ETimeRetentionType__Yearly:
                            yearlyTimeRetentions.Add(new YearlyTimeRetention(YearlyTimeRetentionOption.from(timeRetentionOption)));
                            break;
                    }
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

                // Delete or Move Obsolete Data
                bool obsoleteData = retentionRule.getMoveObsoleteDataToBLM();

                // Assign Property Values
                RetentionRuleId = retentionRule.getID();
                Name = retentionRule.getName();
                BackupSets = retentionRule.getAssignedBackupSets();
                IntervalTimeRetention = intervalTimeRetentions.ToArray();
                WeeklyTimeRetention = weeklyTimeRetentions.ToArray();
                MonthlyTimeRetention = monthlyTimeRetentions.ToArray();
                YearlyTimeRetention = yearlyTimeRetentions.ToArray();
                ArchiveRule = dSClientArchiveRules.ToArray();
                ArchiveSpecialFiles = retentionRule.getArchiveSpecialFiles();
                ArchiveLatestSpecialFiles = retentionRule.getArchiveLatestSpecialFiles();
                CleanupRemovedFiles = retentionRule.getCleanupRemovedFiles();
                CleanupRemovedAfter = new DSClientTimeSpan(retentionRule.getCleanupRemovedAfter());
                CleanupRemovedKeep = retentionRule.getCleanupRemovedKeep();
                NewBLMPackage = retentionRule.getCreateNewBLMPackage();
                DeleteIncompleteComponenets = retentionRule.getDeleteIncompleteComponents();
                DeleteStub = retentionRule.getDeleteStub();
                DeleteStubAllGens = retentionRule.getDeleteStubAllGens();
                DeleteUnreferencedFiles = retentionRule.getDeleteUnreferencedFiles();
                KeepGensByPeriod = retentionRule.getKeepGenerationsByPeriod();
                KeepLastGens = retentionRule.getKeepLastGenerations();
                KeepPeriodTimeSpan = new DSClientTimeSpan(retentionRule.getKeepPeriodTimeSpan());
                LocalStorageRetention = new DSClientTimeSpan(retentionRule.getLocalStorageRetention());
                LSCleanupRemovedFiles = retentionRule.getLSCleanupRemovedFiles();
                LSCleanupRemovedafter = new DSClientTimeSpan(retentionRule.getLSCleanupRemovedAfter());
                LSCleanupRemovedKeep = retentionRule.getLSCleanupRemovedKeep();
                DeleteObsoleteData = !obsoleteData;
                MoveObsoleteToBLM = obsoleteData;
                OnlyCompareBackupTime = retentionRule.getOnlyCompareBackupTime();
            }

            public override string ToString()
            {
                return Name;
            }
        }

        public class IntervalTimeRetention
        {
            public DSClientTimeSpan RepeatInterval { get; private set; }
            public DSClientTimeSpan ValidFor { get; private set; }

            public IntervalTimeRetention(IntervalTimeRetentionOption interval)
            {
                RepeatInterval = new DSClientTimeSpan(interval.getRepeatTime());
                ValidFor = new DSClientTimeSpan(interval.getValidFor());
            }

            public override string ToString()
            {
                return RepeatInterval.ToString();
            }
        }

        public class WeeklyTimeRetention
        {
            public string Day { get; private set; }
            public TimeInDay SnapshotTime { get; private set; }
            public DSClientTimeSpan ValidFor { get; private set; }

            public WeeklyTimeRetention(WeeklyTimeRetentionOption weekly)
            {
                Day = EnumToString(weekly.getTriggerDay());
                SnapshotTime = new TimeInDay(weekly.getSnapshotTime());
                ValidFor = new DSClientTimeSpan(weekly.getValidFor());
            }

            public override string ToString()
            {
                return Day;
            }
        }

        public class MonthlyTimeRetention
        {
            public string DayOfMonth { get; private set; }
            public TimeInDay SnapshotTime { get; private set; }
            public DSClientTimeSpan ValidFor { get; private set; }

            public MonthlyTimeRetention(MonthlyTimeRetentionOption monthly)
            {
                DayOfMonth = MonthlyDayToString(monthly.getDayOfMonth());
                SnapshotTime = new TimeInDay(monthly.getSnapshotTime());
                ValidFor = new DSClientTimeSpan(monthly.getValidFor());
            }

            public override string ToString()
            {
                return DayOfMonth;
            }
        }

        public class YearlyTimeRetention
        {
            public string DayOfMonth { get; private set; }
            public string Month { get; private set; }
            public TimeInDay SnapshotTime { get; private set; }
            public DSClientTimeSpan ValidFor { get; private set; }

            public YearlyTimeRetention(YearlyTimeRetentionOption yearly)
            {
                DayOfMonth = MonthlyDayToString(yearly.getDayOfMonth());
                Month = EnumToString(yearly.getTriggerMonth());
                SnapshotTime = new TimeInDay(yearly.getSnapshotTime());
                ValidFor = new DSClientTimeSpan(yearly.getValidFor());
            }

            public override string ToString()
            {
                return $"{DayOfMonth} of {Month}";
            }
        }

        protected class TimeRetentionOverview
        {
            private readonly retention_time_span _validFor;
            private readonly ETimeRetentionType _type;
            public int TimeRetentionId { get; private set; }
            public string Type { get; private set; }
            public IntervalTimeRetention Interval { get; private set; }
            public WeeklyTimeRetention Weekly { get; private set; }
            public MonthlyTimeRetention Monthly { get; private set; }
            public YearlyTimeRetention Yearly { get; private set; }
            public DSClientTimeSpan ValidFor { get; private set; }

            public TimeRetentionOverview(int id, TimeRetentionOption timeRetention)
            {
                _validFor = timeRetention.getValidFor();
                _type = timeRetention.getType();            

                TimeRetentionId = id;
                Type = EnumToString(_type);
                Interval = (_type == ETimeRetentionType.ETimeRetentionType__Interval) ? new IntervalTimeRetention(IntervalTimeRetentionOption.from(timeRetention)) : null;
                Weekly = (_type == ETimeRetentionType.ETimeRetentionType__Weekly) ? new WeeklyTimeRetention(WeeklyTimeRetentionOption.from(timeRetention)) : null;
                Monthly = (_type == ETimeRetentionType.ETimeRetentionType__Monthly) ? new MonthlyTimeRetention(MonthlyTimeRetentionOption.from(timeRetention)) : null;
                Yearly = (_type == ETimeRetentionType.ETimeRetentionType__Yearly) ? new YearlyTimeRetention(YearlyTimeRetentionOption.from(timeRetention)) : null;
                ValidFor = new DSClientTimeSpan(_validFor);
            }
        }

        public class DSClientArchiveRule
        {
            public DSClientTimeSpan TimeSpan { get; private set; }
            public DSClientArchiveFilterRule FilterRule { get; private set; }

            public DSClientArchiveRule(ArchiveRule archiveRule)
            {
                retention_time_span timeSpan = archiveRule.getTimeSpan();
                ArchiveFilterRule archiveFilterRule = archiveRule.getFilterRule();

                TimeSpan = new DSClientTimeSpan(timeSpan);
                FilterRule = (archiveFilterRule == null) ? new DSClientArchiveFilterRule() : new DSClientArchiveFilterRule(archiveFilterRule);
            }

            public override string ToString()
            {
                return FilterRule.Name;
            }
        }

        protected static string MonthlyDayToString(int day)
        {
            if (day >= 28)
                return "Last";
            else if (day > 0)
                return day.ToString();

            return null;
        }

        protected static string TimeRetentionHash(RetentionRule retentionRule, TimeRetentionOption timeRetention)
        {
            ETimeRetentionType type = timeRetention.getType();
            retention_time_span timeSpan = timeRetention.getValidFor();

            string strId = retentionRule.getID().ToString();
            string strName = retentionRule.getName();
            string strType = type.ToString();
            string strValidPeriod = timeSpan.period.ToString();
            string strValidUnit = timeSpan.unit.ToString();

            string strToHash = strId + strName + strType + strValidPeriod + strValidUnit;

            time_in_day snapshotTime;
            switch (type)
            {
                case ETimeRetentionType.ETimeRetentionType__Interval:
                    IntervalTimeRetentionOption interval = IntervalTimeRetentionOption.from(timeRetention);
                    retention_time_span repeatSpan = interval.getRepeatTime();
                    strToHash += repeatSpan.period.ToString() + repeatSpan.unit.ToString();
                    break;
                case ETimeRetentionType.ETimeRetentionType__Weekly:
                    WeeklyTimeRetentionOption weekly = WeeklyTimeRetentionOption.from(timeRetention);
                    snapshotTime = weekly.getSnapshotTime();
                    strToHash += snapshotTime.hour.ToString() + snapshotTime.minute.ToString() + snapshotTime.second.ToString() + weekly.getTriggerDay().ToString();
                    break;
                case ETimeRetentionType.ETimeRetentionType__Monthly:
                    MonthlyTimeRetentionOption monthly = MonthlyTimeRetentionOption.from(timeRetention);
                    snapshotTime = monthly.getSnapshotTime();
                    strToHash += snapshotTime.hour.ToString() + snapshotTime.minute.ToString() + snapshotTime.second.ToString() + monthly.getDayOfMonth().ToString();
                    break;
                case ETimeRetentionType.ETimeRetentionType__Yearly:
                    YearlyTimeRetentionOption yearly = YearlyTimeRetentionOption.from(timeRetention);
                    snapshotTime = yearly.getSnapshotTime();
                    strToHash += snapshotTime.hour.ToString() + snapshotTime.minute.ToString() + snapshotTime.second.ToString() + yearly.getDayOfMonth().ToString() + yearly.getTriggerMonth().ToString();
                    break;
            }

            return strToHash.GetSha1Hash();
        }

        protected static IntervalTimeRetentionOption IntervalTimeRetentionRule(IntervalTimeRetentionOption intervalTimeRetention, int timeValue, string timeUnit, int validForValue, string validForUnit)
        {
            retention_time_span intTimeSpan = new retention_time_span
            {
                period = timeValue,
                unit = StringToEnum<RetentionTimeUnit>(timeUnit)
            };
            intervalTimeRetention.setRepeatTime(intTimeSpan);

            SetTimeRetentionValidityPeriod(intervalTimeRetention, validForValue, validForUnit);

            return intervalTimeRetention;
        }

        protected static WeeklyTimeRetentionOption WeeklyTimeRetentionRule(WeeklyTimeRetentionOption weeklyTimeRetention, string weekDay, string time, int validForValue, string validForUnit)
        {
            weeklyTimeRetention.setTriggerDay(StringToEnum<EWeekDay>(weekDay));

            time_in_day weeklyTime = StringTotime_in_day(time);
            weeklyTimeRetention.setSnapshotTime(weeklyTime);

            SetTimeRetentionValidityPeriod(weeklyTimeRetention, validForValue, validForUnit);

            return weeklyTimeRetention;
        }

        protected static MonthlyTimeRetentionOption MonthlyTimeRetentionRule(MonthlyTimeRetentionOption monthlyTimeRetention, int retentionDay, string time, int validForValue, string validForUnit)
        {
            monthlyTimeRetention.setDayOfMonth(retentionDay);
            monthlyTimeRetention.setSnapshotTime(StringTotime_in_day(time));

            SetTimeRetentionValidityPeriod(monthlyTimeRetention, validForValue, validForUnit);

            return monthlyTimeRetention;
        }

        protected static YearlyTimeRetentionOption YearlyTimeRetentionRule(YearlyTimeRetentionOption yearlyTimeRetention, int monthDay, string month, string time, int validForValue, string validForUnit)
        {
            yearlyTimeRetention.setDayOfMonth(monthDay);

            yearlyTimeRetention.setTriggerMonth(StringToEnum<EMonth>(month));

            time_in_day yearlyTime = StringTotime_in_day(time);
            yearlyTimeRetention.setSnapshotTime(yearlyTime);

            SetTimeRetentionValidityPeriod(yearlyTimeRetention, validForValue, validForUnit);

            return yearlyTimeRetention;
        }

        protected static TimeRetentionOption SetTimeRetentionValidityPeriod(TimeRetentionOption timeRetentionOption, int validForValue, string validForUnit)
        {
            retention_time_span validTimeSpan = new retention_time_span
            {
                period = validForValue,
                unit = StringToEnum<RetentionTimeUnit>(validForUnit)
            };
            timeRetentionOption.setValidFor(validTimeSpan);

            return timeRetentionOption;
        }

        protected static void KeepAllGenerationsRule(RetentionRule retentionRule, int keepAllGensTimeValue, string keepAllGensTimeUnit)
        {
            retentionRule.setKeepGenerationsByPeriod(true);

            retention_time_span timeSpan = new retention_time_span
            {
                period = keepAllGensTimeValue,
                unit = StringToEnum<RetentionTimeUnit>(keepAllGensTimeUnit)
            };
            retentionRule.setKeepPeriodTimeSpan(timeSpan);
        }

        protected static TimeRetentionOption SelectTimeRetentionOption(RetentionRule retentionRule, int timeRetentionId, Dictionary<string, int> optionHashes)
        {
            TimeRetentionOption timeRetentionOption = null;

            // Get all the Time Retention Options in this Retention Rule
            TimeRetentionOption[] timeRetentions = retentionRule.getTimeRetentions();

            foreach (TimeRetentionOption timeRetention in timeRetentions)
            {
                string optionHash = TimeRetentionHash(retentionRule, timeRetention);

                optionHashes.TryGetValue(optionHash, out int optionId);

                if (optionId == timeRetentionId)
                    return timeRetention;

                timeRetention.Dispose();
            }

            return timeRetentionOption;
        }
    }
}