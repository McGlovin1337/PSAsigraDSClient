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

### inclusion
```
Add-DSClientMSSqlServerBackupSetItem [-Filter <String>] [-RunDBCC] [-DBCCErrorStop] [-BackupLog]
 [-BackupSetId] <Int32> [-Inclusion] -Path <String> [-ExcludeSubDirs] -MaxGenerations <Int32>
 [<CommonParameters>]
```

### exclusion
```
Add-DSClientMSSqlServerBackupSetItem [-Filter <String>] [-RunDBCC] [-DBCCErrorStop] [-BackupLog]
 [-BackupSetId] <Int32> [-Exclusion] -Path <String> [-ExcludeSubDirs] [<CommonParameters>]
```

### regex
```
Add-DSClientMSSqlServerBackupSetItem -Filter <String> [-RunDBCC] [-DBCCErrorStop] [-BackupLog]
 [-BackupSetId] <Int32> [-RegexExclusion] -Path <String> [-ExcludeSubDirs] [-RegexMatchDirectory]
 [-RegexCaseInsensitive] [<CommonParameters>]
```

## DESCRIPTION
Adds an MS SQL Server Database Item to an existing MS SQL Server Backup Set

## EXAMPLES

### Example 1 - Add a Database Inclusion
```powershell
PS C:\> Add-DSClientMSSqlServerBackupSetItem -BackupSetId 2 -Inclusion -Path 'SqlServer\Database1' -RunDBCC -MaxGenerations 9999
```

Adds a Database named "Database1" to the Backup Set with Id 2 and runs DBCC

### Example 2 - Add a Database Instance Inclusion
```powershell
PS C:\> Add-DSClientMSSqlServerBackupSetItem -BackupSetId 10 -Inclusion -Path '10.0.0.1' -RunDBCC -BackupLog -MaxGenerations 30
```

Adds all Databases in the Default Instance on 10.0.0.1, and runs DBCC Check and performs Transaction Log Backup

### Example 3 - Exclude a Database
```powershell
PS C:\> Add-DSClientMSSqlServerBackupSetItem -BackupSetId 5 -Exclusion -Path '10.0.0.1\tempdb'
```

Excludes the database named tempdb from the Default Instance on 10.0.0.1

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

Required: True
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

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

```yaml
Type: String
Parameter Sets: exclusion
Aliases: Expression, Item

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

```yaml
Type: String
Parameter Sets: regex
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
