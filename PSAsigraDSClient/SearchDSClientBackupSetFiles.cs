﻿using System;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Search, "DSClientBackupSetFiles")]
    [OutputType(typeof(DSClientBSFileInfo))]

    public class SearchDSClientBackupSetFiles: BaseDSClientBackupSetDataBrowser
    {
        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the File Search Filters")]
        [SupportsWildcards]
        [ValidateNotNullOrEmpty]
        public string[] Filter { get; set; }

        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Directory Search Filters")]
        [SupportsWildcards]
        [ValidateNotNullOrEmpty]
        public string[] DirectoryFilter { get; set; }

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

            FoundBSFiles.ForEach(WriteObject);
        }

        private class DSClientBSFileInfo
        {
            public int DirectoryId { get; set; }
            public string Path { get; set; }
            public string Directory { get; set; }
            public int FileId { get; set; }
            public string FileName { get; set; }
            public long FileSize { get; set; }
            public int GenerationId { get; set; }
            public DateTime LastModified { get; set; }

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
    }
}