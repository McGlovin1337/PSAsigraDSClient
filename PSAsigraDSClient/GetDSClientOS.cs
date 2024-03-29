﻿using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient.PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientOS")]
    [OutputType(typeof(DSClientOperatingSystem))]

    sealed public class GetDSClientOS: DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            WriteVerbose("Performing Action: Retrieve DS-Client Operating System Type");
            EOSFlavour osType = DSClientConfigMgr.getClientOSType();

            DSClientOperatingSystem dSclientOperatingSystem = new DSClientOperatingSystem(osType);

            WriteObject(dSclientOperatingSystem);

            DSClientConfigMgr.Dispose();
        }

        private class DSClientOperatingSystem
        {
            public string OperatingSystem { get; private set; }

            public DSClientOperatingSystem(EOSFlavour osType)
            {
                OperatingSystem = EnumToString(osType);
            }
        }
    }
}