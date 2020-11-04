---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Set-DSClientDefaultConfiguration

## SYNOPSIS
Set DS-Client Default Configuration Options

## SYNTAX

```
Set-DSClientDefaultConfiguration [[-CompressionType] <String>] [[-DSClientBuffer] <String>]
 [[-LocalStoragePath] <String>] [[-NotificationMethod] <String>] [[-NotificationRecipient] <String>]
 [[-NotificationCompletion] <String[]>] [[-NotificationEmailOptions] <String[]>] [[-OnlineGenerations] <Int32>]
 [[-RetentionRuleId] <Int32>] [[-RetentionRule] <String>] [[-ScheduleId] <Int32>] [[-Schedule] <String>]
 [[-OpenFileOperation] <String>] [[-OpenFileRetryInterval] <Int32>] [[-OpenFileRetryTimes] <Int32>]
 [-BackupFilePermissions] [<CommonParameters>]
```

## DESCRIPTION
Set DS-Client Default Configuration Options

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DSClientDefaultConfiguration -CompressionType "ZLIB_HI" -ScheduleId 3 -RetentionRuleId 5
```

Sets the Default Compression Method to ZLIB High and Default Schedule to Schedule with Id of 3 and Retention Rule with Id of 5

## PARAMETERS

### -BackupFilePermissions
Value for backing up File Permissions (Windows Only)

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: 15
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CompressionType
Default Compression Level

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: None, ZLIB, LZOP, ZLIB_LO, ZLIB_MED, ZLIB_HI

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DSClientBuffer
DS-Client Buffer Path

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -LocalStoragePath
Local Storage Path

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -NotificationCompletion
Completion Status to Notify on

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:
Accepted values: Incomplete, CompletedWithErrors, Successful, CompletedWithWarnings

Required: False
Position: 5
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -NotificationEmailOptions
Email Notification Options

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:
Accepted values: DetailedInfo, AttachDetailedLog, CompressAttachment, HtmlFormat

Required: False
Position: 6
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -NotificationMethod
Notification Method

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Email, Pager, Broadcast, Event

Required: False
Position: 3
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -NotificationRecipient
Notification Recipient

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 4
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -OnlineGenerations
Number of Online Generations

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: 7
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -OpenFileOperation
Open File Operation/Method (Windows Only)

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: TryDenyWrite, DenyWrite, PreventWrite, AllowWrite

Required: False
Position: 12
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -OpenFileRetryInterval
Open File retry interval in seconds (Windows Only)

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: 13
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -OpenFileRetryTimes
Number of times to retry Open File Operation (Windows Only)

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: 14
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RetentionRule
Default Retention Rule by Name

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 9
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RetentionRuleId
Default Retention Rule by RetentionRuleId

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: 8
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Schedule
Default Schedule by Name

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 11
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -ScheduleId
Default Schedule by ScheduleId

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: 10
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

### System.String[]

### System.Int32

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
