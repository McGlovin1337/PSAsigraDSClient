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
ModuleVersion = '0.3.0'

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
"Add-DSClientDailySchedule",
"Add-DSClientMonthlySchedule",
"Add-DSClientMSSqlServerBackupSetItem",
"Add-DSClientMultiFactorAuthUser",
"Add-DSClientOneTimeSchedule",
"Add-DSClientSNMPCommunity",
"Add-DSClientTimeRetentionRule",
"Add-DSClientUnixFsBackupSetItem",
"Add-DSClientUserGroupRole",
"Add-DSClientVMwareVADPBackupSetItem",
"Add-DSClientWeeklySchedule",
"Add-DSClientWinFsBackupSetItem",
"Compare-DSClientEncryptionKeys",
"Disable-DSClientBackupSet",
"Disable-DSClientSchedule",
"Enable-DSClientBackupSet",
"Enable-DSClientSchedule",
"Enter-DSClientSession",
"Exit-DSClientSession",
"Get-DSClientActivityLog",
"Get-DSClientAdvancedConfig",
"Get-DSClientArchiveFilterRule",
"Get-DSClientAuditTrail",
"Get-DSClientBackupSet",
"Get-DSClientBackupSetInfo",
"Get-DSClientBackupSetItem",
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
"Get-DSClientRetentionRule",
"Get-DSClientRunningActivity",
"Get-DSClientSchedule",
"Get-DSClientScheduleDetail",
"Get-DSClientSMTPNotification",
"Get-DSClientSNMPCommunity",
"Get-DSClientSNMPInfo",
"Get-DSClientStoredItem",
"Get-DSClientSupportedDataType",
"Get-DSClientTools",
"Get-DSClientUser",
"Get-DSClientUserGroup",
"Get-DSClientUserGroupRole",
"Initialize-DSClientBackupSetDelete",
"Initialize-DSClientBackupSetRestore",
"Initialize-DSClientBackupSetValidation",
"New-DSClientArchiveFilterRule",
"New-DSClientMSSqlServerBackupSet",
"New-DSClientRetentionRule",
"New-DSClientSchedule",
"New-DSClientUnixFsBackupSet",
"New-DSClientVMwareVADPBackupSet",
"New-DSClientWinFsBackupSet",
"Publish-DSClientEncryptionKeys",
"Read-DSClientMSSqlServerSource",
"Read-DSClientUnixFsSource",
"Read-DSClientVMwareVADPSource",
"Read-DSClientWinFsSource",
"Register-DSClient",
"Remove-DSClientBackupSet",
"Remove-DSClientBackupSetItem",
"Remove-DSClientInitialBackupPath",
"Remove-DSClientRetentionRule",
"Remove-DSClientSchedule",
"Remove-DSClientScheduleDetail",
"Remove-DSClientSNMPCommunity",
"Remove-DSClientUser",
"Remove-DSClientUserGroup",
"Remove-DSClientUserGroupRole",
"Restore-DSClientDatabase",
"Restore-DSClientOrphanedBackupSet",
"Search-DSClientBackupSetData",
"Set-DSClientAdvancedConfig",
"Set-DSClientConfiguration",
"Set-DSClientDefaultConfiguration",
"Set-DSClientMSSqlServerBackupSet",
"Set-DSClientRegistrationInfo",
"Set-DSClientRetentionRule",
"Set-DSClientSMTPNotification",
"Set-DSClientSNMPHeartbeat",
"Set-DSClientUnixFsBackupSet",
"Set-DSClientUser",
"Set-DSClientUserGroup",
"Set-DSClientUserGroupRole",
"Set-DSClientVMwareVADPBackupSet",
"Set-DSClientWinFsBackupSet",
"Start-DSClientBackup",
"Start-DSClientBackupSetDelete",
"Start-DSClientBackupSetRetention",
"Start-DSClientBackupSetValidation",
"Start-DSClientMSSqlServerRestore",
"Start-DSClientSystemActivity",
"Start-DSClientUnixFsRestore",
"Start-DSClientWinFsRestore",
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

