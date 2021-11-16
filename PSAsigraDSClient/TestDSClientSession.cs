using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Management.Automation;
using System.Net;
using System.Net.Sockets;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient.PSAsigraDSClient
{
    [Cmdlet(VerbsDiagnostic.Test, "DSClientSession")]
    [OutputType(typeof(DSClientConnection))]

    sealed public class TestDSClientSession : BaseDSClientSession
    {
        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Test DS-Client Sessions by Id")]
        public int[] Id { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Test DS-Client Sessions by Name")]
        [SupportsWildcards]
        public string[] Name { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Test DS-Client Sessions by HostName")]
        [SupportsWildcards]
        public string[] HostName { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Test DS-Client Sessions by Operating System")]
        [ValidateSet("Linux", "Mac", "Windows")]
        public string[] OperatingSystem { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Retrieve DS-Client Sessions by Connection State")]
        [ValidateSet("Connected", "Disconnected")]
        public string State { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Set the number of connection attempts")]
        public int Retries { get; set; } = 0;

        private List<DSClientSession> _sessions;

        protected override void ProcessDSClientSession(IEnumerable<DSClientSession> sessions)
        {
            _sessions = sessions.ToList();

            if (_sessions.Count() > 0 && MyInvocation.BoundParameters.ContainsOneOfKeys(new string[] { nameof(Id), nameof(Name), nameof(HostName), nameof(OperatingSystem), nameof(State)}))
            {
                List<DSClientSession> filtered = new List<DSClientSession>();

                WildcardOptions wcOptions = WildcardOptions.IgnoreCase |
                            WildcardOptions.Compiled;

                if (MyInvocation.BoundParameters.ContainsKey(nameof(Id)))
                {
                    foreach (int id in Id)
                    {
                        try
                        {
                            filtered.Add(_sessions.Single(session => session.Id == id));
                        }
                        catch
                        {
                            ErrorRecord errorRecord = new ErrorRecord(
                                new Exception($"Specified Session Id {id} not found"),
                                "Exception",
                                ErrorCategory.ObjectNotFound,
                                null);
                            WriteError(errorRecord);
                        }
                    }
                }

                if (MyInvocation.BoundParameters.ContainsKey(nameof(Name)))
                {
                    foreach (string name in Name)
                    {
                        WildcardPattern wcPattern = new WildcardPattern(name, wcOptions);
                        filtered.AddRange(_sessions.Where(session => wcPattern.IsMatch(session.Name)));
                    }
                }

                if (MyInvocation.BoundParameters.ContainsKey(nameof(HostName)))
                {
                    foreach (string host in HostName)
                    {
                        WildcardPattern wcPattern = new WildcardPattern(host, wcOptions);
                        filtered.AddRange(_sessions.Where(session => wcPattern.IsMatch(session.HostName)));
                    }
                }

                if (MyInvocation.BoundParameters.ContainsKey(nameof(State)))
                {
                    filtered.AddRange(_sessions.Where(session => session.State == (DSClientSession.ConnectionState)Enum.Parse(typeof(DSClientSession.ConnectionState), State, true)));
                }

                if (MyInvocation.BoundParameters.ContainsKey(nameof(OperatingSystem)))
                {
                    foreach (string os in OperatingSystem)
                    {
                        filtered.AddRange(_sessions.Where(session => session.OperatingSystem == os));
                    }
                }

                _sessions = filtered;
            }

            List<DSClientConnection> connections = new List<DSClientConnection>();
            int numSessions = _sessions.Count();
            ProgressRecord progressRecord = new ProgressRecord(1, "Test DS-Client Session Connection", $"0/{numSessions} Sessions Complete")
            {
                PercentComplete = 0,
                RecordType = ProgressRecordType.Processing
            };

            for (int i = 0; i < _sessions.Count(); i++)
            {
                if (i != 0)
                {
                    progressRecord.PercentComplete = i / numSessions;
                }
                WriteProgress(progressRecord);

                DSClientSession session = _sessions[i];

                // ICMP Ping Test
                WriteVerbose($"Performing Action: Ping DS-Client Address: {session.HostName}");
                PingReply reply = null;
                for (int x = 0; x <= Retries; x++)
                {
                    progressRecord.CurrentOperation = $"ICMP Ping Session: {session.Name} (Attempt {x})";
                    WriteProgress(progressRecord);
                    WriteVerbose($"Notice: Ping Attempt: {x}");
                    using (Ping ping = new Ping())
                    {
                        reply = ping.Send(session.HostName, 1000);
                    }
                    WriteVerbose($"Notice: Response Status: {reply.Status}");
                    
                    if (reply.Status == IPStatus.Success)
                    {
                        break;
                    }
                }
                bool pingSuccess = (reply.Status == IPStatus.Success);

                // DS-Client API TCP Port Test
                WriteVerbose($"Performing Action: TCP Connection Test: {session.Port}");
                bool tcpSuccess = false;
                using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    if (IPAddress.TryParse(session.HostName, out IPAddress ip))
                    {
                        for (int x = 0; x <= Retries; x++)
                        {
                            progressRecord.CurrentOperation = $"TCP Ping Session: {session.Name} (Attempt {x})";
                            WriteProgress(progressRecord);
                            try
                            {
                                s.Connect(ip, session.Port);
                                tcpSuccess = true;
                                s.Disconnect(false);
                                WriteVerbose($"Notice: TCP Connect Attempt '{x}' Succeeded");
                                break;
                            }
                            catch
                            {
                                WriteVerbose($"Notice: TCP Connect Attempt '{x}' Failed");
                            }
                        }
                    }
                    else
                    {
                        for (int x = 0; x <= Retries; x++)
                        {
                            progressRecord.CurrentOperation = $"TCP Ping Session: {session.Name} (Attempt {x})";
                            WriteProgress(progressRecord);
                            try
                            {
                                s.Connect(session.HostName, session.Port);
                                tcpSuccess = true;
                                s.Disconnect(false);
                                WriteVerbose($"Notice: TCP Connect Attempt '{x}' Succeeded");
                                break;
                            }
                            catch
                            {
                                WriteVerbose($"Notice: TCP Connect Attempt '{x}' Failed");
                            }
                        }
                    }
                }

                // DS-Client API Test
                progressRecord.CurrentOperation = $"DS-Client API Connection Test: {session.Name}";
                WriteProgress(progressRecord);
                WriteVerbose($"Performing Action: Test DS-Client Session Connection: {session.Name}");
                bool sessionAlive = session.TestConnection(Retries);
                WriteVerbose($"Notice: {session.Name} result: {sessionAlive}");

                if (!sessionAlive && session.State == DSClientSession.ConnectionState.Connected)
                {
                    session.SetState(DSClientSession.ConnectionState.Disconnected);
                }

                connections.Add(new DSClientConnection(session, pingSuccess, tcpSuccess, sessionAlive));
            }
            progressRecord.PercentComplete = 100;
            progressRecord.RecordType = ProgressRecordType.Completed;
            WriteProgress(progressRecord);

            connections.ForEach(WriteObject);
        }
    }

    public class DSClientConnection
    { 
        public string Name { get; private set; }
        public string HostName { get; private set; }
        public int Port { get; private set; }
        public bool PingTestSucceeded { get; private set; }
        public bool TcpTestSucceeded { get; private set; }
        public bool ApiTestSucceeded { get; private set; }

        public DSClientConnection(DSClientSession session, bool pingResult, bool tcpResult, bool apiResult)
        {
            Name = session.Name;
            HostName = session.HostName;
            Port = session.Port;
            PingTestSucceeded = pingResult;
            TcpTestSucceeded = tcpResult;
            ApiTestSucceeded = apiResult;
        }
    }
}