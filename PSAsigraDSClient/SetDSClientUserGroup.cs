using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientUserGroup")]

    public class SetDSClientUserGroup : BaseDSClientUserManager
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true, HelpMessage = "Specify Id of Group to Modify")]
        public int GroupId { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Maximum Online Quota in MB for Specified Group")]
        public int MaxOnlineSize { get; set; }

        protected override void ProcessUserManager(UserManager userManager)
        {
            WriteVerbose($"Performing Action: Update User with Id: {GroupId}");
            userManager.setGroupMaxOnline(GroupId, MaxOnlineSize);
        }
    }
}