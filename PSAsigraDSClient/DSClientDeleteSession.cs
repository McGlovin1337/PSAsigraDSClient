using System;
using System.Collections.Generic;
using System.Linq;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public class DSClientDeleteSession
    {
        private List<long> _selectedItemIds;
        private DeleteActivityInitiator _deleteActivityInitiator;
        private BackupSetDeleteView _deleteView;
        private readonly List<DSClientBackupSetItemInfo> _browsedItems;
        private readonly BackupSet _backupSet;

        public int DeleteId { get; private set; }
        public int BackupSetId { get; private set; }
        public DateTime DateFrom { get; private set; }
        public DateTime DateTo { get; private set; }
        public MoveToBLMOptions MoveToBLM { get; private set; }
        public DSClientBackupSetItemInfo[] SelectedItems { get; private set; }
        public int KeepGenerations { get; private set; }
        public string DeleteArchive { get; private set; }
        public string DataType { get; private set; }

        internal DSClientDeleteSession(int id, BackupSet backupSet)
        {
            _backupSet = backupSet;
            _browsedItems = new List<DSClientBackupSetItemInfo>();
            _selectedItemIds = new List<long>();
            _deleteActivityInitiator = null;
            _deleteView = backupSet.prepare_delete(0, DateTimeToUnixEpoch(DateTime.Now), 0, 0, DeleteArchiveOptions.DeleteArchiveOptions__Exclude);

            DeleteId = id;
            BackupSetId = backupSet.getID();
            DateFrom = DateTime.Parse("01/01/1970");
            DateTo = DateTime.Now;
            MoveToBLM = new MoveToBLMOptions();
            KeepGenerations = 0;
            DeleteArchive = "Exclude";
            DataType = EnumToString(backupSet.getDataType());
        }

        internal void AddBrowsedItem(DSClientBackupSetItemInfo item)
        {
            if (!_browsedItems.Exists(i => i.ItemId == item.ItemId))
                _browsedItems.Add(item);
        }

        internal void AddBrowsedItems(IEnumerable<DSClientBackupSetItemInfo> items)
        {
            // This method helps keep track of all the items discovered when calling Get-DSClientStoredItem Cmdlet
            // Since the BackedUpDataView class doesn't allow selecting items by id, this allows to select items by id, and view the details of each selected item when added to SelectedItems Property
            _browsedItems.AddRange(items.Except(_browsedItems));
        }

        internal void AddSelectedItem(long itemId)
        {
            if (!_selectedItemIds.Contains(itemId))
                _selectedItemIds.Add(itemId);

            SetSelectedItems();
        }

        internal void AddSelectedItems(long[] itemIds)
        {
            _selectedItemIds.AddRange(itemIds.Except(_selectedItemIds));

            SetSelectedItems();
        }

        internal void Dispose()
        {
            _backupSet.Dispose();

            if (_deleteActivityInitiator != null)
                _deleteActivityInitiator.Dispose();

            _deleteView.Dispose();
        }

        internal BackupSetDeleteView GetDeleteView()
        {
            return _deleteView;
        }

        internal void RemoveSelectedItems(long[] items)
        {
            foreach (long item in items)
                _selectedItemIds.Remove(item);

            SetSelectedItems();
        }

        internal void SetDataTimeRange(DateTime from, DateTime to)
        {
            // Sets the Time Range for which Data is Selected
            // Setting this will clear any existing selected items

            if (_deleteActivityInitiator != null)
                _deleteActivityInitiator.Dispose();

            _browsedItems.Clear();
            _selectedItemIds = null;
            SelectedItems = null;

            DateFrom = from;
            DateTo = to;
        }

        internal void SetKeepGenerations(int v)
        {
            KeepGenerations = v;

            SetDeleteView();
        }

        internal void SetDeleteArchive(DeleteArchiveOptions option)
        {
            DeleteArchive = EnumToString(option);

            SetDeleteView();
        }

        private void SetDeleteView()
        {
            _deleteView = _backupSet.prepare_delete(DateTimeToUnixEpoch(DateFrom), DateTimeToUnixEpoch(DateTo), 0, KeepGenerations, StringToEnum<DeleteArchiveOptions>(DeleteArchive));

            // Any existing itemId's will now be invalid after creating a new BackupSetDeleteView, so reset to null
            _selectedItemIds = null;

            SetSelectedItems();
        }

        internal void SetSelectedItems()
        {
            if (_deleteActivityInitiator != null)
                _deleteActivityInitiator.Dispose();

            long[] items = null;
            if (_selectedItemIds != null && _selectedItemIds.Count() > 0)
                items = _selectedItemIds.ToArray();

            if (items != null)
            {
                _deleteActivityInitiator = _deleteView.prepareDelete(items);

                SelectedItems = new DSClientBackupSetItemInfo[items.Length];
                for (int i = 0; i < items.Length; i++)
                    SelectedItems[i] = _browsedItems.Single(item => item.ItemId == items[i]);
            }
            else
            {
                SelectedItems = null;
                _deleteActivityInitiator = null;
            }
        }

        internal GenericActivity StartValidation()
        {
            if (_deleteActivityInitiator != null)
                return _deleteActivityInitiator.start();

            throw new Exception("Delete Session not Ready to Start");
        }

        public class MoveToBLMOptions
        {
            public bool MoveToBLM { get; private set; }
            public string Label { get; private set; }
            public bool NewPackage { get; private set; }

            internal MoveToBLMOptions()
            {
                MoveToBLM = false;
                Label = $"{DateTimeToUnixEpoch(DateTime.Now)}-Delete";
                NewPackage = true;
            }

            internal void SetMoveToBLM(bool v)
            {
                MoveToBLM = v;
            }

            internal void SetLabel(string label)
            {
                Label = label;
            }

            internal void SetNewPackage(bool v)
            {
                NewPackage = v;
            }

            public override string ToString()
            {
                return MoveToBLM.ToString();
            }
        }
    }
}