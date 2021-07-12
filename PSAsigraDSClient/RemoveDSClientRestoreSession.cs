using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientRestoreSession", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class RemoveDSClientRestoreSession : DSClientCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Restore Session Id to remove")]
        public int RestoreId { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Performing Action: Retrieve Restore Sessions");
            if (!(SessionState.PSVariable.GetValue("RestoreSessions", null) is List<DSClientRestoreSession> restoreSessions))
                throw new Exception("There are no Restore Sessions available");

            DSClientRestoreSession restoreSession = restoreSessions.Single(session => session.RestoreId == RestoreId);

            if (ShouldProcess($"RestoreId: {restoreSession.RestoreId} for BackupSetId: {restoreSession.BackupSetId}", "Removing Restore Session"))
            {
                restoreSession.Dispose();
                restoreSessions.Remove(restoreSession);

                if (restoreSessions.Count > 0)
                    SessionState.PSVariable.Set("RestoreSessions", restoreSessions);
                else
                    SessionState.PSVariable.Remove("RestoreSessions");
            }
        }
    }
}