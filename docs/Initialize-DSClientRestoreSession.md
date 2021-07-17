---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Initialize-DSClientRestoreSession

## SYNOPSIS
Creates a new Restore Session for a specified Backup Set

## SYNTAX

```
Initialize-DSClientRestoreSession [-BackupSetId] <Int32> -RestoreReason <String>
 [-RestoreClassification <String>] [<CommonParameters>]
```

## DESCRIPTION
This Cmdlet Initializes a new Restore Session for the specified Backup Set, ready for Item Selection

## EXAMPLES

### Example 1
```powershell
PS C:\> Initialize-DSClientRestoreSession -BackupSetId 5 -RestoreReason 'UserErrorDataDeletion' -RestoreClassification 'Production'
```

Creates a 'Production' Restore Session for Backup Set with Id 5

## PARAMETERS

### -BackupSetId
Specify the Backup Set to Initialize a Restore Session for

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

### -RestoreClassification
Specify Restore Classification

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Production, Drill, ProductionDrill

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RestoreReason
Specify Reason for Restore

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: UserErrorDataDeletion, MaliciousIntent, DeviceLostOrStolen, HardwareMalfunction, SoftwareMalfunction, DataStolen, DataCorruption, NaturalDisasters, PowerOutages, OtherDisaster, PreviousGeneration, DeviceDamaged

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
