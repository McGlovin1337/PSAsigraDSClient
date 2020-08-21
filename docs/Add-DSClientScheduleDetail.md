---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Add-DSClientScheduleDetail

## SYNOPSIS
{{ Fill in the Synopsis }}

## SYNTAX

```
Add-DSClientScheduleDetail [-ScheduleId] <Int32> [-Type] <String> [-StartTime] <String> [[-EndTime] <String>]
 [-NoEndTime] [[-HourlyFrequency] <Int32>] [-Backup] [-Retention] [-Validation] [-BLM] [-LANScan] [-CleanTrash]
 [-LastGenOnly] [-ExcludeDeleted] [-Resume] [-IncludeAllGenerations] [-BackReference]
 [[-PackageClosing] <String>] [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -BLM
Enable the BLM Task

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: 13
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
Position: 20
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
Position: 10
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
Position: 15
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
Position: 7
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
Position: 17
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
Position: 9
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
Position: 19
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
Position: 14
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
Position: 16
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
Position: 8
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
Position: 21
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
Position: 18
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
Position: 11
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
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -StartTime
Specify the Start Time in 24Hr Notation HH:mm:ss

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 6
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Type
The Type of Schedule Detail to Add

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: OneTime, Daily, Weekly, Monthly

Required: True
Position: 1
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
Position: 12
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
