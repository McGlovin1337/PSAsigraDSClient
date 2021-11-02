---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Add-DSClientMonthlySchedule

## SYNOPSIS
Adds a Monthly Schedule Detail to a Schedule

## SYNTAX

```
Add-DSClientMonthlySchedule [[-RepeatMonths] <Int32>] [[-ScheduleDay] <Int32>] [[-MonthlyStartDay] <String>]
 [[-StartDate] <DateTime>] [[-EndDate] <DateTime>] [-ScheduleId] <Int32> -StartTime <String>
 [-EndTime <String>] [-NoEndTime] [-HourlyFrequency <Int32>] [-Backup] [-Retention] [-Validation] [-BLM]
 [-LANScan] [-CleanTrash] [-LastGenOnly] [-ExcludeDeleted] [-Resume] [-IncludeAllGenerations] [-BackReference]
 [-PackageClosing <String>] [<CommonParameters>]
```

## DESCRIPTION
Adds a Monthly Schedule Detail to aan existing Schedule

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-DSClientMonthlySchedule -ScheduleId 4 -RepeatMonths 1 -ScheduleDay 28 -StartTime "21:30" -NoEndTime -Backup -Retention -Validation
```

Adds a Monthly Schedule that repeats every month on the last day of the month starting at 21:30 and performs Backup, Retention and Validation tasks

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

### -EndDate
Set the End Date for this Schedule Detail

```yaml
Type: DateTime
Parameter Sets: (All)
Aliases:

Required: False
Position: 5
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

### -MonthlyStartDay
Set the Monthly Start Day

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: DayOfMonth, Mon, Tue, Wed, Thu, Fri, Sat, Sun, Day, WeekDay, WeekEndDay

Required: False
Position: 3
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

### -RepeatMonths
Set the Repeat Frequency in Months

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
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

### -ScheduleDay
Set the Day of the Month the Schedule Executes on

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
Position: 4
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

### System.Void

## NOTES

## RELATED LINKS
