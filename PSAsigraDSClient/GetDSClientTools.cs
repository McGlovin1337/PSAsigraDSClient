using System;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

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
                Tool = EnumToString(tool);
                Enabled = clientConfiguration.isDSToolEnabled(tool);
            }
        }
    }
}