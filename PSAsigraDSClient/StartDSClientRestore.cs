using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientRestore")]
    [OutputType(typeof(void), typeof(GenericBackupSetActivity))]

    sealed public class StartDSClientRestore : DSClientCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Restore Session Id")]
        public int RestoreId { get; set; }

        [Parameter(HelpMessage = "Specify to output basic Activity Info")]
        public SwitchParameter PassThru { get; set; }

        protected override void DSClientProcessRecord()
        {
            GenericActivity activity;

            DSClientRestoreSession restoreSession = DSClientSessionInfo.GetRestoreSession(RestoreId);

            if (restoreSession != null)
            {
                if (!restoreSession.Ready.Ready)
                    throw new Exception("Restore Session is Not Ready to Start");

                WriteVerbose("Performing Action: Start Restore");
                activity = restoreSession.StartRestore();
                WriteVerbose($"Notice: Restore Activity Id: {activity.getID()}");

                DSClientSessionInfo.RemoveRestoreSession(restoreSession);
            }
            else
            {
                throw new Exception("Restore Session not found");
            }

            if (PassThru && activity != null)
                WriteObject(new GenericBackupSetActivity(activity));
        }
    }
}