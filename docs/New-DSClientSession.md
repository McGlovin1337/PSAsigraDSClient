---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# New-DSClientSession

## SYNOPSIS
Create and Establish a new DS-Client Session

## SYNTAX

```
New-DSClientSession -HostName <String> [-Credential] <PSCredential> [-Name <String>] [[-Port] <UInt16>]
 [-NoSSL] [[-APIVersion] <String>] [<CommonParameters>]
```

## DESCRIPTION
Create and Establish a new DS-Client Session.

## EXAMPLES

### Example 1
```powershell
PS C:\> $Session = New-DSClientSession -Host mydsclient -Credential (Get-Credential)
```

Creates a new DS-Client Session to the host mydsclient and stores it in the variable $Session

## PARAMETERS

### -APIVersion
Specify the Asigra DS-Client API Version to use

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 4
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Credential
Specify Credentials to use to connect to DSClient

```yaml
Type: PSCredential
Parameter Sets: (All)
Aliases:

Required: True
Position: 5
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -NoSSL
Specify to NOT establish an SSL Connection

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

### -Port
Specify the TCP port to connect to

```yaml
Type: UInt16
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -HostName
Specify the DS-Client Host to connect to

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -Name
Specify a name for this Session

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### PSAsigraDSClient.DSClientCommon+DSClientSession

## NOTES

## RELATED LINKS
