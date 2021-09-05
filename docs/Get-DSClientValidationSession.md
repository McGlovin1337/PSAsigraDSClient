---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientValidationSession

## SYNOPSIS
Retrieve list of Validation Sessions

## SYNTAX

```
Get-DSClientValidationSession [[-ValidationId] <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Retrieve list of Validation Sessions

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientValidationSession
```

Return all initialized Validation Sessions

## PARAMETERS

### -ValidationId
Specify the Validation Session to return

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

## OUTPUTS

### PSAsigraDSClient.DSClientValidationSession

## NOTES

## RELATED LINKS
