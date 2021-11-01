using System;

namespace PSAsigraDSClient
{
    public class DSClientStorageUnit
    {
        public long Bytes { get; private set; }
        public decimal Kilobytes { get; private set; }
        public decimal Megabytes { get; private set; }
        public decimal Gigabytes { get; private set; }
        public decimal Terrabytes { get; private set; }
        public decimal Petabytes { get; private set; }

        public DSClientStorageUnit(long bytes)
        {
            Bytes = bytes;
            Kilobytes = bytes / 1024;
            Megabytes = Kilobytes / 1024;
            Gigabytes = Megabytes / 1024;
            Terrabytes = Gigabytes / 1024;
            Petabytes = Terrabytes / 1024;
        }

        public override string ToString()
        {
            if (Petabytes >= 1)
                return $"{Math.Round(Petabytes, 2)} PB";
            else if (Terrabytes >= 1)
                return $"{Math.Round(Terrabytes, 2)} TB";
            else if (Gigabytes >= 1)
                return $"{Math.Round(Gigabytes, 2)} GB";
            else if (Megabytes >= 1)
                return $"{Math.Round(Megabytes, 2)} MB";
            else if (Kilobytes >= 1)
                return $"{Math.Round(Kilobytes, 2)} KB";
            else
                return $"{Bytes} B";
        }
    }
}