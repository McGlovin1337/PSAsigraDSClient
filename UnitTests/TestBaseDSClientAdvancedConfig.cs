using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using AsigraDSClientApi;
using PSAsigraDSClient;

namespace UnitTests
{
    [TestClass]
    public class TestBaseDSClientAdvancedConfig : BaseDSClientAdvancedConfig
    {
        [TestMethod]
        public void TestDSClientAdvancedConfig()
        {
            advanced_config_info advancedConfig = new advanced_config_info
            {
                category = EAdvConfigCategory.EAdvConfigCategory__Admin,
                default_value = "default",
                ds_client_id = 0,
                flags = 265,
                name = "test",
                restart_required = false,
                type = EAdvConfigType.EAdvConfigType__ConfigTypeString,
                value = "defaultValue"
            };

            DSClientAdvancedConfig dsClientAdvancedConfig = new DSClientAdvancedConfig(advancedConfig);

            Assert.AreEqual("Admin", dsClientAdvancedConfig.Category);
            Assert.AreEqual(advancedConfig.name, dsClientAdvancedConfig.Name);
            Assert.AreEqual(advancedConfig.value, dsClientAdvancedConfig.Value);
            Assert.AreEqual(advancedConfig.default_value, dsClientAdvancedConfig.DefaultValue);
            Assert.AreEqual(true, dsClientAdvancedConfig.RestartRequired);
            Assert.AreEqual(false, dsClientAdvancedConfig.ReconnectRequired);
            Assert.AreEqual(true, dsClientAdvancedConfig.HasDefaultValue);
            Assert.AreEqual(false, dsClientAdvancedConfig.UseLocalSetting);
            Assert.AreEqual(false, dsClientAdvancedConfig.LocalItem);
            Assert.AreEqual(true, dsClientAdvancedConfig.GlobalItem);
            Assert.AreEqual(advancedConfig.ds_client_id, dsClientAdvancedConfig.NodeId);
        }

        protected override void ProcessAdavancedConfig(IEnumerable<advanced_config_info> advancedConfigInfo)
        {
            throw new NotImplementedException();
        }
    }
}