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

```
Add-DSClientTimeRetentionOption [-RetentionRuleId] <Int32> [-KeepLastGens <Int32>]
 [-KeepAllGensTimeValue <Int32>] [-KeepAllGensTimeUnit <String>] [-IntervalTimeValue <Int32>]
 [-IntervalTimeUnit <String>] [-IntervalValidForValue <Int32>] [-IntervalValidForUnit <String>]
 [-WeeklyRetentionDay <String>] [-WeeklyRetentionHour <Int32>] [-WeeklyRetentionMinute <Int32>]
 [-WeeklyValidForValue <Int32>] [-WeeklyValidForUnit <String>] [-MonthlyRetentionDay <Int32>]
 [-MonthlyRetentionHour <Int32>] [-MonthlyRetentionMinute <Int32>] [-MonthlyValidForValue <Int32>]
 [-MonthlyValidForUnit <String>] [-YearlyRetentionMonthDay <Int32>] [-YearlyRetentionMonth <String>]
 [-YearlyRetentionHour <Int32>] [-YearlyRetentionMinute <Int32>] [-YearlyValidForValue <Int32>]
 [-YearlyValidForUnit <String>] [-DeleteObsoleteData] [-MoveObsoleteData] [-CreateNewBLMPackage]
 [-CleanupDeletedFiles] [-CleanupDeletedAfterValue <Int32>] [-CleanupDeletedAfterUnit <String>]
 [-CleanupDeletedKeepGens <Int32>] [-DeleteGensPriorToStub] [-DeleteNonStubGens]
 [-LSRetentionTimeValue <Int32>] [-LSRetentionTimeUnit <String>] [-LSCleanupDeletedFiles]
 [-LSCleanupDeletedAfterValue <Int32>] [-LSCleanupDeletedAfterUnit <String>]
 [-LSCleanupDeletedKeepGens <Int32>] [-DeleteUnreferencedFiles] [-DeleteIncompleteComponents]
 [<CommonParameters>]
```

## DESCRIPTION
Adds a Time Based Retention Option to an Existing Retention Rule

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-DSClientTimeRetentionOption -RetentionRuleId 5 -KeepLastGens 1 -KeepAllGensTimeValue 1 -KeepAllGensTimeUnit "Days" -IntervalTimeValue 1 -IntervalTimeUnit "Days" -IntervalValidForValue 7 -IntervalValidForUnit "Days" -DeleteObsoleteData -DeleteUnreferencedFiles -DeleteIncompleteComponents
```

Adds a Time Based Retention Option to Retention Rule with Id 5 and specifies to keep the last 1 Generations, Keep All Generations for 1 Day, and Keep 1 Generation Every Day for 7 Days, Delete Obsolete Data, Delete Unreferenced Files and Delete Incomplete Components

## PARAMETERS

### -CleanupDeletedAfterUnit
Cleanup Files Deleted from Source after Time period

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

### -CleanupDeletedAfterValue
Cleanup Files Deleted from Source after Time period

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

### -CleanupDeletedFiles
Specify to Cleanup Files Deleted from Source

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -CleanupDeletedKeepGens
Specify Generations to keep of Files Deleted from Source

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

### -CreateNewBLMPackage
Specify to create new BLM Packages when moving to BLM

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DeleteGensPriorToStub
Specify to Delete All Generations prior to Stub

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DeleteIncompleteComponents
Specify to Delete Incomplete Components

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DeleteNonStubGens
Specify to Delete Non-stub Generations prior to Stub

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DeleteObsoleteData
Specify to Delete Obsolete Data

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DeleteUnreferencedFiles
Specify to Delete Unreferenced Files

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
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

### -KeepAllGensTimeUnit
Specify Time Period Unit for keeping ALL Generations

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Minutes, Hours, Days

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -KeepAllGensTimeValue
Specify Time Period to keep ALL Generations

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

### -KeepLastGens
Specify the number of most recent Generations to keep

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

### -LSCleanupDeletedAfterUnit
Cleanup Files Deleted from Source after Time period on Local Storage

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

### -LSCleanupDeletedAfterValue
Cleanup Files Deleted from Source after Time period on Local Storage

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

### -LSCleanupDeletedFiles
Specify to Cleanup Files Deleted from Source on Local Storage

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -LSCleanupDeletedKeepGens
Specify Generations to keep of Files Deleted from Source

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

### -LSRetentionTimeUnit
Specify Local Storage Retention Time Unit

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

### -LSRetentionTimeValue
Specify Local Storage Retention Time Value

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

### -MoveObsoleteData
Specify to Move Obsolete Data to BLM

```yaml
Type: SwitchParameter
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

### System.Management.Automation.SwitchParameter

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
