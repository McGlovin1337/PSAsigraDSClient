---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Connect-DSClientSession

## SYNOPSIS
Connects a previously disconnected DS-Client Session

## SYNTAX

### id (Default)
```
Connect-DSClientSession [-Id] <Int32[]> [<CommonParameters>]
```

### name
```
Connect-DSClientSession -Name <String[]> [<CommonParameters>]
```

### hostname
```
Connect-DSClientSession -HostName <String[]> [<CommonParameters>]
```

### state
```
Connect-DSClientSession -State <String> [<CommonParameters>]
```

### os
```
Connect-DSClientSession -OperatingSystem <String[]> [<CommonParameters>]
```

### sessionObj
```
Connect-DSClientSession [-Session] <DSClientSession[]> [<CommonParameters>]
```

## DESCRIPTION
Connects a previously disconnected DS-Client Session

## EXAMPLES

### Example 1
```powershell
PS C:\> Connect-DSClientSession -Id 1
```

Connects the DS-Client Session with Id of 1

### Example 2
```powershell
PS C:\> Connect-DSClientSession -Session $session
```

Connects the DS-Client Session specified in the variable $session

## PARAMETERS

### -HostName
Specify DS-Client Sessions by HostName

```yaml
Type: String[]
Parameter Sets: hostname
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: True
```

### -Id
Specify DS-Client Session by Id

```yaml
Type: Int32[]
Parameter Sets: id
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -Name
Specify DS-Client Sessions by Name

```yaml
Type: String[]
Parameter Sets: name
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: True
```

### -OperatingSystem
Specify DS-Client Sessions by Operating System

```yaml
Type: String[]
Parameter Sets: os
Aliases:
Accepted values: Linux, Mac, Windows

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Session
Specify DS-Client Session by DSClientSession Object

```yaml
Type: DSClientSession[]
Parameter Sets: sessionObj
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -State
Specify DS-Client Sessions by Connection State

```yaml
Type: String
Parameter Sets: state
Aliases:
Accepted values: Connected, Disconnected

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32[]

### System.String[]

### System.String

### PSAsigraDSClient.DSClientSession[]

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
