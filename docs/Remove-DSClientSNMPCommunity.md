---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Remove-DSClientSNMPCommunity

## SYNOPSIS
Remove SNMP Community and Hosts

## SYNTAX

```
Remove-DSClientSNMPCommunity [-Community] <String> [[-Hosts] <String[]>] [<CommonParameters>]
```

## DESCRIPTION
Removes SNMP Communities and/or hosts from communities.
Removing the last host from a community will also remove the community.

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-DSClientSNMPCommunity -Community public
```

Removes the SNMP Community named public and all associated hosts

### Example 1
```powershell
PS C:\> Remove-DSClientSNMPCommunity -Community public -Hosts monitoring.local
```

Removes the host monitoring.local from the Community named public

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

Required: False
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
