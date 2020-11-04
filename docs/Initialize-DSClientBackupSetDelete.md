---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Initialize-DSClientBackupSetDelete

## SYNOPSIS
Prepare a Backup Set for Data Deletion

## SYNTAX

### Exclude
```
Initialize-DSClientBackupSetDelete [-KeepGenerations <Int32>] [-ExcludeArchivePlaceholder]
 [-DateFrom <DateTime>] [-DateTo <DateTime>] [-DeletedDate <DateTime>] [<CommonParameters>]
```

### Only
```
Initialize-DSClientBackupSetDelete [-KeepGenerations <Int32>] [-OnlyArchivePlaceholder] [-DateFrom <DateTime>]
 [-DateTo <DateTime>] [-DeletedDate <DateTime>] [<CommonParameters>]
```

### BackupSetId
```
Initialize-DSClientBackupSetDelete [-KeepGenerations <Int32>] [-BackupSetId] <Int32> [-DateFrom <DateTime>]
 [-DateTo <DateTime>] [-DeletedDate <DateTime>] [<CommonParameters>]
```

## DESCRIPTION
Initialises a Backup Set for Data Deletion by storing a Data view in Session State. The Session can be queried using the Get-DSClientStoredItem Cmdlet to assist with Data Selection.
The Start-DSClientBackupSetDelete Cmdlet is used to Start the Deletion Activity.

## EXAMPLES

### Example 1
```powershell
PS C:\> Initialize-DSClientBackupSetDelete -BackupSetId 4 -KeepGenerations 1
```

Initialises a Backup Set deletion for the Backup Set with Id 4

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

### -ExcludeArchivePlaceholder
Specify to Exclude Archive Placeholders in Delete

```yaml
Type: SwitchParameter
Parameter Sets: Exclude
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -KeepGenerations
Specify the number of Generations to keep online

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

### -OnlyArchivePlaceholder
Specify to Only Delete Archive Placeholders

```yaml
Type: SwitchParameter
Parameter Sets: Only
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

### System.DateTime

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
