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

### Inclusion
```
Remove-DSClientBackupSetItem [-BackupSetId] <Int32> [-Item] <String> [-Inclusion] [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

### Exclusion
```
Remove-DSClientBackupSetItem [-BackupSetId] <Int32> [-Item] <String> [-Exclusion] [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

### RegexExclusion
```
Remove-DSClientBackupSetItem [-BackupSetId] <Int32> [-Item] <String> [-RegexExclusion] [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

## DESCRIPTION
Removes an Inclusion or Exclusion Item from a Backup Set

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-DSClientBackupSetItem -BackupSetId 5 -Item "C$\Users\*.*" -Inclusion
```

Removes the Inclusion Item "C$\Users\*.*" from Backup Set with Id 5

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

### -Exclusion
Specify to only remove Exclusion Item

```yaml
Type: SwitchParameter
Parameter Sets: Exclusion
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Inclusion
Specify to only remove Inclusion Item

```yaml
Type: SwitchParameter
Parameter Sets: Inclusion
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Item
Specify the Item to remove

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

### -RegexExclusion
Specify to only remove Regex Exclusion Item

```yaml
Type: SwitchParameter
Parameter Sets: RegexExclusion
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

### System.String

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
