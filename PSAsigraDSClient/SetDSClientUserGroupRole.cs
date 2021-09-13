using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientUserGroupRole", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class SetDSClientUserGroupRole : BaseDSClientUserManager
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true, HelpMessage = "Specify Id of User or Group Role to Modify")]
        public int RoleId { get; set; }

        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Name of User or Group")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the User/Group Domain or Computer")]
        public string From { get; set; }

        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Role to Add")]
        [ValidateSet("Administrator", "BackupOperator", "RegularUser")]
        public string Role { get; set; }

        [Parameter(HelpMessage = "Specify that the Name is a Group")]
        public SwitchParameter IsGroup { get; set; }

        protected override void ProcessUserManager(UserManager userManager)
        {
            WriteVerbose($"Performing Action: Retrieve User Group Role with Id {RoleId} from DS-Client");
            user_group_role role = userManager.listUserGroupRoles()
                                                .Single(ugr => ugr.role_id == RoleId);

            if (Name != null)
                if (ShouldProcess($"Role User/Name '{role.user_name}'", $"Set User/Group Name to '{Name}'"))
                    role.user_name = Name;

            if (From != null)
                if (ShouldProcess($"Role User/Group '{role.user_name}'", $"Set User/Group From to '{From}'"))
                    role.user_from = From;

            if (Role != null)
                if (ShouldProcess($"Role User/Group '{role.user_name}'", $"Set User/Group Role to '{Role}'"))
                    role.user_role = StringToEnum<EUserGroupRole>(Role);

            if (MyInvocation.BoundParameters.ContainsKey("IsGroup"))
                if (ShouldProcess($"Role IsGroup for '{role.user_name}'", $"Set Value '{IsGroup}'"))
                    role.role_type = (IsGroup) ? EUserGroupRoleType.EUserGroupRoleType__Group : EUserGroupRoleType.EUserGroupRoleType__User;

            if (ShouldProcess($"Role User/Group with Id '{RoleId}'", "Update User/Group Role"))
            {
                WriteVerbose("Performing Action: Update User Group Role");
                userManager.updateUserGroupRole(role);
            }
        }
    }
}