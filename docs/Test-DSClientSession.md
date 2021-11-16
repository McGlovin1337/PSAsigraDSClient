---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Test-DSClientSession

## SYNOPSIS
Test connectivity of existing DS-Client Session(s)

## SYNTAX

```
Test-DSClientSession [-Id <Int32[]>] [-Name <String[]>] [-HostName <String[]>] [-OperatingSystem <String[]>]
 [-State <String>] [-Retries <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Tests the ICMP, TCP and API Connectivity of a previously established DS-Client Session.

## EXAMPLES

### Example 1
```powershell
PS C:\> Test-DSClientSession -Retries 10
```

Tests connectivity of all DS-Client Sessions, and attempts to establish a connection up-to 10 times.

## PARAMETERS

### -HostName
Test DS-Client Sessions by HostName

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: True
```

### -Id
Test DS-Client Sessions by Id

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
Test DS-Client Sessions by Name

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: True
```

### -OperatingSystem
Test DS-Client Sessions by Operating System

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

### -Retries
Set the number of connection attempts

```yaml
Type: Int32
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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32[]

### System.String[]

### System.String

### System.Int32

## OUTPUTS

### PSAsigraDSClient.PSAsigraDSClient.DSClientConnection

## NOTES

## RELATED LINKS
