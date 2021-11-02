---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Set-DSClientVMwareVADPBackupSet

## SYNOPSIS
Update the Configuration of an existing VMware VADP Backup Set

## SYNTAX

```
Set-DSClientVMwareVADPBackupSet [-BackupSetId] <Int32> [[-Name] <String>] [-Compression <String>]
 [-Credential <PSCredential>] [-IncrementalP2VBackup] [-BackupVMMemory] [-SnapshotQuiesce] [-SameTimeSnapshot]
 [-UseCBT] [-UseFLR] [-UseLocalVDR] [-VMLibraryVersion <String>] [-UseBuffer] [-Disabled] [-ScheduleId <Int32>]
 [-RetentionRuleId <Int32>] [-SchedulePriority <Int32>] [-ForceBackup] [-PreScan] [-ReadBufferSize <Int32>]
 [-BackupErrorLimit <Int32>] [-UseDetailedLog] [-InfinateBLMGenerations] [-UseLocalStorage]
 [-LocalStoragePath <String>] [-UseTransmissionCache] [-SnmpTrapNotifications <String[]>] [-PassThru] [-WhatIf]
 [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
This Cmdlet can be used to update the Configuration of an existing VMware VADP Backup Set.

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DSClientVMwareVADPBackupSet -BackupSetId 10 -BackupVMMemory
```

Configures the VMware VADP Backup Set with Id 10 to Include the Virtual Machine Memory in the Backup.

## PARAMETERS

### -BackupErrorLimit
Set the Backup Error limit

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

### -BackupSetId
Specify the Backup Set to Modify

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

### -BackupVMMemory
Backup VM Memory

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

### -Compression
Set the Compression Method to use

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: None, ZLIB, LZOP, ZLIB_LO, ZLIB_MED, ZLIB_HI

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Confirm
Prompts you for confirmation before running the cmdlet.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: cf

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Credential
Specify Computer Credentials

```yaml
Type: PSCredential
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Disabled
Specify this Backup Set should be set to Disabled

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

### -ForceBackup
Force Re-Backup of File even if it hasn't been modified

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

### -IncrementalP2VBackup
Backup P2V VMs Incrementally

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

### -InfinateBLMGenerations
Set to use Infinate BLM Generations

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

### -LocalStoragePath
Local Storage Path For Local Backups and Cache

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Name
Rename the Backup Set

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

### -PassThru
Output Basic Backup Set Properties

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
Set to PreScan before Backup

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

### -ReadBufferSize
Set the Read Buffer Size

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

### -RetentionRuleId
Set the Retention Rule this Backup Set will use

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

### -SameTimeSnapshot
Snapshot all VMs at same time

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

### -ScheduleId
Set the Schedule this Backup Set will use

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

### -SchedulePriority
Schedule Priority of Backup Set when assigned to Schedule

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

### -SnapshotQuiesce
Quiesce IO before Snapshot

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

### -SnmpTrapNotifications
Specify Completion Status to send SNMP Traps

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:
Accepted values: Incomplete, CompletedWithErrors, Successful, CompletedWithWarnings

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -UseBuffer
Specify to use DS-Client Buffer

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

### -UseCBT
Use Change Block Tracking (CBT)

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
Set to use Detailed Log

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

### -UseFLR
Use File Level Restore (FLR)

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

### -UseLocalStorage
Set to use Local Storage

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

### -UseLocalVDR
Use Local Virtual Disaster Recovery (VDR)

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

### -UseTransmissionCache
Set to use Local Transmission Cache for Offsite Backup Sets

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

### -VMLibraryVersion
Specify VM Library Version

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Latest, VDDK6_0, VDDK5_5

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WhatIf
Shows what would happen if the cmdlet runs.
The cmdlet is not run.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: wi

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

### System.String

### System.String[]

## OUTPUTS

### System.Void

## NOTES

## RELATED LINKS
