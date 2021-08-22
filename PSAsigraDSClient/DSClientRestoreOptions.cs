using System.Reflection;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public class DSClientRestoreOptions
    {
        public class RestoreOptions_Base
        {
            public bool UseDetailedLog { get; protected set; }
            public string LocalStorageMethod { get; protected set; }
            public int DSSystemReadThreads { get; protected set; }
            public int MaxPendingAsyncIO { get; protected set; }

            internal RestoreOptions_Base()
            {
                UseDetailedLog = false;
                LocalStorageMethod = "ContinueIfDisconnect";
                DSSystemReadThreads = 0;
                MaxPendingAsyncIO = 0;
            }

            internal DSClientRestoreOption[] GetRestoreOptions()
            {
                PropertyInfo[] props = this.GetType().GetProperties();
                DSClientRestoreOption[] restoreOptions = new DSClientRestoreOption[props.Length];
                for (int i = 0; i < props.Length; i++)
                    restoreOptions[i] = new DSClientRestoreOption(this.GetType(), props[i].Name, props[i].GetValue(this, null));

                return restoreOptions;
            }

            internal void SetUseDetailedLog(bool v)
            {
                UseDetailedLog = v;
            }

            internal void SetLocalStorageMethod(string localStorageMethod)
            {
                LocalStorageMethod = localStorageMethod;
            }

            internal void SetDSSystemReadThreads(int readThreads)
            {
                DSSystemReadThreads = readThreads;
            }

            internal void SetMaxPendingIO(int maxIO)
            {
                MaxPendingAsyncIO = maxIO;
            }
        }

        public class RestoreOptions_FileSystem : RestoreOptions_Base
        {
            public string FileOverwriteOption { get; protected set; }
            public string RestoreMethod { get; protected set; }
            public string RestorePermissions { get; protected set; }

            internal RestoreOptions_FileSystem() : base()
            {
                FileOverwriteOption = "RestoreAll";
                RestoreMethod = "Fast";
                RestorePermissions = "Yes";
            }

            internal void SetOverwriteOption(string overwriteOption)
            {
                FileOverwriteOption = overwriteOption;
            }

            internal void SetRestoreMethod(string restoreMethod)
            {
                RestoreMethod = restoreMethod;
            }

            internal void SetRestorePermissions(string restorePermissions)
            {
                RestorePermissions = restorePermissions;
            }
        }

        public class RestoreOptions_Win32FileSystem : RestoreOptions_FileSystem
        {
            public bool AuthoritativeRestore { get; private set; }
            public bool OverwriteJunctionPoint { get; private set; }
            public bool SkipOfflineFiles { get; private set; }

            internal RestoreOptions_Win32FileSystem() : base()
            {
                AuthoritativeRestore = false;
                OverwriteJunctionPoint = false;
                SkipOfflineFiles = true;
            }

            internal static RestoreOptions_Win32FileSystem From(RestoreOptions_FileSystem restoreOptions_FileSystem)
            {
                RestoreOptions_Win32FileSystem restoreOptions = new RestoreOptions_Win32FileSystem();

                foreach (PropertyInfo parentProp in restoreOptions_FileSystem.GetType().GetProperties())
                    foreach (PropertyInfo thisProp in restoreOptions.GetType().GetProperties())
                        if (thisProp.Name == parentProp.Name)
                            thisProp.SetValue(restoreOptions, parentProp.GetValue(restoreOptions_FileSystem), null);

                return restoreOptions;
            }

            internal void SetAuthoritative(bool v)
            {
                AuthoritativeRestore = v;
            }

            internal void SetOverwriteJunctionPoint(bool v)
            {
                OverwriteJunctionPoint = v;
            }

            internal void SetSkipOfflineFile(bool v)
            {
                SkipOfflineFiles = v;
            }
        }

        public class RestoreOptions_MSSQLServer : RestoreOptions_Base
        {
            public string DumpMethod { get; private set; }
            public string DumpPath { get; private set; }
            public bool RestoreDumpOnly { get; private set; }
            public bool LeaveRestoringMode { get; private set; }
            public bool PreserveOriginalLocation { get; private set; }


            internal RestoreOptions_MSSQLServer() : base()
            {
                DumpMethod = EnumToString(ESQLDumpMethod.ESQLDumpMethod__DumpToPipe);
                DumpPath = null;
                RestoreDumpOnly = false;
                LeaveRestoringMode = false;
                PreserveOriginalLocation = false;
            }

            internal void SetDumpMethod(string dumpMethod)
            {
                // Check the string is a valid Enum
                StringToEnum<ESQLDumpMethod>(dumpMethod);

                DumpMethod = dumpMethod;
            }

            internal void SetDumpPath(string dumpPath)
            {
                DumpPath = dumpPath;
            }

            internal void SetRestoreDumpOnly(bool restoreDumpOnly)
            {
                RestoreDumpOnly = restoreDumpOnly;
            }

            internal void SetLeaveRestoringMode(bool leaveRestoring)
            {
                LeaveRestoringMode = leaveRestoring;
            }

            internal void SetPreserveOriginalLocation(bool preserveOriginalLocation)
            {
                PreserveOriginalLocation = preserveOriginalLocation;
            }
        }

        public class RestoreOptions_VMWareVADP : RestoreOptions_Base
        {
            public bool AttemptIncrementalRestore { get; private set; }

            internal RestoreOptions_VMWareVADP() : base()
            {
                AttemptIncrementalRestore = false;
            }

            internal void SetAttemptIncrementalRestore(bool attemptIncrementalRestore)
            {
                AttemptIncrementalRestore = attemptIncrementalRestore;
            }
        }
    }
}