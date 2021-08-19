﻿using System;
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
        private ClientConnection _clientConnection;
        private List<DSClientRestoreSession> _restoreSessions;

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
            _restoreSessions = new List<DSClientRestoreSession>();

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

        internal void AddRestoreSession(DSClientRestoreSession restoreSession)
        {
            for (int i = 0; i < _restoreSessions.Count(); i++)
                if (_restoreSessions[i].RestoreId == restoreSession.RestoreId)
                    throw new Exception($"Session with RestoreId {restoreSession.RestoreId} already exits");

            _restoreSessions.Add(restoreSession);
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

        internal int GenerateRestoreId()
        {
            int id = 1;

            for (int i = 0; i < _restoreSessions.Count(); i++)
                if (_restoreSessions[i].RestoreId >= id)
                    id = _restoreSessions[i].RestoreId + 1;

            return id;
        }

        internal string GetApiUrl()
        {
            return _url;
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

        internal void RemoveRestoreSession(DSClientRestoreSession restoreSession)
        {
            restoreSession.Dispose();
            _restoreSessions.Remove(restoreSession);
        }

        internal void UpdateState()
        {
            try
            {
                _clientConnection.keepAlive();
            }
            catch
            {
                State = ConnectionState.Disconnected;
                return;
            }

            State = ConnectionState.Connected;
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