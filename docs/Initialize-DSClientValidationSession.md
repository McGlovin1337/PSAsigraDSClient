---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Initialize-DSClientValidationSession

## SYNOPSIS
Create a new Backup Set Validation Session

## SYNTAX

```
Initialize-DSClientValidationSession [-BackupSetId] <Int32> [<CommonParameters>]
```

## DESCRIPTION
Create a new Backup Set Validation Session

## EXAMPLES

### Example 1
```powershell
PS C:\> Initialize-DSClientValidationSession -BackupSetId 3
```

Creates a new Validation Session for the Backup Set with Id 3

## PARAMETERS

### -BackupSetId
Specify the Backup Set to Initialize a Validation Session for

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

### PSAsigraDSClient.DSClientValidationSession

## NOTES

## RELATED LINKS
