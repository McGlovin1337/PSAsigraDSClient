using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public class DSClientVMwareRestoreOptions
    {
        public int Id { get; private set; }
        public string Datacenter { get; private set; }
        public string VMName { get; private set; }
        public string Datastore { get; private set; }
        public string FolderName { get; private set; }
        public string Hostname { get; private set; }
        public bool TimeStampVMName { get; private set; }
        public bool PowerOn { get; private set; }
        public bool Unregister { get; private set; }
        public bool ForceSAN { get; private set; }

        public DSClientVMwareRestoreOptions(int id, vmware_options options)
        {
            Id = id;
            Datacenter = options.dataCenter;
            VMName = options.vmName;
            Datastore = options.dataStore;
            FolderName = options.folderName;
            Hostname = options.hostName;
            TimeStampVMName = options.addTimestampVMName;
            PowerOn = options.powerOnVMAfterRestore;
            Unregister = options.unregisterAfterRestore;
            ForceSAN = options.forceSAN;
        }
    }

    public static class VMwareRestoreOptionMethods
    {
        public static string VMwareOptionHash(vmware_options options)
        {
            string strToHash = options.addTimestampVMName.ToString() +
                options.dataCenter +
                options.dataStore +
                options.folderName +
                options.forceSAN.ToString() +
                options.hostName +
                options.powerOnVMAfterRestore.ToString() +
                options.unregisterAfterRestore.ToString() +
                options.vmName;

            return strToHash.GetSha1Hash();
        }
    }
}