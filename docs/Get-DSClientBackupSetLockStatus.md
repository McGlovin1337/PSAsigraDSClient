---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientBackupSetLockStatus

## SYNOPSIS
Return the Lock Status of a Backup Set

## SYNTAX

```
Get-DSClientBackupSetLockStatus [-BackupSetId] <Int32> [-ActivityType] <String> [<CommonParameters>]
```

## DESCRIPTION
Return the Lock Status of a Backup Set for a given Activity Type to be started.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientBackupSetLockStatus -BackupSetId 5 -ActivityType Backup
```

Checks the Lock Status of Backup Set with Id 5 for starting a Backup Activity

## PARAMETERS

### -ActivityType
Specify the Activity Type to check Lock Status

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Backup, Restore, Delete, Synchronization, DiscTapeRestore, BLMRequest, BLMRestore, Validation, Retention, SnapshotRestore, SnapshotTransfer

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -BackupSetId
Specify the Backup Set Id

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

### System.String

## OUTPUTS

### PSAsigraDSClient.GetDSClientBackupSetLockStatus+BackupSetLockStatus

## NOTES

## RELATED LINKS
