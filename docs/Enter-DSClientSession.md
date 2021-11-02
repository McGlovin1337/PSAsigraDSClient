---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Enter-DSClientSession

## SYNOPSIS
Establishes a connection to an Asigra DS-Client

## SYNTAX

### default (Default)
```
Enter-DSClientSession -HostName <String> [[-Port] <UInt16>] [-NoSSL] [[-APIVersion] <String>]
 [[-Credential] <PSCredential>] [<CommonParameters>]
```

### session
```
Enter-DSClientSession -Session <DSClientSession> [<CommonParameters>]
```

## DESCRIPTION
Establishes a connection to an Asigra DS-Client and stores as a session for management.

## EXAMPLES

### Example 1
```
PS C:\> Enter-DSClientSession -Host my.dsclient.local -Credential joe.bloggs
```

Establishes a connection to the DS-Client running on computer "my.dsclient.local", using Credential "joe.bloggs"

### Example 2
```
PS C:\> Enter-DSClientSession -Session $Session
```

Enters the DS-Client Session specified in $Session variable.

## PARAMETERS

### -APIVersion
Specify the Asigra DSClient API Version to use

```yaml
Type: String
Parameter Sets: default
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
Parameter Sets: default
Aliases:

Required: False
Position: 5
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -NoSSL
Specify to NOT establish an SSL Connection

```yaml
Type: SwitchParameter
Parameter Sets: default
Aliases:

Required: False
Position: 3
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Port
Specify the TCP port to connect to

```yaml
Type: UInt16
Parameter Sets: default
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Session
Specify an existing Session to use

```yaml
Type: DSClientSession
Parameter Sets: session
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -HostName
Specify the DS-Client Host to connect to

```yaml
Type: String
Parameter Sets: default
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

### System.String

## NOTES

## RELATED LINKS
