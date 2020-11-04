---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Add-DSClientSNMPCommunity

## SYNOPSIS
Add a new SNMP Community and/or Hosts

## SYNTAX

```
Add-DSClientSNMPCommunity [-Community] <String> [-Hosts] <String[]> [<CommonParameters>]
```

## DESCRIPTION
Add a new SNMP Community and Hosts or add hosts to an existing Community

## EXAMPLES

### Example 1
```
PS C:\> Add-DSClientSNMPCommunity -Community "private" -Hosts "192.168.1.100"
```

Creates a new SNMP Community named "private" if it doesn't already exist, and adds the host "192.168.1.100"

## PARAMETERS

### -Community
Specify the SNMP Community

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -Hosts
Specify Hosts to add to SNMP Community

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String
### System.String[]
## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
