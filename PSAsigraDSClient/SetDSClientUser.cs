using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientUser")]

    public class SetDSClientUser : BaseDSClientUserManager
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true, HelpMessage = "Specify Id of User to Modify")]
        public int UserId { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Maximum Online Quota in MB for Specified User")]
        public int MaxOnlineSize { get; set; }

        protected override void ProcessUserManager(UserManager userManager)
        {
            WriteVerbose($"Performing Action: Update User with Id: {UserId}");
            userManager.setUserMaxOnline(UserId, MaxOnlineSize);
        }
    }
}