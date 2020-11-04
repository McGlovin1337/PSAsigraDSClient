---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Set-DSClientAdvancedConfig

## SYNOPSIS
Changes Advanced Configuration Options

## SYNTAX

```
Set-DSClientAdvancedConfig [-Name] <String> [-Value] <String> [<CommonParameters>]
```

## DESCRIPTION
Changes Advanced Configuration Options. Use Get-DSClientAdvancedConfig to get a list of configurable Advanced Configuration options

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DSClientAdvancedConfig -Name "AllowAutoUpgrade" -Value "0"
```

Disables the "AllowAutoUpgrade" Advanced Configuration option

## PARAMETERS

### -Name
Specify the Advanced Config Item to modify

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -Value
Specify the Advanced Config Item Value

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
