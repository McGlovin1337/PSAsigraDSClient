using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommunications.Connect, "DSClientSession", DefaultParameterSetName = "id")]
    [OutputType(typeof(void))]

    sealed public class ConnectDSClientSession : BaseDSClientSessionAction
    {
        protected override void ProcessDSClientSessionAction(DSClientSession session)
        {
            if (session.State == DSClientSession.ConnectionState.Disconnected)
            {
                WriteVerbose($"Performing Action: Connect DS-Client Session '{session.Name}' with Id '{session.Id}'");
                session.Connect();
            }
        }
    }
}