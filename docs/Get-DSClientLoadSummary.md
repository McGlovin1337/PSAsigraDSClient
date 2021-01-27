---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientLoadSummary

## SYNOPSIS
Provides System Utilisation Information

## SYNTAX

```
Get-DSClientLoadSummary [[-NodeId] <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Provides DS-Client System Utilisation Information such as CPU, Memory and Network

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientLoadSummary
```

## PARAMETERS

### -NodeId
DS-Client Node Id

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

### PSAsigraDSClient.GetDSClientLoadSummary+DSClientLoadSummary

## NOTES

## RELATED LINKS
