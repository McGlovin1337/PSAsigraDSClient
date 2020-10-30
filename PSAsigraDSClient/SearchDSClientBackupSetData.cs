using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Search, "DSClientBackupSetData")]
    [OutputType(typeof(DSClientBSFileInfo))]

    public class SearchDSClientBackupSetData: BaseDSClientBackupSetDataBrowser
    {
        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the File Search Filters")]
        [SupportsWildcards]
        [ValidateNotNullOrEmpty]
        public string[] Filter { get; set; }

        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Directory Search Filters")]
        [SupportsWildcards]
        [ValidateNotNullOrEmpty]
        public string[] DirectoryFilter { get; set; }

        [Parameter(HelpMessage = "Specify to display the most recent Generation only")]
        public SwitchParameter LatestGenerationOnly { get; set; }

        protected override void ProcessBackupSetData(BackedUpDataView DSClientBackedUpDataView)
        {
            List<DSClientBSFileInfo> FoundBSFiles = new List<DSClientBSFileInfo>();

            foreach (string filter in Filter)
            {
                WriteVerbose("Performing search for: " + filter + " ...");
                find_file_info[] findFileInfos = DSClientBackedUpDataView.FindFilesByFileName(filter);

                foreach (find_file_info file in findFileInfos)
                {
                    DSClientBSFileInfo fileInfo = new DSClientBSFileInfo(file);
                    FoundBSFiles.Add(fileInfo);
                }
            }

            if (MyInvocation.BoundParameters.ContainsKey("DirectoryFilter"))
            {
                List<DSClientBSFileInfo> dirFilterFileInfo = new List<DSClientBSFileInfo>();

                WildcardOptions wcOptions = WildcardOptions.IgnoreCase |
                                        WildcardOptions.Compiled;

                foreach (string dir in DirectoryFilter)
                {
                    WildcardPattern wcPattern = new WildcardPattern(dir, wcOptions);

                    foreach (DSClientBSFileInfo file in FoundBSFiles)
                    {
                        if (wcPattern.IsMatch(file.Directory))
                            dirFilterFileInfo.Add(file);
                    }
                }

                FoundBSFiles = dirFilterFileInfo;
            }

            // Filter the results to only display files where the LastModified Date is within range of the DateFrom and DateTo Parameters
            FoundBSFiles = FoundBSFiles.Where(found => found.LastModified > DateFrom && found.LastModified < DateTo).ToList();

            // If only displaying the latest generation only, then sort the list, first by Path then GenerationId descending and finally remove duplicates
            if (LatestGenerationOnly)
            {
                FoundBSFiles = FoundBSFiles.OrderBy(found => found.Path)
                        .ThenByDescending(found => found.GenerationId)
                        .Distinct(new FileInfoComparer())
                        .ToList();
            }

            FoundBSFiles.ForEach(WriteObject);
        }

        private class DSClientBSFileInfo
        {
            public int DirectoryId { get; private set; }
            public string Path { get; private set; }
            public string Directory { get; private set; }
            public int FileId { get; private set; }
            public string FileName { get; private set; }
            public long FileSize { get; private set; }
            public int GenerationId { get; private set; }
            public DateTime LastModified { get; private set; }

            public DSClientBSFileInfo(find_file_info fileInfo)
            {
                DirectoryId = fileInfo.dir_id;
                Path = fileInfo.dir_name + fileInfo.file_name;
                Directory = fileInfo.dir_name;
                FileId = fileInfo.file_id;
                FileName = fileInfo.file_name;
                FileSize = fileInfo.file_size;
                GenerationId = fileInfo.generation;
                LastModified = UnixEpochToDateTime(fileInfo.last_modified_time);
            }
        }

        private class FileInfoComparer: IEqualityComparer<DSClientBSFileInfo>
        {
            public bool Equals(DSClientBSFileInfo fileInfoA, DSClientBSFileInfo fileInfoB)
            {
                return fileInfoA.Path == fileInfoB.Path;
            }

            public int GetHashCode(DSClientBSFileInfo obj)
            {
                return obj.Path.GetHashCode();
            }
        }
    }
}