---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Remove-DSClientRestoreItem

## SYNOPSIS
Remove an Item selected for Restore

## SYNTAX

```
Remove-DSClientRestoreItem [-RestoreId] <Int32> [-ItemId <Int64[]>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Removes and Item selected for Restore from the specified Restore Session

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-DSClientRestoreItem -RestoreId 1 -ItemId 80
```

Removes the Item with ItemId 80 from Restore Session with RestoreId 1

## PARAMETERS

### -Confirm
Prompts you for confirmation before running the cmdlet.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: cf

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ItemId
Specify Item(s) to remove by Id

```yaml
Type: Int64[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RestoreId
Specify the Restore Session to remove item from

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

### -WhatIf
Shows what would happen if the cmdlet runs.
The cmdlet is not run.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: wi

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

### System.Int64[]

## OUTPUTS

### System.Void

## NOTES

## RELATED LINKS
