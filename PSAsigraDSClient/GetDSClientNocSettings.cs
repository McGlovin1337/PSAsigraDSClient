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

            WriteVerbose("Performing Action: Retrieve DS-NOC Settings");
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
            public string DsSysName { get; private set; }
            public string Address { get; private set; }
            public bool ClientMonitored { get; private set; }
            public int ConnectFrequency { get; private set; }
            public DateTime LastConnection { get; private set; }
            public DateTime LastSuccessConnection { get; private set; }
            public int RetryAttempts { get; private set; }
            public int RetryInterval { get; private set; }

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