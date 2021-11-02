---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Set-DSClientInitialBackupStatus

## SYNOPSIS
Update the Status of an Initial Backup

## SYNTAX

```
Set-DSClientInitialBackupStatus [-BackupSetId] <Int32> [-Completed] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Update the Status of an Initial Backup

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DSClientInitialBackupStatus -BackupSetId 2 -Completed
```

Sets the Initial Backup Status to Complete for Backup Set with Id 2

### Example 2
```powershell
PS C:\> Set-DSClientInitialBackupStatus -BackupSetId 2 -Completed:$false
```

Sets the Initial Backup Status to Incomplete for Backup Set with Id 2

## PARAMETERS

### -BackupSetId
Specify the Backup Set Id

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

### -Completed
Set the Status of the Initial Backup

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

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

### System.Management.Automation.SwitchParameter

## OUTPUTS

### System.Void

## NOTES

## RELATED LINKS
