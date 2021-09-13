using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientUserGroupRole", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class RemoveDSClientUserGroupRole : BaseDSClientUserManager
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true, HelpMessage = "Specify Id of Role to Remove")]
        public int RoleId { get; set; }

        protected override void ProcessUserManager(UserManager userManager)
        {
            WriteDebug($"Performing Action: Retrieve User Group Role with Id: {RoleId}");
            user_group_role role = userManager.listUserGroupRoles()
                                                .Single(ugr => ugr.role_id == RoleId);

            if (ShouldProcess($"{role.user_name} with Id {role.role_id}"))
            {
                WriteVerbose($"Performing Action: Remove User/Group Role with Id: {role.role_id} From DS-Client");
                userManager.deleteUserGroupRole(role.role_id);
            }
        }
    }
}