using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientBackupSetDelResVal: DSClientCmdlet
    {
        [Parameter(Position = 0, ParameterSetName = "Selective", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the items to validate")]
        [ValidateNotNullOrEmpty]
        [Alias("Path")]
        public string[] Items { get; set; }

        [Parameter(Position = 1, ParameterSetName = "Selective", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the items to validate by ItemId")]
        public long[] ItemId { get; set; }
    }
}