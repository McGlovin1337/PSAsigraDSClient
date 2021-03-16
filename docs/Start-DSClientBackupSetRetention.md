---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Start-DSClientBackupSetRetention

## SYNOPSIS
Perform Backup Set Retention

## SYNTAX

### Id
```
Start-DSClientBackupSetRetention [-BackupSetId] <Int32> [-PassThru] [<CommonParameters>]
```

### Name
```
Start-DSClientBackupSetRetention [-Name] <String[]> [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
Perform Backup Set Retention

## EXAMPLES

### Example 1
```powershell
PS C:\> Start-DSClientBackupSetRetention -Name "*"
```

Performs Retention for all Backup Sets

## PARAMETERS

### -BackupSetId
Specify the Backup Set Id

```yaml
Type: Int32
Parameter Sets: Id
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -Name
Specify Backup Set(s) by Name

```yaml
Type: String[]
Parameter Sets: Name
Aliases:

Required: True
Position: 0
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

### System.String[]

## OUTPUTS

### PSAsigraDSClient.BaseDSClientStartBackupSetActivity+DSClientStartBackupSetActivity

## NOTES

## RELATED LINKS
