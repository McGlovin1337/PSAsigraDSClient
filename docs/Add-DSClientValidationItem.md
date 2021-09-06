---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Add-DSClientValidationItem

## SYNOPSIS
Add a Backup Set Item to a Validation Session

## SYNTAX

```
Add-DSClientValidationItem -ValidationId <Int32> [-ItemId <Int64[]>] [-Item <String[]>] [<CommonParameters>]
```

## DESCRIPTION
Select a Stored Backup Set Item for Validation by adding it to the specified Validation Session

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-DSClientValidationItem -ValidationId 2 -ItemId 5, 15
```

Adds the Items with Id 5 and 15 to the Validation Session with Id 2

## PARAMETERS

### -Item
Specify Items for Restore by Name

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ItemId
Specify Items for Restore by ItemId

```yaml
Type: Int64[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValidationId
Specify the Validation Session to Select Items for

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

## OUTPUTS

### System.Void

## NOTES

## RELATED LINKS
