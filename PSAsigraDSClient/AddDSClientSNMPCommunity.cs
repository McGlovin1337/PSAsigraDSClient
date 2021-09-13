using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientSNMPCommunity")]
    [OutputType(typeof(void))]

    sealed public class AddDSClientSNMPCommunity: BaseDSClientSNMPConfig
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the SNMP Community")]
        [ValidateNotNullOrEmpty]
        public string Community { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Hosts to add to SNMP Community")]
        [ValidateNotNullOrEmpty]
        public string[] Hosts { get; set; }

        protected override void ProcessSNMPConfig(DSClientSNMPInfo DSClientSNMPInfo, IEnumerable<DSClientSNMPCommunities> DSClientSNMPCommunities)
        {
            List<DSClientSNMPCommunities> SNMPCommunities = DSClientSNMPCommunities.ToList();

            // Check if the Community Name is already configured
            DSClientSNMPCommunities existingCommunity = DSClientSNMPCommunities.SingleOrDefault(comm => comm.Community == Community);

            if (existingCommunity != null && Hosts.Count() > 0)
            {
                WriteVerbose("Notice: Specified SNMP Community Name already exists");

                // If the Community Name already exists, get any duplicates between the Hosts we're trying to add and those that already exist
                string[] hostExist = Hosts.Intersect(existingCommunity.Hosts).ToArray();
                WriteVerbose($"Notice: Yielded {hostExist.Count()} existing hosts");

                // Update the list of Hosts to be added, excluding any duplicates
                Hosts = Hosts.Except(hostExist).ToArray();

                // Merge the new Hosts to be added, with the existing hosts
                string[] mergeHosts = Hosts.Union(existingCommunity.Hosts).ToArray();

                // Form an updated DSClientSNMPCommunities object
                DSClientSNMPCommunities updatedCommunity = new DSClientSNMPCommunities(existingCommunity.Community, mergeHosts);

                // Remove the existing Community from original DSClientSNMPCommunities
                SNMPCommunities.Remove(existingCommunity);

                // Add the updated Community
                SNMPCommunities.Add(updatedCommunity);
            }
            else
            {
                WriteVerbose("Performing Action: Create new SNMP Community");
                // Create a new Community
                SNMPCommunities.Add(new DSClientSNMPCommunities(Community, Hosts));
            }

            // Create a new snmp_community_info object with all the updated and existing Communities
            List<snmp_community_info> snmpCommunityInfo = apiSNMPCommunityInfoBuilder(SNMPCommunities);

            // Create a new snmp_info object with the existing HeartbeatInterval setting and the new snmp_community_info object
            snmp_info newSNMPInfo = apiSNMPInfoBuilder(snmpCommunityInfo, DSClientSNMPInfo);

            // Set the new SNMP Configuration
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            SNMPConfiguration DSClientSNMPCfg = DSClientConfigMgr.getSNMPConfiguration();

            DSClientSNMPCfg.setSNMPInfo(newSNMPInfo);
            WriteObject("Notice: Added SNMP Community Configuration");

            DSClientSNMPCfg.Dispose();
            DSClientConfigMgr.Dispose();
        }
    }
}