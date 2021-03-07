---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Set-DSClientTimeRetentionOption

## SYNOPSIS
Updates and existing Time Retention Option

## SYNTAX

```
Set-DSClientTimeRetentionOption [-RetentionRuleId] <Int32> [-TimeRetentionId] <Int32>
 [-IntervalTimeValue <Int32>] [-IntervalTimeUnit <String>] [-IntervalValidForValue <Int32>]
 [-IntervalValidForUnit <String>] [-WeeklyRetentionDay <String>] [-WeeklyRetentionHour <Int32>]
 [-WeeklyRetentionMinute <Int32>] [-WeeklyValidForValue <Int32>] [-WeeklyValidForUnit <String>]
 [-MonthlyRetentionDay <Int32>] [-MonthlyRetentionHour <Int32>] [-MonthlyRetentionMinute <Int32>]
 [-MonthlyValidForValue <Int32>] [-MonthlyValidForUnit <String>] [-YearlyRetentionMonthDay <Int32>]
 [-YearlyRetentionMonth <String>] [-YearlyRetentionHour <Int32>] [-YearlyRetentionMinute <Int32>]
 [-YearlyValidForValue <Int32>] [-YearlyValidForUnit <String>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Updates and existing Time Retention Option for a given Retention Rule. Use Get-DSClientTimeRetentionOption to identify the Time Retention Option to modify.

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DSClientTimeRetentionOption -RetentionRuleId 70 -TimeRetentionId 3 -IntervalTimeValue 5 -IntervalTimeUnit Days
```

Updates an Interval Time Retention Option with Id 3 of Retention Rule with Id 70 to "Keep 1 Generation every 5 Days"

## PARAMETERS

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

### -IntervalTimeUnit
Specify Interval Retention Time Unit

```yaml
Type: String
Parameter Sets: (All)
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
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -IntervalValidForUnit
Specify Time Unit Interval is Valid For

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Hours, Days, Weeks, Months, Years

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -IntervalValidForValue
Specify Time Value Interval is Valid For

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

### -MonthlyRetentionDay
Specify Day of Month for Monthly Time Retention

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

### -MonthlyRetentionHour
Specify Monthly Retention Time Hour

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

### -MonthlyRetentionMinute
Specify Monthly Retention Time Minute

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

### -MonthlyValidForUnit
Specify Time Unit Monthly Time Retention is Valid for

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Hours, Days, Weeks, Months, Years

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -MonthlyValidForValue
Specify Time Value Monthly Time Retention is Valid for

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

### -TimeRetentionId
Specify the Time Retention Option Id to Remove

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -WeeklyRetentionDay
Specify Weekday for Weekly Time Retention

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -WeeklyRetentionHour
Specify Weekly Retention Time Hour

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

### -WeeklyRetentionMinute
Specify Weekly Retention Time Minute

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

### -WeeklyValidForUnit
Specify Time Unit Weekly Time Retention is Valid for

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Hours, Days, Weeks, Months, Years

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -WeeklyValidForValue
Specify Time Value Weekly Time Retention is Valid for

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

### -WhatIf
Shows what would happen if the cmdlet runs.
The cmdlet is not run.

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

### -YearlyRetentionHour
Specify Yearly Retention Time Hour

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

### -YearlyRetentionMinute
Specify Yearly Retention Time Minute

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

### -YearlyRetentionMonth
Specify Month for Yearly Time Retention

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: January, February, March, April, May, June, July, August, September, October, November, December

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -YearlyRetentionMonthDay
Specify Day of Month for Yearly Time Retention

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

### -YearlyValidForUnit
Specify Time Unit Yearly Time Retention is Valid for

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Hours, Days, Weeks, Months, Years

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -YearlyValidForValue
Specify Time Value Yearly Time Retention is Valid for

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

### System.String

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
