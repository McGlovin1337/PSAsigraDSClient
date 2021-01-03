---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Watch-DSClientActivity

## SYNOPSIS
Watch the Status of a Running DS-Client Activity

## SYNTAX

```
Watch-DSClientActivity [-ActivityId] <Int32> [-Refresh <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Watches the Status of a Running DS-Client Activity by providing a Progress Indicator

## EXAMPLES

### Example 1
```powershell
PS C:\> Watch-DSClientActivity -ActivityId 1234
```

Starts Watching the DS-Client Activity with Id 1234

## PARAMETERS

### -ActivityId
Specify the ActivityId to Watch

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

### -Refresh
Specify the Refresh Interval in Seconds of the Activity

```yaml
Type: Int32
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

### System.Int32

## OUTPUTS

### PSAsigraDSClient.BaseDSClientActivityLog+DSClientAcivityLog

## NOTES

## RELATED LINKS
