using System.IO;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using System.Linq;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsData.Export, "DSClientConfig")]

    public class ExportDSClientConfig : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify where to output the configuration")]
        public string Path { get; set; }

        [Parameter(HelpMessage = "Export DS-Client and Account Encryption Keys")]
        public SwitchParameter IncludeEncryptionKeys { get; set; }

        [Parameter(HelpMessage = "Use a Meta Symbol to allow XML to be used in different localisations (Windows Only)")]
        public SwitchParameter UseMetaSymbol { get; set; }

        [Parameter(HelpMessage = "Specify Output to use UTF16 Format (Default is UTF8)")]
        public SwitchParameter UseUTF16 { get; set; }

        [Parameter(HelpMessage = "Include DS-Client Configuration in Export")]
        public SwitchParameter IncludeConfig { get; set; }

        [Parameter(HelpMessage = "Include All Backup Sets")]
        public SwitchParameter IncludeAllBackupSets { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Include specific Backup Sets")]
        public int[] BackupSetId { get; set; }

        [Parameter(HelpMessage = "Include All Schedules")]
        public SwitchParameter IncludeAllSchedules { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Include specific Schedules")]
        public int[] ScheduleId { get; set; }

        [Parameter(HelpMessage = "Include All Retention Rules")]
        public SwitchParameter IncludeAllRetentionRules { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Include specific Retention Rules")]
        public int[] RetentionRuleId { get; set; }

        protected override void DSClientProcessRecord()
        {
            List<XMLConfigSelection> configSelections = new List<XMLConfigSelection>();

            if (!MyInvocation.BoundParameters.ContainsKey("RetentionRuleId") &&
                !MyInvocation.BoundParameters.ContainsKey("ScheduleId") &&
                !MyInvocation.BoundParameters.ContainsKey("BackupSetId") &&
                !IncludeAllRetentionRules &&
                !IncludeAllSchedules &&
                !IncludeAllBackupSets &&
                !IncludeConfig)
            {
                WriteVerbose("Notice: Selected Full DS-Client Configuration");
                configSelections.Add(new XMLConfigSelection
                {
                    id = 0,
                    type = EXmlConfigSelectionType.EXmlConfigSelectionType__All
                });
            }
            else
            {
                if (MyInvocation.BoundParameters.ContainsKey("RetentionRuleId"))
                    foreach (int rule in RetentionRuleId)
                    {
                        WriteVerbose($"Notice: Selected RetentionRuleId '{rule}'");
                        configSelections.Add(new XMLConfigSelection
                        {
                            id = rule,
                            type = EXmlConfigSelectionType.EXmlConfigSelectionType__Rettention
                        });
                    }
                else if (IncludeAllRetentionRules)
                {
                    WriteVerbose("Notice: Selected All Retention Rules");
                    configSelections.Add(new XMLConfigSelection
                    {
                        id = 0,
                        type = EXmlConfigSelectionType.EXmlConfigSelectionType__AllRettentions
                    });
                }

                if (MyInvocation.BoundParameters.ContainsKey("ScheduleId"))
                    foreach (int schedule in ScheduleId)
                    {
                        WriteVerbose($"Notice: Selected ScheduleId '{schedule}'");
                        configSelections.Add(new XMLConfigSelection
                        {
                            id = schedule,
                            type = EXmlConfigSelectionType.EXmlConfigSelectionType__Schedule
                        });
                    }
                else if (IncludeAllSchedules)
                {
                    WriteVerbose("Notice: Selected All Schedules");
                    configSelections.Add(new XMLConfigSelection
                    {
                        id = 0,
                        type = EXmlConfigSelectionType.EXmlConfigSelectionType__AllSchedules
                    });
                }

                if (MyInvocation.BoundParameters.ContainsKey("BackupSetId"))
                    foreach (int set in BackupSetId)
                    {
                        WriteVerbose($"Notice: Selected BackupSetId '{set}'");
                        configSelections.Add(new XMLConfigSelection
                        {
                            id = set,
                            type = EXmlConfigSelectionType.EXmlConfigSelectionType__BackupSet
                        });
                    }
                else if (IncludeAllBackupSets)
                {
                    WriteVerbose("Notice: Selected All Backup Sets");
                    configSelections.Add(new XMLConfigSelection
                    {
                        id = 0,
                        type = EXmlConfigSelectionType.EXmlConfigSelectionType__AllBackupSets
                    });
                }

                if (IncludeConfig)
                {
                    WriteVerbose("Notice: Selected DS-Client Configuration");
                    configSelections.Add(new XMLConfigSelection
                    {
                        id = 0,
                        type = EXmlConfigSelectionType.EXmlConfigSelectionType__Configuration
                    });
                }
            }

            if (IncludeEncryptionKeys)
            {
                if (!configSelections.Any(cfg => cfg.type == EXmlConfigSelectionType.EXmlConfigSelectionType__All || cfg.type == EXmlConfigSelectionType.EXmlConfigSelectionType__Configuration))
                {
                    WriteWarning("Encryption Keys only included with Configuration Selection");
                    IncludeEncryptionKeys = false;
                }
            }

            WriteVerbose($"Notice: Encryption Keys Included: {IncludeEncryptionKeys}");
            if (UseUTF16)
                WriteVerbose($"Notice: Export Format = UTF16, Using Meta Symbol = {UseMetaSymbol}");
            else
                WriteVerbose($"Notice: Export Format = UTF8, Using Meta Symbol = {UseMetaSymbol}");

            XMLConfigContent configContent = new XMLConfigContent
            {
                bEncryptionKey = IncludeEncryptionKeys,
                bMetaSymbol = UseMetaSymbol,
                bUTF16 = UseUTF16,
                selection = configSelections.ToArray()
            };

            string xmlData = DSClientSession.saveConfigToXML(configContent);

            WriteVerbose($"Performing Action: Write Selected Configuration to '{Path}'");
            File.WriteAllText(Path, xmlData);
        }
    }
}