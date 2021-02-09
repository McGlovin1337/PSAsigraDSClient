---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Remove-DSClientBackupSetNotification

## SYNOPSIS
Remove a Notification from a Backup Set

## SYNTAX

```
Remove-DSClientBackupSetNotification [-BackupSetId] <Int32> [-NotificationId] <Int32> [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

## DESCRIPTION
Removes a specific Notification from the specified Backup Set.

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-DSClientBackupSetNotification -BackupSetId 5 -NotificationId 20
```

Removes the Notification with Id 20 from the Backup Set with Id 5

## PARAMETERS

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

### -Confirm
Prompts you for confirmation before running the cmdlet.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: cf

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -NotificationId
Specify the Id of the Notification to Remove

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -WhatIf
Shows what would happen if the cmdlet runs.
The cmdlet is not run.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: wi

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

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
