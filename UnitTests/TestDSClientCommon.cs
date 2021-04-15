using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using AsigraDSClientApi;
using PSAsigraDSClient;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class TestDSClientCommon
    {
        [TestMethod]
        public void TestEnumToString()
        {
            // This Tests the EnumToString<T>(T e) Method to check it returns the expected string value of an AsigraDSClientApi Enum
            Assert.AreEqual("LZOP", DSClientCommon.EnumToString(ECompressionType.ECompressionType__LZOP));
        }

        [TestMethod]
        public void TestStringToEnum()
        {
            // This Tests the StringToEnum<T>(string s) Method to check it returns the expected Enum
            Assert.AreEqual(ECompressionType.ECompressionType__ZLIB, DSClientCommon.StringToEnum<ECompressionType>("ZLIB"));
        }

        [TestMethod]
        public void TestDSClientOSType()
        {
            // This Tests the DSClientOSType Class and checks the OsType Property returns the expected String
            DSClientCommon.DSClientOSType osType = new DSClientCommon.DSClientOSType(EOSFlavour.EOSFlavour__Windows);

            Assert.AreEqual("Windows", osType.OsType);
        }

        [TestMethod]
        public void TestTimeInDay()
        {
            // This Tests the TimeInDay Class and checks the Properties and ToString() Override returns the expected values
            time_in_day apiTimeInDay = new time_in_day
            {
                hour = 23,
                minute = 30,
                second = 45
            };

            DSClientCommon.TimeInDay timeInDay = new DSClientCommon.TimeInDay(apiTimeInDay);

            Assert.AreEqual(23, timeInDay.Hour);
            Assert.AreEqual(30, timeInDay.Minute);
            Assert.AreEqual(45, timeInDay.Second);
            Assert.AreEqual("23:30:45", timeInDay.ToString());
        }

        [TestMethod]
        public void TestStringTo_time_in_day()
        {
            // This Tests the StringTo_time_in_day method for the expected time_in_day class
            time_in_day timeInDay = DSClientCommon.StringTotime_in_day("19:15:30");

            Assert.AreEqual(19, timeInDay.hour);
            Assert.AreEqual(15, timeInDay.minute);
            Assert.AreEqual(30, timeInDay.second);
        }

        [TestMethod]
        public void TestUnixEpochToDateTime()
        {
            // This Tests the UnixEpochToDateTime method returns the expected DateTime object
            DateTime dateTime = DSClientCommon.UnixEpochToDateTime(1623758400);
            DateTime expectedDateTime = DateTime.Parse("15/06/2021 13:00:00");

            Assert.AreEqual(expectedDateTime, dateTime);
        }

        [TestMethod]
        public void TestDateTimeToUnixEpoch()
        {
            // This Tests the DateTimeToUnixEpoch method returns the expected int
            int epoch = DSClientCommon.DateTimeToUnixEpoch(DateTime.Parse("15/06/2021 13:00:00"));

            Assert.AreEqual(1623758400, epoch);
        }

        [TestMethod]
        public void TestResolveWinComputer()
        {
            // This Tests the ResolveWinComputer method returns a string that represents a computer that can be passed to the AsigraDSClientApi
            Assert.AreEqual("DS-Client Computer", DSClientCommon.ResolveWinComputer("DS-Client Computer"));
            Assert.AreEqual("DS-Client Computer", DSClientCommon.ResolveWinComputer(@"\\DS-Client Computer"));
            Assert.AreEqual(@"Microsoft Windows Network\RemoteComputer", DSClientCommon.ResolveWinComputer(@"\\RemoteComputer"));
            Assert.AreEqual(@"Microsoft Windows Network\RemoteComputer", DSClientCommon.ResolveWinComputer(@"Microsoft Windows Network\RemoteComputer"));
        }

        [TestMethod]
        public void TestValidateHostname()
        {
            // This Tests the ValidateHostName Class and method that the returned object contains expected Property values
            string[] hostnames = { "172.22.10.254", "Computer", "computer.domain.com", "172..10.50.100", "computer." };
            string[] validHosts = { "172.22.10.254", "Computer", "computer.domain.com" };
            string[] invalidHosts = { "172..10.50.100", "computer." };

            IEnumerable<DSClientCommon.ValidateHostname> validatedHostnames = DSClientCommon.ValidateHostname.ValidateHostnames(hostnames);

            int validHostCount = 0;
            int invalidHostCount = 0;
            foreach (DSClientCommon.ValidateHostname host in validatedHostnames)
            {
                if (host.ValidHostnames != null)
                {
                    validHostCount++;
                }
                else if (host.InvalidHostnames != null)
                {
                    invalidHostCount++;
                }
            }

            Assert.AreEqual(validHosts.Count(), validHostCount);
            Assert.AreEqual(invalidHosts.Count(), invalidHostCount);

            Assert.AreEqual(true, DSClientCommon.ValidateHostname.ValidateHost("computer.xyz.com"));
            Assert.AreEqual(false, DSClientCommon.ValidateHostname.ValidateHost(@"\\computer"));
        }

        [TestMethod]
        public void TestGetSha1Hash()
        {
            // This Tests the GetSha1Hash Extension Method returns the expected Hash
            string testStr = "This is a Test String";

            Assert.AreEqual("0570639cf0edd074e827d286f07999854129e2ba", testStr.GetSha1Hash());
        }
    }
}