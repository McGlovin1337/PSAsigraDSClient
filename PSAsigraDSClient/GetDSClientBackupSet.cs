using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientBackupSet")]
    [OutputType(typeof(DSClientBackupSet))]

    public class GetDSClientBackupSet: BaseDSClientBackupSet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Backup Set Id")]
        [ValidateNotNullOrEmpty]
        public int Id { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Retrieving Backup Set Configuration...");
            BackupSet backupSet = DSClientSession.backup_set(Id);

            DSClientBackupSet dSClientBackupSet = new DSClientBackupSet(backupSet, DSClientOSType);

            WriteObject(dSClientBackupSet);
        }
    }
}