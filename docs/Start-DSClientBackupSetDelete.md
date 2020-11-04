---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Start-DSClientBackupSetDelete

## SYNOPSIS
Start a Backup Set Delete Activity

## SYNTAX

### BLM
```
Start-DSClientBackupSetDelete [-MoveToBLM] -BLMLabel <String> [-NewPackage] [<CommonParameters>]
```

### Selective
```
Start-DSClientBackupSetDelete [-MoveToBLM] [-BLMLabel <String>] [-NewPackage] [[-Items] <String[]>]
 [[-ItemId] <Int64[]>] [<CommonParameters>]
```

## DESCRIPTION
Start a Backup Set Delete Activity. The Initialize-DSClientBackupSetDelete Cmdlet must be used before using this Cmdlet.

## EXAMPLES

### Example 1
```powershell
PS C:\> Start-DSClientBackupSetDelete
```


## PARAMETERS

### -BLMLabel
Specify BLM Label

```yaml
Type: String
Parameter Sets: BLM
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

```yaml
Type: String
Parameter Sets: Selective
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ItemId
Specify the items to validate by ItemId

```yaml
Type: Int64[]
Parameter Sets: Selective
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Items
Specify the items to validate

```yaml
Type: String[]
Parameter Sets: Selective
Aliases: Path

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -MoveToBLM
Specify to Move Data to BLM rather than Delete

```yaml
Type: SwitchParameter
Parameter Sets: BLM
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

```yaml
Type: SwitchParameter
Parameter Sets: Selective
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -NewPackage
Specify to Create a new BLM Archive Package

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

### System.String[]

### System.Int64[]

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
