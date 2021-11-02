---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Export-DSClientConfig

## SYNOPSIS
Export the DS-Client Config to File

## SYNTAX

```
Export-DSClientConfig [-Path] <String> [-IncludeEncryptionKeys] [-UseMetaSymbol] [-UseUTF16] [-IncludeConfig]
 [-IncludeAllBackupSets] [-BackupSetId <Int32[]>] [-IncludeAllSchedules] [-ScheduleId <Int32[]>]
 [-IncludeAllRetentionRules] [-RetentionRuleId <Int32[]>] [<CommonParameters>]
```

## DESCRIPTION
Exports the DS-Client Configuration to the specified file in XML Format

## EXAMPLES

### Example 1
```powershell
PS C:\> Export-DSClientConfig -Path 'DSClientConfig.xml' -IncludeEncryptionKeys
```

Exports ALL DS-Client Configuration and Encryption Keys to the file DSClientConfig.xml

## PARAMETERS

### -BackupSetId
Include specific Backup Sets

```yaml
Type: Int32[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -IncludeAllBackupSets
Include All Backup Sets

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

### -IncludeAllRetentionRules
Include All Retention Rules

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

### -IncludeAllSchedules
Include All Schedules

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

### -IncludeConfig
Include DS-Client Configuration in Export

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

### -IncludeEncryptionKeys
Export DS-Client and Account Encryption Keys

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

### -Path
Specify where to output the configuration

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

### -RetentionRuleId
Include specific Retention Rules

```yaml
Type: Int32[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -ScheduleId
Include specific Schedules

```yaml
Type: Int32[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -UseMetaSymbol
Use a Meta Symbol to allow XML to be used in different localisations (Windows Only)

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

### -UseUTF16
Specify Output to use UTF16 Format (Default is UTF8)

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

### System.String

### System.Int32[]

## OUTPUTS

### System.IO.File

## NOTES

## RELATED LINKS
