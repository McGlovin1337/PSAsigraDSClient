using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public class DSClientSession
    {
        private readonly PSCredential _credential;
        private readonly string _url;
        private readonly string _apiVersion;
        private readonly bool _logoutOnExit;
        private readonly List<DSClientDeleteSession> _deleteSessions;
        private readonly List<DSClientRestoreSession> _restoreSessions;
        private readonly List<DSClientValidationSession> _validationSessions;
        private Dictionary<string, int> _timeRetentionHashes;
        private Dictionary<string, int> _scheduleDetailHashes;
        private ClientConnection _clientConnection;
        private int _connectionRetries;

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string HostName { get; private set; }
        public int Port { get; private set; }
        public ConnectionState State { get; private set; }
        public DateTime Established { get; private set; }
        public string Transport { get; private set; }
        public string OperatingSystem { get; private set; }

        internal DSClientSession(int id, string computer, UInt16 port, bool nossl, string apiVersion, PSCredential credential, string name = null, bool logoutExit = false)
        {
            string prefix = (nossl) ? "http" : "https";

            _url = $@"{prefix}://{computer}:{port}/api";
            _apiVersion = apiVersion;
            _credential = credential;
            _logoutOnExit = logoutExit;
            _clientConnection = ApiFactory.CreateConnection(_url, _apiVersion, credential.UserName, credential.GetNetworkCredential().Password, 0);
            _deleteSessions = new List<DSClientDeleteSession>();
            _restoreSessions = new List<DSClientRestoreSession>();
            _validationSessions = new List<DSClientValidationSession>();
            _connectionRetries = 1;

            Id = id;
            Name = (string.IsNullOrEmpty(name)) ? $"Session{id}" : name;
            HostName = computer;
            Port = port;
            State = ConnectionState.Connected;
            Established = DateTime.Now;
            Transport = prefix;

            ClientConfiguration cfgMgr = _clientConnection.getConfigurationManager();
            OperatingSystem = EnumToString(cfgMgr.getClientOSType());
            cfgMgr.Dispose();
        }

        internal void AddDeleteSession(DSClientDeleteSession deleteSession)
        {
            for (int i = 0; i < _deleteSessions.Count(); i++)
                if (_deleteSessions[i].DeleteId == deleteSession.DeleteId)
                    throw new Exception($"Session with DeleteId {deleteSession.DeleteId} already exists");

            _deleteSessions.Add(deleteSession);
        }

        internal void AddRestoreSession(DSClientRestoreSession restoreSession)
        {
            for (int i = 0; i < _restoreSessions.Count(); i++)
                if (_restoreSessions[i].RestoreId == restoreSession.RestoreId)
                    throw new Exception($"Session with RestoreId {restoreSession.RestoreId} already exists");

            _restoreSessions.Add(restoreSession);
        }

        internal void AddValidationSession(DSClientValidationSession validationSession)
        {
            for (int i = 0; i < _validationSessions.Count(); i++)
                if (_validationSessions[i].ValidationId == validationSession.ValidationId)
                    throw new Exception($"Session with ValidationId {validationSession.ValidationId} already exists");

            _validationSessions.Add(validationSession);
        }

        internal void Connect()
        {
            _clientConnection = ApiFactory.CreateConnection(_url, _apiVersion, _credential.UserName, _credential.GetNetworkCredential().Password, 0);
            Established = DateTime.Now;
            UpdateState();
        }

        internal void Disconnect()
        {
            UpdateState();

            // Free resources for any Delete Sessions
            foreach (DSClientDeleteSession deleteSession in _deleteSessions)
                RemoveDeleteSession(deleteSession);

            // Free resources for any Restore Sessions
            foreach (DSClientRestoreSession restoreSession in _restoreSessions)
                RemoveRestoreSession(restoreSession);

            // Free resources for any Validation Sessions
            foreach (DSClientValidationSession validationSession in _validationSessions)
                RemoveValidationSession(validationSession);

            while (State == ConnectionState.Connected)
            {
                try
                {
                    _clientConnection.logout();
                }
                catch
                {
                    // If we fail here, then we finally logged out
                    // The next Update State should exit this loop
                }
                UpdateState();
            }

            if (State == ConnectionState.Disconnected)
                _clientConnection.Dispose();
        }

        internal int GenerateDeleteId()
        {
            int id = 1;

            for (int i = 0; i < _deleteSessions.Count(); i++)
                if (_deleteSessions[i].DeleteId >= id)
                    id = _deleteSessions[i].DeleteId + 1;

            return id;
        }

        internal int GenerateRestoreId()
        {
            int id = 1;

            for (int i = 0; i < _restoreSessions.Count(); i++)
                if (_restoreSessions[i].RestoreId >= id)
                    id = _restoreSessions[i].RestoreId + 1;

            return id;
        }

        internal int GenerateValidationId()
        {
            int id = 1;

            for (int i = 0; i < _validationSessions.Count(); i++)
                if (_validationSessions[i].ValidationId >= id)
                    id = _validationSessions[i].ValidationId + 1;

            return id;
        }

        internal string GetApiUrl()
        {
            return _url;
        }

        internal DSClientDeleteSession GetDeleteSession(int deleteId)
        {
            for (int i = 0; i < _deleteSessions.Count(); i++)
                if (_deleteSessions[i].DeleteId == deleteId)
                    return _deleteSessions[i];

            return null;
        }

        internal IEnumerable<DSClientDeleteSession> GetDeleteSessions()
        {
            return _deleteSessions;
        }

        internal bool GetLogoutOnExit()
        {
            return _logoutOnExit;
        }

        internal DSClientRestoreSession GetRestoreSession(int restoreId)
        {
            for (int i = 0; i < _restoreSessions.Count(); i++)
                if (_restoreSessions[i].RestoreId == restoreId)
                    return _restoreSessions[i];

            return null;
        }

        internal IEnumerable<DSClientRestoreSession> GetRestoreSessions()
        {
            return _restoreSessions;
        }

        internal Dictionary<string, int> GetScheduleOrRetentionDictionary(bool schedule)
        {
            if (schedule)
                return _scheduleDetailHashes;

            return _timeRetentionHashes;
        }

        internal DSClientValidationSession GetValidationSession(int validationId)
        {
            for (int i = 0; i < _validationSessions.Count(); i++)
                if (_validationSessions[i].ValidationId == validationId)
                    return _validationSessions[i];

            return null;
        }

        internal IEnumerable<DSClientValidationSession> GetValidationSessions()
        {
            return _validationSessions;
        }

        internal void RemoveDeleteSession(DSClientDeleteSession deleteSession)
        {
            deleteSession.Dispose();
            _deleteSessions.Remove(deleteSession);
        }

        internal void RemoveRestoreSession(DSClientRestoreSession restoreSession)
        {
            restoreSession.Dispose();
            _restoreSessions.Remove(restoreSession);
        }

        internal void RemoveValidationSession(DSClientValidationSession validationSession)
        {
            validationSession.Dispose();
            _validationSessions.Remove(validationSession);
        }

        internal void SetScheduleOrRetentionDictonary(Dictionary<string, int> dictonary, bool schedule)
        {
            if (schedule)
                _scheduleDetailHashes = dictonary;
            else
                _timeRetentionHashes = dictonary;
        }

        internal void SetState(ConnectionState state)
        {
            State = state;
        }

        internal bool TestConnection(int retries)
        {
            for (int i = 0; i <= retries; i++)
            {
                try
                {
                    _clientConnection.keepAlive();
                    return true;
                }
                catch
                {
                    continue;
                }
            }

            return false;
        }

        internal void UpdateState()
        {
            State = TestConnection(_connectionRetries) ? ConnectionState.Connected : ConnectionState.Disconnected;
        }

        internal ClientConnection GetClientConnection()
        {
            return _clientConnection;
        }

        public enum ConnectionState
        {
            Connected,
            Disconnected
        }
    }
}