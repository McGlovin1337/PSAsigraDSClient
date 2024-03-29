﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientSession")]
    [OutputType(typeof(DSClientSession))]

    sealed public class GetDSClientSession : BaseDSClientSession
    {
        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Retrieve DS-Client Sessions by Id")]
        public int[] Id { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Retrieve DS-Client Sessions by Name")]
        [SupportsWildcards]
        public string[] Name { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Retrieve DS-Client Sessions by HostName")]
        [SupportsWildcards]
        public string[] HostName { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Retrieve DS-Client Sessions by Port Number")]
        public int[] Port { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Retrieve DS-Client Sessions by Connection State")]
        [ValidateSet("Connected", "Disconnected")]
        public string State { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Retrieve DS-Client Sessions by Transport Protocol")]
        [ValidateSet("http", "https")]
        public string Transport { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Retrieve DS-Client Sessions by Operating System")]
        [ValidateSet("Linux", "Mac", "Windows")]
        public string[] OperatingSystem { get; set; }

        private List<DSClientSession> _sessions;

        protected override void ProcessDSClientSession(IEnumerable<DSClientSession> sessions)
        {
            WriteVerbose("Performing Action: Retrieve DS-Client Sessions");
            if (sessions != null)
                _sessions = sessions.ToList();

            if (sessions != null && MyInvocation.BoundParameters.Count > 0)
            {
                List<DSClientSession> filtered = new List<DSClientSession>();

                WildcardOptions wcOptions = WildcardOptions.IgnoreCase |
                            WildcardOptions.Compiled;

                if (MyInvocation.BoundParameters.ContainsKey(nameof(Id)))
                    foreach (int id in Id)
                        filtered.Add(_sessions.SingleOrDefault(session => session.Id == id));

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

                if (MyInvocation.BoundParameters.ContainsKey(nameof(Port)))
                    foreach (int port in Port)
                        filtered.AddRange(_sessions.Where(session => session.Port == port));

                if (MyInvocation.BoundParameters.ContainsKey(nameof(State)))
                    filtered.AddRange(_sessions.Where(session => session.State == (DSClientSession.ConnectionState)Enum.Parse(typeof(DSClientSession.ConnectionState), State, true)));

                if (MyInvocation.BoundParameters.ContainsKey(nameof(Transport)))
                    filtered.AddRange(_sessions.Where(session => session.Transport == Transport.ToLower()));

                if (MyInvocation.BoundParameters.ContainsKey(nameof(OperatingSystem)))
                    foreach (string os in OperatingSystem)
                        filtered.AddRange(_sessions.Where(session => session.OperatingSystem == os));

                _sessions = filtered;
            }

            _sessions.ForEach(WriteObject);
        }
    }
}