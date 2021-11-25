using System.Linq;
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
        public DSClientCredential Credential { get; set; }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify an initial Path to browse")]
        public string Path { get; set; }

        [Parameter(HelpMessage = "Specify if items should be returned recursively")]
        public SwitchParameter Recursive { get; set; }

        [Parameter(HelpMessage = "Specify the recursive depth")]
        public int RecursiveDepth { get; set; } = 0;

        protected class SourceItemInfo
        {
            public string Path { get; private set; }
            public string Name { get; private set; }
            public bool IsFile { get; private set; }
            public DSClientStorageUnit Size { get; private set; }
            public string Type { get; private set; }

            public SourceItemInfo(string path, browse_item_info item)
            {
                Path = path;
                Name = item.name;
                IsFile = item.isfile;
                Size = new DSClientStorageUnit(item.datasize);
                Type = EBrowseItemTypeToString(item.type);
            }
        }

        protected class SourceMSSqlItemInfo
        {
            public string Instance { get; private set; }
            public string Name { get; private set; }
            public string[] DataPath { get; private set; }
            public string[] LogPath { get; private set; }

            public SourceMSSqlItemInfo(mssql_db_path databaseItem)
            {
                mssql_path_item[] dbPaths = databaseItem.files_path;

                Instance = databaseItem.instance;
                Name = databaseItem.destination_db;
                DataPath = dbPaths.Where(data => data.is_data)
                    .Select(path => path.path)
                    .ToArray();
                LogPath = dbPaths.Where(log => !log.is_data)
                    .Select(path => path.path)
                    .ToArray();
            }
        }

        public class ItemPath
        {
            public string Path { get; private set; }
            public int Depth { get; private set; }

            public ItemPath(string path, int depth)
            {
                Path = path;
                Depth = depth;
            }
        }

        public static string EBrowseItemTypeToString(EBrowseItemType itemType)
        {
            switch (itemType)
            {
                case EBrowseItemType.EBrowseItemType__Drive:
                    return "Drive";
                case EBrowseItemType.EBrowseItemType__Share:
                    return "Share";
                case EBrowseItemType.EBrowseItemType__Directory:
                    return "Directory";
                case EBrowseItemType.EBrowseItemType__File:
                    return "File";
                case EBrowseItemType.EBrowseItemType__SystemState:
                    return "SystemState";
                case EBrowseItemType.EBrowseItemType__ServicesDB:
                    return "ServicesDatabase";
                case EBrowseItemType.EBrowseItemType__DatabaseInstance:
                    return "DatabaseInstance";
                case EBrowseItemType.EBrowseItemType__Database:
                    return "Database";
                case EBrowseItemType.EBrowseItemType__Tablespace:
                    return "OracleTablespace";
                case EBrowseItemType.EBrowseItemType__ControlFile:
                    return "OracleControlFile";
                case EBrowseItemType.EBrowseItemType__ArchiveLog:
                    return "OracleArchiveLog";
                case EBrowseItemType.EBrowseItemType__VirtualMachine:
                    return "VirtualMachine";
                case EBrowseItemType.EBrowseItemType__VssExchange:
                    return "VssExchange";
                case EBrowseItemType.EBrowseItemType__VmDisk:
                    return "VirtualMachineDisk";
                default:
                    return null;
            }
        }
    }
}