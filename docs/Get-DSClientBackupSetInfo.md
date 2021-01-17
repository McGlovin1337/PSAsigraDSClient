---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientBackupSetInfo

## SYNOPSIS
Returns an overview of configured Backup Sets

## SYNTAX

### General (Default)
```
Get-DSClientBackupSetInfo [[-Computer] <String>] [[-DataType] <String>] [-Enabled] [-Synchronized]
 [[-ScheduleId] <Int32>] [[-RetentionRuleId] <Int32>] [[-SetType] <String>] [-UseLocalStorage] [-IsCDP]
 [-CreatedByPolicy] [<CommonParameters>]
```

### Id
```
Get-DSClientBackupSetInfo [-BackupSetId] <Int32> [<CommonParameters>]
```

### Name
```
Get-DSClientBackupSetInfo [-Name] <String> [<CommonParameters>]
```

## DESCRIPTION
Returns an overview of configured Backup Sets

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientBackupSetInfo
```

## PARAMETERS

### -BackupSetId
Id of the Backup Set

```yaml
Type: Int32
Parameter Sets: Id
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Computer
Backup Sets configured for specified Computer

```yaml
Type: String
Parameter Sets: General
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -CreatedByPolicy
List Backup Sets created by policy

```yaml
Type: SwitchParameter
Parameter Sets: General
Aliases:

Required: False
Position: 9
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DataType
List Backup Sets of a specific Data Type

```yaml
Type: String
Parameter Sets: General
Aliases:
Accepted values: FileSystem, MSSQLServer, MSExchange, Oracle, PermissionsOnly, MSExchangeItemLevel, OutlookEmail, SystemI, MySQL, PostgreSQL, DB2, LotusNotesEmail, GroupWiseEmail, SharepointItemLevel, VMWareVMDK, XenServer, Sybase, HyperVServer, VMWareVADP, MSSQLServerVSS, MSExchangeVSS, SharepointVSS, SalesForce, GoogleApps, Office365, OracleSBT, LotusDomino, HyperVCluster, P2V, MSExchangeEWS

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Enabled
List Active/Inactive Backup Sets

```yaml
Type: SwitchParameter
Parameter Sets: General
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -IsCDP
List CDP Backup Sets

```yaml
Type: SwitchParameter
Parameter Sets: General
Aliases:

Required: False
Position: 8
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Backup Sets with the given Name

```yaml
Type: String
Parameter Sets: Name
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RetentionRuleId
List Backup Sets using the specified RetentionId

```yaml
Type: Int32
Parameter Sets: General
Aliases:

Required: False
Position: 5
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -ScheduleId
List Backup Sets using the specified ScheduleId

```yaml
Type: Int32
Parameter Sets: General
Aliases:

Required: False
Position: 4
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -SetType
List Backup Sets of a specific type

```yaml
Type: String
Parameter Sets: General
Aliases:
Accepted values: OffSite, Statistical, SelfContained, LocalOnly

Required: False
Position: 6
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Synchronized
List Synchronized/Unsynchronized Backup Sets

```yaml
Type: SwitchParameter
Parameter Sets: General
Aliases:

Required: False
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UseLocalStorage
List Backup Sets using Local Storage

```yaml
Type: SwitchParameter
Parameter Sets: General
Aliases:

Required: False
Position: 7
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

### System.String

## OUTPUTS

### PSAsigraDSClient.BaseDSClientBackupSetInfo+DSClientBackupSetInfo

## NOTES

## RELATED LINKS
