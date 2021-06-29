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

### id (Default)
```
Disconnect-DSClientSession -Id <Int32[]> [<CommonParameters>]
```

### name
```
Disconnect-DSClientSession -Name <String[]> [<CommonParameters>]
```

### hostname
```
Disconnect-DSClientSession -HostName <String[]> [<CommonParameters>]
```

### state
```
Disconnect-DSClientSession -State <String> [<CommonParameters>]
```

### os
```
Disconnect-DSClientSession -OperatingSystem <String[]> [<CommonParameters>]
```

### sessionObj
```
Disconnect-DSClientSession [-Session] <DSClientSession[]> [<CommonParameters>]
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
Type: DSClientSession[]
Parameter Sets: sessionObj
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
Type: Int32[]
Parameter Sets: id
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

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
Accept wildcard characters: False
```

### -OperatingSystem
Specify DS-Client Sessions by Operating System

```yaml
Type: String[]
Parameter Sets: os
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -State
Specify DS-Client Sessions by Connection State

```yaml
Type: String
Parameter Sets: state
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
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
