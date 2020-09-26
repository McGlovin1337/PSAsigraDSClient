using System;
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
        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the File Search Filters")]
        [ValidateNotNullOrEmpty]
        public string[] Filter { get; set; }

        protected override void ProcessBackupSetData(BackupSetRestoreView DSClientBackupSetRestoreView)
        {
            List<DSClientBSFileInfo> FoundBSFiles = new List<DSClientBSFileInfo>();

            foreach (string filter in Filter)
            {
                WriteVerbose("Performing search for: " + filter + " ...");
                find_file_info[] findFileInfos = DSClientBackupSetRestoreView.FindFilesByFileName(filter);

                foreach (find_file_info file in findFileInfos)
                {
                    DSClientBSFileInfo fileInfo = new DSClientBSFileInfo(file);
                    FoundBSFiles.Add(fileInfo);
                }
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