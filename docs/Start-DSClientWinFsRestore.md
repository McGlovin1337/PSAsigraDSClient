---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Start-DSClientWinFsRestore

## SYNOPSIS
Starts a Restore of a Windows File System Backup Set

## SYNTAX

```
Start-DSClientWinFsRestore [-MaxPendingAsyncIO <Int32>] [-ReadThreads <Int32>] [-AuthoritativeRestore]
 [-OverwriteJunctionPoint] [-SkipOfflineFiles] [[-ItemId] <Int64[]>] [-DestinationPath <String>]
 -OverwriteOption <String> [-RestoreMethod <String>] [-RestorePermissions <String>] [[-Items] <String[]>]
 -Computer <String> [-Credential <PSCredential>] [-TruncateSource <Int32>] -RestoreReason <String>
 [-RestoreClassification <String>] [-UseDetailedLog] [-LocalStorageMethod <String>] [-PassThru]
 [<CommonParameters>]
```

## DESCRIPTION
Starts a Restore of a Windows File System Backup Set, the Initialize-DSClientBackupSetRestore Cmdlet must be executed prior to this Cmdlet

## EXAMPLES

### Example 1
```powershell
PS C:\> Start-DSClientWinFsRestore -Computer "\\WindowsServer02" -Items "C$\Users" -DestinationPath "C$\Restore"
```

Performs a restore of "C$\Users" to "C$\Restore" on Computer "WindowsServer02"

## PARAMETERS

### -AuthoritativeRestore
Specify AD (System State) Authoritative Restore

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

### -Computer
Specify the Computer to Restore to

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Credential
Specify Credentials for Destination Computer

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

### -DestinationPath
Destination to restore items to

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

### -ItemId
Select Items by ItemId

```yaml
Type: Int64[]
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Items
Specify the items to validate

```yaml
Type: String[]
Parameter Sets: (All)
Aliases: Path

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -LocalStorageMethod
Specify Local Storage Handling

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: None, ConnectDsSysFirst, ConnectDsSysNeeded, ContinueWithoutDsSys

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MaxPendingAsyncIO
Specify Max Pending Asynchronous I/O per File

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OverwriteJunctionPoint
Specify if to Overwrite Junction Points

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

### -OverwriteOption
File Overwrite Option

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: RestoreAll, RestoreNewer, RestoreOlder, RestoreDifferent, SkipExisting

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ReadThreads
Specify the number of DS-System Read Threads

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RestoreClassification
Specify Restore Classification

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Production, Drill, ProductionDrill

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RestoreMethod
File Restore Method

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Save, Fast, UseBuffer

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RestorePermissions
Specify Restoration of Permissions

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Yes, Skip, Only

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RestoreReason
Specify the Restore Reason

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: AccidentalDeletion, MaliciousIntent, DeviceLostOrStolen, HardwareFault, SoftwareFault, DataStolen, DataCorruption, NaturalDisaster, PowerOutage, OtherDisaster, PreviousGeneration, DeviceDamaged

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SkipOfflineFiles
Specify if to Skip Offline Files (stubs)

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

### -TruncateSource
Specify how many levels to Truncate Source Path

```yaml
Type: Int32
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

### System.Int64[]

### System.String[]

### System.String

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
