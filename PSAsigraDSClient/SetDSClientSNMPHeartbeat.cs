using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientSNMPHeartbeat", SupportsShouldProcess = true)]
    public class SetDSClientSNMPHeartbeat: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, HelpMessage = "Set the SNMP Heartbeat Interval")]
        [ValidateNotNullOrEmpty]
        public int HeartbeatInterval { get; set; }

        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            SNMPConfiguration DSClientSNMPCfg = DSClientConfigMgr.getSNMPConfiguration();

            snmp_info SNMPInfo = DSClientSNMPCfg.getSNMPInfo();

            if (ShouldProcess("DS-Client SNMP Configuration", $"Set Value '{HeartbeatInterval}'"))
                SNMPInfo.heartbeat_interval = HeartbeatInterval;

            DSClientSNMPCfg.setSNMPInfo(SNMPInfo);
            WriteObject("SNMP Heartbeat Interval Set");

            DSClientSNMPCfg.Dispose();
            DSClientConfigMgr.Dispose();
        }
    }
}