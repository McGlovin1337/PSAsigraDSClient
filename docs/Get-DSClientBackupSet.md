---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientBackupSet

## SYNOPSIS
Get the details of a specific Backup Set

## SYNTAX

```
Get-DSClientBackupSet -BackupSetId <Int32> [<CommonParameters>]
```

## DESCRIPTION
Get full details of a specific Backup Set

## EXAMPLES

### Example 1
```
PS C:\>Get-DSClientBackupSet -BackupSetId 3
```

Returns the Backup Set configuration details for the Backup Set with Id 3

## PARAMETERS

### -BackupSetId
Backup Set Id

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None
## OUTPUTS

### PSAsigraDSClient.GetDSClientBackupSet+BackupSetInfo

## NOTES

## RELATED LINKS
