---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Add-DSClientRestoreItem

## SYNOPSIS
Adds an Item to a Restore Session for Restore

## SYNTAX

```
Add-DSClientRestoreItem -RestoreId <Int32> [-ItemId <Int64[]>] [-Item <String[]>] [<CommonParameters>]
```

## DESCRIPTION
This Cmdlet is used to select an item for Restore by Adding it to the specified Restore Session

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-DSClientRestoreItem -RestoreId 1 -ItemId 150
```

Adds the Item with ItemId 150 to the Restore Session with RestoreId 1
(Use Get-DSClientStoreItem -RestoreId to retrieve ItemId's')

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

### -RestoreId
Specify the Restore Session to Select Items for

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

### System.Object
## NOTES

## RELATED LINKS
