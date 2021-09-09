---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Add-DSClientDeleteItem

## SYNOPSIS
Add an Item from a Backup Set to a Delete Session

## SYNTAX

```
Add-DSClientDeleteItem -DeleteId <Int32> [-ItemId <Int64[]>] [-Item <String[]>] [<CommonParameters>]
```

## DESCRIPTION
Add items from a Backup Set to an initialized Delete Session, so that it can be deleted from the stored backup set data.

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-DSClientDeleteItem -DeleteId 1 -ItemId 10
```

Adds the Item with ItemId 10 to Delete Session with Id 1

### Example 2
```powershell
PS C:\> Add-DSClientDeleteItem -DeleteId 1 -Item 'C$\Windows\Temp'
```

Adds the Item 'C$\Windows\Temp' by name to the Delete Session with Id 1

## PARAMETERS

### -DeleteId
Specify the Delete Session to Select Items for

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

### -Item
Specify Items for Deletion by Name

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
Specify Items for Deletion by ItemId

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

## OUTPUTS

### System.Void

## NOTES

## RELATED LINKS
