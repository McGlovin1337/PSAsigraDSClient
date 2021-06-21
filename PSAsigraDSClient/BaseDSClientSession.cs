using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientSession : PSCmdlet
    {
        public class DSClientSession
        {
            private readonly PSCredential _credential;
            private readonly string _url;
            private readonly string _apiVersion;
            private ClientConnection _clientConnection;

            public int Id { get; private set; }
            public string Name { get; private set; }
            public string Host { get; private set; }
            public int Port { get; private set; }
            public string State { get; private set; }
            public DateTime Established { get; private set; }
            public string Transport { get; private set; }
            public string OperatingSystem { get; private set; }

            public DSClientSession(int id, string computer, UInt16 port, bool nossl, string apiVersion, PSCredential credential, string name = null)
            {
                string prefix = (nossl) ? "http" : "https";

                _url = $@"{prefix}://{computer}:{port}/api";
                _apiVersion = apiVersion;
                _credential = credential;
                _clientConnection = ApiFactory.CreateConnection(_url, _apiVersion, credential.UserName, credential.GetNetworkCredential().Password, 0);

                Id = id;
                Name = (string.IsNullOrEmpty(name)) ? $"Session{id}" : name;
                Host = computer;
                Port = port;
                State = "Connected";                
                Established = DateTime.Now;
                Transport = prefix;

                ClientConfiguration cfgMgr = _clientConnection.getConfigurationManager();
                OperatingSystem = EnumToString(cfgMgr.getClientOSType());
                cfgMgr.Dispose();
            }

            public void Connect()
            {
                _clientConnection = ApiFactory.CreateConnection(_url, _apiVersion, _credential.UserName, _credential.GetNetworkCredential().Password, 0);
                Established = DateTime.Now;
            }

            public void Disconnect()
            {
                _clientConnection.logout();
                State = "Disconnected";
            }

            public void UpdateState()
            {
                try
                {
                    _clientConnection.keepAlive();
                    State = "Connected";
                }
                catch
                {
                    State = "Disconnected";
                }
            }

            public ClientConnection GetClientConnection()
            {
                return _clientConnection;
            }
        }
    }
}