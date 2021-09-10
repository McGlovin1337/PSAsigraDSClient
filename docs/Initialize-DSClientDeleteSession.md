---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Initialize-DSClientDeleteSession

## SYNOPSIS
Create a new Backup Set Delete Session

## SYNTAX

```
Initialize-DSClientDeleteSession [-BackupSetId] <Int32> [<CommonParameters>]
```

## DESCRIPTION
Creates a new Backup Set Delete Session, which allows for items to be selected for deletion from the specified Backup Set

## EXAMPLES

### Example 1
```powershell
PS C:\> Initialize-DSClientDeleteSession -BackupSetId 5
```

Creates a new Backup Set Delete Session for the Backup Set with Id 5

## PARAMETERS

### -BackupSetId
Specify the Backup Set to Initialize a Delete Session for

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

### PSAsigraDSClient.DSClientDeleteSession

## NOTES

## RELATED LINKS
