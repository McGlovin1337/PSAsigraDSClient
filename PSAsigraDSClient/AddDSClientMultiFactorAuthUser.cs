using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientMultiFactorAuthUser")]
    [OutputType(typeof(void))]

    sealed public class AddDSClientMultiFactorAuthUser: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Email Address to add")]
        [ValidateNotNullOrEmpty]
        public string Email { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify User Name")]
        [ValidateNotNullOrEmpty]
        public string Username { get; set; }

        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Domain")]
        public string Domain { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Performing Action: Retrieve current list of MFA Users");
            TFAManager DSClientTFAMgr = DSClientSession.getTFAManager();

            List<user_email> userEmails = DSClientTFAMgr.getUserEmails().ToList();

            user_email newUserEmail = new user_email
            {
                domain = Domain,
                email = Email,
                username = Username
            };
            userEmails.Add(newUserEmail);

            WriteVerbose("Performing Action: Add MFA User");
            DSClientTFAMgr.setUserEmails(userEmails.ToArray());
            WriteObject("Added new MFA User");

            DSClientTFAMgr.Dispose();
        }
    }
}