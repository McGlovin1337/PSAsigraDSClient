using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientDelete")]
    [OutputType(typeof(void), typeof(GenericBackupSetActivity))]

    sealed public class StartDSClientDelete : DSClientCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Delete Session Id")]
        public int DeleteId { get; set; }

        [Parameter(HelpMessage = "Specify to output basic Activity Info")]
        public SwitchParameter PassThru { get; set; }

        protected override void DSClientProcessRecord()
        {
            GenericActivity activity;

            DSClientDeleteSession deleteSession = DSClientSessionInfo.GetDeleteSession(DeleteId);

            if (deleteSession != null)
            {
                WriteVerbose("Performing Action: Start Backup Set Validation");
                activity = deleteSession.StartValidation();
                WriteVerbose($"Notice: Validation Session Id: {deleteSession.DeleteId}");

                DSClientSessionInfo.RemoveDeleteSession(deleteSession);
            }
            else
            {
                throw new Exception("Delete Session not found");
            }

            if (PassThru && activity != null)
                WriteObject(new GenericBackupSetActivity(activity));
        }
    }
}