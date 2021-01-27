---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Add-DSClientVMwareVADPBackupSetItem

## SYNOPSIS
Adds items to a VMware VADP Backup Set

## SYNTAX

```
Add-DSClientVMwareVADPBackupSetItem [[-BackupSetId] <Int32>] [-IncludeItem <String[]>]
 [-MaxGenerations <Int32>] [-ExcludeItem <String[]>] [<CommonParameters>]
```

## DESCRIPTION
Add a new Inclusion and/or Exclusion Item to an existing VMware VADP Backup Set

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-DSClientVMwareVADPBackupSetItem -BackupSetid 5 -IncludeItem 'CLOUD\MyNewVirtualMachine' -MaxGenerations 30
```

Include the Item 'CLOUD\MyNewVirtualMachine' with a Maximum Number of Generations of 30 to the Backup Set with Id 5

## PARAMETERS

### -BackupSetId
Specify the Backup Set to modify

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -ExcludeItem
Items to Exclude from Backup Set

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -IncludeItem
Items to Include in Backup Set

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -MaxGenerations
Max Number of Generations for Included Items

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

### System.String[]

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
