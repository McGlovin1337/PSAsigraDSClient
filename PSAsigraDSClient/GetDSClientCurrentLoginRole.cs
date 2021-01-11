using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientCurrentLoginRole")]
    [OutputType(typeof(DSClientCurrentLoginRole))]

    public class GetDSClientCurrentLoginRole : BaseDSClientUserManager
    {
        protected override void ProcessUserManager(UserManager userManager)
        {
            WriteVerbose("Performing Action: Retrieve Current User DS-Client Role");
            WriteObject(new DSClientCurrentLoginRole(userManager.getCurrentLoginUserRole()));
        }
    }
}