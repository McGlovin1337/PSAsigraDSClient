using System.Linq;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientRestoreSession")]
    [OutputType(typeof(DSClientRestoreSession))]

    sealed public class GetDSClientRestoreSession : DSClientCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Select a specific Restore Session by Id")]
        public int RestoreId { get; set; }

        protected override void DSClientProcessRecord()
        {
            if (MyInvocation.BoundParameters.ContainsKey(nameof(RestoreId)))
                WriteObject(DSClientSessionInfo.GetRestoreSession(RestoreId));
            else
                DSClientSessionInfo.GetRestoreSessions()
                                    .ToList()
                                    .ForEach(WriteObject);
        }
    }
}