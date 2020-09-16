﻿using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientBackupSetInfo", DefaultParameterSetName =  "General")]
    [OutputType(typeof(DSClientBackupSetInfo))]
    public class GetDSClientBackupSetInfo: BaseDSClientBackupSetInfo
    {
        [Parameter(Position = 0, ParameterSetName = "Id", Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Id of the Backup Set")]
        [ValidateNotNullOrEmpty]
        public int BackupSetId { get; set; }

        [Parameter(Position = 0, ParameterSetName = "General", ValueFromPipelineByPropertyName = true, HelpMessage = "Backup Sets configured for specified Computer")]
        public string Computer { get; set; }

        [Parameter(Position = 0, ParameterSetName = "Name", Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Backup Sets with the given Name")]
        public string Name { get; set; }

        [Parameter(Position = 1, ParameterSetName = "General", ValueFromPipelineByPropertyName = true, HelpMessage = "List Backup Sets of a specific Data Type")]
        [ValidateSet("FileSystem",
            "MSSQLServer",
            "MSExchange",
            "Oracle",
            "PermissionsOnly",
            "MSExchangeItemLevel",
            "OutlookEmail",
            "SystemI",
            "MySQL",
            "PostgreSQL",
            "DB2",
            "LotusNotesEmail",
            "GroupWiseEmail",
            "SharepointItemLevel",
            "VMWareVMDK",
            "XenServer",
            "Sybase",
            "HyperVServer",
            "VMWareVADP",
            "MSSQLServerVSS",
            "MSExchangeVSS",
            "SharepointVSS",
            "SalesForce",
            "GoogleApps",
            "Office365",
            "OracleSBT",
            "LotusDomino",
            "HyperVCluster",
            "P2V",
            "MSExchangeEWS")]
        public string DataType { get; set; }

        [Parameter(Position = 2, ParameterSetName = "General", HelpMessage = "List Active/Inactive Backup Sets")]
        public SwitchParameter Active { get; set; }

        [Parameter(Position = 3, ParameterSetName = "General", HelpMessage = "List Synchronized/Unsynchronized Backup Sets")]
        public SwitchParameter Synchronized { get; set; }

        [Parameter(Position = 4, ParameterSetName = "General", ValueFromPipelineByPropertyName = true, HelpMessage = "List Backup Sets using the specified ScheduleId")]
        public int ScheduleId { get; set; }

        [Parameter(Position = 5, ParameterSetName = "General", ValueFromPipelineByPropertyName = true, HelpMessage = "List Backup Sets using the specified RetentionId")]
        public int RetentionRuleId { get; set; }

        [Parameter(Position = 6, ParameterSetName = "General", ValueFromPipelineByPropertyName = true, HelpMessage = "List Backup Sets of a specific type")]
        [ValidateSet("Offsite", "Statistical", "SelfContained", "LocalOnly")]
        public string SetType { get; set; }

        [Parameter(Position = 7, ParameterSetName = "General", HelpMessage = "List Backup Sets using Local Storage")]
        public SwitchParameter UseLocalStorage { get; set; }

        [Parameter(Position = 8, ParameterSetName = "General", HelpMessage = "List CDP Backup Sets")]
        public SwitchParameter IsCDP { get; set; }

        [Parameter(Position = 9, ParameterSetName = "General", HelpMessage = "List Backup Sets created by policy")]
        public SwitchParameter CreatedByPolicy { get; set; }

        protected override void ProcessBackupSet(IEnumerable<DSClientBackupSetInfo> dSClientBackupSetsInfo)
        {
            WildcardOptions wcOptions = WildcardOptions.IgnoreCase |
                                        WildcardOptions.Compiled;

            if (MyInvocation.BoundParameters.ContainsKey("Id"))
                dSClientBackupSetsInfo = dSClientBackupSetsInfo.Where(set => set.BackupSetId == BackupSetId);

            if (Computer != null)
            {
                WildcardPattern wcPattern = new WildcardPattern(Computer, wcOptions);
                dSClientBackupSetsInfo = dSClientBackupSetsInfo.Where(set => wcPattern.IsMatch(set.Computer));
            }

            if (Name != null)
            {
                WildcardPattern wcPattern = new WildcardPattern(Name, wcOptions);
                dSClientBackupSetsInfo = dSClientBackupSetsInfo.Where(set => wcPattern.IsMatch(set.Name));
            }

            if (DataType != null)            
                dSClientBackupSetsInfo = dSClientBackupSetsInfo.Where(set => set.DataType == DataType);

            if (MyInvocation.BoundParameters.ContainsKey("Active"))
                dSClientBackupSetsInfo = dSClientBackupSetsInfo.Where(set => set.Active == Active);

            if (MyInvocation.BoundParameters.ContainsKey("Synchronized"))
                dSClientBackupSetsInfo = dSClientBackupSetsInfo.Where(set => set.Synchronized == Synchronized);

            if (MyInvocation.BoundParameters.ContainsKey("ScheduleId"))
                dSClientBackupSetsInfo = dSClientBackupSetsInfo.Where(set => set.ScheduleId == ScheduleId);

            if (MyInvocation.BoundParameters.ContainsKey("RetentionRuleId"))
                dSClientBackupSetsInfo = dSClientBackupSetsInfo.Where(set => set.RetentionRuleId == RetentionRuleId);

            if (SetType != null)
                dSClientBackupSetsInfo = dSClientBackupSetsInfo.Where(set => set.SetType == SetType);

            if (MyInvocation.BoundParameters.ContainsKey("UseLocalStorage"))
                dSClientBackupSetsInfo = dSClientBackupSetsInfo.Where(set => set.UseLocalStorage == UseLocalStorage);

            if (MyInvocation.BoundParameters.ContainsKey("IsCDP"))
                dSClientBackupSetsInfo = dSClientBackupSetsInfo.Where(set => set.IsCDP == IsCDP);

            if (MyInvocation.BoundParameters.ContainsKey("CreatedByPolicy"))
                dSClientBackupSetsInfo = dSClientBackupSetsInfo.Where(set => set.CreatedByPolicy == CreatedByPolicy);

            dSClientBackupSetsInfo.ToList().ForEach(WriteObject);
        }
    }
}