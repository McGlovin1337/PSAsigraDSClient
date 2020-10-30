using System.Collections.Generic;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientAdvancedConfig : DSClientCmdlet
    {
        protected abstract void ProcessAdavancedConfig(IEnumerable<advanced_config_info> advancedConfigInfo);

        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            WriteVerbose("Retrieving DS-Client Advanced Configuration Parameters...");
            advanced_config_info[] advancedConfigInfo = DSClientConfigMgr.getAdvancedConfigurationParameters();

            DSClientConfigMgr.Dispose();

            ProcessAdavancedConfig(advancedConfigInfo);
        }

        protected class DSClientAdvancedConfig
        {
            public string Category { get; private set; }
            public string Name { get; private set; }
            public string Value { get; private set; }
            public string DefaultValue { get; private set; }
            public bool RestartRequired { get; private set; }
            public bool ReconnectRequired { get; private set; }
            public bool HasDefaultValue { get; private set; }
            public bool UseLocalSetting { get; private set; }
            public bool LocalItem { get; private set; }
            public bool GlobalItem { get; private set; }
            public int NodeId { get; private set; }

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

                switch (category)
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