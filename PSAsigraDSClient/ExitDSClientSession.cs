using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Exit, "DSClientSession")]
    public class ExitDSClientSession: PSCmdlet
    {
        protected override void ProcessRecord()
        {
            // Remove any previous DataType from SessionState and store the current Backup Set DataType
            SessionState.PSVariable.Remove("RestoreType");

            // Check for a previous Backup Set Restore View stored in Session State
            WriteVerbose("Performing Action: Check for previous DS-Client Validation View Sessions");
            BackupSetRestoreView previousRestoreSession = SessionState.PSVariable.GetValue("RestoreView", null) as BackupSetRestoreView;

            // If a previous session is found, remove it
            if (previousRestoreSession != null)
            {
                WriteVerbose("Notice: Previous Restore View found");
                try
                {
                    WriteVerbose("Performing Action: Dispose Restore View");
                    previousRestoreSession.Dispose();
                }
                catch
                {
                    WriteVerbose("Notice: Previous Session failed to Dispose");
                }
                WriteVerbose("Performing Action: Remove on Restore View Session");
                SessionState.PSVariable.Remove("RestoreView");
            }

            // Check for a previous Backup Set Validation View stored in Session State
            WriteVerbose("Performing Action: Check for previous DS-Client Delete View Sessions");
            BackupSetDeleteView previousDeleteSession = SessionState.PSVariable.GetValue("DeleteView", null) as BackupSetDeleteView;

            // If a previous session is found, remove it
            if (previousDeleteSession != null)
            {
                WriteVerbose("Notice: Previous Delete View found");
                try
                {
                    WriteVerbose("Performing Action: Dispose Delete View");
                    previousDeleteSession.Dispose();
                }
                catch
                {
                    WriteVerbose("Notice: Previous Session failed to Dispose");
                }
                WriteVerbose("Performing Action: Remove on Delete View Session");
                SessionState.PSVariable.Remove("DeleteView");
            }

            // Check for a previous Backup Set Validation View stored in Session State
            WriteVerbose("Performing Action: Check for previous DS-Client Validation View Sessions");
            BackupSetValidationView previousValidationSession = SessionState.PSVariable.GetValue("ValidateView", null) as BackupSetValidationView;

            // If a previous session is found, remove it
            if (previousValidationSession != null)
            {
                WriteVerbose("Notice: Previous Validation View found");
                try
                {
                    WriteVerbose("Performing Action: Dispose Validation View");
                    previousValidationSession.Dispose();
                }
                catch
                {
                    WriteVerbose("Notice: Previous Session failed to Dispose");
                }
                WriteVerbose("Performing Action: Remove on Validation View Session");
                SessionState.PSVariable.Remove("ValidateView");
            }

            WriteVerbose("Performing Action: Check for existing DS-Client Sessions");
            ClientConnection previousSession = SessionState.PSVariable.GetValue("DSClientSession", null) as ClientConnection;

            if (previousSession != null)
            {
                WriteVerbose("Notice: Previous DS-Client Session found");
                try
                {
                    WriteVerbose("Performing Action: Logout DS-Client Session");
                    previousSession.logout();
                    WriteVerbose("Performing Action: Dispose DS-Client Session");
                    previousSession.Dispose();
                }
                catch
                {
                    WriteVerbose("Notice: Previous session failed to dispose");
                }
                WriteVerbose("Performing Action: Remove on DS-Client Session");
                SessionState.PSVariable.Remove("DSClientSession");
                SessionState.PSVariable.Remove("DSClientOSType");
                WriteObject("DS-Client Session removed.");
            }
            else
            {
                WriteVerbose("Notice: No previous DSClient Session found");
                WriteObject("DS-Client Session does not exist.");
            }
        }
    }
}