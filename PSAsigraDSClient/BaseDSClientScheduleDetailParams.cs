using System;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientScheduleDetailParams: BaseDSClientSchedule
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The ScheduleId to add Schedule Detail to")]
        [ValidateNotNullOrEmpty]
        public int ScheduleId { get; set; }

        [Parameter(HelpMessage = "Set the Start Date for this Schedule Detail")]
        [ValidateNotNullOrEmpty]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Parameter(HelpMessage = "Set the End Date for this Schedule Detail")]
        [ValidateNotNullOrEmpty]
        public DateTime EndDate { get; set; }

        [Parameter(HelpMessage = "Specify the Start Time in 24Hr Notation HH:mm:ss")]
        [ValidateNotNullOrEmpty]
        public string StartTime { get; set; }

        [Parameter(HelpMessage = "Specify the End Time in 24Hr Notation HH:mm:ss")]
        [ValidateNotNullOrEmpty]
        public string EndTime { get; set; } = "35:00:00";

        [Parameter(HelpMessage = "Specify No End Time")]
        public SwitchParameter NoEndTime { get; set; }

        [Parameter(HelpMessage = "Set the Hourly Frequency")]
        [ValidateNotNullOrEmpty]
        public int HourlyFrequency { get; set; } = 0;

        [Parameter(HelpMessage = "Enable the Backup Task")]
        public SwitchParameter Backup { get; set; }

        [Parameter(HelpMessage = "Enable the Retention Task")]
        public SwitchParameter Retention { get; set; }

        [Parameter(HelpMessage = "Enable the Validation Task")]
        public SwitchParameter Validation { get; set; }

        [Parameter(HelpMessage = "Enable the BLM Task")]
        public SwitchParameter BLM { get; set; }

        [Parameter(HelpMessage = "Enable the LAN Scan Shares Task")]
        public SwitchParameter LANScan { get; set; }

        [Parameter(HelpMessage = "Enable the Clean Local-only Trash Task")]
        public SwitchParameter CleanTrash { get; set; }

        [Parameter(HelpMessage = "Validation Task Option: Validate Last Generation Only")]
        public SwitchParameter LastGenOnly { get; set; }

        [Parameter(HelpMessage = "Validation Task Option: Exclude Deleted Files from Source")]
        public SwitchParameter ExcludeDeleted { get; set; }

        [Parameter(HelpMessage = "Validation Task Option: Resume from previously interrupted location")]
        public SwitchParameter Resume { get; set; }

        [Parameter(HelpMessage = "BLM Task Option: Specify if to Include All Generations")]
        public SwitchParameter IncludeAllGenerations { get; set; }

        [Parameter(HelpMessage = "BLM Task Option: Specify if to use Back Referencing")]
        public SwitchParameter BackReference { get; set; }

        [Parameter(HelpMessage = "BLM Task Option: Specify how the package should be closed")]
        [ValidateSet("DoNotClose", "CloseAtStart", "CloseAtEnd")]
        public string PackageClosing { get; set; }
    }
}