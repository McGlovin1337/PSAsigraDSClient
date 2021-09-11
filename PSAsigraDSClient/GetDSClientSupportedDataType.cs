using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientSupportedDataType")]
    [OutputType(typeof(DSClientDataType))]

    sealed public class GetDSClientSupportedDataType: DSClientCmdlet
    {
        [Parameter(HelpMessage = "Specify to Get the Data Types Supported by the DS-Client API")]
        public SwitchParameter ApiDataTypes { get; set; }

        protected override void DSClientProcessRecord()
        {
            EBackupDataType[] dataTypes;

            if (ApiDataTypes)
            {
                WriteVerbose("Performing Action: Retrieve API Supported Data Types");
                dataTypes = DSClientSession.getAPIDataTypesSupport();
            }
            else
            {
                WriteVerbose("Performing Action: Retrieve DS-Client Supported Data Types");
                dataTypes = DSClientSession.getClientDataTypesSupport();
            }

            List<DSClientDataType> DataTypes = new List<DSClientDataType>();

            foreach (EBackupDataType dataType in dataTypes)
                DataTypes.Add(new DSClientDataType(dataType));

            DataTypes.ForEach(WriteObject);
        }

        private class DSClientDataType
        {
            public string DataType { get; private set; }

            public DSClientDataType(EBackupDataType dataType)
            {
                DataType = BaseDSClientBackupSet.EBackupDataTypeToString(dataType);
            }
        }
    }
}