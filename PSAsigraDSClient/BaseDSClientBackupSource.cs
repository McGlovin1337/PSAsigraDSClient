using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientBackupSource: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Source Computer")]
        [ValidateNotNullOrEmpty]
        public string Computer { get; set; }

        [Parameter(HelpMessage = "Specify Credentials for specified Computer")]
        public PSCredential Credential { get; set; }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify an initial Path to browse")]
        public string Path { get; set; }

        [Parameter(HelpMessage = "Specify if items should be returned recursively")]
        public SwitchParameter Recursive { get; set; }

        [Parameter(HelpMessage = "Specify the recursive depth")]
        public int RecursiveDepth { get; set; } = 0;

        protected class SourceItemInfo
        {
            public string Path { get; set; }
            public string Name { get; set; }
            public bool IsFile { get; set; }
            public long Size { get; set; }
            public string Type { get; set; }

            public SourceItemInfo(string path, browse_item_info item)
            {
                Path = path;
                Name = item.name;
                IsFile = item.isfile;
                Size = item.datasize;
                Type = EBrowseItemTypeToString(item.type);
            }
        }

        public class ItemPath
        {
            public string Path { get; set; }
            public int Depth { get; set; }

            public ItemPath(string path, int depth)
            {
                Path = path;
                Depth = depth;
            }
        }

        public static string EBrowseItemTypeToString(EBrowseItemType itemType)
        {
            string ItemType = null;

            switch (itemType)
            {
                case EBrowseItemType.EBrowseItemType__Drive:
                    ItemType = "Drive";
                    break;
                case EBrowseItemType.EBrowseItemType__Share:
                    ItemType = "Share";
                    break;
                case EBrowseItemType.EBrowseItemType__Directory:
                    ItemType = "Directory";
                    break;
                case EBrowseItemType.EBrowseItemType__File:
                    ItemType = "File";
                    break;
                case EBrowseItemType.EBrowseItemType__SystemState:
                    ItemType = "SystemState";
                    break;
                case EBrowseItemType.EBrowseItemType__ServicesDB:
                    ItemType = "ServicesDatabase";
                    break;
                case EBrowseItemType.EBrowseItemType__DatabaseInstance:
                    ItemType = "DatabaseInstance";
                    break;
                case EBrowseItemType.EBrowseItemType__Database:
                    ItemType = "Database";
                    break;
                case EBrowseItemType.EBrowseItemType__Tablespace:
                    ItemType = "OracleTablespace";
                    break;
                case EBrowseItemType.EBrowseItemType__ControlFile:
                    ItemType = "OracleControlFile";
                    break;
                case EBrowseItemType.EBrowseItemType__ArchiveLog:
                    ItemType = "OracleArchiveLog";
                    break;
                case EBrowseItemType.EBrowseItemType__VirtualMachine:
                    ItemType = "VirtualMachine";
                    break;
                case EBrowseItemType.EBrowseItemType__VssExchange:
                    ItemType = "VssExchange";
                    break;
                case EBrowseItemType.EBrowseItemType__VmDisk:
                    ItemType = "VirtualMachineDisk";
                    break;
            }

            return ItemType;
        }
    }
}