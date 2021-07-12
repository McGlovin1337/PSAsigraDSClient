---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Start-DSClientRestore

## SYNOPSIS
Initiates the Restore for the specified Restore Session

## SYNTAX

```
Start-DSClientRestore -RestoreId <Int32> [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
Initiates the Restore for the specified Restore Session that is Ready

## EXAMPLES

### Example 1
```powershell
PS C:\> Start-DSClientRestore -RestoreId 1
```

Initiates the restore for Restore Session with RestoreId 1

## PARAMETERS

### -PassThru
Specify to output basic Activity Info

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RestoreId
Specify the Restore Session Id

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
