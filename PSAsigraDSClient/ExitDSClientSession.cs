using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Exit, "DSClientSession")]
    public class ExitDSClientSession: PSCmdlet
    {
        protected override void ProcessRecord()
        {
            // Check for a previous Backup Set Validation View stored in Session State
            WriteVerbose("Checking for previous DS-Client Delete View Sessions...");
            BackupSetDeleteView previousDeleteSession = SessionState.PSVariable.GetValue("DeleteView", null) as BackupSetDeleteView;

            // If a previous session is found, remove it
            if (previousDeleteSession != null)
            {
                WriteVerbose("Previous Delete View found, attempting to Dispose...");
                try
                {
                    previousDeleteSession.Dispose();
                }
                catch
                {
                    WriteVerbose("Previous Session failed to Dispose, deleting session...");
                }
                SessionState.PSVariable.Remove("DeleteView");
            }

            // Check for a previous Backup Set Validation View stored in Session State
            WriteVerbose("Checking for previous DS-Client Validation View Sessions...");
            BackupSetValidationView previousValidationSession = SessionState.PSVariable.GetValue("ValidateView", null) as BackupSetValidationView;

            // If a previous session is found, remove it
            if (previousValidationSession != null)
            {
                WriteVerbose("Previous Validation View found, attempting to Dispose...");
                try
                {
                    previousValidationSession.Dispose();
                }
                catch
                {
                    WriteVerbose("Previous Session failed to Dispose, deleting session...");
                }
                SessionState.PSVariable.Remove("ValidateView");
            }

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
                SessionState.PSVariable.Remove("DSClientOSType");
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