using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientUserGroup", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class SetDSClientUserGroup : BaseDSClientUserManager
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true, HelpMessage = "Specify Id of Group to Modify")]
        public int GroupId { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Maximum Online Quota in MB for Specified Group")]
        public int MaxOnlineSize { get; set; }

        protected override void ProcessUserManager(UserManager userManager)
        {
            string groupName = userManager.getGroup(GroupId).group;

            if (ShouldProcess($"DS-Client Group '{groupName}'", $"Set Online Quota to '{MaxOnlineSize}'"))
            {
                WriteVerbose($"Performing Action: Update Group '{groupName}' with Id: {GroupId}");
                userManager.setGroupMaxOnline(GroupId, MaxOnlineSize);
            }
        }
    }
}