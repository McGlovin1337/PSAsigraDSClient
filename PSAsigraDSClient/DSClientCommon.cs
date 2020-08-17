using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public class DSClientCommon
    {
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
            long lepoch = offset.ToUnixTimeSeconds();
            int epoch = (int)offset.ToUnixTimeSeconds();

            return epoch;
        }
    }
}