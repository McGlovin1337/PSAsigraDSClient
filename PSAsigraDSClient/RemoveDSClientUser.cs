using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientUser", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class RemoveDSClientUser : BaseDSClientUserManager
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true, HelpMessage = "Specify Id of User to Remove")]
        public int UserId { get; set; }

        protected override void ProcessUserManager(UserManager userManager)
        {
            WriteDebug($"Performing Action: Retrieve User with Id: {UserId}");
            dsclient_user_info user = userManager.getUser(UserId);

            if (ShouldProcess($"{user.name} with Id: {user.id}"))
            {
                WriteVerbose($"Performing Action: Remove {user.name} with Id: {user.id} from DS-Client");
                userManager.deleteUser(UserId);
            }
        }
    }
}