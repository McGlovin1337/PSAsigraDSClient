---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Enable-DSClientBackupSet

## SYNOPSIS
Enables a Backup Set

## SYNTAX

```
Enable-DSClientBackupSet [-BackupSetId] <Int32> [<CommonParameters>]
```

## DESCRIPTION
Sets a Backup Set to Enabled State

## EXAMPLES

### Example 1
```powershell
PS C:\> Enable-DSClientBackupSet -BackupSetId 7
```

Sets the Backup Set with Id 7 to Enabled

## PARAMETERS

### -BackupSetId
Specify the Backup Set to Enable

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
