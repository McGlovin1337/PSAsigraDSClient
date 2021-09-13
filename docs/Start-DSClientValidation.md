---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Start-DSClientValidation

## SYNOPSIS
Initiate a Backup Set Validation

## SYNTAX

```
Start-DSClientValidation -ValidationId <Int32> [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
Initiate a Backup Set Validation for a previously created Validation Session identified by ValidationId

## EXAMPLES

### Example 1
```powershell
PS C:\> Start-DSClientValidation -ValidationId 1 -PassThru
```

Start the Validation process for the Validation Session with Id 1, and output the Activity Info

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

### -ValidationId
Specify the Validation Session Id

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

### PSAsigraDSClient.DSClientCommon+GenericBackupSetActivity

## NOTES

## RELATED LINKS
