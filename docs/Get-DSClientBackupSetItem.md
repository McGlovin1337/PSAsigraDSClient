---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientBackupSetItem

## SYNOPSIS
Returns the Inclusion and Exclusion Items of a Backup Set

## SYNTAX

```
Get-DSClientBackupSetItem [[-BackupSetId] <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Returns the Inclusion and Exclusion Items of a Backup Set

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientBackupSetItem -BackupSetId 3
```

Returns the Inclusion and Exclusion items configured in Backup Set with Id 3

## PARAMETERS

### -BackupSetId
Specify the Backup Set to query

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

## OUTPUTS

### PSAsigraDSClient.BaseDSClientBackupSet+DSClientBackupSetItem

## NOTES

## RELATED LINKS
