---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientRetentionRule

## SYNOPSIS
Returns Retention Rule Details

## SYNTAX

```
Get-DSClientRetentionRule [[-RetentionRuleId] <Int32>] [[-Name] <String>] [<CommonParameters>]
```

## DESCRIPTION
Returns details of configured retention rules

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientRetentionRule
```


## PARAMETERS

### -Name
The Name of the Retention Rule

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -RetentionRuleId
The Retention Rule Id

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

### System.String

## OUTPUTS

### PSAsigraDSClient.BaseDSClientRetentionRule+DSClientRetentionRule

## NOTES

## RELATED LINKS
