﻿using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientDefaultConfiguration")]
    [OutputType(typeof(DSClientDefaultConfiguration))]

    sealed public class GetDSClientDefaultConfiguration: BaseDSClientDefaultConfiguration
    {
        protected override void ProcessDefaultConfiguration(DSClientDefaultConfiguration dSClientDefaultConfiguration)
        {
            WriteObject(dSClientDefaultConfiguration);
        }
    }
}