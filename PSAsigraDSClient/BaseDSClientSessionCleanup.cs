using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientSessionCleanup: PSCmdlet
    {
        protected abstract void ProcessCleanSession();

        protected override void ProcessRecord()
        {
            // Remove any Restore Data Types from Session State
            SessionState.PSVariable.Remove("RestoreType");

            // Remove any Time Retention Options from Session State
            SessionState.PSVariable.Remove("TimeRetention");

            // Remove any Schedule Details from Session State
            SessionState.PSVariable.Remove("ScheduleDetail");

            // Check for a previous Backup Set Restore View stored in Session State
            WriteVerbose("Performing Action: Check for previous DS-Client Restore View Sessions");

            // If a previous session is found, remove it
            if (SessionState.PSVariable.GetValue("RestoreView", null) is BackupSetRestoreView previousRestoreSession)
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

            // Check for a previous Backup Set Delete View stored in Session State
            WriteVerbose("Performing Action: Check for previous DS-Client Delete View Sessions");

            // If a previous session is found, remove it
            if (SessionState.PSVariable.GetValue("DeleteView", null) is BackupSetDeleteView previousDeleteSession)
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

            // If a previous session is found, remove it
            if (SessionState.PSVariable.GetValue("ValidateView", null) is BackupSetValidationView previousValidationSession)
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