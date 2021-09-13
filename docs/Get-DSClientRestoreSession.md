---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientRestoreSession

## SYNOPSIS
Retrieve list of Restore Sessions

## SYNTAX

```
Get-DSClientRestoreSession [[-RestoreId] <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Retrieve a list of Restore Sessions created with Initialize-DSClientRestoreSession

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientRestoreSession
```

Returns all Restore Sessions

## PARAMETERS

### -RestoreId
Select a specific Restore Session by Id

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

## OUTPUTS

### PSAsigraDSClient.DSClientRestoreSession

### System.Void

## NOTES

## RELATED LINKS
