using System;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientInitializeBackupSetDataBrowser: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "BackupSetId", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Backup Set to Search")]
        public int BackupSetId { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Date to Search From")]
        [Alias("StartTime")]
        public DateTime DateFrom { get; set; } = DateTime.Parse("1/1/1970");

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Date to Search To")]
        [Alias("EndTime")]
        public DateTime DateTo { get; set; } = DateTime.Now;

        [Parameter(HelpMessage = "Specify Date for Deleted Files from Source")]
        public DateTime DeletedDate { get; set; } = DateTime.Parse("1/1/1970");
    }
}