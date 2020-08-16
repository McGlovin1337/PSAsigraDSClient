using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using AsigraDSClientApi;
using System.Runtime.CompilerServices;
using System.Dynamic;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientSchedule: DSClientCmdlet
    {
        protected abstract void ProcessScheduleDetail();

        protected override void DSClientProcessRecord()
        {
            ProcessScheduleDetail();
        }

        protected class DSClientScheduleInfo
        {
            public bool Active { get; set; }
            public bool AdminOnly { get; set; }
            public int CPUThrottle { get; set; }
            public int ConcurrentBackupSets { get; set; }
            public int ScheduleId { get; set; }
            public string Name { get; set; }
            public string ShortName { get; set; }
            public bool NetworkDetection { get; set; }
        }

        protected class DSClientScheduleDetail
        {
            public int ScheduleId { get; set; }
            public int DetailId { get; set; }
            public string Name { get; set; }
            public dynamic Type { get; set; }
            public DSClientCommon.TimeInDay StartTime { get; set; }
            public DSClientCommon.TimeInDay EndTime { get; set; }
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
            public DateTime StartDate { get; set; }

            public OneTimeScheduleType(int startDate)
            {
                StartDate = DSClientCommon.UnixEpochToDateTime(startDate);
            }

            public override string ToString()
            {
                return "OneTime";
            }
        }

        protected class DailyScheduleType
        {
            public int RepeatDays { get; set; }

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
            public int RepeatWeeks { get; set; }
            public string[] ScheduleDays { get; set; }

            public WeeklyScheduleType(int repeat, int days)
            {
                RepeatWeeks = repeat;

                List<string> schedDays = new List<string>();
                if ((days & (int)EScheduleWeekDays.EScheduleWeekDays__Monday) > 0)
                    schedDays.Add("Mon");
                if ((days & (int)EScheduleWeekDays.EScheduleWeekDays__Tuesday) > 0)
                    schedDays.Add("Tue");
                if ((days & (int)EScheduleWeekDays.EScheduleWeekDays__Wednesday) > 0)
                    schedDays.Add("Wed");
                if ((days & (int)EScheduleWeekDays.EScheduleWeekDays__Thrusday) > 0)
                    schedDays.Add("Thu");
                if ((days & (int)EScheduleWeekDays.EScheduleWeekDays__Friday) > 0)
                    schedDays.Add("Fri");
                if ((days & (int)EScheduleWeekDays.EScheduleWeekDays__Saturday) > 0)
                    schedDays.Add("Sat");
                if ((days & (int)EScheduleWeekDays.EScheduleWeekDays__Sunday) > 0)
                    schedDays.Add("Sun");

                ScheduleDays = schedDays.ToArray();
            }

            public override string ToString()
            {
                return "Weekly";
            }
        }

        protected class MonthlyScheduleType
        {
            public int RepeatMonths { get; set; }
            public int ScheduleDay { get; set; }
            public string MonthlyStartDay { get; set; }

            public MonthlyScheduleType(int repeat, int schedDay, EScheduleMonthlyStartDay startDay)
            {
                RepeatMonths = repeat;
                ScheduleDay = schedDay;

                switch (startDay)
                {
                    case EScheduleMonthlyStartDay.EScheduleMonthlyStartDay__DayOfMonth:
                        MonthlyStartDay = "DayOfMonth";
                        break;
                    case EScheduleMonthlyStartDay.EScheduleMonthlyStartDay__Monday:
                        MonthlyStartDay = "Monday";
                        break;
                    case EScheduleMonthlyStartDay.EScheduleMonthlyStartDay__Tuesday:
                        MonthlyStartDay = "Tuesday";
                        break;
                    case EScheduleMonthlyStartDay.EScheduleMonthlyStartDay__Wednesday:
                        MonthlyStartDay = "Wednesday";
                        break;
                    case EScheduleMonthlyStartDay.EScheduleMonthlyStartDay__Thrusday:
                        MonthlyStartDay = "Thursday";
                        break;
                    case EScheduleMonthlyStartDay.EScheduleMonthlyStartDay__Friday:
                        MonthlyStartDay = "Friday";
                        break;
                    case EScheduleMonthlyStartDay.EScheduleMonthlyStartDay__Saturday:
                        MonthlyStartDay = "Saturday";
                        break;
                    case EScheduleMonthlyStartDay.EScheduleMonthlyStartDay__Sunday:
                        MonthlyStartDay = "Sunday";
                        break;
                    case EScheduleMonthlyStartDay.EScheduleMonthlyStartDay__Day:
                        MonthlyStartDay = "Day";
                        break;
                    case EScheduleMonthlyStartDay.EScheduleMonthlyStartDay__WeekDay:
                        MonthlyStartDay = "WeekDay";
                        break;
                    case EScheduleMonthlyStartDay.EScheduleMonthlyStartDay__WeekEndDay:
                        MonthlyStartDay = "WeekEndDay";
                        break;
                }
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

            public bool Backup { get; set; }
            public bool Retention { get; set; }
            public bool Validation { get; set; }
            public bool BLM { get; set; }
            public bool LANScan { get; set; }
            public bool CleanTrash { get; set; }
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
                PackageClosing = PackageCloseConvert(blmOptions.package_close);

                if ((IncludeAllGenerations || BackReference) == true || PackageClosing != null || PackageClosing != "Undefined")
                    _optionsSet = true;
            }

            // Convert the EActivePackageClosing Enum to a Human Friendly String
            private string PackageCloseConvert(EActivePackageClosing packageClose)
            {
                string PackageClose = null;

                switch(packageClose)
                {
                    case EActivePackageClosing.EActivePackageClosing__DoNotClose:
                        PackageClose = "DoNotClose";
                        break;
                    case EActivePackageClosing.EActivePackageClosing__CloseAtStart:
                        PackageClose = "AtStart";
                        break;
                    case EActivePackageClosing.EActivePackageClosing__CloseAtEnd:
                        PackageClose = "AtEnd";
                        break;
                    /*case EActivePackageClosing.EActivePackageClosing__UNDEFINED:
                        PackageClose = "Undefined";
                        break;*/
                }

                return PackageClose;
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
    }
}