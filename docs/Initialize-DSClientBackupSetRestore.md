---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Initialize-DSClientBackupSetRestore

## SYNOPSIS
Prepare a Backup Set for Restore

## SYNTAX

```
Initialize-DSClientBackupSetRestore [-HideDeleted] [-ShowOnlyDeleted] [-VssComponentRestore]
 [-BackupSetId] <Int32> [-DateFrom <DateTime>] [-DateTo <DateTime>] [-DeletedDate <DateTime>]
 [<CommonParameters>]
```

## DESCRIPTION
Initialises a Backup Set ready for a Restore Activity by storing a Data view in Session State. The Session can be queried using the Get-DSClientStoredItem Cmdlet to assist with Data Selection.
The appropriate "Start-" Restore Cmdlet based on the Backup Set Data Type is used to Start the Restore Activity.

## EXAMPLES

### Example 1
```powershell
PS C:\> Initialize-DSClientBackupSetRestore -BackupSetId 10 -ShowOnlyDeleted
```

Initialises the Backup Set with Id 10 for a Restore Activity, showing only Items deleted from Source

## PARAMETERS

### -BackupSetId
Specify Backup Set to Search

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

### -HideDeleted
Specify to Hide Files Deleted from Source

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

### -ShowOnlyDeleted
Specify to Only Show Deleted files from Source

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

### -VssComponentRestore
For VSS Backup Sets, select if this will be a Component Level Restore

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

### System.DateTime

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
