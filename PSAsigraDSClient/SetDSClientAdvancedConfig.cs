using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientAdvancedConfig", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class SetDSClientAdvancedConfig: BaseDSClientAdvancedConfig
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Advanced Config Item to modify")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Advanced Config Item Value")]
        [ValidateNotNullOrEmpty]
        public string Value { get; set; }

        protected override void ProcessAdavancedConfig(IEnumerable<advanced_config_info> advancedConfigInfo)
        {
            // Get the config item (validates the config item exists, will throw an exception if it doesn't)
            advanced_config_info configItem = advancedConfigInfo.Single(item => item.name == Name);

            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            if (ShouldProcess($"{Name}", $"Assign Value '{Value}'"))
            {
                // Set the config value
                if (configItem.type == EAdvConfigType.EAdvConfigType__ConfigTypeString)
                    DSClientConfigMgr.setAdvancedConfigString(Name, Value);
                else if (configItem.type == EAdvConfigType.EAdvConfigType__ConfigTypeNumber)
                    DSClientConfigMgr.setAdvancedConfigNumber(Name, Convert.ToInt32(Value));
            }

            DSClientConfigMgr.Dispose();
        }
    }
}