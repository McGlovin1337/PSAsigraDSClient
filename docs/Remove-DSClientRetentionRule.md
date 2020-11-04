---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Remove-DSClientRetentionRule

## SYNOPSIS
Remove a Retention Rule from DS-Client

## SYNTAX

```
Remove-DSClientRetentionRule [-RetentionRuleId] <Int32> [<CommonParameters>]
```

## DESCRIPTION
Remove a Retention Rule from DS-Client

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-DSClientRetentionRule -RetentionRuleId 4
```

Removes the Retention Rule with Id 4 from the DS-Client

## PARAMETERS

### -RetentionRuleId
Specify Retention Rule Id

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: True
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

### System.Object
## NOTES

## RELATED LINKS
