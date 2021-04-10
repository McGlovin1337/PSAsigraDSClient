---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Add-DSClientWinFsBackupSetItem

## SYNOPSIS
Adds a Windows File System Inclusion/Exclusion Item to a Backup Set

## SYNTAX

```
Add-DSClientWinFsBackupSetItem [[-BackupSetId] <Int32>] [-ExcludeAltDataStreams] [-ExcludePermissions]
 [-IncludeItem <String[]>] [-MaxGenerations <Int32>] [-ExcludeItem <String[]>] [-RegexExcludeItem <String[]>]
 [-RegexExclusionPath <String>] [-RegexMatchDirectory] [-RegexCaseInsensitive] [-ExcludeSubDirs]
 [<CommonParameters>]
```

## DESCRIPTION
Adds a Windows File System Inclusion/Exclusion to an existing Windows File System Backup Set

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-DSClientWinFsBackupSetItem -BackupSetId 4 -IncludeItem "F$\*.*", "E$\*.*" -MaxGenerations 15 -ExcludeItem "F$\Pagefile.sys"
```

Adds all inclusion items on F$\ and E$\ shares but excludes the file F$\Pagefile.sys

## PARAMETERS

### -BackupSetId
Specify the Backup Set to modify

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

### -ExcludeAltDataStreams
Include Alternate Data Streams for IncludedItems

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

### -ExcludePermissions
Include Permissions for IncludedItems

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
Specify to also Match Directory Names with Regex pattern

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
