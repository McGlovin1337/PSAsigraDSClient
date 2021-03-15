---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Set-DSClientTimeRetentionOption

## SYNOPSIS
Updates an existing Time Retention Option

## SYNTAX

### interval
```
Set-DSClientTimeRetentionOption [-RetentionRuleId] <Int32> [-TimeRetentionId] <Int32>
 [-IntervalTimeValue <Int32>] [-IntervalTimeUnit <String>] [-ValidForValue <Int32>] [-ValidForUnit <String>]
 [-WhatIf] [-Confirm] [<CommonParameters>]
```

### weekly
```
Set-DSClientTimeRetentionOption [-RetentionRuleId] <Int32> [-TimeRetentionId] <Int32>
 [-WeeklyRetentionDay <String>] [-RetentionTime <String>] [-ValidForValue <Int32>] [-ValidForUnit <String>]
 [-WhatIf] [-Confirm] [<CommonParameters>]
```

### monthly
```
Set-DSClientTimeRetentionOption [-RetentionRuleId] <Int32> [-TimeRetentionId] <Int32>
 [-RetentionDayOfMonth <Int32>] [-RetentionTime <String>] [-ValidForValue <Int32>] [-ValidForUnit <String>]
 [-WhatIf] [-Confirm] [<CommonParameters>]
```

### yearly
```
Set-DSClientTimeRetentionOption [-RetentionRuleId] <Int32> [-TimeRetentionId] <Int32>
 [-RetentionDayOfMonth <Int32>] [-RetentionTime <String>] -YearlyRetentionMonth <String>
 [-ValidForValue <Int32>] [-ValidForUnit <String>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Updates an existing Time Retention Option for a given Retention Rule. Use Get-DSClientTimeRetentionOption to identify the Time Retention Option to modify.
This Cmdlet CANNOT change the Type of the Retention Option, e.g. an Interval Retention Option cannot be changed to a Weekly Retention Option. Instead Remove the Retention Option and Add a new one.

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
Parameter Sets: weekly
Aliases:
Accepted values: Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday

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

Required: False
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
