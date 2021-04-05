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

### inclusion
```
Add-DSClientUnixFsBackupSetItem [-ExcludeACLs] [-ExcludePosixACLs] [-BackupSetId] <Int32> [-Inclusion]
 -Path <String> -Filter <String> [-ExcludeSubDirs] -MaxGenerations <Int32> [<CommonParameters>]
```

### exclusion
```
Add-DSClientUnixFsBackupSetItem [-ExcludeACLs] [-ExcludePosixACLs] [-BackupSetId] <Int32> [-Exclusion]
 -Path <String> -Filter <String> [-ExcludeSubDirs] [<CommonParameters>]
```

### regex
```
Add-DSClientUnixFsBackupSetItem [-ExcludeACLs] [-ExcludePosixACLs] [-BackupSetId] <Int32> [-RegexExclusion]
 -Path <String> -Filter <String> [-ExcludeSubDirs] [-RegexMatchDirectory] [-RegexCaseInsensitive]
 [<CommonParameters>]
```

## DESCRIPTION
Adds a Unix File System Inclusion/Exclusion Item to an existing Unix File System Backup Set

## EXAMPLES

### Example 1 - Create Inclusion Item
```powershell
PS C:\> Add-DSClientUnixFsBackupSetItem -BackupSetId 1 -Inclusion -Path '/\home' -Filter '*' -MaxGenerations 9999
```

Adds the "/\home" directory and all folders and files as an inclusion item to the Backup Set

### Example 2 - Create Exclusion Item
```powershell
PS C:\> Add-DSClientUnixFsBackupSetItem -BackupSetId 1 -Exclusion -Path '/\home\user' -Filter '*'
```

Excludes all the folders and files in the path '/\home\user'

### Example 3 - Create Regex Exclusion Item
```powershell
PS C:\> Add-DSClientUnixFsBackupSetItem -BackupSetId 1 -RegexExclusion -Path '/' -Expression '.*\.txt'
```

Excludes all .txt files

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
