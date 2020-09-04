using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientRunningActivity")]
    [OutputType(typeof(DSClientRunningActivity))]    
    public class GetDSClientRunningActivity: BaseDSClientRunningActivity
    {
        protected override void ProcessRunningActivity(IEnumerable<DSClientRunningActivity> dSClientRunningActivities)
        {
            dSClientRunningActivities.ToList().ForEach(WriteObject);
        }
    }
}