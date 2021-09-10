---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientDeleteSession

## SYNOPSIS
Return List of Initialized Delete Sessions

## SYNTAX

```
Get-DSClientDeleteSession [[-DeleteId] <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Return List of Initialized Delete Sessions

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientDeleteSession
```

Returns all Delete Sessions

## PARAMETERS

### -DeleteId
Specify Delete Session Id to return

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

### PSAsigraDSClient.DSClientDeleteSession

## NOTES

## RELATED LINKS
