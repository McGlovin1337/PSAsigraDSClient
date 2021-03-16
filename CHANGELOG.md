# PSAsigraDSClient ChangeLog

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
