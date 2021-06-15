using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientRestore")]

    public class StartDSClientRestore : DSClientCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Restore Session Id")]
        public int RestoreId { get; set; }

        [Parameter(HelpMessage = "Specify to output basic Activity Info")]
        public SwitchParameter PassThru { get; set; }

        protected override void DSClientProcessRecord()
        {
            List<DSClientRestoreSession> restoreSessions = SessionState.PSVariable.GetValue("RestoreSessions", null) as List<DSClientRestoreSession>;

            if (restoreSessions == null)
                throw new Exception("No Restore Sessions found");

            GenericActivity activity = null;

            bool found = false;
            for (int i = 0; i < restoreSessions.Count(); i++)
            {
                if (restoreSessions[i].RestoreId == RestoreId)
                {
                    DSClientRestoreSession restoreSession = restoreSessions[i];

                    if (!restoreSession.Ready.Ready)
                        throw new Exception("Restore Session is Not Ready to Start");

                    activity = restoreSession.StartRestore();

                    restoreSessions.Remove(restoreSession);

                    found = true;
                    break;
                }
            }

            if (!found)
                throw new Exception("Restore Session not found");

            if (PassThru)
                WriteObject(new GenericBackupSetActivity(activity));
        }
    }
}