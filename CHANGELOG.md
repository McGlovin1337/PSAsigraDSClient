# PSAsigraDSClient ChangeLog

### v0.6.0
- Added capability to establish and manage multiple DS-Client Sessions, added Cmdlets: New-DSClientSession, Get-DSClientSession, Remove-DSClientSession, Connect-DSClientSession, Disconnect-DSClientSession, Invoke-DSClientCommand
- Rewrite Backup Set Restore Process. Added capability to create multiple restore sessions.
-- Added Cmdlets: Initialize-DSClientRestoreSession, New-DSClientFSRestoreOptions, New-DSClientMSSQLRestoreOptions, New-DSClientMSSQLDatabaseRestoreMapping, Add-DSClientRestoreItem, Get-DSClientRestoreSession, Remove-DSClientRestoreSession, Remove-DSClientRestoreItem, Set-DSClientRestoreSession, Start-DSClientRestore, New-DSClientUnixCredential, New-DSClientWindowsCredential
-- Removed Cmdlets: Initialize-DSClientBackupSetRestore, Start-DSClientUnixFSRestore, Start-DSClientMSSqlServerRestore, Start-DSClientWinFsRestore
- Rewrite Backup Set Validation Process. Added capability to create multiple validation sessions.
-- Added Cmdlets: Initialize-DSClientValidationSession, Get-DSClientValidationSession, Add-DSClientValidationItem, Remove-DSClientValidationItem, Remove-DSClientValidationSession, Start-DSClientValidation
-- Removed Cmdlets: Initialize-DSClientBackupSetValidation, Start-DSClientBackupSetValidation
- Rewrite Backup Set Delete Process. Added capability to create multiple delete sessions.
-- Added Cmdlets: Initialize-DSClientDeleteSession, Get-DSClientDeleteSession, Start-DSClientDelete, Add-DSClientDeleteItem, Remove-DSClientDeleteItem, Remove-DSClientDeleteSession, Set-DSClientDeleteSession
- Rename "-Host" Parameter to "-HostName" in Enter-DSClientSession Cmdlet
- Added Parameter "-CalculateDirectorySize" to Get-DSClientStoredItem, Directory Sizes are no longer calculated by default
- Fixed ValidateSet in Set-DSClientRegistrationInfo Cmdlet
- Fixed Exception "Cannot process the argument because the value of statusDescription cannot be null or empty" in Watch-DSClientActivity
- Improved Get-DSClientActivityLog and Get-DSClientEventLog, output is now sorted by Start Date/Time
- Set the PSAsigraDSClient.dll Assembly version to 0.6.0.0 to match Module Version

### v0.5.0
- Added Cmdlets that allow changing existing Schedule Details: Set-DSClientOneTimeSchedule, Set-DSClientDailySchedule, Set-DSClientWeeklySchedule, Set-DSClientMonthlySchedule
- Added Remove-DSClientArchiveFilterRule Cmdlet
- Added Rename-DSClientArchiveFilterRule Cmdlet
- Added Remove-DSclientArchiveFilter Cmdlet
- Added Cmdlet Export-DSClientConfig allowing the DS-Client Config to be exported to an XML File
- Added "-ExcludeSubDirs" Parameter to Cmdlets used for Adding Backup Set Items, Inclusion & Exclusion Items now always include sub-directories
- Added "-LiteralFolder" Parameter to Remove-DSClientBackupSetItem Cmdlet, also replaced the "-Inclusion", "-Exclusion" and "-RegexExclusion" SwitchParameters with "-Type" Parameter
- Added Progress Indicator when using Get-DSClientTimeRetentionOption Cmdlet
- Fixed DS-Client Service crash when adding an invalid Exclusion item to a Backup Set
- Fixed Remove-DSClientBackupSetItem Cmdlet failing to remove Items from Unix Backup Sets
- Fixed Read-DSClientUnixFsSource not returning all drives
- Fixed incorrect formatting of Exclusion Items added to Unix Backup Sets
- Improved Adding Time Retention Options, now prevents adding duplicate Time Retention Options to Time Based Retention Rules
- Improved Remove-DSClientBackupSet Cmdlet, now sets ConfirmImpact to High thus prompting the user for confirmation before purging the backup set from the DS-Client
- Improved notification of loss of connectivity to DS-Client when executing Cmdlets
- Improved Id generation for Time Retention Options and Schedule Details by using SHA1 Hashing
- Improved the Properties used in the Schedule Details removing a dynamic property
- Improved performance of the Read-DSClientMsSqlServerSource Cmdlet
- Improved output of Read-DSClientUnixFsSource Cmdlet so that Directory Path separators now use a backslash instead (since this is the format the DS-Client expects as input when adding Unix Items to a Backup Set)
- Improved the Properties in the Output of Backup Set Items, removing dynamic Properties
- Improved Progress Indicator when using Get-DSClientScheduleDetail Cmdlet
- Renamed "-RegexExcludeDirectory" Parameter to "-RegexMatchDirectory" for Cmdlets that add Backup Set Items
- Renamed "-RegexExcludeItem" Parameter to "-RegexExcludePattern" on all Cmdlets that Add Backup Set Items

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
