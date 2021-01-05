using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientSystemActivity")]
    [OutputType(typeof(SystemActivityStart))]

    public class StartDSClientSystemActivity: DSClientCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "daily", HelpMessage = "Start Daily Admin Activity")]
        public SwitchParameter DailyAdmin { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "weekly", HelpMessage = "Start Weekly Admin Activity")]
        public SwitchParameter WeeklyAdmin { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "stats", HelpMessage = "Start Statistical Admin Activity")]
        public SwitchParameter StatisticalAdmin { get; set; }

        [Parameter(HelpMessage = "Specify to Output Basic Activity Details")]
        public SwitchParameter PassThru { get; set; }

        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfig = DSClientSession.getConfigurationManager();

            GenericActivity activity = null;

            if (DailyAdmin)
            {
                WriteVerbose("Performing Action: Start DS-Client Daily Admin");
                activity = DSClientConfig.start_daily_admin();
            }
            else if (WeeklyAdmin)
            {
                WriteVerbose("Performing Action: Start DS-Client Weekly Admin");
                activity = DSClientConfig.start_weekly_admin();
            }
            else if (StatisticalAdmin)
            {
                WriteVerbose("Performing Action: Start DS-Client Statistical Admin");
                activity = DSClientConfig.start_statistics_admin();
            }

            DSClientConfig.Dispose();

            if (PassThru && activity != null)
                WriteObject(new SystemActivityStart(activity));
        }
        
        private class SystemActivityStart
        {
            public int ActivityId { get; private set; }
            public string Type { get; private set; }

            public SystemActivityStart(GenericActivity activity)
            {
                ActivityId = activity.getID();
                Type = EnumToString(activity.getCurrentStatus().type);
            }
        }
    }
}