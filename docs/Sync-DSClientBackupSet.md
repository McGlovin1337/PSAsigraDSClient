---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Sync-DSClientBackupSet

## SYNOPSIS
Force a Synchronization of a Backup Set

## SYNTAX

### Id
```
Sync-DSClientBackupSet [-DSSystemBased] [-BackupSetId] <Int32> [<CommonParameters>]
```

### Name
```
Sync-DSClientBackupSet [-DSSystemBased] [-Name] <String[]> [<CommonParameters>]
```

## DESCRIPTION
Force a Synchronization of a Backup Set

## EXAMPLES

### Example 1
```powershell
PS C:\> Sync-DSClientBackupSet -Name "Backup of*"
```

Starts a Synchronization of all backup sets with a name starting with "Backup of"

## PARAMETERS

### -BackupSetId
Specify the Backup Set Id

```yaml
Type: Int32
Parameter Sets: Id
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -DSSystemBased
Specify Sync should be DS-System based

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

### -Name
Specify Backup Set(s) by Name

```yaml
Type: String[]
Parameter Sets: Name
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

### System.String[]

## OUTPUTS

### PSAsigraDSClient.BaseDSClientStartBackupSetActivity+DSClientStartBackupSetActivity

## NOTES

## RELATED LINKS
