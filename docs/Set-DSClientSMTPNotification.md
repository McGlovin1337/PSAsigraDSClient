---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Set-DSClientSMTPNotification

## SYNOPSIS
Configure the SMTP Notification Configuration

## SYNTAX

```
Set-DSClientSMTPNotification [[-SmtpServer] <String>] [[-SmtpPort] <Int32>] [[-SmtpCredential] <PSCredential>]
 [-RequireSsl] [-RequireTls] [[-FromName] <String>] [[-FromAddress] <String>] [[-AdminEmail] <String>]
 [[-PagerEmail] <String>] [-SendSummary] [-SendDetail] [-SendHtmlSummary] [[-SubjectAdmin] <String>]
 [[-SubjectBackup] <String>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Configure the SMTP Notification Configuration

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DSClientSMTPNotification -SmtpServer "127.0.0.1" -SmtpPort 25 -RequireTls
```

Configures the DS-Client to use an SMTP Server running on the localhost and listening on port 25 and using TLS

## PARAMETERS

### -AdminEmail
Administrator Email Address(es)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 7
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -FromAddress
Email From Address

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 6
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -FromName
Email From Display Name

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 5
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -PagerEmail
Pager Email Address(es)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 8
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RequireSsl
SMTP Server Requires SSL?

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RequireTls
SMTP Server Requires TLS?

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: 4
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SendDetail
Send Backup Detail with Summary

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: 10
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SendHtmlSummary
Send Summary in HTML Format

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: 11
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SendSummary
Send Summary Email

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: 9
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SmtpCredential
SMTP Server Authentication Credentials

```yaml
Type: PSCredential
Parameter Sets: (All)
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SmtpPort
SMTP Server TCP Port

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -SmtpServer
SMTP Server Hostname or IP Address

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -SubjectAdmin
Subject line for Admin Activity notifcations (256 character limit)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 12
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -SubjectBackup
Subject line for Backup Activity notifications (256 character limit)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 13
Default value: None
Accept pipeline input: True (ByPropertyName)
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

### -WhatIf
Shows what would happen if the cmdlet runs. The cmdlet is not run.

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

### System.String

### System.Int32

## OUTPUTS

### System.String

## NOTES

## RELATED LINKS
