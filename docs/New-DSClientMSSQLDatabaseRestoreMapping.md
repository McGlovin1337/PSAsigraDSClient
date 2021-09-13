---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# New-DSClientMSSQLDatabaseRestoreMapping

## SYNOPSIS
Create a new Database Mapping for MSSQL Database Restore Operations

## SYNTAX

```
New-DSClientMSSQLDatabaseRestoreMapping [-SourceDatabase] <String> [-DestinationDatabase] <String>
 [<CommonParameters>]
```

## DESCRIPTION
Use this Cmdlet to Map a Source MS SQL Database to a Destination MS SQL Database. Note, that the Source Database must be selected for restore and the Destination Database must exist on the Destination SQL Instance.

## EXAMPLES

### Example 1
```powershell
PS C:\> $dbmap = New-DSClientMSSQLDatabaseRestoreMapping -SourceDatabase 'ProdDatabase' -DestinationDatabase 'DevDatabase'
PS C:\> Set-DSClientRestoreSession -RestoreId 1 -DestinationId 1 -DatabaseMapping $dbmap
```

Maps the Source Database named 'ProdDatabase' to the Destination Database named 'DevDatabase' in the selected Restore Session

## PARAMETERS

### -DestinationDatabase
Specify the Destination Database to Map Source Databse to

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -SourceDatabase
Specify the Source Database

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

## OUTPUTS

### PSAsigraDSClient.MSSQLDatabaseMap

## NOTES

## RELATED LINKS
