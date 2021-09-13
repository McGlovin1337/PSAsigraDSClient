using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientUserGroupRole")]
    [OutputType(typeof(void))]

    sealed public class AddDSClientUserGroupRole : BaseDSClientUserManager
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true, HelpMessage = "Specify Name of User or Group")]
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
            user_group_role newRole = new user_group_role
            {
                user_name = Name,
                user_from = From,
                user_role = StringToEnum<EUserGroupRole>(Role),
                role_type = (IsGroup) ? EUserGroupRoleType.EUserGroupRoleType__Group : EUserGroupRoleType.EUserGroupRoleType__User
            };

            WriteVerbose("Performing Action: Add User Group Role to DS-Client Database");
            userManager.addUserGroupRole(newRole);
        }
    }
}