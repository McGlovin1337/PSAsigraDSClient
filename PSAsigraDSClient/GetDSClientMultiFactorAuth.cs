using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientMultiFactorAuth")]
    [OutputType(typeof(DSClientMFA))]

    sealed public class GetDSClientMultiFactorAuth: DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            TFAManager DSClientTFAMgr = DSClientSession.getTFAManager();

            DSClientMFA DSClientMFA = new DSClientMFA(DSClientTFAMgr.is2FAEnabled(), DSClientTFAMgr.getUserEmails());

            WriteObject(DSClientMFA);

            DSClientTFAMgr.Dispose();
        }

        private class DSClientMFA
        {
            public bool MFAEnabled { get; set; }
            public DSClientMFAUserEmails[] UserEmails { get; set; }

            public DSClientMFA (bool mfaEnabled, user_email[] userEmails)
            {
                MFAEnabled = mfaEnabled;

                List<DSClientMFAUserEmails> dSClientMFAUserEmails = new List<DSClientMFAUserEmails>();

                foreach (var email in userEmails)
                {
                    DSClientMFAUserEmails userEmail = new DSClientMFAUserEmails(email);
                    dSClientMFAUserEmails.Add(userEmail);
                }

                UserEmails = dSClientMFAUserEmails.ToArray();
            }
        }

        private class DSClientMFAUserEmails
        {
            public string Username { get; private set; }
            public string Domain { get; private set; }
            public string Email { get; private set; }

            public DSClientMFAUserEmails (user_email userEmail)
            {
                Username = userEmail.username;
                Domain = userEmail.domain;
                Email = userEmail.email;
            }

            public override string ToString()
            {
                return Email;
            }
        }
    }
}