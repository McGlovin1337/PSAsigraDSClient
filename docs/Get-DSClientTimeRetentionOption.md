---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientTimeRetentionOption

## SYNOPSIS
Return the Time Retention Options assigned to a given Retention Rule

## SYNTAX

```
Get-DSClientTimeRetentionOption [-RetentionRuleId] <Int32> [<CommonParameters>]
```

## DESCRIPTION
Returns the Time Retention Options assigned to a given Retention Rule.
The DS-Client API does not provide an identifier for Time Retention Options, thus an TimeRetentionId is generated for each Retention Option and stored in Session State.
This allows for reliable removal of Time Retention Options when using the Remove-DSClientTimeRetentionOption Cmdlet.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientTimeRetentionOption -RetentionRuleId 65
```

Returns an overview of the Time Retention Options applied to Retention Rule with Id 65.

## PARAMETERS

### -RetentionRuleId
Specify Retention Rule

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

### PSAsigraDSClient.BaseDSClientRetentionRule+TimeRetentionOverview

## NOTES

## RELATED LINKS
