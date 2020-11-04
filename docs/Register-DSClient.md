---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Register-DSClient

## SYNOPSIS
Registers the DS-Client with a DS-System

## SYNTAX

### Initial
```
Register-DSClient [-Initial] [<CommonParameters>]
```

### ReRegister
```
Register-DSClient [-ReRegister] [<CommonParameters>]
```

## DESCRIPTION
Registers the DS-Client with a DS-System using the configured Registration Info. Use Set-DSClientRegistrationInfo to configure.

## EXAMPLES

### Example 1
```powershell
PS C:\> Register-DSClient -Initial
```

Performs and Initial Registration of the DS-Client with DS-System

## PARAMETERS

### -Initial
Perform Initial DS-Client Registration

```yaml
Type: SwitchParameter
Parameter Sets: Initial
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ReRegister
Re-register DS-Client following Hardware Change

```yaml
Type: SwitchParameter
Parameter Sets: ReRegister
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
