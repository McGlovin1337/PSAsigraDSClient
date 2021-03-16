---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Start-DSClientMSSqlServerRestore

## SYNOPSIS
Start a Restore of an MS SQL Server Backup Set

## SYNTAX

### InstanceRestore
```
Start-DSClientMSSqlServerRestore [-Items] <String[]> [-DbCredential <PSCredential>] -Instance <String>
 [-MapDatabase <String[]>] [-DumpMethod <String>] [-DumpPath <String>] [-LeaveDatabaseRestoring]
 [-PreserveFileLocation] [-MaxPendingAsyncIO <Int32>] [-ReadThreads <Int32>] -Computer <String>
 [-Credential <PSCredential>] [-TruncateSource <Int32>] -RestoreReason <String>
 [-RestoreClassification <String>] [-UseDetailedLog] [-LocalStorageMethod <String>] [-PassThru]
 [<CommonParameters>]
```

### DumpRestore
```
Start-DSClientMSSqlServerRestore [-Items] <String[]> [-DbCredential <PSCredential>] [-MapDatabase <String[]>]
 [-DumpMethod <String>] [-DumpPath <String>] [-LeaveDatabaseRestoring] [-PreserveFileLocation]
 [-RestoreDumpOnly] [-MaxPendingAsyncIO <Int32>] [-ReadThreads <Int32>] -Computer <String>
 [-Credential <PSCredential>] [-TruncateSource <Int32>] -RestoreReason <String>
 [-RestoreClassification <String>] [-UseDetailedLog] [-LocalStorageMethod <String>] [-PassThru]
 [<CommonParameters>]
```

## DESCRIPTION
Start a Restore of an MS SQL Server Backup Set. The Initialize-DSClientBackupSetRestore Cmdlet must be used before this Cmdlet

## EXAMPLES

### Example 1
```powershell
PS C:\> Start-DSClientMSSqlServerRestore -Items "MSSQLSERVER\\master" -Computer "\\SQLSERVER01" -DumpPath "F$\Restore" -RestoreDumpOnly
```

Restores the "master" database as a File Dump to the "F$\Restore" location on the Computer "SQLSERVER01"

### Example 2
```powershell
PS C:\> Start-DSClientMSSqlServerRestore -Items "MSSQLSERVER\\UserDatabase" -Instance "MSSQLSERVER" -Computer "\\SQLSERVER01" -MapDatabase "UserDatabase,UserDatabaseRestore,D:\Data,L:\Logs" -LeaveDatabaseRestoring
```

Restores the Database named "UserDatabase" to the "MSSQLSERVER" instance running on Computer "SQLSERVER01" and maps the database to "UserDatabaseRestore" on the destination Instance, and leaves the database in a restoring state upon completion

## PARAMETERS

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

### -DbCredential
Database Credentials

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

### -DumpMethod
Specify the Database Dump Method

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: DumpLocal, DumpBuffer

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DumpPath
Specify the Dump Path

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

### -Instance
Specify the Database Instance

```yaml
Type: String
Parameter Sets: InstanceRestore
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Items
Specify the items to validate

```yaml
Type: String[]
Parameter Sets: (All)
Aliases: Path

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -LeaveDatabaseRestoring
Leave Database(s) in Restoring Mode

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
Accepted values: None, ConnectDsSysFirst, ConnectDsSysNeeded, ContinueWithoutDsSys

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MapDatabase
Map Source and Destination Databases in format 'sourceDb,destinationDb,dataPath,logPath'

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

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

### -PreserveFileLocation
Preserve Database(s) File Location

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

### -RestoreDumpOnly
Only Restore the Database Dump File

```yaml
Type: SwitchParameter
Parameter Sets: DumpRestore
Aliases:

Required: True
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

### System.String[]

### System.String

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
