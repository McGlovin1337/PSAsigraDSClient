---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientAuditTrail

## SYNOPSIS
Return an Audit Trail of DS-Client Database Operations

## SYNTAX

```
Get-DSClientAuditTrail [-From <DateTime>] [-To <DateTime>] [-Operation <String>] [-Table <String>]
 [-User <String>] [<CommonParameters>]
```

## DESCRIPTION
Returns the Audit Trail of DS-Client Database Operations

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientAuditTrail -From "01/01/2020"
```

Return all Operations since 01/01/2020

### Example 2
```powershell
PS C:\> Get-DSClientAuditTrail -Operation 'Delete'
```

Return all Delete Operations across all Tables

## PARAMETERS

### -From
From Date and Time

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

### -Operation
Filter by Operation

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: All, Insert, Delete, Update, Action

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Table
Filter by Table

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: All, Setup, BackupSchedule, Permission, BackupSet, BackupItems, UserId, Notification, PrePost, ScheduleDetail, GroupId, Roles, Retention, RetentionRule, Config, SetOption, SetAdditionalOpt, NasFilter, NasVolume, NasSchedule, NasRetention, NasVault

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -To
To Date and Time

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

### -User
Filter by User

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### PSAsigraDSClient.GetDSClientAuditTrail+DSClientAuditTrail

## NOTES

## RELATED LINKS
