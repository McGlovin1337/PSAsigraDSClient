using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientSession")]
    [OutputType(typeof(void))]

    sealed public class SetDSClientSession : BaseDSClientSession
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "sessionId", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify DS-Client Session Id to Modify")]
        public int Id { get; set; }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "sessionObject", ValueFromPipeline = true, HelpMessage = "Specify a DS-Client Session Object to Modify")]
        public DSClientSession Session { get; set; }

        [Parameter(HelpMessage = "Specify a New Name for the Session")]
        [ValidateNotNullOrEmpty]
        public string NewName { get; set; }

        [Parameter(HelpMessage = "The number of connection attempts for the specified session")]
        [Alias("ConnectionRetries")]
        public int ConnectionAttempts { get; set; }

        protected override void ProcessDSClientSession(IEnumerable<DSClientSession> sessions)
        {
            DSClientSession session = null;

            if (MyInvocation.BoundParameters.ContainsKey(nameof(Id)))
            {
                try
                {
                    session = sessions.Single(s => s.Id == Id);
                }
                catch
                {
                    ErrorRecord errorRecord = new ErrorRecord(
                        new Exception("DS-Client Session not found"),
                        "Exception",
                        ErrorCategory.ObjectNotFound,
                        null);
                    WriteError(errorRecord);
                }
            }
            else if (Session != null)
            {
                session = Session;
            }

            if (session != null && MyInvocation.BoundParameters.Count > 1)
            {
                if (MyInvocation.BoundParameters.ContainsKey(nameof(NewName)))
                {
                    session.SetName(NewName);
                }

                if (MyInvocation.BoundParameters.ContainsKey(nameof(ConnectionAttempts)))
                {
                    session.SetConnectionRetries(ConnectionAttempts);
                }
            }
        }
    }
}