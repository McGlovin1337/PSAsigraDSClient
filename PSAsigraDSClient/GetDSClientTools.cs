using System;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientTools")]
    [OutputType(typeof(DSClientTools))]

    public class GetDSClientTools: DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            List<DSClientTools> DSClientTools = new List<DSClientTools>();

            WriteVerbose("Retrieving Status of DS-Tools...");
            foreach (EDSTools tool in Enum.GetValues(typeof(EDSTools)))
            {
                if (tool == EDSTools.EDSTools__UNDEFINED)
                    continue;

                DSClientTools dSClientTool = new DSClientTools(DSClientConfigMgr, tool);
                DSClientTools.Add(dSClientTool);
            }

            DSClientTools.ForEach(WriteObject);

            DSClientConfigMgr.Dispose();
        }

        private class DSClientTools
        {
            public string Tool { get; private set; }
            public bool Enabled { get; private set; }

            public DSClientTools(ClientConfiguration clientConfiguration, EDSTools tool)
            {
                Tool = EDSToolsToString(tool);
                Enabled = clientConfiguration.isDSToolEnabled(tool);
            }

            private string EDSToolsToString(EDSTools tool)
            {
                string Tool = null;

                switch(tool)
                {
                    case EDSTools.EDSTools__DSRecoveryTools:
                        Tool = "DSRecoveryTools";
                        break;
                    case EDSTools.EDSTools__LocalStorage:
                        Tool = "LocalStorage";
                        break;
                    case EDSTools.EDSTools__DiscTape:
                        Tool = "DiscTape";
                        break;
                    case EDSTools.EDSTools__DisableCommonFile:
                        Tool = "DisableCommonFile";
                        break;
                    case EDSTools.EDSTools__BackupLifecycleManagement:
                        Tool = "BackupLifecycleManagement";
                        break;
                    case EDSTools.EDSTools__LocalOnly:
                        Tool = "LocalOnly";
                        break;
                    case EDSTools.EDSTools__LocalDSVDR:
                        Tool = "LocalDSVDR";
                        break;
                    case EDSTools.EDSTools__RemoteDSVDR:
                        Tool = "RemoteDSVDR";
                        break;
                    case EDSTools.EDSTools__SnapshotManager:
                        Tool = "SnapshotManager";
                        break;
                }

                return Tool;
            }
        }
    }
}