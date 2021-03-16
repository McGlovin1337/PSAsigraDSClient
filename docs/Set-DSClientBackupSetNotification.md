---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Set-DSClientBackupSetNotification

## SYNOPSIS
Updates an Existing Notification within a Backup Set

## SYNTAX

```
Set-DSClientBackupSetNotification [-BackupSetId] <Int32> [-NotificationId] <Int32>
 [-NotificationMethod <String>] [-NotificationRecipient <String>] [-NotificationCompletion <String[]>]
 [-NotificationEmailOptions <String[]>] [-PassThru] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Updates an Existing Notification within a Backup Set

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DSClientBackupSetNotification -BackupSetId 5 -NotificationId 10 -NotificationRecipient '<ADMIN>' -NotificationCompletion Incomplete
```

Updates the Notification with Id 10 within the Backup Set with Id 5 and sets the Recipient to <ADMIN> and the Notification on Completion Status to Incomplete

## PARAMETERS

### -BackupSetId
Specify the Backup Set with the Notification to Modify

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

### -NotificationCompletion
Completion Status to Notify on

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:
Accepted values: Incomplete, CompletedWithErrors, Successful, CompletedWithWarnings

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -NotificationEmailOptions
Email Notification Options

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:
Accepted values: DetailedInfo, AttachDetailedLog, CompressAttachment, HtmlFormat

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -NotificationId
Specify the Notification to Modify

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

### -NotificationMethod
Notification Method

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Email, Page, Broadcast, Event

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -NotificationRecipient
Notification Recipient

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -PassThru
Specify to Output Created Notification

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

### System.String

### System.String[]

## OUTPUTS

### PSAsigraDSClient.BaseDSClientNotification+DSClientBackupSetNotification

## NOTES

## RELATED LINKS
