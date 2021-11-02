---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Remove-DSClientBackupSetItem

## SYNOPSIS
Removes an Inclusion or Exclusion Item from a Backup Set

## SYNTAX

### wcFolder
```
Remove-DSClientBackupSetItem [-BackupSetId] <Int32> -Folder <String> [-Filter <String>] -Type <String>
 [-WhatIf] [-Confirm] [<CommonParameters>]
```

### literalFolder
```
Remove-DSClientBackupSetItem [-BackupSetId] <Int32> -LiteralFolder <String> [-Filter <String>] -Type <String>
 [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Removes an Inclusion or Exclusion Item from a Backup Set

## EXAMPLES

### Example 1 - Remove by Wildcard Folder
```powershell
PS C:\> Remove-DSClientBackupSetItem -BackupSetId 5 -Folder 'C$*' -Type Inclusion
```

Removes all Inclusion Items matching the Folder path 'C$*'

### Example 2 - Remove by Literal Folder
```powershell
PS C:\> Remove-DSClientBackupSetItem -BackupSetId 10 -LiteralFolder '[192.168.1.10]master' -Type Inclusion
```

Removes the Database Item '[192.168.1.10]master'

## PARAMETERS

### -BackupSetId
Specify the Backup Set to remove Items from

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
Shows what would happen if the cmdlet runs. The cmdlet is not run.

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

### -Filter
Specify the Filter of the Item

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Folder
Specify the Folder to remove

```yaml
Type: String
Parameter Sets: wcFolder
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -LiteralFolder
Specify the Literal Folder to remove

```yaml
Type: String
Parameter Sets: literalFolder
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Type
Specify to only remove Inclusion Item

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
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
