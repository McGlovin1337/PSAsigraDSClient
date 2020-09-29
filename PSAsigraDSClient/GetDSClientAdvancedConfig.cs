using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientAdvancedConfig")]
    [OutputType(typeof(DSClientAdvancedConfig))]

    public class GetDSClientAdvancedConfig: DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            WriteVerbose("Retrieving DS-Client Advanced Configuration Parameters...");
            advanced_config_info[] advancedConfigInfo = DSClientConfigMgr.getAdvancedConfigurationParameters();

            List<DSClientAdvancedConfig> dSClientAdvancedConfig = new List<DSClientAdvancedConfig>();

            foreach (advanced_config_info configItem in advancedConfigInfo)
            {
                DSClientAdvancedConfig advConfigItem = new DSClientAdvancedConfig(configItem);
                dSClientAdvancedConfig.Add(advConfigItem);
            }

            // Sort the Properties first by Category then by Name
            dSClientAdvancedConfig = dSClientAdvancedConfig.OrderBy(order => order.Category)
                .ThenBy(order => order.Name)
                .ToList();

            dSClientAdvancedConfig.ForEach(WriteObject);

            DSClientConfigMgr.Dispose();
        }

        private class DSClientAdvancedConfig
        {
            public string Category { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
            public string DefaultValue { get; set; }
            public bool RestartRequired { get; set; }
            public bool ReconnectRequired { get; set; }
            public bool HasDefaultValue { get; set; }
            public bool UseLocalSetting { get; set; }
            public bool LocalItem { get; set; }
            public bool GlobalItem { get; set; }
            public int NodeId { get; set; }

            public DSClientAdvancedConfig(advanced_config_info advancedConfig)
            {
                Category = EAdvConfigCategoryToString(advancedConfig.category);
                Name = advancedConfig.name;
                Value = advancedConfig.value;
                DefaultValue = advancedConfig.default_value;
                RestartRequired = ((advancedConfig.flags & (int)EDSClientConfigurationFlags.EDSClientConfigurationFlags__NeedRestartDSClient) > 0) ? true : false;
                ReconnectRequired = ((advancedConfig.flags & (int)EDSClientConfigurationFlags.EDSClientConfigurationFlags__NeedReconnectDSClient) > 0) ? true : false;
                HasDefaultValue = ((advancedConfig.flags & (int)EDSClientConfigurationFlags.EDSClientConfigurationFlags__HasDefaultValue) > 0) ? true : false;
                UseLocalSetting = ((advancedConfig.flags & (int)EDSClientConfigurationFlags.EDSClientConfigurationFlags__UseLocalSetting) > 0) ? true : false;
                LocalItem = ((advancedConfig.flags & (int)EDSClientConfigurationFlags.EDSClientConfigurationFlags__LocalItem) > 0) ? true : false;
                GlobalItem = ((advancedConfig.flags & (int)EDSClientConfigurationFlags.EDSClientConfigurationFlags__GlobalItem) > 0) ? true : false;
                NodeId = advancedConfig.ds_client_id;
            }

            private string EAdvConfigCategoryToString(EAdvConfigCategory category)
            {
                string Category = null;

                switch(category)
                {
                    case EAdvConfigCategory.EAdvConfigCategory__APIConectivity:
                        Category = "ApiConnectivity";
                        break;
                    case EAdvConfigCategory.EAdvConfigCategory__Admin:
                        Category = "Admin";
                        break;
                    case EAdvConfigCategory.EAdvConfigCategory__Miscellaneous:
                        Category = "Miscellaneous";
                        break;
                    case EAdvConfigCategory.EAdvConfigCategory__Notification:
                        Category = "Notification";
                        break;
                    case EAdvConfigCategory.EAdvConfigCategory__Performance:
                        Category = "Performance";
                        break;
                    case EAdvConfigCategory.EAdvConfigCategory__Communication:
                        Category = "Communication";
                        break;
                    case EAdvConfigCategory.EAdvConfigCategory__Installation:
                        Category = "Installation";
                        break;
                }

                return Category;
            }
        }
    }
}