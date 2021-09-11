using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientAdvancedConfig")]
    [OutputType(typeof(DSClientAdvancedConfig))]

    sealed public class GetDSClientAdvancedConfig: BaseDSClientAdvancedConfig  
    {
        protected override void ProcessAdavancedConfig(IEnumerable<advanced_config_info> advancedConfigInfo)
        {
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
        }
    }
}