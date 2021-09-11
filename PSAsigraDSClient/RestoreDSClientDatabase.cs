using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.BaseDSClientRunningActivity;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsData.Restore, "DSClientDatabase", SupportsShouldProcess = true)]
    [OutputType(typeof(DSClientRunningActivity))]

    sealed public class RestoreDSClientDatabase: DSClientCmdlet
    {
        [Parameter(Position = 0, HelpMessage = "Specify to ONLY recover DS-Client Database")]
        public SwitchParameter DSClientDatabaseOnly { get; set; }

        protected override void DSClientProcessRecord()
        {
            string db = "DSClient and DSDelta";
            ERecoveryType recoveryType = ERecoveryType.ERecoveryType__DSClientAndDSDelta;

            if (DSClientDatabaseOnly == true)
            {
                db = "DSClient";
                recoveryType = ERecoveryType.ERecoveryType__DSClientOnly;
            }            

            if (ShouldProcess(db + " Database"))
            {
                SystemActivityManager DSClientSystemActivityMgr = DSClientSession.getSystemActivityManager();
                GenericActivity recoverDBActivity = DSClientSystemActivityMgr.recover_dsclient_database(recoveryType);
                running_activity_info recoveryProgressInfo = recoverDBActivity.getCurrentStatus();
                DSClientRunningActivity recoveryProgress = new DSClientRunningActivity(recoveryProgressInfo);

                WriteObject(recoveryProgress);
            }
        }
    }
}