---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Start-DSClientBackupSetValidation

## SYNOPSIS
Start a Backup Set Data Validation Activity

## SYNTAX

### Full
```
Start-DSClientBackupSetValidation [-FullValidation] [<CommonParameters>]
```

### Selective
```
Start-DSClientBackupSetValidation [[-Items] <String[]>] [[-ItemId] <Int64[]>] [<CommonParameters>]
```

## DESCRIPTION
Start a Backup Set Data Validation Activity. The Initialize-DSClientBackupSetValidation Cmdlet must be used before using this Cmdlet

## EXAMPLES

### Example 1
```powershell
PS C:\> Start-DSClientBackupSetValidation -FullValidation
```

Performs Validation of all Backed Up Data

## PARAMETERS

### -FullValidation
Specify to Validate All Backup Set Data

```yaml
Type: SwitchParameter
Parameter Sets: Full
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ItemId
Specify the items to validate by ItemId

```yaml
Type: Int64[]
Parameter Sets: Selective
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Items
Specify the items to validate

```yaml
Type: String[]
Parameter Sets: Selective
Aliases: Path

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String[]

### System.Int64[]

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
