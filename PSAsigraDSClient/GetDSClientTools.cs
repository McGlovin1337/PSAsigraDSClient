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
                switch(tool)
                {
                    case EDSTools.EDSTools__DSRecoveryTools:
                        return "DSRecoveryTools";
                    case EDSTools.EDSTools__LocalStorage:
                        return "LocalStorage";
                    case EDSTools.EDSTools__DiscTape:
                        return "DiscTape";
                    case EDSTools.EDSTools__DisableCommonFile:
                        return "DisableCommonFile";
                    case EDSTools.EDSTools__BackupLifecycleManagement:
                        return "BackupLifecycleManagement";
                    case EDSTools.EDSTools__LocalOnly:
                        return "LocalOnly";
                    case EDSTools.EDSTools__LocalDSVDR:
                        return "LocalDSVDR";
                    case EDSTools.EDSTools__RemoteDSVDR:
                        return "RemoteDSVDR";
                    case EDSTools.EDSTools__SnapshotManager:
                        return "SnapshotManager";
                    default:
                        return null;
                }
            }
        }
    }
}