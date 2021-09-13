using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientMSSQLDatabaseRestoreMapping")]
    [OutputType(typeof(MSSQLDatabaseMap))]

    sealed public class NewDSClientMSSQLDatabaseRestoreMapping : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Source Database")]
        [ValidateNotNullOrEmpty]
        public string SourceDatabase { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Destination Database to Map Source Databse to")]
        [ValidateNotNullOrEmpty]
        public string DestinationDatabase { get; set; }

        protected override void ProcessRecord()
        {
            MSSQLDatabaseMap databaseMapping = new MSSQLDatabaseMap(SourceDatabase, DestinationDatabase);

            WriteObject(databaseMapping);
        }
    }
}