using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientSessionCleanup: PSCmdlet
    {
        protected abstract void ProcessCleanSession();

        protected override void ProcessRecord()
        {
            WriteVerbose("Performing Action: Check for existing DS-Client Sessions");

            if (SessionState.PSVariable.GetValue("DSClientSession", null) is DSClientSession previousSession)
            {
                WriteVerbose("Notice: Previous DS-Client Session found");
                if (previousSession.GetLogoutOnExit())
                    previousSession.Disconnect();

                WriteVerbose("Performing Action: Remove on DS-Client Session");
                SessionState.PSVariable.Remove("DSClientSession");
                WriteObject("DS-Client Session removed.");
            }
            else
            {
                WriteVerbose("Notice: No previous DSClient Session found");
            }

            ProcessCleanSession();
        }
    }
}