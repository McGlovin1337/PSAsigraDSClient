---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientBackupSetNotification

## SYNOPSIS
Returns the Notifications Configured for a Backup Set

## SYNTAX

```
Get-DSClientBackupSetNotification [-BackupSetId] <Int32> [<CommonParameters>]
```

## DESCRIPTION
Returns the Notifications Configured for a Backup Set

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientBackupSetNotification -BackupSetId 10
```

Returns the Notifications configured for the Backup Set with Id 10

## PARAMETERS

### -BackupSetId
Specify the Backup Set to retrieve Notification Configuration

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

### PSAsigraDSClient.BaseDSClientNotification+DSClientBackupSetNotification

## NOTES

## RELATED LINKS
