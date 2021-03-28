using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientSchedule: DSClientCmdlet
    {
        protected int ScheduleWeekDaysToInt(string[] weekDays)
        {
            int WeekDays = 0;

            foreach (string weekday in weekDays)
            {
                string day = weekday;
                if (day.ToLower() == "thursday")
                    day = "thrusday"; //Conversion required due to spelling mistake in enum

                WeekDays += (int)StringToEnum<EScheduleWeekDays>(day);
            }

            return WeekDays;
        }

        public class DSClientScheduleInfo
        {
            public int ScheduleId { get; private set; }
            public string Name { get; private set; }
            public int[] BackupSetId { get; private set; }
            public bool Active { get; private set; }
            public bool AdminOnly { get; private set; }
            public int CPUThrottle { get; private set; }
            public int ConcurrentBackupSets { get; private set; }           
            public bool NetworkDetection { get; private set; }
            public string ShortName { get; private set; }

            public DSClientScheduleInfo(schedule_info scheduleInfo)
            {
                ScheduleId = scheduleInfo.id;
                Name = scheduleInfo.name;
                Active = scheduleInfo.active;
                AdminOnly = scheduleInfo.administratorsOnly;
                CPUThrottle = scheduleInfo.backupCPUThrottle;
                ConcurrentBackupSets = scheduleInfo.concurrentBackupSets;
                NetworkDetection = scheduleInfo.usingNetworkDetection;
                ShortName = scheduleInfo.shortName;
            }

            public DSClientScheduleInfo(Schedule schedule)
            {
                ScheduleId = schedule.getID();
                Name = schedule.getName();
                BackupSetId = schedule.getAssignedBackupSets();
                Active = schedule.isActive();
                AdminOnly = schedule.isAdministratorsOnly();
                CPUThrottle = schedule.getBackupCPUThrottle();
                ConcurrentBackupSets = schedule.getConcurrentBackupSets();
                NetworkDetection = schedule.isUsingNetworkDetection();
                ShortName = schedule.getShortName();
            }

            public override string ToString()
            {
                return Name;
            }
        }

        protected class DSClientScheduleDetail
        {
            private readonly EScheduleDetailType _detailType;
            private readonly int _tasks;
            public int DetailId { get; private set; }
            public int ScheduleId { get; private set; }            
            public string ScheduleName { get; private set; }
            public dynamic Type { get; private set; }
            public TimeInDay StartTime { get; private set; }
            public TimeInDay EndTime { get; private set; }
            public DateTime StartDate { get; private set; }
            public DateTime EndDate { get; private set; }
            public bool EndTimeEnabled { get; private set; }
            public bool Excluded { get; private set; }
            public DSClientScheduleTasks EnabledTasks { get; private set; }
            public DSClientValidationScheduleOptions ValidationOptions { get; private set; }
            public DSClientBLMScheduleOptions BLMScheduleOptions { get; private set; }

            public DSClientScheduleDetail(int detailId, schedule_info scheduleInfo, ScheduleDetail schedule)
            {
                _detailType = schedule.getType();
                _tasks = schedule.getTasks();

                DetailId = detailId;
                ScheduleId = scheduleInfo.id;
                ScheduleName = scheduleInfo.name;
                
                switch (_detailType)
                {
                    case EScheduleDetailType.EScheduleDetailType__OneTime:
                        Type = new OneTimeScheduleType(schedule);
                        break;
                    case EScheduleDetailType.EScheduleDetailType__Daily:
                        Type = new DailyScheduleType(schedule);
                        break;
                    case EScheduleDetailType.EScheduleDetailType__Weekly:
                        Type = new WeeklyScheduleType(schedule);
                        break;
                    case EScheduleDetailType.EScheduleDetailType__Monthly:
                        Type = new MonthlyScheduleType(schedule);
                        break;
                }

                StartTime = new TimeInDay(schedule.getStartTime());
                EndTime = new TimeInDay(schedule.getEndTime());
                StartDate = UnixEpochToDateTime(schedule.getPeriodStartDate());
                EndDate = UnixEpochToDateTime(schedule.getPeriodEndDate());
                EndTimeEnabled = schedule.hasEndTime();
                Excluded = schedule.isExcluded();
                EnabledTasks = new DSClientScheduleTasks(_tasks);
                ValidationOptions = new DSClientValidationScheduleOptions(schedule.getValidationOptions());
                BLMScheduleOptions = new DSClientBLMScheduleOptions(schedule.getBLMOptions());
            }
        }

        protected class OneTimeScheduleType
        {
            public DateTime StartDate { get; private set; }

            public OneTimeScheduleType(ScheduleDetail schedule)
            {
                OneTimeScheduleDetail detail = OneTimeScheduleDetail.from(schedule);

                StartDate = UnixEpochToDateTime(detail.get_start_date());
            }

            public override string ToString()
            {
                return "OneTime";
            }
        }

        protected class DailyScheduleType
        {
            public int RepeatDays { get; private set; }

            public DailyScheduleType(ScheduleDetail schedule)
            {
                DailyScheduleDetail detail = DailyScheduleDetail.from(schedule);

                RepeatDays = detail.getRepeatDays();
            }

            public override string ToString()
            {
                return "Daily";
            }
        }

        protected class WeeklyScheduleType
        {
            public int RepeatWeeks { get; private set; }
            public string[] ScheduleDays { get; private set; }

            public WeeklyScheduleType(ScheduleDetail schedule)
            {
                WeeklyScheduleDetail detail = WeeklyScheduleDetail.from(schedule);

                RepeatWeeks = detail.getRepeatWeeks();
                ScheduleDays = EScheduleWeekDaysIntToArray(detail.getScheduleDays());
            }

            public override string ToString()
            {
                return "Weekly";
            }
        }

        protected class MonthlyScheduleType
        {
            public int RepeatMonths { get; private set; }
            public int ScheduleDay { get; private set; }
            public string MonthlyStartDay { get; private set; }

            public MonthlyScheduleType(ScheduleDetail schedule)
            {
                MonthlyScheduleDetail detail = MonthlyScheduleDetail.from(schedule);

                RepeatMonths = detail.getRepeatMonths();
                ScheduleDay = detail.getScheduleDay();
                MonthlyStartDay = (detail.getScheduleWhen() == EScheduleMonthlyStartDay.EScheduleMonthlyStartDay__Thrusday) ? "Thursday" : EnumToString(detail.getScheduleWhen());
            }

            public override string ToString()
            {
                return "Monthly";
            }
        }

        protected class DSClientScheduleTasks
        {
            /* The AsigraDSClientApi ScheduleDetail getTasks() method returns 0 for the Replication setting
             * Thus it is impossible to determine if the "Perform Replication" task is Enabled or Disabled */
            private readonly string _replication = "CheckGUI";

            private int _numTasks = 0;

            public bool Backup { get; private set; }
            public bool Retention { get; private set; }
            public bool Validation { get; private set; }
            public bool BLM { get; private set; }
            public bool LANScan { get; private set; }
            public bool CleanTrash { get; private set; }
            public string Replication 
            {
                get { return _replication; }
            }

            public DSClientScheduleTasks(int tasks)
            {
                if ((tasks & (int)ETaskToRun.ETaskToRun__Backup) > 0)
                {
                    Backup = true;
                    _numTasks += 1;
                }

                if ((tasks & (int)ETaskToRun.ETaskToRun__Retention) > 0)
                {
                    Retention = true;
                    _numTasks += 1;
                }

                if ((tasks & (int)ETaskToRun.ETaskToRun__Validation) > 0)
                {
                    Validation = true;
                    _numTasks += 1;
                }

                if ((tasks & (int)ETaskToRun.ETaskToRun__BLM) > 0)
                {
                    BLM = true;
                    _numTasks += 1;
                }

                if ((tasks & (int)ETaskToRun.ETaskToRun__LANScan) > 0)
                {
                    LANScan = true;
                    _numTasks += 1;
                }

                if ((tasks & (int)ETaskToRun.ETaskToRun__CleanTrash) > 0)
                {
                    CleanTrash = true;
                    _numTasks += 1;
                }
            }

            public override string ToString()
            {
                return _numTasks.ToString();
            }
        }

        protected class DSClientBLMScheduleOptions
        {
            private bool _optionsSet;
            public bool IncludeAllGenerations { get; private set; }

            public bool BackReference { get; private set; }

            public string PackageClosing { get; private set; }

            public DSClientBLMScheduleOptions(blm_schedule_options blmOptions)
            {
                IncludeAllGenerations = blmOptions.include_all_generations;
                BackReference = blmOptions.use_back_reference;
                PackageClosing = EnumToString(blmOptions.package_close);

                if ((IncludeAllGenerations || BackReference) == true || PackageClosing != null || PackageClosing != "Undefined")
                    _optionsSet = true;
            }

            public override string ToString()
            {
                string boolString = "False";

                if (_optionsSet == true)
                    boolString = "True";

                return boolString;
            }
        }

        protected class DSClientValidationScheduleOptions
        {
            public bool LastGenOnly { get; private set; } = false;
            public bool ExcludeDeleted { get; private set; } = false;
            public bool Resume { get; private set; } = false;

            public DSClientValidationScheduleOptions(int optionValue)
            {
                if ((optionValue & (int)EScheduleValidationOption.EScheduleValidationOption__LastGen) > 0)
                    LastGenOnly = true;

                if ((optionValue & (int)EScheduleValidationOption.EScheduleValidationOption__ExcludeDeletedFiles) > 0)
                    ExcludeDeleted = true;

                if ((optionValue & (int)EScheduleValidationOption.EScheduleValidationOption__Resume) > 0)
                    Resume = true;
            }

            public override string ToString()
            {
                List<string> selectedOptions = new List<string>();

                if (LastGenOnly)
                    selectedOptions.Add("LastGenOnly");

                if (ExcludeDeleted)
                    selectedOptions.Add("ExcludeDeleted");

                if (Resume)
                    selectedOptions.Add("Resume");

                string options = null;
                for (int i = 0; i < selectedOptions.Count(); i++)
                {
                    options += selectedOptions[i];
                    if (i < selectedOptions.Count()-1)
                        options += ",";
                }

                return options;
            }
        }

        public static string[] EScheduleWeekDaysIntToArray(int weekDays)
        {
            List<string> schedDays = new List<string>();
            foreach (EScheduleWeekDays weekday in Enum.GetValues(typeof(EScheduleWeekDays)))
                if ((weekDays & (int)weekday) > 0)
                {
                    string strWeekday = (weekday == EScheduleWeekDays.EScheduleWeekDays__Thrusday) ? "Thursday" : EnumToString(weekday); // Required due to spelling mistake in enum
                    schedDays.Add(EnumToString(strWeekday));
                }

            return schedDays.ToArray();
        }

        protected static string ScheduleDetailHash(Schedule schedule, ScheduleDetail detail)
        {
            EScheduleDetailType type = detail.getType();

            string strId = schedule.getID().ToString();
            string strName = schedule.getName();
            string strType = type.ToString();
            string strStart = new TimeInDay(detail.getStartTime()).ToString();
            string strEnd = new TimeInDay(detail.getEndTime()).ToString();
            string strTasks = detail.getTasks().ToString();
            string strPeriodStart = detail.getPeriodStartDate().ToString();
            string strPeriodEnd = detail.getPeriodEndDate().ToString();

            string strToHash = strId + strName + strType + strStart + strEnd + strTasks + strPeriodStart + strPeriodEnd;

            switch (type)
            {
                case EScheduleDetailType.EScheduleDetailType__OneTime:
                    OneTimeScheduleDetail oneTime = OneTimeScheduleDetail.from(detail);
                    strToHash += oneTime.get_start_date().ToString();
                    break;
                case EScheduleDetailType.EScheduleDetailType__Daily:
                    DailyScheduleDetail daily = DailyScheduleDetail.from(detail);
                    strToHash += daily.getRepeatDays().ToString();
                    break;
                case EScheduleDetailType.EScheduleDetailType__Weekly:
                    WeeklyScheduleDetail weekly = WeeklyScheduleDetail.from(detail);
                    strToHash += weekly.getRepeatWeeks().ToString() + weekly.getScheduleDays().ToString();
                    break;
                case EScheduleDetailType.EScheduleDetailType__Monthly:
                    MonthlyScheduleDetail monthly = MonthlyScheduleDetail.from(detail);
                    strToHash += monthly.getRepeatMonths().ToString() + monthly.getScheduleDay().ToString() + monthly.getScheduleWhen().ToString();
                    break;
            }

            SHA1Managed sha1 = new SHA1Managed();

            byte[] hashData = sha1.ComputeHash(Encoding.UTF8.GetBytes(strToHash));

            sha1.Dispose();

            StringBuilder strBuilder = new StringBuilder();

            for (int i = 0; i < hashData.Length; i++)
                strBuilder.Append(hashData[i].ToString("x2"));

            return strBuilder.ToString();
        }

        protected static ScheduleDetail SelectScheduleDetail(Schedule schedule, int detailId, Dictionary<string, int> detailHashes)
        {
            ScheduleDetail scheduleDetail = null;

            // Get all the Schedule Details for this Schedule
            ScheduleDetail[] details = schedule.getDetails();

            foreach (ScheduleDetail detail in details)
            {
                string detailHash = ScheduleDetailHash(schedule, detail);

                detailHashes.TryGetValue(detailHash, out int id);

                if (id == detailId)
                    return detail;

                detail.Dispose();
            }

            return scheduleDetail;
        }
    }
}