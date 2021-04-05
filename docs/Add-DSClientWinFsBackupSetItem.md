---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Add-DSClientWinFsBackupSetItem

## SYNOPSIS
Adds a Windows File System Inclusion or Exclusion Item to a Backup Set

## SYNTAX

### inclusion
```
Add-DSClientWinFsBackupSetItem [-ExcludeAltDataStreams] [-ExcludePermissions] [-BackupSetId] <Int32>
 [-Inclusion] -Path <String> -Filter <String> [-ExcludeSubDirs] -MaxGenerations <Int32> [<CommonParameters>]
```

### exclusion
```
Add-DSClientWinFsBackupSetItem [-BackupSetId] <Int32> [-Exclusion] -Path <String> -Filter <String>
 [-ExcludeSubDirs] [<CommonParameters>]
```

### regex
```
Add-DSClientWinFsBackupSetItem [-BackupSetId] <Int32> [-RegexExclusion] -Path <String> -Filter <String>
 [-ExcludeSubDirs] [-RegexMatchDirectory] [-RegexCaseInsensitive] [<CommonParameters>]
```

## DESCRIPTION
Adds a Windows File System Inclusion or Exclusion Item to an existing Windows File System Backup Set

## EXAMPLES

### Example 1 - Create an Inclusion
```powershell
PS C:\> Add-DSClientWinFsBackupSetItem -BackupSetId 4 -Path 'F$\' -Filter '*.*' -MaxGenerations 30 -Inclusion
```

Creates an Inclusion Item for all Files & Folders in the 'F$\' Path

### Example 2 - Create an Exclusion
```powershell
PS C:\> Add-DSClientWinFsBackupSetItem -BackupSetId 4 -Path 'C$\Windows\Temp' -Filter '*.*' -Exclusion
```

Creates an Exclusion Item for all Files & Folders in the 'C$\Windows\Temp' Path

### Example 3 - Create a Regex Exclusion
```powershell
PS C:\> Add-DSClientWinFsBackupSetItem -BackupSetId 4 -Path 'C$\' -Expression '.*\*.sys' -ExcludeSubDirs -RegexExclusion
```

Creates a Regex Exclusion that Excludes all Files matching the expression '.*\*.sys' in 'C$\' only

### Example 4 - Include a Specific File
```powershell
PS C:\> Add-DSClientWinFsBackupSetItem -BackupSetId 4 -Path 'E$\docs' -Filter 'myfile.txt' -MaxGenerations 9999 -ExcludeSubDirs -Inclusion
```

Creates an Inclusion Item for myfile.txt in the 'E$\docs' Path

## PARAMETERS

### -BackupSetId
Specify the Backup Set to modify

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

### -ExcludeAltDataStreams
Include Alternate Data Streams for IncludedItems

```yaml
Type: SwitchParameter
Parameter Sets: inclusion
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ExcludePermissions
Include Permissions for IncludedItems

```yaml
Type: SwitchParameter
Parameter Sets: inclusion
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MaxGenerations
Max Number of Generations for Included Items

```yaml
Type: Int32
Parameter Sets: inclusion
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RegexCaseInsensitive
Specify if Regex Exclusions Items are case insensitive

```yaml
Type: SwitchParameter
Parameter Sets: regex
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

### -Exclusion
Specify that this is an Exclusion Item

```yaml
Type: SwitchParameter
Parameter Sets: exclusion
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Filter
Specify the Item Filter

```yaml
Type: String
Parameter Sets: inclusion
Aliases: Expression, Item

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

```yaml
Type: String
Parameter Sets: exclusion, regex
Aliases: Expression, Item

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Inclusion
Specify that this is an Inclusion Item

```yaml
Type: SwitchParameter
Parameter Sets: inclusion
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Path
Path to add to Backup Set

```yaml
Type: String
Parameter Sets: (All)
Aliases: Folder, Directory

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RegexExclusion
Specify that this is a Regex Exclusion Item

```yaml
Type: SwitchParameter
Parameter Sets: regex
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RegexMatchDirectory
Specify to also Directory Names with Regex pattern

```yaml
Type: SwitchParameter
Parameter Sets: regex
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
