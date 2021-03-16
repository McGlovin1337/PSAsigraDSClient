---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Start-DSClientBackup

## SYNOPSIS
Starts an On-Demand Backup

## SYNTAX

### Id
```
Start-DSClientBackup [-PerformRetention] [-PreScan] [-ErrorLimit <Int32>] [-UseDetailedLog]
 [-StartTime <DateTime>] [-BackupSetId] <Int32> [-PassThru] [<CommonParameters>]
```

### Name
```
Start-DSClientBackup [-PerformRetention] [-PreScan] [-ErrorLimit <Int32>] [-UseDetailedLog]
 [-StartTime <DateTime>] [-Name] <String[]> [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
Starts a Backup On-Demand

## EXAMPLES

### Example 1
```powershell
PS C:\> Start-DSClientBackup -BackupSetId 10 -PerformRetention
```

Starts a Backup for the Backup Set with Id 10 and Performs Retention afterwards

## PARAMETERS

### -BackupSetId
Specify the Backup Set Id

```yaml
Type: Int32
Parameter Sets: Id
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -ErrorLimit
Stop Backup Error Limit

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

### -Name
Specify Backup Set(s) by Name

```yaml
Type: String[]
Parameter Sets: Name
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -PerformRetention
Specify to Run Retention After Backup

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

### -PreScan
Specify to Prescan Backup Source

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

### -StartTime
Specify Start Time

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

### -PassThru
Specify to output basic Activity Info

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

### System.DateTime

### System.String[]

## OUTPUTS

### PSAsigraDSClient.BaseDSClientStartBackupSetActivity+DSClientStartBackupSetActivity

## NOTES

## RELATED LINKS
