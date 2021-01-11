using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientUser")]
    [OutputType(typeof(DSClientUser))]

    public class GetDSClientUser : BaseDSClientUserManager
    {
        [Parameter(HelpMessage = "Specify a specifc User by UserId")]
        public int UserId { get; set; }

        [Parameter(ParameterSetName = "groupid", HelpMessage = "List All Users in Group by GroupId")]
        public int GroupId { get; set; }

        protected override void ProcessUserManager(UserManager userManager)
        {
            List<DSClientUser> dsClientUsers = new List<DSClientUser>();

            if (MyInvocation.BoundParameters.ContainsKey("UserId"))
            {
                WriteVerbose($"Performing Action: Retrieve DS-Client User with UserId: {UserId}");
                dsclient_user_info user = userManager.getUser(UserId);

                dsClientUsers.Add(new DSClientUser(user));
            }
            else if (MyInvocation.BoundParameters.ContainsKey("GroupId"))
            {
                WriteVerbose($"Performing Action: Retrieve All DS-Client Users in Group with Id: {GroupId}");
                dsclient_user_info[] users = userManager.getUsers(GroupId);

                foreach (dsclient_user_info user in users)
                    dsClientUsers.Add(new DSClientUser(user));
            }
            else
            {
                WriteVerbose("Performing Action: Retrieve All DS-Client Users");
                dsclient_user_info[] users = userManager.getAllUsers();

                foreach (dsclient_user_info user in users)
                    dsClientUsers.Add(new DSClientUser(user));
            }

            dsClientUsers.ForEach(WriteObject);
        }
    }
}