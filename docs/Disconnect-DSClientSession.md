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

### session
```
Disconnect-DSClientSession [-Session] <DSClientSession> [<CommonParameters>]
```

### id
```
Disconnect-DSClientSession -Id <Int32> [<CommonParameters>]
```

## DESCRIPTION
Disconnect and Logout from a DS-Client Session

## EXAMPLES

### Example 1
```powershell
PS C:\> Disconnect-DSClientSession -Session $Session
```

Logout of the DS-Client specified in $Session

### Example 2
```powershell
PS C:\> Disconnect-DSClientSession -Id 2
```

Logout of the DS-Client specified by the Session Id

## PARAMETERS

### -Session
Specify the Session to Disconnect

```yaml
Type: DSClientSession
Parameter Sets: session
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -Id
Specify the Id of the Session to Disconnect

```yaml
Type: Int32
Parameter Sets: id
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

### PSAsigraDSClient.DSClientCommon+DSClientSession

## OUTPUTS

### PSAsigraDSClient.DSClientCommon+DSClientSession

## NOTES

## RELATED LINKS
