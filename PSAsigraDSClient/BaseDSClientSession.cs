using System;
using System.Management.Automation;
using AsigraDSClientApi;

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

            public string Host { get; private set; }
            public string State { get; private set; }
            public DateTime Established { get; private set; }

            public DSClientSession(string computer, UInt16 port, bool nossl, string apiVersion, PSCredential credential)
            {
                string prefix = (nossl) ? "http" : "https";

                _url = $@"{prefix}://{computer}:{port}/api";
                _apiVersion = apiVersion;
                _credential = credential;
                _clientConnection = ApiFactory.CreateConnection(_url, _apiVersion, credential.UserName, credential.GetNetworkCredential().Password, 0);

                Host = computer;
                State = "Connected";
                Established = DateTime.Now;
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

            public ClientConnection GetClientConnection()
            {
                return _clientConnection;
            }
        }
    }
}