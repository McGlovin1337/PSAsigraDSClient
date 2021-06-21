using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommunications.Disconnect, "DSClientSession")]
    [OutputType(typeof(DSClientSession))]

    public class DisconnectDSClientSession : BaseDSClientSession
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Session to Disconnect")]
        public DSClientSession Session { get; set; }

        protected override void ProcessRecord()
        {
            WriteVerbose("Performing Action: Disconnect DS-Client Session");
            Session.Disconnect();

            WriteObject(Session);
        }
    }
}