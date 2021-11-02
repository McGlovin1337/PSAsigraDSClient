---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Set-DSClientConfiguration

## SYNOPSIS
Update the DS-Client Configuration Parameters

## SYNTAX

```
Set-DSClientConfiguration [-DisableDailyAdmin] [-DailyAdminTime <String>] [-DisableWeeklyAdmin]
 [-WeeklyAdminDay <String>] [-WeeklyAdminTime <String>] [-RebootAfterAdmin] [-CDPStrategy <String>]
 [-DatabaseBackup <String>] [-KeepDatabaseDump <String>] [-LogDuration <String>] [-ReconnectAttempts <Int32>]
 [-ReconnectInterval <Int32>] [-SkipPreScan] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Updates the DS-Client basic Configuration Parameters

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DSClientConfiguration -DailyAdminTime "13:00:00" -WeeklyAdminTime "15:00:00" -WeeklyAdminDay "Saturday" -RebootAfterAdmin
```

Sets the Daily Admin run time to 13:00, the Weekly Admin run time to 15:00 on Saturday and instructs the DS-Client Computer to Reboot following running Admin Task

## PARAMETERS

### -CDPStrategy
Specify the CDP Strategy

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Suspend, Skip

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DailyAdminTime
Set DS-Client Daily Admin Run Time

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

### -DatabaseBackup
Specify DS-Client Database Backup During Admin

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: None, WithDailyAdmin, WithWeeklyAdmin

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DisableDailyAdmin
Disable DS-Client Daily Admin

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

### -DisableWeeklyAdmin
Disable DS-Client Weekly Admin

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

### -KeepDatabaseDump
Specify Keeping DS-Client Database Dump Following Backup

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: AlwaysDelete, DeleteAfterSuccessfulBackup, DoNotDelete

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -LogDuration
Specify Duration to keep DS-Client Log Files

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: SixMonths, OneYear, TwoYears, ThreeYears, FourYears, FiveYears, SixYears, SevenYears

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RebootAfterAdmin
Reboot DS-Client After Running Daily or Weekly Admin

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

### -ReconnectAttempts
Specify the number of times to re-connect during backup

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

### -ReconnectInterval
Specify the interval in minutes to re-connect

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

### -SkipPreScan
Specify to Skip PreScan for Scheduled Backups

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

### -WeeklyAdminDay
Set DS-Client Weekly Admin Day

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

### -WeeklyAdminTime
Set DS-Client Weekly Admin Run Time

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

### None

## OUTPUTS

### System.Void

## NOTES

## RELATED LINKS
