---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Remove-DSClientInitialBackupPath

## SYNOPSIS
Remove an Initial Backup Path from the DS-Client

## SYNTAX

### id
```
Remove-DSClientInitialBackupPath [-PathId] <Int32> [-WhatIf] [-Confirm] [<CommonParameters>]
```

### path
```
Remove-DSClientInitialBackupPath [-Path] <String> [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Use this Cmdlet to remove a specific path used for Initial Backup

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-DSClientInitialBackupPath -PathId 2
```

Removes the Initial Backup Path by PathId

### Example 2
```powershell
PS C:\> Remove-DSClientInitialBackupPath -Path '\\share\backup'
```

Removes the Initial Backup Path '\\share\backup'

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

### -Path
Specify the Path to Remove

```yaml
Type: String
Parameter Sets: path
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -PathId
Specify the PathId to Remove

```yaml
Type: Int32
Parameter Sets: id
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

### System.String

## OUTPUTS

### System.Void

## NOTES

## RELATED LINKS
