using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public class DSClientCommon
    {
        public class DSClientOSType
        {
            public string OsType { get; set; }

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

        public static string EWeekDayToString(EWeekDay weekDay)
        {
            string WeekDay = null;

            switch(weekDay)
            {
                case EWeekDay.EWeekDay__Monday:
                    WeekDay = "Monday";
                    break;
                case EWeekDay.EWeekDay__Tuesday:
                    WeekDay = "Tuesday";
                    break;
                case EWeekDay.EWeekDay__Wednesday:
                    WeekDay = "Wednesday";
                    break;
                case EWeekDay.EWeekDay__Thursday:
                    WeekDay = "Thursday";
                    break;
                case EWeekDay.EWeekDay__Friday:
                    WeekDay = "Friday";
                    break;
                case EWeekDay.EWeekDay__Saturday:
                    WeekDay = "Saturday";
                    break;
                case EWeekDay.EWeekDay__Sunday:
                    WeekDay = "Sunday";
                    break;
            }

            return WeekDay;
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
            ECompressionType eCompressionType = ECompressionType.ECompressionType__UNDEFINED;

            switch(compressionType)
            {
                case "NONE":
                    eCompressionType = ECompressionType.ECompressionType__NONE;
                    break;
                case "ZLIB":
                    eCompressionType = ECompressionType.ECompressionType__ZLIB;
                    break;
                case "LZOP":
                    eCompressionType = ECompressionType.ECompressionType__LZOP;
                    break;
                case "ZLIB_LO":
                    eCompressionType = ECompressionType.ECompressionType__ZLIB_LO;
                    break;
                case "ZLIB_MED":
                    eCompressionType = ECompressionType.ECompressionType__ZLIB_MED;
                    break;
                case "ZLIB_HI":
                    eCompressionType = ECompressionType.ECompressionType__ZLIB_HI;
                    break;
                default:
                    eCompressionType = ECompressionType.ECompressionType__UNDEFINED;
                    break;
            }

            return eCompressionType;
        }

        public static string ETimeUnitToString(ETimeUnit timeUnit)
        {
            string TimeUnit = null;

            switch(timeUnit)
            {
                case ETimeUnit.ETimeUnit__Seconds:
                    TimeUnit = "Seconds";
                    break;
                case ETimeUnit.ETimeUnit__Minutes:
                    TimeUnit = "Minutes";
                    break;
                case ETimeUnit.ETimeUnit__Hours:
                    TimeUnit = "Hours";
                    break;
                case ETimeUnit.ETimeUnit__Days:
                    TimeUnit = "Days";
                    break;
                case ETimeUnit.ETimeUnit__Weeks:
                    TimeUnit = "Weeks";
                    break;
                case ETimeUnit.ETimeUnit__Months:
                    TimeUnit = "Months";
                    break;
                case ETimeUnit.ETimeUnit__Years:
                    TimeUnit = "Years";
                    break;
            }

            return TimeUnit;
        }

        public static ETimeUnit StringToETimeUnit(string timeUnit)
        {
            ETimeUnit TimeUnit = ETimeUnit.ETimeUnit__UNDEFINED;

            switch(timeUnit)
            {
                case "Seconds":
                    TimeUnit = ETimeUnit.ETimeUnit__Seconds;
                    break;
                case "Minutes":
                    TimeUnit = ETimeUnit.ETimeUnit__Minutes;
                    break;
                case "Hours":
                    TimeUnit = ETimeUnit.ETimeUnit__Hours;
                    break;
                case "Days":
                    TimeUnit = ETimeUnit.ETimeUnit__Days;
                    break;
                case "Weeks":
                    TimeUnit = ETimeUnit.ETimeUnit__Weeks;
                    break;
                case "Months":
                    TimeUnit = ETimeUnit.ETimeUnit__Months;
                    break;
                case "Years":
                    TimeUnit = ETimeUnit.ETimeUnit__Years;
                    break;
            }

            return TimeUnit;
        }
    }
}