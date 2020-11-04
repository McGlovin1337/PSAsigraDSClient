---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# New-DSClientSchedule

## SYNOPSIS
Create a New Schedule

## SYNTAX

```
New-DSClientSchedule [-Name] <String> [[-ShortName] <String>] [[-CPUThrottle] <Int32>]
 [[-ConcurrentBackups] <Int32>] [-AdminOnly] [-Inactive] [-UseNetworkDetection] [-PassThru]
 [<CommonParameters>]
```

## DESCRIPTION
Creates a New blank Schedule. Use the appropriate Add-DSClientOneTimeSchedule, Add-DSClientDailySchedule, Add-DSClientWeeklySchedule and Add-DSClientMonthlySchedule to add appropriate schedule details and tasks.

## EXAMPLES

### Example 1
```powershell
PS C:\> New-DSClientSchedule -Name "Regular Backup and Retention"
```

Creates a new Schedule named "Regular Backup and Retention"

## PARAMETERS

### -AdminOnly
Specifies only Administrators can use this schedule

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

### -CPUThrottle
Set Backup CPU Throttle

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ConcurrentBackups
Concurrent backup sets allowed

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Inactive
Set the Schedule to Inactive

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

### -Name
The name of the Schedule

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -PassThru
Specify to return Schedule Info

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

### -ShortName
Set a Short Name

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

### -UseNetworkDetection
Start only if DS-System Connection available

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

## OUTPUTS

### PSAsigraDSClient.BaseDSClientSchedule+DSClientScheduleInfo

## NOTES

## RELATED LINKS
