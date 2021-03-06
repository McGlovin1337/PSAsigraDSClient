﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public class DSClientCommon
    {
        public static string EnumToString<T>(T e)
        {
            string result = Regex.Split(e.ToString(), "__")
                                    .Last();

            return result;
        }

        public static T StringToEnum<T>(string s)
        {
            string enumType = (typeof(T).ToString())
                                        .Split('.')
                                        .Last();
            T result = (T)Enum.Parse(typeof(T), $"{enumType}__{s}", true);

            return result;
        }

        public class DSClientOSType
        {
            public string OsType { get; private set; }

            public DSClientOSType(EOSFlavour osType)
            {
                OsType = EnumToString(osType);
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
                string hour = (Hour < 10 && Hour >= 0) ? $"0{Hour}" : Hour.ToString();
                string minute = (Minute < 10 && Minute >= 0) ? $"0{Minute}" : Minute.ToString();
                string second = (Second < 10 && Second >= 0) ? $"0{Second}" : Second.ToString();

                return $"{hour}:{minute}:{second}";
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

            time_in_day TimeInDay = new time_in_day
            {
                hour = Hour,
                minute = (Minute > 59) ? 0 : Minute,
                second = (Second > 59) ? 0 : Second
            };

            return TimeInDay;
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
                return Regex.Match(Hostname, @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$").Success;
            }
        }
    }
}