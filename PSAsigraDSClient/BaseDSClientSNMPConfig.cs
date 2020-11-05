using System;
using System.Collections.Generic;
using System.Linq;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientSNMPConfig: DSClientCmdlet
    {
        protected abstract void ProcessSNMPConfig(DSClientSNMPInfo DSClientSNMPInfo, IEnumerable<DSClientSNMPCommunities> DSClientSNMPCommunities);
        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            SNMPConfiguration DSClientSNMPCfg = DSClientConfigMgr.getSNMPConfiguration();

            snmp_info SNMPInfo = DSClientSNMPCfg.getSNMPInfo();

            int numCommunities = SNMPInfo.communities.Count();

            DSClientSNMPInfo DSClientSNMPInfo = new DSClientSNMPInfo(numCommunities, SNMPInfo.heartbeat_interval);

            List<DSClientSNMPCommunities> DSClientSNMPCommunities = new List<DSClientSNMPCommunities>();

            int hostCounter = 0;

            foreach (var community in SNMPInfo.communities)
            {
                /* NB: It appears the getSNMPInfo() method provided by the AsigraDSClientApi is bugged
                 * It returns hosts from the previous Community and prepends them to the next Community
                 * The follwoing removes those prepended hosts*/
                string[] actualHosts = new string[community.hosts.Count() - hostCounter];
                if (hostCounter > 0)
                {
                    Array.Copy(community.hosts, hostCounter, actualHosts, 0, community.hosts.Count() - hostCounter);
                }
                else
                {
                    actualHosts = community.hosts;
                }

                DSClientSNMPCommunities.Add(new DSClientSNMPCommunities(community.community, actualHosts));

                hostCounter = community.hosts.Count();
            }

            ProcessSNMPConfig(DSClientSNMPInfo, DSClientSNMPCommunities);

            DSClientSNMPCfg.Dispose();
            DSClientConfigMgr.Dispose();
        }

        protected List<snmp_community_info> apiSNMPCommunityInfoBuilder(IEnumerable<DSClientSNMPCommunities> dSClientSNMPCommunities)
        {
            List<snmp_community_info> snmpCommunityInfo = new List<snmp_community_info>();

            foreach (var community in dSClientSNMPCommunities)
            {
                snmp_community_info addCommunity = new snmp_community_info
                {
                    community = community.Community,
                    hosts = community.Hosts
                };

                snmpCommunityInfo.Add(addCommunity);
            }

            return snmpCommunityInfo;
        }

        protected snmp_info apiSNMPInfoBuilder(IEnumerable<snmp_community_info> snmpCommunityInfo, DSClientSNMPInfo existingSNMPInfo)
        {
            snmp_info newSNMPInfo = new snmp_info
            {
                communities = snmpCommunityInfo.ToArray(),
                heartbeat_interval = existingSNMPInfo.HeartbeatInterval
            };

            return newSNMPInfo;
        }

        protected class DSClientSNMPInfo
        {
            public int CommunityCount { get; private set; }
            public int HeartbeatInterval { get; private set; }

            public DSClientSNMPInfo(int numCommunities, int heartbeatInterval)
            {
                CommunityCount = numCommunities;
                HeartbeatInterval = heartbeatInterval;
            }
        }

        protected class DSClientSNMPCommunities
        {
            public string Community { get; private set; }
            public string[] Hosts { get; private set; }

            public DSClientSNMPCommunities(string community, IEnumerable<string> hosts)
            {
                Community = community;
                Hosts = hosts.ToArray();
            }
        }
    }
}