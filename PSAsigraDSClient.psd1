#
# Module manifest for module 'PSAsigraDSClient'
#
# Generated by: James
#
# Generated on: 09/08/2020
#

@{

# Script module or binary module file associated with this manifest.
RootModule = 'PSAsigraDSClient'

# Version number of this module.
ModuleVersion = '0.5.0'

# Supported PSEditions
# CompatiblePSEditions = @()

# ID used to uniquely identify this module
GUID = 'f6f580c3-10e5-434f-b360-6cf3740e6bba'

# Author of this module
Author = 'James Riach'

# Company or vendor of this module
CompanyName = 'Unknown'

# Copyright statement for this module
Copyright = '(c) 2020 James. All rights reserved.'

# Description of the functionality provided by this module
Description = 'PowerShell Module to Manage the Asigra DS-Client'

# Minimum version of the Windows PowerShell engine required by this module
PowerShellVersion = '5.1'

# Name of the Windows PowerShell host required by this module
# PowerShellHostName = ''

# Minimum version of the Windows PowerShell host required by this module
# PowerShellHostVersion = ''

# Minimum version of Microsoft .NET Framework required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
DotNetFrameworkVersion = '4.7.2'

# Minimum version of the common language runtime (CLR) required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
# CLRVersion = ''

# Processor architecture (None, X86, Amd64) required by this module
ProcessorArchitecture = 'Amd64'

# Modules that must be imported into the global environment prior to importing this module
# RequiredModules = @()

# Assemblies that must be loaded prior to importing this module
# RequiredAssemblies = @()

# Script files (.ps1) that are run in the caller's environment prior to importing this module.
# ScriptsToProcess = @()

# Type files (.ps1xml) to be loaded when importing this module
TypesToProcess = @("PSAsigraDSClient.Types.ps1xml")

# Format files (.ps1xml) to be loaded when importing this module
FormatsToProcess = @("PSAsigraDSClient.Format.ps1xml")

# Modules to import as nested modules of the module specified in RootModule/ModuleToProcess
# NestedModules = @()

# Functions to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no functions to export.
FunctionsToExport = @()

# Cmdlets to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no cmdlets to export.
CmdletsToExport = @(
"Add-DSClientArchiveFilter",
"Add-DSClientBackupSetNotification",
"Add-DSClientDailySchedule",
"Add-DSClientMonthlySchedule",
"Add-DSClientMSSqlServerBackupSetItem",
"Add-DSClientMultiFactorAuthUser",
"Add-DSClientOneTimeSchedule",
"Add-DSClientRestoreItem",
"Add-DSClientSNMPCommunity",
"Add-DSClientTimeRetentionOption",
"Add-DSClientUnixFsBackupSetItem",
"Add-DSClientUserGroupRole",
"Add-DSClientVMwareVADPBackupSetItem",
"Add-DSClientWeeklySchedule",
"Add-DSClientWinFsBackupSetItem",
"Compare-DSClientEncryptionKeys",
"Connect-DSClientSession",
"Disable-DSClientBackupSet",
"Disable-DSClientSchedule",
"Disconnect-DSClientSession",
"Enable-DSClientBackupSet",
"Enable-DSClientSchedule",
"Enter-DSClientSession",
"Exit-DSClientSession",
"Export-DSClientConfig",
"Get-DSClientActivityLog",
"Get-DSClientAdvancedConfig",
"Get-DSClientArchiveFilterRule",
"Get-DSClientAuditTrail",
"Get-DSClientBackupSet",
"Get-DSClientBackupSetInfo",
"Get-DSClientBackupSetItem",
"Get-DSClientBackupSetLockStatus",
"Get-DSClientBackupSetNotification",
"Get-DSClientBackupSetSessions",
"Get-DSClientConfiguration",
"Get-DSClientCurrentLoginRole",
"Get-DSClientDefaultConfiguration",
"Get-DSClientEventLog",
"Get-DSClientGridInfo",
"Get-DSClientGridLog",
"Get-DSClientInfo",
"Get-DSClientInitialBackupPath",
"Get-DSClientInitialBackupStatus",
"Get-DSClientLoadSummary",
"Get-DSClientMultiFactorAuth",
"Get-DSClientNocSettings",
"Get-DSClientNotification",
"Get-DSClientOrphanedBackupSet",
"Get-DSClientOS",
"Get-DSClientQuota",
"Get-DSClientRegistrationInfo",
"Get-DSClientRestoreSession",
"Get-DSClientRetentionRule",
"Get-DSClientRunningActivity",
"Get-DSClientSchedule",
"Get-DSClientScheduleDetail",
"Get-DSClientSession",
"Get-DSClientSMTPNotification",
"Get-DSClientSNMPCommunity",
"Get-DSClientSNMPInfo",
"Get-DSClientStoredItem",
"Get-DSClientSupportedDataType",
"Get-DSClientTimeRetentionOption",
"Get-DSClientTools",
"Get-DSClientUser",
"Get-DSClientUserGroup",
"Get-DSClientUserGroupRole",
"Initialize-DSClientBackupSetDelete",
"Initialize-DSClientBackupSetValidation",
"Initialize-DSClientRestoreSession",
"Initialize-DSClientValidationSession",
"Invoke-DSClientCommand",
"New-DSClientArchiveFilterRule",
"New-DSClientFSRestoreOptions",
"New-DSClientInitialBackupPath",
"New-DSClientMSSQLDatabaseRestoreMapping",
"New-DSClientMSSQLRestoreOptions",
"New-DSClientMSSqlServerBackupSet",
"New-DSClientRetentionRule",
"New-DSClientSchedule",
"New-DSClientSession",
"New-DSClientUnixCredential",
"New-DSClientUnixFsBackupSet",
"New-DSClientVMwareVADPBackupSet",
"New-DSClientWinFsBackupSet",
"New-DSclientWindowsCredential",
"Publish-DSClientEncryptionKeys",
"Read-DSClientMSSqlServerSource",
"Read-DSClientUnixFsSource",
"Read-DSClientVMwareVADPSource",
"Read-DSClientWinFsSource",
"Register-DSClient",
"Remove-DSClientArchiveFilter",
"Remove-DSClientArchiveFilterRule",
"Remove-DSClientBackupSet",
"Remove-DSClientBackupSetItem",
"Remove-DSClientBackupSetNotification",
"Remove-DSClientInitialBackupPath",
"Remove-DSClientRestoreItem",
"Remove-DSClientRestoreSession",
"Remove-DSClientRetentionRule",
"Remove-DSClientSchedule",
"Remove-DSClientScheduleDetail",
"Remove-DSClientSession",
"Remove-DSClientSNMPCommunity",
"Remove-DSClientTimeRetentionOption",
"Remove-DSClientUser",
"Remove-DSClientUserGroup",
"Remove-DSClientUserGroupRole",
"Rename-DSClientArchiveFilterRule",
"Restore-DSClientDatabase",
"Restore-DSClientOrphanedBackupSet",
"Search-DSClientBackupSetData",
"Set-DSClientAdvancedConfig",
"Set-DSClientBackupSetNotification",
"Set-DSClientConfiguration",
"Set-DSClientDailySchedule",
"Set-DSClientDefaultConfiguration",
"Set-DSClientInitialBackupStatus",
"Set-DSClientMonthlySchedule",
"Set-DSClientMSSqlServerBackupSet",
"Set-DSClientOneTimeSchedule",
"Set-DSClientRegistrationInfo",
"Set-DSClientRestoreSession",
"Set-DSClientRetentionRule",
"Set-DSClientSMTPNotification",
"Set-DSClientSNMPHeartbeat",
"Set-DSClientTimeRetentionOption",
"Set-DSClientUnixFsBackupSet",
"Set-DSClientUser",
"Set-DSClientUserGroup",
"Set-DSClientUserGroupRole",
"Set-DSClientVMwareVADPBackupSet",
"Set-DSClientWeeklySchedule",
"Set-DSClientWinFsBackupSet",
"Start-DSClientBackup",
"Start-DSClientBackupSetDelete",
"Start-DSClientBackupSetRetention",
"Start-DSClientBackupSetValidation",
"Start-DSClientRestore",
"Start-DSClientSystemActivity",
"Start-DSClientValidation",
"Stop-DSClientActivity",
"Stop-DSClientGridNode",
"Sync-DSClientBackupSet",
"Unpublish-DSClientEncryptionKeys",
"Watch-DSClientActivity")

# Variables to export from this module
VariablesToExport = '*'

# Aliases to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no aliases to export.
AliasesToExport = @()

# DSC resources to export from this module
# DscResourcesToExport = @()

# List of all modules packaged with this module
# ModuleList = @()

# List of all files packaged with this module
# FileList = @()

# Private data to pass to the module specified in RootModule/ModuleToProcess. This may also contain a PSData hashtable with additional module metadata used by PowerShell.
PrivateData = @{

    PSData = @{

        # Tags applied to this module. These help with module discovery in online galleries.
        Tags = @("Asigra", "DS-Client", "DSClient", "Backup")

        # A URL to the license for this module.
        # LicenseUri = ''

        # A URL to the main website for this project.
        ProjectUri = 'https://github.com/McGlovin1337/PSAsigraDSClient'

        # A URL to an icon representing this module.
        # IconUri = ''

        # ReleaseNotes of this module
        ReleaseNotes = '
        v0.5.0
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

        v0.4.1
        - Fixed Add-DSClientUnixFsBackupSetItem clearing out existing Backup Set Items from the specified Backup Set
        - Fixed Read-DSClientUnixFsSource Recursion not working beyond one sub-item depth

        v0.4.0
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

        v0.3.1
        - Fixed Read-DSClientUnixFsSource "Sequence Contains no Elements" Exception when attempting to read top-level items/directories

        v0.3.0
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

        v0.2.0
        - Added 4x New Cmdlets: Get-DSClientConfiguration, Get-DSClientOS, Set-DSClientConfiguration, Watch-DSClientActivity
        - Added Progress Indicators to Read-DSClientUnixFsSource, Read-DSClientWinFsSource, Get-DSClientStoredItem, Get-DSClientRetentionRule and Get-DSClientScheduleDetail cmdlets
        - Improved Pipelining for a number of Cmdlets
        - Improved Verbose Output
        - Replaced Parameter "-Activity" with 3x Switch Parameters: "-DailyAdmin", "-WeeklyAdmin" and "-StatisticalAdmin"
        - Removed "-SetType" Parameter from Set-DSClientMSSqlServerBackupSet, Set-DSClientUnixFsBackupSet and Set-DSClientWinFsBackupSet Cmdlets
        - Fixed Cmdlet Export for Add-DSClientWinFsBackupSetItem        
        - Fixed Read-DSClientWinFsSource Cmdlet only showing File Items when the Recurse Switch Parameter was specified
        - Fixed "-BackupSetId" parameter not working with Get-DSClientBackupSetInfo Cmdlet

        v0.1.1
        - Fix typo in Module Manifest

        v0.1.0
        - Initial Public Pre-release Module Version'

    } # End of PSData hashtable

} # End of PrivateData hashtable

# HelpInfo URI of this module
# HelpInfoURI = ''

# Default prefix for commands exported from this module. Override the default prefix using Import-Module -Prefix.
# DefaultCommandPrefix = ''

}