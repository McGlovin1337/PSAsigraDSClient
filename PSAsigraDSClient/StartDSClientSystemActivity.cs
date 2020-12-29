using System;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientSystemActivity")]
    public class StartDSClientSystemActivity: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "Sepcify the System Activity to Start")]
        [ValidateSet("DailyAdmin","StatisticsAdmin", "WeeklyAdmin")]
        public string Activity { get; set; }

        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfig = DSClientSession.getConfigurationManager();

            switch (Activity)
            {
                case "DailyAdmin":
                    WriteVerbose("Performing Action: Start DS-Client Daily Admin System Activity");
                    DSClientConfig.start_daily_admin();
                    break;
                case "StatisticsAdmin":
                    WriteVerbose("Performing Action: Start Statistics Update Admin System Activity");
                    DSClientConfig.start_statistics_admin();
                    break;
                case "WeeklyAdmin":
                    WriteVerbose("Performing Action: Start DS-Client Weekly Admin System Activity");
                    DSClientConfig.start_weekly_admin();
                    break;
                default:
                    throw new Exception("Unable to start an activity");                    
            }

            DSClientConfig.Dispose();
        }        
    }
}