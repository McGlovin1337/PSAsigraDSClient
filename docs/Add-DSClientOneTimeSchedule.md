---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Add-DSClientOneTimeSchedule

## SYNOPSIS
Adds a One-Time Schedule Detail to a Schedule

## SYNTAX

```
Add-DSClientOneTimeSchedule [[-StartDate] <DateTime>] [-ScheduleId] <Int32> -StartTime <String>
 [-EndTime <String>] [-NoEndTime] [-HourlyFrequency <Int32>] [-Backup] [-Retention] [-Validation] [-BLM]
 [-LANScan] [-CleanTrash] [-LastGenOnly] [-ExcludeDeleted] [-Resume] [-IncludeAllGenerations] [-BackReference]
 [-PackageClosing <String>] [<CommonParameters>]
```

## DESCRIPTION
Adds a One-Time Schedule Detail to an existing Schedule

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-DSClientOneTimeSchedule -ScheduleId 3 -StartDate "31/12/2020" -StartTime "18:00" -NoEndTime -Validation -BLM -PackageClosing "CloseAtEnd"
```

Adds a One-Time Schedule Detail to Schedule with Id 3, starting on 31 December 2020 at 18:00 and performs Validation and BLM activities

## PARAMETERS

### -BLM
Enable the BLM Task

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

### -BackReference
BLM Task Option: Specify if to use Back Referencing

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

### -Backup
Enable the Backup Task

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

### -CleanTrash
Enable the Clean Local-only Trash Task

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

### -EndTime
Specify the End Time in 24Hr Notation HH:mm:ss

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

### -ExcludeDeleted
Validation Task Option: Exclude Deleted Files from Source

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

### -HourlyFrequency
Set the Hourly Frequency

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

### -IncludeAllGenerations
BLM Task Option: Specify if to Include All Generations

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

### -LANScan
Enable the LAN Scan Shares Task

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

### -LastGenOnly
Validation Task Option: Validate Last Generation Only

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

### -NoEndTime
Specify No End Time

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

### -PackageClosing
BLM Task Option: Specify how the package should be closed

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: DoNotClose, CloseAtStart, CloseAtEnd

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Resume
Validation Task Option: Resume from previously interrupted location

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

### -Retention
Enable the Retention Task

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

### -ScheduleId
The ScheduleId to add Schedule Detail to

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

### -StartDate
Set the Start Date for this Schedule Detail

```yaml
Type: DateTime
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -StartTime
Specify the Start Time in 24Hr Notation HH:mm:ss

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Validation
Enable the Validation Task

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

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
