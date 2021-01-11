using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientUserGroup", SupportsShouldProcess = true)]

    public class RemoveDSClientUserGroup : BaseDSClientUserManager
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true, HelpMessage = "Specify Id of Group to Remove")]
        public int GroupId { get; set; }

        protected override void ProcessUserManager(UserManager userManager)
        {
            WriteDebug($"Performing Action: Retrieve User Group with Id {GroupId}");
            user_group_info group = userManager.getGroup(GroupId);

            if (ShouldProcess($"{group.group} with Id: {group.id}"))
            {
                WriteVerbose($"Performing Action: Removing User Group with Id: {group.id} from DS-Client");
                userManager.deleteGroup(group.id);
            }
        }
    }
}