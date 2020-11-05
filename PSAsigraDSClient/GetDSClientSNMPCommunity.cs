using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientSNMPCommunity")]
    [OutputType(typeof(DSClientSNMPCommunities))]
    public class GetDSClientSNMPCommunity: BaseDSClientSNMPConfig
    {
        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Community Name to Match")]
        [ValidateNotNullOrEmpty]
        public string Community { get; set; }

        [Parameter(Position = 1, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Literal Community Name to Match")]
        [ValidateNotNullOrEmpty]
        public string[] LiteralCommunity { get; set; }

        protected override void ProcessSNMPConfig(DSClientSNMPInfo dSClientSNMPInfo, IEnumerable<DSClientSNMPCommunities> dSClientSNMPCommunities)
        {
            if (Community != null || LiteralCommunity != null)
            {
                List<DSClientSNMPCommunities> filteredCommunities = new List<DSClientSNMPCommunities>();

                if (MyInvocation.BoundParameters.ContainsKey("Community"))
                {
                    WildcardOptions wcOptions = WildcardOptions.IgnoreCase |
                                                WildcardOptions.Compiled;

                    WildcardPattern wcPattern = new WildcardPattern(Community, wcOptions);

                    foreach (var community in dSClientSNMPCommunities)
                    {
                        if (wcPattern.IsMatch(community.Community))
                            filteredCommunities.Add(new DSClientSNMPCommunities(community.Community, community.Hosts));
                    }
                }

                if (MyInvocation.BoundParameters.ContainsKey("LiteralCommunity"))
                {
                    foreach (var community in dSClientSNMPCommunities)
                    {
                        bool matchCommunity = Array.Exists(LiteralCommunity, comm => comm == community.Community);

                        if (matchCommunity == true)
                        {
                            // Check we haven't already added this matching Community Name to the new list
                            bool matchList = filteredCommunities.Contains(community);

                            if (matchList == false)
                                filteredCommunities.Add(new DSClientSNMPCommunities(community.Community, community.Hosts));
                        }
                    }
                }

                filteredCommunities.ForEach(WriteObject);
            }
            else
            {
                dSClientSNMPCommunities.ToList().ForEach(WriteObject);
            }
        }
    }
}