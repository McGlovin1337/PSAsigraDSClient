---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientSupportedDataType

## SYNOPSIS
Lists the Supported Backup Source Data Types

## SYNTAX

```
Get-DSClientSupportedDataType [-ApiDataTypes] [<CommonParameters>]
```

## DESCRIPTION
Lists the DS-Clients Supported Backup Source Data Types, or the Data Types supported through the DS-Client API

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientSupportedDataType
```

Lists the DS-Clients Supported Backup Source Data Types

## PARAMETERS

### -ApiDataTypes
Specify to Get the Data Types Supported by the DS-Client API

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### PSAsigraDSClient.GetDSClientSupportedDataType+DSClientDataType

## NOTES

## RELATED LINKS
