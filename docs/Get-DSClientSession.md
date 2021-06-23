---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientSession

## SYNOPSIS
Returns available DS-Client Sessions

## SYNTAX

```
Get-DSClientSession [-Id <Int32[]>] [-Name <String>] [-HostName <String>] [-Port <Int32[]>] [-State <String>]
 [-Transport <String>] [-OperatingSystem <String[]>] [<CommonParameters>]
```

## DESCRIPTION
Returns the available DS-Client Sessions created using New-DSClientSession Cmdlet

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientSession
```

Returns all sessions

## PARAMETERS

### -HostName
Retrieve DS-Client Sessions by HostName

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: True
```

### -Id
Retrieve DS-Client Sessions by Id

```yaml
Type: Int32[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Name
Retrieve DS-Client Sessions by Name

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: True
```

### -OperatingSystem
Retrieve DS-Client Sessions by Operating System

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:
Accepted values: Linux, Mac, Windows

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Port
Retrieve DS-Client Sessions by Port Number

```yaml
Type: Int32[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -State
Retrieve DS-Client Sessions by Connection State

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Connected, Disconnected

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Transport
Retrieve DS-Client Sessions by Transport Protocol

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: http, https

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32[]

### System.String

### System.String[]

## OUTPUTS

### PSAsigraDSClient.BaseDSClientSession+DSClientSession

## NOTES

## RELATED LINKS
