using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public class DSClientCommon
    {
        public class DSClientOSType
        {
            public string OsType { get; private set; }

            public DSClientOSType(EOSFlavour osType)
            {
                switch(osType)
                {
                    case EOSFlavour.EOSFlavour__Windows:
                        OsType = "Windows";
                        break;
                    case EOSFlavour.EOSFlavour__Linux:
                        OsType = "Linux";
                        break;
                    case EOSFlavour.EOSFlavour__Mac:
                        OsType = "Mac";
                        break;
                    case EOSFlavour.EOSFlavour__UNDEFINED:
                        OsType = "Undefined";
                        break;
                }
            }
        }

        public class TimeInDay
        {
            public int Hour { get; set; }
            public int Minute { get; set; }
            public int Second { get; set; }

            public TimeInDay(time_in_day timeInDay)
            {
                Hour = timeInDay.hour;
                Minute = timeInDay.minute;
                Second = timeInDay.second;
            }

            public override string ToString()
            {
                return Hour + ":" + Minute + ":" + Second;
            }
        }

        public static time_in_day StringTotime_in_day(string timeInDay)
        {
            string[] splitTime = timeInDay.Split(':');

            if (splitTime == null || splitTime.Count() == 0)
                throw new Exception("time_in_day cannot be null or empty");

            int Hour = Convert.ToInt32(splitTime[0]);
            int Minute = 0;
            int Second = 0;

            if (splitTime.Count() > 1)
                Minute = Convert.ToInt32(splitTime[1]);

            if (splitTime.Count() > 2)
                Second = Convert.ToInt32(splitTime[2]);

            time_in_day TimeInDay = new time_in_day();
            TimeInDay.hour = Hour;
            TimeInDay.minute = Minute;
            TimeInDay.second = Second;

            if (TimeInDay.minute > 59)
                TimeInDay.minute = 0;

            if (TimeInDay.second > 59)
                TimeInDay.second = 0;

            return TimeInDay;
        }

        public static EWeekDay StringToEWeekDay(string weekDay)
        {
            switch (weekDay.ToLower())
            {
                case "monday":
                    return EWeekDay.EWeekDay__Monday;
                case "tuesday":
                    return EWeekDay.EWeekDay__Tuesday;
                case "wednesday":
                    return EWeekDay.EWeekDay__Wednesday;
                case "thursday":
                    return EWeekDay.EWeekDay__Thursday;
                case "friday":
                    return EWeekDay.EWeekDay__Friday;
                case "saturday":
                    return EWeekDay.EWeekDay__Saturday;
                case "sunday":
                    return EWeekDay.EWeekDay__Sunday;
                default:
                    return EWeekDay.EWeekDay__UNDEFINED;
            }
        }

        public static string EWeekDayToString(EWeekDay weekDay)
        {
            switch(weekDay)
            {
                case EWeekDay.EWeekDay__Monday:
                    return "Monday";
                case EWeekDay.EWeekDay__Tuesday:
                    return "Tuesday";
                case EWeekDay.EWeekDay__Wednesday:
                    return "Wednesday";
                case EWeekDay.EWeekDay__Thursday:
                    return "Thursday";
                case EWeekDay.EWeekDay__Friday:
                    return "Friday";
                case EWeekDay.EWeekDay__Saturday:
                    return "Saturday";
                case EWeekDay.EWeekDay__Sunday:
                    return "Sunday";
                default:
                    return null;
            }
        }

        public static string EMonthToString(EMonth month)
        {
            switch(month)
            {
                case EMonth.EMonth__January:
                    return "January";
                case EMonth.EMonth__February:
                    return "February";
                case EMonth.EMonth__March:
                    return "March";
                case EMonth.EMonth__April:
                    return "April";
                case EMonth.EMonth__May:
                    return "May";
                case EMonth.EMonth__June:
                    return "June";
                case EMonth.EMonth__July:
                    return "July";
                case EMonth.EMonth__August:
                    return "August";
                case EMonth.EMonth__September:
                    return "September";
                case EMonth.EMonth__October:
                    return "October";
                case EMonth.EMonth__November:
                    return "November";
                case EMonth.EMonth__December:
                    return "December";
                default:
                    return null;
            }
        }

        public static DateTime UnixEpochToDateTime(int epoch)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(epoch);

            DateTime dateTime = dateTimeOffset.LocalDateTime;

            return dateTime;
        }

        public static int DateTimeToUnixEpoch(DateTime dateTime)
        {
            DateTimeOffset offset = new DateTimeOffset(dateTime);
            int epoch = (int)offset.ToUnixTimeSeconds();

            return epoch;
        }

        public class ValidateHostname
        {
            public string ValidHostnames { get; set; }
            public string InvalidHostnames { get; set; }

            public static IEnumerable<ValidateHostname> ValidateHostnames(string[] Hostnames)
            {
                List<ValidateHostname> validatedHostnames = new List<ValidateHostname>();
                bool ValidateResult;

                foreach (string Hostname in Hostnames)
                {
                    ValidateResult = ValidateHost(Hostname);
                    if (ValidateResult == true)
                        validatedHostnames.Add(new ValidateHostname { ValidHostnames = Hostname });
                    else
                        validatedHostnames.Add(new ValidateHostname { InvalidHostnames = Hostname });
                }

                return validatedHostnames;
            }

            public static bool ValidateHost(string Hostname)
            {
                var Validated = Regex.Match(Hostname, @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$");
                if (Validated.Success) return true;
                return false;
            }
        }

        public static string ECompressionTypeToString(ECompressionType compressionType)
        {
            string compressType = null;

            switch (compressionType)
            {
                case ECompressionType.ECompressionType__NONE:
                    compressType = "None";
                    break;
                case ECompressionType.ECompressionType__ZLIB:
                    compressType = "ZLIB";
                    break;
                case ECompressionType.ECompressionType__LZOP:
                    compressType = "LZOP";
                    break;
                case ECompressionType.ECompressionType__ZLIB_LO:
                    compressType = "ZLIB_LO";
                    break;
                case ECompressionType.ECompressionType__ZLIB_MED:
                    compressType = "ZLIB_MED";
                    break;
                case ECompressionType.ECompressionType__ZLIB_HI:
                    compressType = "ZLIB_HI";
                    break;
                case ECompressionType.ECompressionType__UNDEFINED:
                    compressType = "Undefined";
                    break;
            }

            return compressType;
        }

        public static ECompressionType StringToECompressionType(string compressionType)
        {
            switch(compressionType.ToLower())
            {
                case "none":
                    return ECompressionType.ECompressionType__NONE;
                case "zlib":
                    return ECompressionType.ECompressionType__ZLIB;
                case "lzop":
                    return ECompressionType.ECompressionType__LZOP;
                case "zlib_lo":
                    return ECompressionType.ECompressionType__ZLIB_LO;
                case "zlib_med":
                    return ECompressionType.ECompressionType__ZLIB_MED;
                case "zlib_hi":
                    return ECompressionType.ECompressionType__ZLIB_HI;
                default:
                    return ECompressionType.ECompressionType__UNDEFINED;
            }
        }

        public static string ETimeUnitToString(ETimeUnit timeUnit)
        {
            switch(timeUnit)
            {
                case ETimeUnit.ETimeUnit__Seconds:
                    return "Seconds";
                case ETimeUnit.ETimeUnit__Minutes:
                    return "Minutes";
                case ETimeUnit.ETimeUnit__Hours:
                    return "Hours";
                case ETimeUnit.ETimeUnit__Days:
                    return "Days";
                case ETimeUnit.ETimeUnit__Weeks:
                    return "Weeks";
                case ETimeUnit.ETimeUnit__Months:
                    return "Months";
                case ETimeUnit.ETimeUnit__Years:
                    return "Years";
                default:
                    return null;
            }
        }

        public static ETimeUnit StringToETimeUnit(string timeUnit)
        {
            switch(timeUnit.ToLower())
            {
                case "seconds":
                    return ETimeUnit.ETimeUnit__Seconds;
                case "minutes":
                    return ETimeUnit.ETimeUnit__Minutes;
                case "hours":
                    return ETimeUnit.ETimeUnit__Hours;
                case "days":
                    return ETimeUnit.ETimeUnit__Days;
                case "weeks":
                    return ETimeUnit.ETimeUnit__Weeks;
                case "months":
                    return ETimeUnit.ETimeUnit__Months;
                case "years":
                    return ETimeUnit.ETimeUnit__Years;
                default:
                    return ETimeUnit.ETimeUnit__UNDEFINED;
            }
        }
    }
}