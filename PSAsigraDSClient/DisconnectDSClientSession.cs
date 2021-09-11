using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommunications.Disconnect, "DSClientSession", DefaultParameterSetName = "id")]
    [OutputType(typeof(void))]

    sealed public class DisconnectDSClientSession : BaseDSClientSessionAction
    {
        protected override void ProcessDSClientSessionAction(DSClientSession session)
        {
            if (session.State == DSClientSession.ConnectionState.Connected)
            {
                WriteVerbose($"Performing Action: Disconnect DS-Client Session '{session.Name}' with Id '{session.Id}'");
                session.Disconnect();
            }
        }
    }
}