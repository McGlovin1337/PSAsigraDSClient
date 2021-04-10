---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Add-DSClientMSSqlServerBackupSetItem

## SYNOPSIS
Adds an MS SQL Server Database Item to a Backup Set

## SYNTAX

```
Add-DSClientMSSqlServerBackupSetItem [[-BackupSetId] <Int32>] [-IncludeItem <String[]>]
 [-MaxGenerations <Int32>] [-ExcludeItem <String[]>] [-RegexExcludeItem <String[]>]
 [-RegexExclusionPath <String>] [-RegexMatchDirectory] [-RegexCaseInsensitive] [-RunDBCC] [-DBCCErrorStop]
 [-BackupLog] [-ExcludeSubDirs] [<CommonParameters>]
```

## DESCRIPTION
Adds an MS SQL Server Database Item to an existing MS SQL Server Backup Set

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-DSClientMSSqlServerBackupSetItem -BackupSetId 2 -IncludeItem "Instance\\Database1" -RunDBCC
```

Adds a Database named "Database1" to the Backup Set with Id 2 and runs DBCC

## PARAMETERS

### -BackupLog
Specify to Backup Transaction Log

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

### -BackupSetId
Specify the Backup Set to Add items to

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

### -DBCCErrorStop
Specify to Stop on DBCC Errors

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

### -ExcludeItem
Items to Exclude from Backup Set

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -IncludeItem
Items to Include in Backup Set

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -MaxGenerations
Max Number of Generations for Included Items

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

### -RegexCaseInsensitive
Specify if Regex Exclusions Items are case insensitive

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

### -RegexExcludeItem
Specify Regex Item Exclusion Patterns

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RegexExclusionPath
Specify Path for Regex Exclusion Item

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

### -RunDBCC
Specify to run Database Consistency Check DBCC

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

### -ExcludeSubDirs
Specify to exclude Sub-Directories

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

### -RegexMatchDirectory
Specify to also Exclude Directories with Regex pattern

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

### System.String[]

### System.String

### System.Management.Automation.SwitchParameter

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
