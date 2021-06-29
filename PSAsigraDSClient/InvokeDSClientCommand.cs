using System.Collections.ObjectModel;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Invoke, "DSClientCommand")]

    sealed public class InvokeDSClientCommand : BaseDSClientSessionAction
    {
        [Parameter(Mandatory = true, HelpMessage = "Specify Command(s) to Execute")]
        public ScriptBlock ScriptBlock { get; set; }

        protected override void ProcessDSClientSessionAction(DSClientSession session)
        {
            SessionState.PSVariable.Set("DSClientSession", session);

            Collection<PSObject> execute = ScriptBlock.Invoke();

            foreach (PSObject obj in execute)
                WriteObject(obj);

            SessionState.PSVariable.Remove("DSClientSession");
        }
    }
}