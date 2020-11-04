---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientBackupSetSessions

## SYNOPSIS
Returns the list of Backup Sessions for a Backup Set

## SYNTAX

```
Get-DSClientBackupSetSessions [-BackupSetId] <Int32> [<CommonParameters>]
```

## DESCRIPTION
Returns the list of Backup Sessions for a specified Backup Set

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientBackupSetSessions -BackupSetId 5
```

Returns the Backup Sessions for Backup Set with Id 5

## PARAMETERS

### -BackupSetId
Specify Backup Set to retrieve Sessions for

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

## OUTPUTS

### PSAsigraDSClient.GetDSClientBackupSetSessions+DSClientBackupSessions

## NOTES

## RELATED LINKS
