using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientBackupSet")]
    [OutputType(typeof(DSClientBackupSetInfo))]
    public class GetDSClientBackupSet: BaseDSClientBackupSet
    {
        protected override void ProcessBackupSet(IEnumerable<DSClientBackupSetInfo> dSClientBackupSetsInfo)
        {
            dSClientBackupSetsInfo.ToList().ForEach(WriteObject);
        }
    }
}