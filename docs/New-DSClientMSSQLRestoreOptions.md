---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# New-DSClientMSSQLRestoreOptions

## SYNOPSIS
Creates a new set of MSSQL Restore Options

## SYNTAX

```
New-DSClientMSSQLRestoreOptions [-DumpMethod <String>] [-DumpPath <String>] [-RestoreDumpOnly]
 [-LeaveRestoringMode] [-PreserveOriginalLocation] [-UseDetailedLog] [-LocalStorageMethod <String>]
 [-DSSystemReadThreads <Int32>] [-MaxPendingAsyncIO <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Use this Cmdlet to Create a New Set of MSSQL Restore Options

## EXAMPLES

### Example 1
```powershell
PS C:\> New-DSClientMSSQLRestoreOptions
```

Creates a Default Set of Restore Options

## PARAMETERS

### -DSSystemReadThreads
Set the number of DS-System Read Threads

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

### -DumpMethod
Specify the Dump Method

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: DumpToPipe, DumpToClientBuffer, DumpToSQLPath

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DumpPath
If using DumpToSQLPath, specify the path

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

### -LeaveRestoringMode
Leave the Restored Database in Restoring Mode once restore is complete

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

### -LocalStorageMethod
Specify Local Storage Handling

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: None, ConnectFirst, ConnectIfNeeded, ContinueIfDisconnect

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MaxPendingAsyncIO
Set the maximum pending Asynchronious IO

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

### -PreserveOriginalLocation
Preserve the Original Databases File Locations

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

### -RestoreDumpOnly
Specify to ONLY Restore the Dump File

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

### -UseDetailedLog
Specify to use Detailed Log

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

## OUTPUTS

### PSAsigraDSClient.RestoreOptions_MSSQLServer

## NOTES

## RELATED LINKS
