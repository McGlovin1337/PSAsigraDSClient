---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Search-DSClientBackupSetData

## SYNOPSIS
Perform a Search of DS-Client Backed Up Data

## SYNTAX

### DeleteId
```
Search-DSClientBackupSetData [-Filter] <String[]> [[-DirectoryFilter] <String[]>] [-LatestGenerationOnly]
 -DeleteId <Int32> [-DateFrom <DateTime>] [-DateTo <DateTime>] [-DeletedDate <DateTime>] [<CommonParameters>]
```

### RestoreId
```
Search-DSClientBackupSetData [-Filter] <String[]> [[-DirectoryFilter] <String[]>] [-LatestGenerationOnly]
 -RestoreId <Int32> [-DateFrom <DateTime>] [-DateTo <DateTime>] [-DeletedDate <DateTime>] [<CommonParameters>]
```

### ValidationId
```
Search-DSClientBackupSetData [-Filter] <String[]> [[-DirectoryFilter] <String[]>] [-LatestGenerationOnly]
 -ValidationId <Int32> [-DateFrom <DateTime>] [-DateTo <DateTime>] [-DeletedDate <DateTime>]
 [<CommonParameters>]
```

### BackupSetId
```
Search-DSClientBackupSetData [-Filter] <String[]> [[-DirectoryFilter] <String[]>] [-LatestGenerationOnly]
 [-BackupSetId] <Int32> [-DateFrom <DateTime>] [-DateTo <DateTime>] [-DeletedDate <DateTime>]
 [<CommonParameters>]
```

## DESCRIPTION
Perform a Search of DS-Client Backed Up Data. This uses the information contained in the DS-Client database about backed up items.
NOTE: This Cmdlet will throw an exception if there are too many matching items.

## EXAMPLES

### Example 1
```powershell
PS C:\> Search-DSClientBackupSetData -Filter "*.doc"
```

Returns all the items matching "*.doc"

## PARAMETERS

### -BackupSetId
Specify Backup Set to Search

```yaml
Type: Int32
Parameter Sets: BackupSetId
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -DateFrom
Specify Date to Search From

```yaml
Type: DateTime
Parameter Sets: (All)
Aliases: StartTime

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DateTo
Specify Date to Search To

```yaml
Type: DateTime
Parameter Sets: (All)
Aliases: EndTime

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DeletedDate
Specify Date for Deleted Files from Source

```yaml
Type: DateTime
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DirectoryFilter
Specify Directory Search Filters

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Filter
Specify the File Search Filters

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -LatestGenerationOnly
Specify to display the most recent Generation only

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

### -RestoreId
Specify an existing Restore Session Id

```yaml
Type: Int32
Parameter Sets: RestoreId
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValidationId
Specify and existing Validation Session Id

```yaml
Type: Int32
Parameter Sets: ValidationId
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DeleteId
Specify an existing Delete Session Id

```yaml
Type: Int32
Parameter Sets: DeleteId
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String[]

### System.Int32

### System.DateTime

## OUTPUTS

### PSAsigraDSClient.SearchDSClientBackupSetData+DSClientBSFileInfo

## NOTES

## RELATED LINKS
