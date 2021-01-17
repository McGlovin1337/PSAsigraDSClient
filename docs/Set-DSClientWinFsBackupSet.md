---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Set-DSClientWinFsBackupSet

## SYNOPSIS
Changes the Configuration of a Windows File System Backup Set

## SYNTAX

```
Set-DSClientWinFsBackupSet [-BackupSetId] <Int32> [[-Name] <String>] [-Compression <String>]
 [-BackupRemoteStorage] [-BackupSingleInstanceStore] [-CheckCommonFiles] [-UseVSS] [-ExcludeVSSComponents]
 [-IgnoreVSSComponents] [-IgnoreVSSWriters] [-FollowJunctionPoints] [-NoAutoFileFilter]
 [-ExcludeOldFilesByDate] [-ExcludeOldFilesDate <DateTime>] [-ExcludeOldFilesByTimeSpan]
 [-ExcludeOldFilesTimeSpan <String>] [-ExcludeOldFilesTimeSpanValue <Int32>] [-UseBuffer]
 [-ExcludeAltDataStreams] [-ExcludePermissions] [-CDPInterval <Int32>] [-CDPStoppedChangingForInterval]
 [-CDPStopForRetention] [-CDPStopForBLM] [-CDPStopForValidation] [-Disabled] [-ScheduleId <Int32>]
 [-RetentionRuleId <Int32>] [-SchedulePriority <Int32>] [-ForceBackup] [-PreScan] [-ReadBufferSize <Int32>]
 [-BackupErrorLimit <Int32>] [-UseDetailedLog] [-InfinateBLMGenerations] [-UseLocalStorage]
 [-LocalStoragePath <String>] [-UseTransmissionCache] [-NotificationMethod <String>]
 [-NotificationRecipient <String>] [-NotificationCompletion <String[]>] [-NotificationEmailOptions <String[]>]
 [-SnmpTrapNotifications <String[]>] [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
Changes the Configuration of a Windows File System Backup Set

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DSClientWinFsBackupSet -BackupSetId 5 -ScheduleId 7
```

Assigns the Schedule with Id 7 to Backup Set with Id 5

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

### -BackupSetId
Specify the Backup Set to Modify

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
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

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
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

### -Name
Set the Name of the Backup Set

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
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

### System.Int32

### System.String

### System.String[]

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
