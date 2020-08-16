using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Exit, "DSClientSession")]
    public class ExitDSClientSession: PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteVerbose("Checking for existing DSClient Sessions...");
            ClientConnection previousSession = SessionState.PSVariable.GetValue("DSClientSession", null) as ClientConnection;

            if (previousSession != null)
            {
                WriteVerbose("Previous DSClient Session found, attempting to dispose...");
                try
                {
                    previousSession.logout();
                    previousSession.Dispose();
                }
                catch
                {
                    WriteVerbose("Previous session failed to dispose, removing session...");
                }
                SessionState.PSVariable.Remove("DSClientSession");
                WriteObject("DSClient Session removed.");
            }
            else
            {
                WriteVerbose("No previous DSClient Session found");
                WriteObject("DSClient Session does not exist.");
            }
        }
    }
}