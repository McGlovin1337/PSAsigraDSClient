using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientUserGroup")]
    [OutputType(typeof(DSClientUserGroup))]

    sealed public class GetDSClientUserGroup : BaseDSClientUserManager
    {
        [Parameter(HelpMessage = "Specify a specific GroupId")]
        public int GroupId { get; set; }

        protected override void ProcessUserManager(UserManager userManager)
        {
            List<DSClientUserGroup> userGroups = new List<DSClientUserGroup>();

            if (MyInvocation.BoundParameters.ContainsKey("GroupId"))
            {
                WriteVerbose($"Performing Action: Retrieve User Group with Id: {GroupId}");
                user_group_info group = userManager.getGroup(GroupId);

                userGroups.Add(new DSClientUserGroup(group));
            }
            else
            {
                WriteVerbose("Performing Action: Retrieve All DS-Client User Groups");
                user_group_info[] groups = userManager.getAllGroups();

                foreach (user_group_info group in groups)
                    userGroups.Add(new DSClientUserGroup(group));
            }

            userGroups.ForEach(WriteObject);
        }
    }
}