using System;
using System.Collections.Generic;
using System.Linq;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public class DSClientValidationSession
    {        
        private List<long> _selectedItemIds;
        private ValidationActivityInitiator _validationActivityInitiator;
        private BackupSetValidationView _validationView;
        private readonly List<DSClientBackupSetItemInfo> _browsedItems;

        public int ValidationId { get; private set; }
        public int BackupSetId { get; private set; }
        public bool SelectiveValidation { get; private set; }
        public DSClientBackupSetItemInfo[] SelectedItems { get; private set; }
        public string DataType { get; private set; }

        internal DSClientValidationSession(int id, BackupSet backupSet)
        {
            _browsedItems = new List<DSClientBackupSetItemInfo>();
            _selectedItemIds = new List<long>();
            _validationActivityInitiator = null;
            _validationView = backupSet.prepare_validation(0, DateTimeToUnixEpoch(DateTime.Now), 0);

            ValidationId = id;
            BackupSetId = backupSet.getID();
            SelectiveValidation = false;
            DataType = EnumToString(backupSet.getDataType());

            backupSet.Dispose();
        }

        internal void AddBrowsedItems(IEnumerable<DSClientBackupSetItemInfo> items)
        {
            // This method helps keep track of all the items discovered when calling Get-DSClientStoredItem Cmdlet
            // Since the BackedUpDataView class doesn't allow selecting items by id, this allows to select items by id, and view the details of each selected item when added to SelectedItems Property
            _browsedItems.AddRange(items.Except(_browsedItems));
        }

        internal void AddSelectedItems(long[] itemIds)
        {
            _selectedItemIds.AddRange(itemIds.Except(_selectedItemIds));

            SetSelectedItems();
        }

        internal void Dispose()
        {
            _validationActivityInitiator.Dispose();
            _validationView.Dispose();
        }

        internal void SetDataTimeRange(DateTime from, DateTime to)
        {
            // Sets the Time Range for which Data is Selected
            // Setting this will clear any existing selected items

            if (_validationActivityInitiator != null)
                _validationActivityInitiator.Dispose();

            _browsedItems.Clear();
            _selectedItemIds = null;
            SelectedItems = null;

            _validationView.setTimeInterval(DateTimeToUnixEpoch(from), DateTimeToUnixEpoch(to));
        }

        internal void SetSelectedItems()
        {
            if (_validationActivityInitiator != null)
                _validationActivityInitiator.Dispose();

            long[] items = _selectedItemIds.ToArray();

            if (items.Length > 0)
            {
                _validationActivityInitiator = _validationView.prepareValidation(items);

                SelectedItems = new DSClientBackupSetItemInfo[items.Length];
                for (int i = 0; i < items.Length; i++)
                    SelectedItems[i] = _browsedItems.Single(item => item.ItemId == items[i]);
            }
            else
            {
                SelectedItems = null;
                _validationActivityInitiator = null;
            }

            SelectiveValidation = (SelectedItems.Length > 0);
        }

        internal GenericActivity StartValidation()
        {
            if (!SelectiveValidation)
                _validationActivityInitiator = _validationView.prepareValidation(new long[0]);

            if (_validationActivityInitiator != null)
                return _validationActivityInitiator.start(SelectiveValidation);

            throw new Exception("Validation Session not Ready to Start");
        }
    }
}