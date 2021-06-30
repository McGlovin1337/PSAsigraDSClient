using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientRestoreSession")]
    [OutputType(typeof(DSClientRestoreSession), typeof(void))]

    sealed public class GetDSClientRestoreSession : DSClientCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Select a specific Restore Session by Id")]
        public int RestoreId { get; set; }

        protected override void DSClientProcessRecord()
        {
            List<DSClientRestoreSession> restoreSessions = SessionState.PSVariable.GetValue("RestoreSessions", null) as List<DSClientRestoreSession>;

            if (restoreSessions != null)
            {
                if (MyInvocation.BoundParameters.ContainsKey(nameof(RestoreId)))
                    WriteObject(restoreSessions.Single(session => session.RestoreId == RestoreId));
                else
                    restoreSessions.ForEach(WriteObject);
            }
        }
    }
}