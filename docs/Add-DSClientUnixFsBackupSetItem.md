---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Add-DSClientUnixFsBackupSetItem

## SYNOPSIS
Adds a Unix based Inclusion/Exclusion item to a Backup Set

## SYNTAX

```
Add-DSClientUnixFsBackupSetItem [[-BackupSetId] <Int32>] [-IncludeItem <String[]>] [-MaxGenerations <Int32>]
 [-ExcludeItem <String[]>] [-RegexExcludeItem <String[]>] [-RegexExclusionPath <String>] [-RegexMatchDirectory]
 [-RegexCaseInsensitive] [-ExcludeACLs] [-ExcludePosixACLs] [-ExcludeSubDirs] [<CommonParameters>]
```

## DESCRIPTION
Adds a Unix File System Inclusion/Exclusion Item to an existing Unix File System Backup Set

## EXAMPLES

### Example 1 - Add Inclusion Item
```powershell
PS C:\> Add-DSClientUnixFsBackupSetItem -BackupSetId 1 -IncludeItem '/\home\*' -MaxGenerations 15
```

Adds the "/\home\" directory and all files and sub-folders as an Inclusion Item to the Backup Set with Id 1

### Example 2 - Add Exclusion Item
```powershell
PS C:\> Add-DSClientUnixFsBackupSetItem -BackupSetId 1 -ExcludeItem '/\home\user\*.txt'
```

Excludes .txt files from the '/\home\user' path

### Example 3 - Add Regex Exclusion
```powershell
PS C:\> Add-DSClientUnixFsBackupSetItem -BackupSetId 1 -RegexExcludeItem '.*\*.bak' -RegexExclusionPath '/'
```

Excludes .bak files from the '/' drive

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

### -ExcludeACLs
Exclude ACLs

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

### -ExcludePosixACLs
Exclude POSIX ACLs

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
