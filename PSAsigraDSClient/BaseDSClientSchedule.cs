﻿using System;
using System.Collections.Generic;
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
            public int DetailId { get; set; }
            public int ScheduleId { get; set; }            
            public string ScheduleName { get; set; }
            public dynamic Type { get; set; }
            public TimeInDay StartTime { get; set; }
            public TimeInDay EndTime { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public bool EndTimeEnabled { get; set; }
            public bool Excluded { get; set; }
            public DSClientScheduleTasks EnabledTasks { get; set; }
            public DSClientValidationScheduleOptions ValidationOptions { get; set; }
            public DSClientBLMScheduleOptions BLMScheduleOptions { get; set; }
        }

        protected class OneTimeScheduleType
        {
            public DateTime StartDate { get; private set; }

            public OneTimeScheduleType(int startDate)
            {
                StartDate = UnixEpochToDateTime(startDate);
            }

            public override string ToString()
            {
                return "OneTime";
            }
        }

        protected class DailyScheduleType
        {
            public int RepeatDays { get; private set; }

            public DailyScheduleType(int days)
            {
                RepeatDays = days;
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

            public WeeklyScheduleType(int repeat, int days)
            {
                RepeatWeeks = repeat;
                ScheduleDays = EScheduleWeekDaysIntToArray(days);
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

            public MonthlyScheduleType(int repeat, int schedDay, EScheduleMonthlyStartDay startDay)
            {
                RepeatMonths = repeat;
                ScheduleDay = schedDay;
                MonthlyStartDay = (startDay == EScheduleMonthlyStartDay.EScheduleMonthlyStartDay__Thrusday) ? "Thursday" : EnumToString(startDay);
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
            private int _optionsSet = 0;
            public bool LastGenOnly { get; private set; } = false;
            public bool ExcludeDeleted { get; private set; } = false;
            public bool Resume { get; private set; } = false;

            public DSClientValidationScheduleOptions(int optionValue)
            {
                if ((optionValue & (int)EScheduleValidationOption.EScheduleValidationOption__LastGen) > 0)
                {
                    LastGenOnly = true;
                    _optionsSet += 1;
                }

                if ((optionValue & (int)EScheduleValidationOption.EScheduleValidationOption__ExcludeDeletedFiles) > 0)
                {
                    ExcludeDeleted = true;
                    _optionsSet += 1;
                }

                if ((optionValue & (int)EScheduleValidationOption.EScheduleValidationOption__Resume) > 0)
                {
                    Resume = true;
                    _optionsSet += 1;
                }
            }

            public override string ToString()
            {
                return _optionsSet.ToString();
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
    }
}