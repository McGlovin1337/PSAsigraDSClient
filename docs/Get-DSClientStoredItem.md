---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientStoredItem

## SYNOPSIS
Return the Backed Up Items of a Backup Set

## SYNTAX

### hidefiles
```
Get-DSClientStoredItem [-Path] <String> [[-Filter] <String>] [-Recursive] [-RecursiveDepth <Int32>]
 [-ExcludePath <String[]>] [-HideFiles] [-CalculateDirectorySize] [-DateFrom <DateTime>] [-DateTo <DateTime>]
 [-DeletedDate <DateTime>] [<CommonParameters>]
```

### BackupSetId
```
Get-DSClientStoredItem [-Path] <String> [[-Filter] <String>] [-Recursive] [-RecursiveDepth <Int32>]
 [-ExcludePath <String[]>] [-HideFiles] [-HideDirectories] [-CalculateDirectorySize] [-BackupSetId] <Int32>
 [-DateFrom <DateTime>] [-DateTo <DateTime>] [-DeletedDate <DateTime>] [<CommonParameters>]
```

### hidedirs
```
Get-DSClientStoredItem [-Path] <String> [[-Filter] <String>] [-Recursive] [-RecursiveDepth <Int32>]
 [-ExcludePath <String[]>] [-HideDirectories] [-CalculateDirectorySize] [-DateFrom <DateTime>]
 [-DateTo <DateTime>] [-DeletedDate <DateTime>] [<CommonParameters>]
```

### DeleteSession
```
Get-DSClientStoredItem [-Path] <String> [[-Filter] <String>] [-Recursive] [-RecursiveDepth <Int32>]
 [-ExcludePath <String[]>] [-CalculateDirectorySize] [-UseDeleteSession] [-DateFrom <DateTime>]
 [-DateTo <DateTime>] [-DeletedDate <DateTime>] [<CommonParameters>]
```

### RestoreId
```
Get-DSClientStoredItem [-Path] <String> [[-Filter] <String>] [-Recursive] [-RecursiveDepth <Int32>]
 [-ExcludePath <String[]>] [-CalculateDirectorySize] -RestoreId <Int32> [-DateFrom <DateTime>]
 [-DateTo <DateTime>] [-DeletedDate <DateTime>] [<CommonParameters>]
```

### ValidationId
```
Get-DSClientStoredItem [-Path] <String> [[-Filter] <String>] [-Recursive] [-RecursiveDepth <Int32>]
 [-ExcludePath <String[]>] [-CalculateDirectorySize] -ValidationId <Int32> [-DateFrom <DateTime>]
 [-DateTo <DateTime>] [-DeletedDate <DateTime>] [<CommonParameters>]
```

## DESCRIPTION
Returns a list of items that have been backed up by a Backup Set

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientStoredItem -BackupSetId 5 -Path "C$\Users\user\Documents\myfile.txt"
```

Returns information for the file myfile.txt stored in the Backup Set with Id 5

### Example 2
```powershell
PS C:\> Get-DSClientStoredItem -BackupSetId 5 -Path "C$\" -Recursive -RecursiveDepth 2
```

Returns information for all Items stored withn the "C$\" path including 2 sub-direcotries deep

### Example 3
```powershell
PS C:\> Get-DSClientStoredItem -BackupSetId 5 -Path "C$\Users" -ExcludePath "C$\Users\John.Smith", "*bloggs" -Recursive -RecursiveDepth 2
```

Enumerates all Paths in C$\Users, but skips C$\Users\John.Smith and any path matching the pattern "*bloggs"

## PARAMETERS

### -BackupSetId
Specify Backup Set to Search

```yaml
Type: Int32
Parameter Sets: BackupSetId
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -DateFrom
Specify Date to Search From

```yaml
Type: DateTime
Parameter Sets: (All)
Aliases: StartTime

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DateTo
Specify Date to Search To

```yaml
Type: DateTime
Parameter Sets: (All)
Aliases: EndTime

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DeletedDate
Specify Date for Deleted Files from Source

```yaml
Type: DateTime
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Filter
Filter returned items

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Path
Specify the Full Path to the Item

```yaml
Type: String
Parameter Sets: (All)
Aliases: Folder

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Recursive
Specify to return items recursively

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

### -RecursiveDepth
Specify the rescursion depth

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UseDeleteSession
Specify to use Delete View stored in SessionState

```yaml
Type: SwitchParameter
Parameter Sets: DeleteSession
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -HideDirectories
{{ Fill HideDirectories Description }}

```yaml
Type: SwitchParameter
Parameter Sets: BackupSetId
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

```yaml
Type: SwitchParameter
Parameter Sets: hidedirs
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -HideFiles
Specify to Hide Files

```yaml
Type: SwitchParameter
Parameter Sets: hidefiles
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

```yaml
Type: SwitchParameter
Parameter Sets: BackupSetId
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ExcludePath
Specify Paths to Exclude during Recursion

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RestoreId
Specify an existing Restore Session Id

```yaml
Type: Int32
Parameter Sets: RestoreId
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CalculateDirectorySize
Return the Size of Directories/Folders

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

### -ValidationId
Specify and existing Validation Session Id

```yaml
Type: Int32
Parameter Sets: ValidationId
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

### System.Int32

### System.DateTime

## OUTPUTS

### PSAsigraDSClient.GetDSClientStoredItem+DSClientBackupSetItemInfo

## NOTES

## RELATED LINKS
