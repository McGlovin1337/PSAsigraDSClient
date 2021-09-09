---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Start-DSClientDelete

## SYNOPSIS
Initiates the Deletion of Items for the given Delete Session

## SYNTAX

```
Start-DSClientDelete -DeleteId <Int32> [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
Initiates the Deletion of Items for the given Delete Session

## EXAMPLES

### Example 1
```powershell
PS C:\> Start-DSClientDelete -DeleteId 1
```

Initiates the Deletion of Items for Delete Session with Id 1

## PARAMETERS

### -DeleteId
Specify the Delete Session Id

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

## OUTPUTS

### System.Void

### PSAsigraDSClient.DSClientCommon+GenericBackupSetActivity

## NOTES

## RELATED LINKS
