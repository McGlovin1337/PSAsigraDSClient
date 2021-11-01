using AsigraDSClientApi;
using static PSAsigraDSClient.BaseDSClientBackupSource;

namespace PSAsigraDSClient
{
    public class DSClientBackupSetItemInfo
    {
        public long ItemId { get; private set; }
        public string Path { get; private set; }
        public string Name { get; private set; }
        public string DataType { get; private set; }
        public DSClientStorageUnit DataSize { get; private set; }
        public int FileCount { get; private set; }
        public bool IsFile { get; private set; }
        public bool Selectable { get; private set; }

        internal DSClientBackupSetItemInfo(string path, SelectableItem item, selectable_size itemSize)
        {
            ItemId = item.id;
            Path = path;
            Name = item.name;
            DataType = EBrowseItemTypeToString(item.data_type);
            DataSize = new DSClientStorageUnit(itemSize.data_size);
            FileCount = itemSize.file_count;
            IsFile = item.is_file;
            Selectable = item.is_selectable;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}