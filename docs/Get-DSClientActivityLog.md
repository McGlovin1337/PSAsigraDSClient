---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientActivityLog

## SYNOPSIS
Get the DS-Client Activity Log

## SYNTAX

### Id
```
Get-DSClientActivityLog -ActivityId <Int32> [[-StartTime] <DateTime>] [[-EndTime] <DateTime>]
 [<CommonParameters>]
```

### OtherFilters
```
Get-DSClientActivityLog [[-StartTime] <DateTime>] [[-EndTime] <DateTime>] [-ActivityType <String[]>]
 [-Status <String[]>] [-BackupSetId <Int32>] [-ScheduleId <Int32>] [-User <String>] [<CommonParameters>]
```

## DESCRIPTION
Get the DS-Client Activity Log

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientActivityLog -ActivityType "Backup"
```

Get a list of Backup Activities

## PARAMETERS

### -ActivityId
Specify ActivityId

```yaml
Type: Int32
Parameter Sets: Id
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -ActivityType
Specify the Activity Type to Filter on

```yaml
Type: String[]
Parameter Sets: OtherFilters
Aliases:
Accepted values: Backup, CDPBackup, Restore, DailyAdmin, WeeklyAdmin, Delete, Recovery, Synchronization, DiscTapeRequest, DiscTapeRestore, BLMRequest, OnlineFileSummary, Registration, LANAnalyze, BLMRestore, Validation, Retention, TapeConversion, CacheCopy, CacheMonitor, AppAutoUpgrade, Convert, CancelConvert, CleanLocalOnlyTrash, Connection, TestConnection, CloudDatabaseUpload, LANResourceDiscovery, SnapshotRestore, SnapshotTransfer, CancelSnapshotTransfer

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -BackupSetId
Specify Activities related to specific Backup Set

```yaml
Type: Int32
Parameter Sets: OtherFilters
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -EndTime
Specify End Date & Time

```yaml
Type: DateTime
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -ScheduleId
Specify Activities related to a specific Schedule

```yaml
Type: Int32
Parameter Sets: OtherFilters
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -StartTime
Specify Start Date & Time

```yaml
Type: DateTime
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Status
Specify the Activity Completion Status to Filter on

```yaml
Type: String[]
Parameter Sets: OtherFilters
Aliases:
Accepted values: Succeeded, NoConnection, Disconnected, TooManyErrors, Exception, PrePostFailure, NoResource, StorageLimitReached, ShareUnavailable, DSClientShutdown, BackupOutOfSync, TimeLimitReached, UserStopped, BackupSetLocked, UpgradeTriggered, ClientQuotaReached, CustomerQuotaReached, FatalError, UnexpectedStop, SynchronisationFailed, DatabaseOutOfSpace, NoLocalStoragePath, SystemStopped, OracleNotMounted, OracleNotOpen, NoCatalogRoot, NoCatalog, NoDiskSpace, YieldOtherActivity, NoInitialBuffer, SysAdminDisabled, SourceUnavailable, MetadataUnavailable, FailedVSSSnapshot, SelfContainedLimitReached, DSClientQuit

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -User
Specify Activities related to a specific User

```yaml
Type: String
Parameter Sets: OtherFilters
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.DateTime

### System.Int32

### System.String[]

### System.String

## OUTPUTS

### PSAsigraDSClient.GetDSClientActivityLog+DSClientAcivityLog

## NOTES

## RELATED LINKS
