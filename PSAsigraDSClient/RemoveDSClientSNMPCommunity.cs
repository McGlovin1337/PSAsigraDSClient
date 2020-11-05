using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientSNMPCommunity")]

    public class RemoveDSClientSNMPCommunity: BaseDSClientSNMPConfig
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the SNMP Community")]
        [ValidateNotNullOrEmpty]
        public string Community { get; set; }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Hosts to add to SNMP Community")]
        [ValidateNotNullOrEmpty]
        public string[] Hosts { get; set; }

        protected override void ProcessSNMPConfig(DSClientSNMPInfo DSClientSNMPInfo, IEnumerable<DSClientSNMPCommunities> DSClientSNMPCommunities)
        {
            List<DSClientSNMPCommunities> SNMPCommunities = DSClientSNMPCommunities.ToList();

            // Check if the Community Name is actually configured
            DSClientSNMPCommunities existingCommunity = DSClientSNMPCommunities.SingleOrDefault(comm => comm.Community == Community);

            if (existingCommunity != null)
            {
                WriteVerbose("SNMP Community Name exists...");

                if (Hosts != null)
                {
                    WriteVerbose("Removing specified Hosts from Community...");
                    // Build a replacement Community
                    DSClientSNMPCommunities replacementCommunity = new DSClientSNMPCommunities(existingCommunity.Community, existingCommunity.Hosts.Except(Hosts).ToArray());

                    // Remove the original Community
                    SNMPCommunities.Remove(existingCommunity);

                    // Add the replacement Community
                    SNMPCommunities.Add(replacementCommunity);
                }
                else
                {
                    WriteVerbose("Removing SNMP Community...");
                    SNMPCommunities.Remove(existingCommunity);
                }

                // Create a new snmp_community_info object with all the updated and existing Communities
                List<snmp_community_info> snmpCommunityInfo = apiSNMPCommunityInfoBuilder(SNMPCommunities);

                // Create a new snmp_info object with the existing HeartbeatInterval setting and the new snmp_community_info object
                snmp_info newSNMPInfo = apiSNMPInfoBuilder(snmpCommunityInfo, DSClientSNMPInfo);

                // Set the new SNMP Configuration
                ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

                SNMPConfiguration DSClientSNMPCfg = DSClientConfigMgr.getSNMPConfiguration();

                DSClientSNMPCfg.setSNMPInfo(newSNMPInfo);
                WriteObject("SNMP Community Configuration Updated");

                DSClientSNMPCfg.Dispose();
                DSClientConfigMgr.Dispose();
            }
            else
            {
                throw new Exception("Community Name doesn't exist");
            }
        }
    }
}