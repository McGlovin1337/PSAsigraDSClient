using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public static class DSClientCommon
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

        public class GenericBackupSetActivity
        {
            public int ActivityId { get; private set; }
            public string Type { get; private set; }
            public int BackupSetId { get; private set; }
            public DateTime StartTime { get; private set; }

            public GenericBackupSetActivity(GenericActivity genericActivity)
            {
                running_activity_info activityInfo = genericActivity.getCurrentStatus();

                ActivityId = activityInfo.activity_id;
                Type = EnumToString(activityInfo.type);
                BackupSetId = activityInfo.set_id;
                StartTime = UnixEpochToDateTime(activityInfo.start_time);
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

        public class DSClientTimeSpan
        {
            public int Period { get; private set; }
            public string Unit { get; private set; }

            public DSClientTimeSpan(int period, string unit)
            {
                Period = period;
                Unit = unit;
            }

            public DSClientTimeSpan(retention_time_span timeSpan)
            {
                Period = timeSpan.period;
                Unit = EnumToString(timeSpan.unit);
            }

            public override string ToString()
            {
                return $"{Period} {Unit}";
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

        public static string ResolveWinComputer(string computer)
        {
            string result = computer.Split('\\').Last();

            result = result.Trim('\\');

            if (result.ToLower() == "ds-client computer" ||
                computer.Split('\\').First().ToLower() == "ds-client computer")
            {
                return "DS-Client Computer";
            }
            else
            {
                if (ValidateHostname.ValidateHost(result))
                    return $@"Microsoft Windows Network\{result}";
            }

            Console.WriteLine($"Returning computer: {computer}");
            return computer; // If all fails, just return the original string
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

        public static string GetSha1Hash(this string hashStr)
        {
            SHA1Managed sha1 = new SHA1Managed();

            byte[] hashData = sha1.ComputeHash(Encoding.UTF8.GetBytes(hashStr));

            sha1.Dispose();

            StringBuilder strBuilder = new StringBuilder();

            for (int i = 0; i < hashData.Length; i++)
                strBuilder.Append(hashData[i].ToString("x2"));

            return strBuilder.ToString();
        }


    }
}