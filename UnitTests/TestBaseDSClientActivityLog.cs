using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AsigraDSClientApi;
using PSAsigraDSClient;

namespace UnitTests
{
    [TestClass]
    public class TestBaseDSClientActivityLog: BaseDSClientActivityLog
    {
        [TestMethod]
        public void TestDSClientActivityLog()
        {
            activity_log_info logInfo = new activity_log_info
            {
                completion = ECompletionType.ECompletionType__Succeeded,
                data_size = 5368709120,
                description = "Backup of C$",
                end_time = 0,
                errors = 3,
                file_count = 1337,
                id = 101,
                net_transmit_amt = 3489660928,
                node_id = 0,
                schedule_id = 0,
                set_id = 10,
                start_time = 1616976000,
                transmit_amt = 3489660928,
                type = EActivityType.EActivityType__Backup,
                user = "dsclientuser",
                warnings = 10
            };

            DSClientAcivityLog activityLog = new DSClientAcivityLog(logInfo);

            Assert.AreEqual(101, activityLog.ActivityId);
            Assert.AreEqual("Backup", activityLog.ActivityType);
            Assert.AreEqual(10, activityLog.BackupSetId);
            Assert.AreEqual(logInfo.description, activityLog.Description);
            Assert.AreEqual(DateTime.Parse("29/03/2021 01:00:00"), activityLog.StartTime);
            Assert.AreEqual(DateTime.Parse("01/01/1970 00:00:00"), activityLog.EndTime);
            Assert.AreEqual("Succeeded", activityLog.Status);
            Assert.AreEqual(logInfo.data_size, activityLog.DataSize);
            Assert.AreEqual(logInfo.file_count, activityLog.FileCount);
            Assert.AreEqual(logInfo.errors, activityLog.Errors);
            Assert.AreEqual(logInfo.warnings, activityLog.Warnings);
            Assert.AreEqual(logInfo.transmit_amt, activityLog.Transmit);
            Assert.AreEqual(logInfo.net_transmit_amt, activityLog.NetTransmit);
            Assert.AreEqual(logInfo.schedule_id, activityLog.ScheduleId);
            Assert.AreEqual(logInfo.user, activityLog.User);
        }

        protected override void DSClientProcessRecord()
        {
            throw new NotImplementedException();
        }
    }
}