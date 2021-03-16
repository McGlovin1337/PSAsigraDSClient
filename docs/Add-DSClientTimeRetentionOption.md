---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Add-DSClientTimeRetentionOption

## SYNOPSIS
Add a Time Based Retention Option

## SYNTAX

### interval
```
Add-DSClientTimeRetentionOption [-RetentionRuleId] <Int32> -ValidForValue <Int32> -ValidForUnit <String>
 [-IntervalTimeValue <Int32>] [-IntervalTimeUnit <String>] [<CommonParameters>]
```

### weekly
```
Add-DSClientTimeRetentionOption [-RetentionRuleId] <Int32> -ValidForValue <Int32> -ValidForUnit <String>
 [-WeeklyRetentionDay <String>] [-RetentionTime <String>] [<CommonParameters>]
```

### monthly
```
Add-DSClientTimeRetentionOption [-RetentionRuleId] <Int32> -ValidForValue <Int32> -ValidForUnit <String>
 [-RetentionDayOfMonth <Int32>] [-RetentionTime <String>] [<CommonParameters>]
```

### yearly
```
Add-DSClientTimeRetentionOption [-RetentionRuleId] <Int32> -ValidForValue <Int32> -ValidForUnit <String>
 [-RetentionDayOfMonth <Int32>] [-RetentionTime <String>] -YearlyRetentionMonth <String> [<CommonParameters>]
```

## DESCRIPTION
Adds a Time Based Retention Option to an Existing Retention Rule

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-DSClientTimeRetentionOption -RetentionRuleId 5 -IntervalTimeValue 1 -IntervalTimeUnit "Days" -ValidForValue 7 -ValidForUnit "Days"
```

Adds a Time Based Retention Option to Retention Rule with Id 5 specifying to Keep 1 Generation Every Day for 7 Days

### Example 2
```powershell
PS C:\> Add-DSClientTimeRetentionOption -RetentionRuleId 10 -WeeklyRetentionDay "Saturday" -RetentionTime "23:59:59" -ValidForValue 1 -ValidForUnit "Months"
```

Adds a Time Based Retention Option to Retention Rule with Id 10 specifying to Keep 1 Generation Every Week On or Before Saturday at 23:59:59 for 1 Month

### Example 3
```powershell
PS C:\> Add-DSClientTimeRetentionOption -RetentionRuleId 15 -MonthlyRetentionDay 28 -RetentionTime "23:59:59" -ValidForValue 1 -ValidForUnit "Years"
```

Adds a Time Based Retention Option to Retention Rule with Id 15 specifying to Keep 1 Generation Every Month On or Before the Last Day of the Month at 23:59:59 for 1 Year

### Example 4
```powershell
PS C:\> Add-DSClientTimeRetentionOption -RetentionRuleId 20 -YearlyRetentionMonth "December" -MonthlyRetentionDay 28 -RetentionTime "23:59:59" -ValidForValue 7 -ValidForUnit "Years"
```

Adds a Time Based Retention Option to Retention Rule with Id 20 specifying to Keep 1 Generation Every Year On or Before the Last Day of December at 23:59:59 for 7 Years

## PARAMETERS

### -IntervalTimeUnit
Specify Interval Retention Time Unit

```yaml
Type: String
Parameter Sets: interval
Aliases:
Accepted values: Minutes, Hours, Days, Weeks, Months, Years

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -IntervalTimeValue
Specify Interval Retention Time Value

```yaml
Type: Int32
Parameter Sets: interval
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RetentionRuleId
Specify RetentionRuleId to Apply Time Retention to

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

### -WeeklyRetentionDay
Specify Weekday for Weekly Time Retention

```yaml
Type: String
Parameter Sets: weekly
Aliases:
Accepted values: Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -YearlyRetentionMonth
Specify Month for Yearly Time Retention

```yaml
Type: String
Parameter Sets: yearly
Aliases:
Accepted values: January, February, March, April, May, June, July, August, September, October, November, December

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RetentionDayOfMonth
Specify Day of Month to Keep One Generation on or before

```yaml
Type: Int32
Parameter Sets: monthly
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

```yaml
Type: Int32
Parameter Sets: yearly
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RetentionTime
Specify the Time of Day to Keep One Generation on or before

```yaml
Type: String
Parameter Sets: weekly
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

```yaml
Type: String
Parameter Sets: monthly, yearly
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -ValidForUnit
Specify the Time Unit this Time Retention Option is Valid For

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -ValidForValue
Specify the Time Value this Time Retention Option is Valid For

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

### System.String

### System.Management.Automation.SwitchParameter

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
