---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Set-DSClientDeleteSession

## SYNOPSIS
Modifies the specified Delete Session Options

## SYNTAX

```
Set-DSClientDeleteSession [-DeleteId] <Int32> [-DateFrom <DateTime>] [-DateEnd <DateTime>]
 [-KeepGenerations <Int32>] [-ArchiveOption <String>] [-MoveToBLM] [-BLMLabel <String>] [-NewBLMPackage]
 [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Modifies the specified Delete Session Options. Note, that applying these options will clear any previously selected items. Set the required options prior to adding items to the Delete Session.

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DSClientDeleteSession -DeleteId 1 -KeepGenerations 1 -ArchiveOption Include
```

Specifies that 1 Generation of the Selected Items should be kept, and to also include Archived Files for Deletion.

## PARAMETERS

### -ArchiveOption
Specify Option for Archive Items

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Only, Include, Exclude

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -BLMLabel
Specify BLM Label

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
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

### -DateEnd
Specify End Date for Data Selection

```yaml
Type: DateTime
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DateFrom
Specify Start Date for Data Selection

```yaml
Type: DateTime
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DeleteId
Specify the Delete Session to Modify

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

### -KeepGenerations
Specify the Number of Generations to Keep for selected items

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

### -MoveToBLM
Specify to Move Items to BLM Archiver

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

### -NewBLMPackage
Create a New BLM Package

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

### System.DateTime

### System.String

## OUTPUTS

### System.Void

## NOTES

## RELATED LINKS
