using System;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientNocSettings")]
    [OutputType(typeof(DSClientNocSettings))]

    public class GetDSClientNocSettings: DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            NOCSettingsConfiguration nocSettingsConfiguration = DSClientConfigMgr.getNOCSettings();

            WriteVerbose("Retrieving DS-Client DS-NOC Settings...");
            noc_settings[] nocSettings = nocSettingsConfiguration.getSettings();

            List<DSClientNocSettings> dSClientNocSettings = new List<DSClientNocSettings>();

            foreach (noc_settings setting in nocSettings)
            {
                DSClientNocSettings nocSetting = new DSClientNocSettings(setting);
                dSClientNocSettings.Add(nocSetting);
            }

            dSClientNocSettings.ForEach(WriteObject);

            nocSettingsConfiguration.Dispose();
            DSClientConfigMgr.Dispose();
        }

        private class DSClientNocSettings
        {
            public string DsSysName { get; set; }
            public string Address { get; set; }
            public bool ClientMonitored { get; set; }
            public int ConnectFrequency { get; set; }
            public DateTime LastConnection { get; set; }
            public DateTime LastSuccessConnection { get; set; }
            public int RetryAttempts { get; set; }
            public int RetryInterval { get; set; }

            public DSClientNocSettings(noc_settings nocSettings)
            {
                DsSysName = nocSettings.system_name;
                Address = nocSettings.address;
                ClientMonitored = nocSettings.is_monitored;
                ConnectFrequency = nocSettings.connection_frequency;
                LastConnection = UnixEpochToDateTime(nocSettings.last_connection);
                LastSuccessConnection = UnixEpochToDateTime(nocSettings.last_successful);
                RetryAttempts = nocSettings.retry_attempts;
                RetryInterval = nocSettings.retry_interval;
            }
        }
    }
}