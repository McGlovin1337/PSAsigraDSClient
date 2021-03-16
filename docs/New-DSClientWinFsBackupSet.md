---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# New-DSClientWinFsBackupSet

## SYNOPSIS
Creates a New Windows File System Backup Set

## SYNTAX

```
New-DSClientWinFsBackupSet [-Name] <String> [-Computer] <String> [[-Credential] <PSCredential>]
 [-SetType] <String> [-IncludeItem <String[]>] [-MaxGenerations <Int32>] [-ExcludeItem <String[]>]
 [-RegexExcludeItem <String[]>] [-RegexExclusionPath <String>] [-RegexExcludeDirectory] [-RegexCaseInsensitive]
 [-NotificationMethod <String>] [-NotificationRecipient <String>] [-NotificationCompletion <String[]>]
 [-NotificationEmailOptions <String[]>] [-BackupRemoteStorage] [-BackupSingleInstanceStore] [-CheckCommonFiles]
 [-UseVSS] [-ExcludeVSSComponents] [-IgnoreVSSComponents] [-IgnoreVSSWriters] [-FollowJunctionPoints]
 [-NoAutoFileFilter] [-ExcludeOldFilesByDate] [-ExcludeOldFilesDate <DateTime>] [-ExcludeOldFilesByTimeSpan]
 [-ExcludeOldFilesTimeSpan <String>] [-ExcludeOldFilesTimeSpanValue <Int32>] [-UseBuffer]
 [-ExcludeAltDataStreams] [-ExcludePermissions] [-CDPInterval <Int32>] [-CDPStoppedChangingForInterval]
 [-CDPStopForRetention] [-CDPStopForBLM] [-CDPStopForValidation] -Compression <String> [-Disabled]
 [-ScheduleId <Int32>] [-RetentionRuleId <Int32>] [-SchedulePriority <Int32>] [-ForceBackup] [-PreScan]
 [-ReadBufferSize <Int32>] [-BackupErrorLimit <Int32>] [-UseDetailedLog] [-InfinateBLMGenerations]
 [-UseLocalStorage] [-LocalStoragePath <String>] [-UseTransmissionCache] [-SnmpTrapNotifications <String[]>]
 [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
Creates a New Windows File System Backup Set

## EXAMPLES

### Example 1
```powershell
PS C:\> New-DSClientWinFsBackupSet -Name "Backup of C$" -Computer "\\WindowsServer01" -Credential (Get-Credential administrator) -IncludeItem "C$\*.*" -MaxGenerations 30 -CheckCommonFiles -SetType "Offsite" -Compression "LZOP" -ScheduleId 3 -RetentionRuleId 1
```

Creates a new Windows File System Backup Set named "Backup of C$" for Computer "WindowsServer01"
Includes the "C$\*.*" item for a Maximum of 30 Generations with Common File Checking Enabled
Specified as an Offsite Backup with LZOP Compression method
Assigned Schedule with Id 3 and Retention Rule with Id 1

## PARAMETERS

### -BackupErrorLimit
Set the Backup Error limit

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -BackupRemoteStorage
Specify to Backup Remote Storage

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -BackupSingleInstanceStore
Specify to Backup Single Instance Store

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CDPInterval
Specify the CDP Backup Interval in Seconds

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CDPStopForBLM
Specify CDP Backup can be Suspended for Scheduled BLM

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CDPStopForRetention
Specify CDP Backup can be Suspended for Scheduled Retention

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CDPStopForValidation
Specify CDP Backup can be Suspended for Scheduled Validation

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CDPStoppedChangingForInterval
CDP Backup File When File Stopped Changing for interval duration

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CheckCommonFiles
Specify Common File Elimination

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Compression
Set the Compression Method to use

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: None, ZLIB, LZOP, ZLIB_LO, ZLIB_MED, ZLIB_HI

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Computer
The Computer the Backup Set will be assigned to

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Credential
Credentials to use

```yaml
Type: PSCredential
Parameter Sets: (All)
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Disabled
Specify this Backup Set should be set to Disabled

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ExcludeAltDataStreams
Include Alternate Data Streams for IncludedItems

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ExcludeItem
Items to Exclude from Backup Set

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -ExcludeOldFilesByDate
Old File Exclusions by Date

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ExcludeOldFilesByTimeSpan
Old File Exclusions by TimeSpan

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ExcludeOldFilesDate
Date for Old File Exclusions

```yaml
Type: DateTime
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ExcludeOldFilesTimeSpan
TimeSpan Unit to use for Old File Exclusions

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Seconds, Minutes, Hours, Days, Weeks, Months, Years

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ExcludeOldFilesTimeSpanValue
TimeSpan Value to use for Old File Exclusions

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ExcludePermissions
Include Permissions for IncludedItems

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ExcludeVSSComponents
Exclude VSS Components

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FollowJunctionPoints
Specify to follow Junction Points

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ForceBackup
Force Re-Backup of File even if it hasn't been modified

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -IgnoreVSSComponents
Ignore VSS Components

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -IgnoreVSSWriters
Ignore VSS Writers

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -IncludeItem
Items to Include in Backup Set

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -InfinateBLMGenerations
Set to use Infinate BLM Generations

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -LocalStoragePath
Local Storage Path For Local Backups and Cache

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -MaxGenerations
Max Number of Generations for Included Items

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Name
The name of the Backup Set

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -NoAutoFileFilter
No Automatic File Filter

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -NotificationCompletion
Completion Status to Notify on

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:
Accepted values: Incomplete, CompletedWithErrors, Successful, CompletedWithWarnings

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -NotificationEmailOptions
Email Notification Options

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:
Accepted values: DetailedInfo, AttachDetailedLog, CompressAttachment, HtmlFormat

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -NotificationMethod
Notification Method

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Email, Pager, Broadcast, Event

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -NotificationRecipient
Notification Recipient

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -PreScan
Set to PreScan before Backup

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ReadBufferSize
Set the Read Buffer Size

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RegexCaseInsensitive
Specify if Regex Exclusions Items are case insensitive

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RegexExcludeDirectory
Specify to also Exclude Directories with Regex pattern

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RegexExcludeItem
Specify Regex Item Exclusion Patterns

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RegexExclusionPath
Specify Path for Regex Exclusion Item

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RetentionRuleId
Set the Retention Rule this Backup Set will use

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -ScheduleId
Set the Schedule this Backup Set will use

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -SchedulePriority
Schedule Priority of Backup Set when assigned to Schedule

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -SetType
Set the Backup Set Type

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Offsite, Statistical, SelfContained, LocalOnly

Required: True
Position: 3
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -SnmpTrapNotifications
Specify Completion Status to send SNMP Traps

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:
Accepted values: Incomplete, CompletedWithErrors, Successful, CompletedWithWarnings

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -UseBuffer
Use DS-Client Buffer

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UseDetailedLog
Set to use Detailed Log

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UseLocalStorage
Set to use Local Storage

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UseTransmissionCache
Set to use Local Transmission Cache for Offsite Backup Sets

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UseVSS
Use Volume Shadow Copies (VSS)

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PassThru
Output Basic Backup Set Properties

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

### System.String[]

### System.Int32

### System.Management.Automation.SwitchParameter

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
