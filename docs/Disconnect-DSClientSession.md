---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Disconnect-DSClientSession

## SYNOPSIS
Disconnect and Logout from a DS-Client Session

## SYNTAX

```
Disconnect-DSClientSession [-Session] <DSClientSession> [<CommonParameters>]
```

## DESCRIPTION
Disconnect and Logout from a DS-Client Session

## EXAMPLES

### Example 1
```powershell
PS C:\> Disconnect-DSClientSession -Session $Session
```

Logout of the DS-Client specified in $Session

## PARAMETERS

### -Session
Specify the Session to Disconnect

```yaml
Type: DSClientSession
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

### PSAsigraDSClient.DSClientCommon+DSClientSession

## OUTPUTS

### PSAsigraDSClient.DSClientCommon+DSClientSession

## NOTES

## RELATED LINKS
