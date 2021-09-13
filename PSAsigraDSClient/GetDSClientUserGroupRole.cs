using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientUserGroupRole")]
    [OutputType(typeof(DSClientUserGroupRole))]

    sealed public class GetDSClientUserGroupRole : BaseDSClientUserManager
    {
        protected override void ProcessUserManager(UserManager userManager)
        {
            WriteVerbose("Performing Action Retrieve All DS-Client User and Group Roles");
            user_group_role[] roles = userManager.listUserGroupRoles();

            List<DSClientUserGroupRole> userGroupRoles = new List<DSClientUserGroupRole>();

            foreach (user_group_role role in roles)
                userGroupRoles.Add(new DSClientUserGroupRole(role));

            userGroupRoles.ForEach(WriteObject);
        }
    }
}