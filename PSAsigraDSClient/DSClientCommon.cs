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
    }
}