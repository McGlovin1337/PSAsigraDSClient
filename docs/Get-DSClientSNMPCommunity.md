---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientSNMPCommunity

## SYNOPSIS
Display SNMP Communities

## SYNTAX

```
Get-DSClientSNMPCommunity [[-Community] <String>] [[-LiteralCommunity] <String[]>] [<CommonParameters>]
```

## DESCRIPTION
Display SNMP Communities and hosts configured on the DS-Client computer

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientSNMPCommunity
```

Display a list of all configured SNMP Communities

### Example 2
```powershell
PS C:\> Get-DSClientSNMPCommunity -Community pub*
```

Display list of SNMP Communities starting with pub

## PARAMETERS

### -Community
Specify the Community Name to Match

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -LiteralCommunity
Literal Community Name to Match

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

### System.String[]

## OUTPUTS

### PSAsigraDSClient.BaseDSClientSNMPConfig+DSClientSNMPCommunities

## NOTES

## RELATED LINKS
