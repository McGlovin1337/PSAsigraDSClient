---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Set-DSClientMSSqlServerBackupSet

## SYNOPSIS
Changes the Configuration of an MS SQL Server Backup Set

## SYNTAX

```
Set-DSClientMSSqlServerBackupSet [[-BackupSetId] <Int32>] [[-Name] <String>] [-Compression <String>]
 [-DumpMethod <String>] [-BackupMethod <String>] [-DisableFullMonthly] [-DisableFullWeekly]
 [-DisableFullPeriod] [-DisableSkipWeekDays] [-Credential <PSCredential>] [-DbCredential <PSCredential>]
 [-DumpPath <String>] [-FullMonthlyDay <Int32>] [-FullMonthlyTime <String>] [-FullWeeklyDay <String>]
 [-FullWeeklyTime <String>] [-FullPeriod <String>] [-FullPeriodValue <Int32>] [-SkipWeekDays <String[]>]
 [-SkipWeekDaysFrom <String>] [-SkipWeekDaysTo <String>] [-Disabled] [-ScheduleId <Int32>]
 [-RetentionRuleId <Int32>] [-SchedulePriority <Int32>] [-ForceBackup] [-PreScan] [-ReadBufferSize <Int32>]
 [-BackupErrorLimit <Int32>] [-UseDetailedLog] [-InfinateBLMGenerations] [-UseLocalStorage]
 [-LocalStoragePath <String>] [-UseTransmissionCache] [-SnmpTrapNotifications <String[]>] [-PassThru] [-WhatIf]
 [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Changes the Configuration of an MS SQL Server Backup Set

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DSClientMSSqlServerBackupSet -BackupSetId 5 -Name "Full Database Backup"
```

Renames the Backup Set with Id of 5 to "Full Database Backup"

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

### -BackupMethod
Specify the SQL Backup Method

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Full, FullDiff, FullInc

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -BackupSetId
Specify the Backup Set to modify

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
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

### -Credential
Specify Computer Credentials

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

### -DbCredential
Specify Database Credentials

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

### -DisableFullMonthly
Disable Monthly Full Backups

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

### -DisableFullPeriod
Disable Periodic Full Backups

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

### -DisableFullWeekly
Disable Weekly Full Backups

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

### -DisableSkipWeekDays
Disable Skipping Weekday Full Backups

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

### -DumpMethod
Specify a Dump Method

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: DumpLocal, DumpBuffer, DumpPipe

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DumpPath
Specify Path to Dump Database

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

### -FullMonthlyDay
Specify Day of Month for Full Backup

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

### -FullMonthlyTime
Specify Time for Monthly Full Backup

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

### -FullPeriod
Specify Periodic Full Backup

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

### -FullPeriodValue
Specify Value for Periodic Full Backup

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

### -FullWeeklyDay
Specify Day for Weekly Full Backup

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FullWeeklyTime
Specify Time for Weekly Full Backup

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
Specify a new Name for the Backup Set

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
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

### -SkipWeekDays
Specify Week Days to Skip Full Backups

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:
Accepted values: Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SkipWeekDaysFrom
Specify Time to Skip Week Day Full Backups From

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

### -SkipWeekDaysTo
Specify Time to Skip Week Day Full Backups To

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

### -Confirm
Prompts you for confirmation before running the cmdlet.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: cf

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

### -WhatIf
Shows what would happen if the cmdlet runs. The cmdlet is not run.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: wi

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
