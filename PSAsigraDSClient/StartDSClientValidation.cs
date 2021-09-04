using System;
using System.Management.Automation;
using static PSAsigraDSClient.DSClientCommon;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientValidation")]
    [OutputType(typeof(void), typeof(GenericBackupSetActivity))]

    sealed public class StartDSClientValidation : DSClientCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Validation Session Id")]
        public int ValidationId { get; set; }

        [Parameter(HelpMessage = "Specify to output basic Activity Info")]
        public SwitchParameter PassThru { get; set; }

        protected override void DSClientProcessRecord()
        {
            GenericActivity activity;

            DSClientValidationSession validationSession = DSClientSessionInfo.GetValidationSession(ValidationId);

            if (validationSession != null)
            {
                WriteVerbose("Performing Action: Start Backup Set Validation");
                activity = validationSession.StartValidation();
                WriteVerbose($"Notice: Validation Session Id: {validationSession.ValidationId}");

                DSClientSessionInfo.RemoveValidationSession(validationSession);
            }
            else
            {
                throw new Exception("Validation Session not found");
            }

            if (PassThru && activity != null)
                WriteObject(new GenericBackupSetActivity(activity));
        }
    }
}