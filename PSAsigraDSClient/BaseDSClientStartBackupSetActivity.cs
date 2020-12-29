using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientStartBackupSetActivity: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Id", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set Id")]
        public int BackupSetId { get; set; }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Name", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Backup Set(s) by Name")]
        [SupportsWildcards]
        public string[] Name { get; set; }

        protected virtual void ProcessBackupSet(BackupSet backupSet)
        {
            throw new NotImplementedException("ProcessBackupSet Method should be overriden");
        }

        protected virtual void ProcessBackupSets(BackupSet[] backupSets)
        {
            throw new NotImplementedException("ProcessBackupSets Method should be overriden");
        }

        protected override void DSClientProcessRecord()
        {
            if (MyInvocation.BoundParameters.ContainsKey("BackupSetId"))
            {
                WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId: {BackupSetId}");
                BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

                ProcessBackupSet(backupSet);

                backupSet.Dispose();
            }
            else if (MyInvocation.BoundParameters.ContainsKey("Name"))
            {
                WildcardOptions wcOptions = WildcardOptions.IgnoreCase |
                                        WildcardOptions.Compiled;

                IEnumerable<BackupSet> backupSets = new List<BackupSet>();
                foreach (string name in Name)
                {
                    WildcardPattern wcPattern = new WildcardPattern(name, wcOptions);
                    IEnumerable<BackupSet> sets = DSClientSession.backup_sets().Where(set => wcPattern.IsMatch(set.getName()));
                    backupSets = backupSets.Concat(sets);
                }
                WriteVerbose($"Notice: Yielded {backupSets.Count()} Backup Sets");

                ProcessBackupSets(backupSets.ToArray());

                backupSets.ToList().ForEach(set => set.Dispose());
            }
        }

        protected class DSClientStartBackupSetActivity
        {
            public int ActivityId { get; set; }
            public int BackupSetId { get; set; }
            public string Name { get; set; }
        }
    }
}