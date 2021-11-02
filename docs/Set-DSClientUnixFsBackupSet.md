---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Set-DSClientUnixFsBackupSet

## SYNOPSIS
Changes the Configuration of a Unix File System Backup Set

## SYNTAX

```
Set-DSClientUnixFsBackupSet [-BackupSetId] <Int32> [[-Name] <String>] [-Compression <String>]
 [-SSHInterpreter <String>] [-SSHInterpreterPath <String>] [-SSHKeyFile <String>]
 [-SudoCredential <PSCredential>] [-CheckCommonFiles] [-FollowMountPoints] [-BackupHardLinks]
 [-IgnoreSnapshotFailure] [-UseSnapDiff] [-ExcludeOldFilesByDate] [-ExcludeOldFilesDate <DateTime>]
 [-ExcludeOldFilesByTimeSpan] [-ExcludeOldFilesTimeSpan <String>] [-ExcludeOldFilesTimeSpanValue <Int32>]
 [-UseBuffer] [-ExcludeACLs] [-ExcludePosixACLs] [-CDPInterval <Int32>] [-CDPStoppedChangingForInterval]
 [-CDPStopForRetention] [-CDPStopForBLM] [-CDPStopForValidation] [-CDPUseFileAlterationMonitor] [-Disabled]
 [-ScheduleId <Int32>] [-RetentionRuleId <Int32>] [-SchedulePriority <Int32>] [-ForceBackup] [-PreScan]
 [-ReadBufferSize <Int32>] [-BackupErrorLimit <Int32>] [-UseDetailedLog] [-InfinateBLMGenerations]
 [-UseLocalStorage] [-LocalStoragePath <String>] [-UseTransmissionCache] [-SnmpTrapNotifications <String[]>]
 [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
Changes the Configuration of a Unix File System Backup Set

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DSClientUnixFsBackupSet -BackupSetId 1 -CheckCommonFiles:$false -ForceBackup
```

Disables Common File Checking and Enables Forced backup of unchanged files for Backup Set with Id of 1

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

### -BackupHardLinks
Backup Hard Links

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

### -CDPUseFileAlterationMonitor
Specify CDP Linux to use File Alteration Monitor

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

### -ExcludeACLs
Exclude ACLs

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

### -ExcludePosixACLs
Exclude POSIX ACLs

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

### -FollowMountPoints
Specify to Follow Mount Points

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

### -IgnoreSnapshotFailure
Ignore Snapshot Failures

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

### -SSHInterpreter
Specify the SSH Interpreter to access the data

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Perl, Python, Direct

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SSHInterpreterPath
Specify SSH Interpreter path

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SSHKeyFile
Specify path to SSH Key File

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
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

### -SudoCredential
Specify SUDO User Credentials

```yaml
Type: PSCredential
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
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

### -UseSnapDiff
For NAS Use Snap Diff feature

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

### System.Void

## NOTES

## RELATED LINKS
