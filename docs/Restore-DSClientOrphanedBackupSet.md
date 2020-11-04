---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Restore-DSClientOrphanedBackupSet

## SYNOPSIS
Recovers Orphanded Backup Sets

## SYNTAX

```
Restore-DSClientOrphanedBackupSet [[-Name] <String>] [[-Computer] <String>] [[-Owner] <String>]
 [<CommonParameters>]
```

## DESCRIPTION
Recovers Orphaned Backup Sets to the DS-Client

## EXAMPLES

### Example 1
```powershell
PS C:\> Restore-DSClientOrphanedBackupSet
```

Restores all Orphaned Backup Sets

## PARAMETERS

### -Computer
Select Orphaned Backup Set by Computer to recover

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Name
Select Orphaned Backup Set by Name to recover

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -Owner
Select Orphaned Backup Set by Owner to recover

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

## OUTPUTS

### PSAsigraDSClient.RestoreDSClientOrphanedBackupSet+DSClientBackupSetId

## NOTES

## RELATED LINKS
