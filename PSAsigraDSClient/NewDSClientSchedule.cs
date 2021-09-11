using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.New, "DSClientSchedule")]
    [OutputType(typeof(DSClientScheduleInfo))]

    sealed public class NewDSClientSchedule: BaseDSClientSchedule
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, HelpMessage = "The name of the Schedule")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Position = 1, HelpMessage = "Set a Short Name")]
        [ValidateNotNullOrEmpty]
        public string ShortName { get; set; }

        [Parameter(Position = 2, HelpMessage = "Set Backup CPU Throttle")]
        public int CPUThrottle { get; set; }

        [Parameter(Position = 3, HelpMessage = "Concurrent backup sets allowed")]
        public int ConcurrentBackups { get; set; }

        [Parameter(HelpMessage = "Specifies only Administrators can use this schedule")]
        public SwitchParameter AdminOnly { get; set; }

        [Parameter(HelpMessage = "Set the Schedule to Inactive")]
        public SwitchParameter Inactive { get; set; }

        [Parameter(HelpMessage = "Start only if DS-System Connection available")]
        public SwitchParameter UseNetworkDetection { get; set; }

        [Parameter(HelpMessage = "Specify to return Schedule Info")]
        public SwitchParameter PassThru { get; set; }

        protected override void DSClientProcessRecord()
        {
            ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();

            // Build a new Schedule
            WriteVerbose("Performing Action: Build new Schedule object");
            Schedule newSchedule = DSClientScheduleMgr.createSchedule();

            newSchedule.setName(Name);

            if (ShortName != null)
                newSchedule.setShortName(ShortName);

            if (MyInvocation.BoundParameters.ContainsKey("CPUThrottle"))
                newSchedule.setBackupCPUThrottle(CPUThrottle);

            if (MyInvocation.BoundParameters.ContainsKey("ConcurrentBackups"))
                newSchedule.setConcurrentBackupSets(ConcurrentBackups);

            if (MyInvocation.BoundParameters.ContainsKey("AdminOnly"))
                newSchedule.setAdministratorsOnly(AdminOnly);

            if (MyInvocation.BoundParameters.ContainsKey("Inactive"))
                newSchedule.setActive(!Inactive);

            if (MyInvocation.BoundParameters.ContainsKey("UseNetworkDetection"))
                newSchedule.setUsingNetworkDetection(UseNetworkDetection);

            // Apply the new Schedule
            WriteVerbose("Performing Action: Add new Schedule to DS-Client");
            DSClientScheduleMgr.addSchedule(newSchedule);

            if (PassThru)
            {
                DSClientScheduleInfo scheduleInfo = new DSClientScheduleInfo(DSClientScheduleMgr.definedScheduleInfo(newSchedule.getID()));
                WriteObject(scheduleInfo);
            }

            DSClientScheduleMgr.Dispose();
        }
    }
}