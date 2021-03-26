# PSAsigraDSClient ChangeLog

### v0.4.1
- Fixed Add-DSClientUnixFsBackupSetItem clearing out existing Backup Set Items from the specified Backup Set
- Fixed Read-DSClientUnixFsSource Recursion not working beyond one sub-item depth

### v0.4.0
- Added 4x Cmdlets to Manage Backup Set Notifications: Add-DSClientBackupSetNotification, Get-DSClientBackupSetNotification, Remove-DSClientBackupSetNotification, Set-DSClientBackupSetNotification
- Added 3x Cmdlets to Manage Time Retention Options configured in Retention Rules: Get-DSClientTimeRetentionOption, Remove-DSClientTimeRetentionOption, Set-DSClientTimeRetentionOption
- Added Cmdlet to return Backup Set Lock Status for a given Activity Type: Get-DSClientBackupSetLockStatus
- Added Pipeline Support to Stop-DSClientActivity Cmdlet
- Added Switch Parameter -KeepGensByPeriod to Enable/Disable Keeping All Generations within Time Period Setting to Set-DSClientRetentionRule Cmdlet
- Added Class for Get-DSClientInfo Cmdlet, Cmdlet now has OutputType of DSClientInfo
- Added -PassThru Parameter to all Cmdlets that Start an Activity
- Renamed Add-DSClientTimeRetentionRule to Add-DSClientTimeRetentionOption
- Improved Retention Rule Cmdlet Parameters: Time Retention Option Parameters removed from New-DSClientRetentionRule Cmdlet. Simplified Parameters for Add-DSClientTimeRetentionOption and Set-DSClientTimeRetentionOption
- Improved Computer Resolution within New-DSClientWinFsBackupSet and Read-DSClientWinFsSource Cmdlets
- Improved reliability for removing Schedule Details from Schedules
- Updated Cmdlet Help
- Fixed "getKeepDBDumpFile-FUNCTION NOT SUPPORTED ON LINUX" Exception thrown when running Get-DSClientConfiguration on Linux DS-Clients
- Fixed Read-DSClientUnixFsSource showing Sub-Items of "/" despite no Path parameter specified
- Fixed Get-DSClientBackupSetInfo -Enabled parameter having no effect
- Fixed issue causing fault manipulating backup set post creation due to unnecessarily setting the Computer Name on the backup set at creation time.
- Fixed missing Cmdlet Export from v0.3.0: New-DSClientInitialBackupPath

### v0.3.1
- Fixed Read-DSClientUnixFsSource "Sequence Contains no Elements" Exception when attempting to read top-level items/directories

### v0.3.0
- Added 11x Cmdlets to Manage Users, Groups and Roles: Add-DSClientUserGroupRole, Get-DSClientCurrentLoginRole, Get-DSClientUser, Get-DSClientUserGroup, Get-DSClientUserGroupRole, Remove-DSClientUser, Remove-DSClientUserGroup, Remove-DSClientUserGroupRole, Set-DSClientUser, Set-DSClientUserGroup, Set-DSClientUserGroupRole
- Added 3x Cmdlets to Manage DS-Client Grid Configurations: Get-DSClientGridInfo, Get-DSClientGridLog, Stop-DSClientGridNode
- Added 4x Cmdlets to Manage VMware VADP Backup Sets: Add-DSClientVMwareVADPBackupSetItem, New-DSClientVMwareVADPBackupSet, Read-DSClientVMwareVADPSource, Set-DSClientVMwareVADPBackupSet
- Added Cmdlet to view DS-Client Audit Trail: Get-DSClientAuditTrail
- Added SupportsShouldProcess to most Cmdlets that make changes
- Improved Information in Progress Indicators
- Updated Help to account for new and updated Cmdlets
- Updated Get-DSClientStoredItem Cmdlet: Added Alias "Folder" for -Path Parameter, Added "-HideFiles" and "-HideDirectories" Parameters
- Fixed "Bad Cast" Exception when using Get-DSClientBackupSet for a VMware VADP Backup Set hosted on a Linux DS-Client
- Fixed Cmdlet Add-DSClientWeeklySchedule missing from Exported Cmdlets in previous releases

### v0.2.0
- Added 4x New Cmdlets: Get-DSClientConfiguration, Get-DSClientOS, Set-DSClientConfiguration, Watch-DSClientActivity
- Added Progress Indicators to Read-DSClientUnixFsSource, Read-DSClientWinFsSource, Get-DSClientStoredItem, Get-DSClientRetentionRule and Get-DSClientScheduleDetail cmdlets
- Improved Pipelining for a number of Cmdlets
- Improved Verbose Output
- Replaced Parameter "-Activity" with 3x Switch Parameters: "-DailyAdmin", "-WeeklyAdmin" and "-StatisticalAdmin"
- Removed "-SetType" Parameter from Set-DSClientMSSqlServerBackupSet, Set-DSClientUnixFsBackupSet and Set-DSClientWinFsBackupSet Cmdlets
- Fixed Cmdlet Export for Add-DSClientWinFsBackupSetItem        
- Fixed Read-DSClientWinFsSource Cmdlet only showing File Items when the Recurse Switch Parameter was specified
- Fixed "-BackupSetId" parameter not working with Get-DSClientBackupSetInfo Cmdlet

### v0.1.1
- Fix typo in Module Manifest

### v0.1.0
- Initial Public Pre-release Module Version'
