---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# New-DSClientFSRestoreOptions

## SYNOPSIS
Creates a new set of File System Restore Options

## SYNTAX

### default
```
New-DSClientFSRestoreOptions [-FileOverwriteOption <String>] [-RestoreMethod <String>]
 [-RestorePermissions <String>] [-AuthoritativeRestore] [-OverwriteJunctionPoint] [-SkipOfflineFiles]
 [-UseDetailedLog] [-LocalStorageMethod <String>] [-DSSystemReadThreads <Int32>] [-MaxPendingAsyncIO <Int32>]
 [<CommonParameters>]
```

### filesys
```
New-DSClientFSRestoreOptions [-DefaultFileSystemOptions] [<CommonParameters>]
```

### winfilesys
```
New-DSClientFSRestoreOptions [-DefaultWinFileSystemOptions] [<CommonParameters>]
```

## DESCRIPTION
Creates a new set of File System Restore Options, which can then be applied to Restore Sessions using Set-DSClientRestoreSession Cmdlet

## EXAMPLES

### Example 1
```powershell
PS C:\> New-DSClientFSRestoreOptions -DefaultFileSystemOptions
```

Creates a set of Default File System Restore Options

### Example 2
```powershell
PS C:\> New-DSClientFSRestoreOptions -UseDetailedLog
```

Creates a set of Default File System Restore Options, but sets the UseDetailedLog option to True

## PARAMETERS

### -AuthoritativeRestore
Specify if this is an Authoritative Restore (Windows Only)

```yaml
Type: SwitchParameter
Parameter Sets: default
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DSSystemReadThreads
Set the number of DS-System Read Threads

```yaml
Type: Int32
Parameter Sets: default
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DefaultFileSystemOptions
Create a Default File System Restore Option Object

```yaml
Type: SwitchParameter
Parameter Sets: filesys
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DefaultWinFileSystemOptions
Create a Default Windows File System Restore Option Object

```yaml
Type: SwitchParameter
Parameter Sets: winfilesys
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FileOverwriteOption
Specify the File System Overwrite Option

```yaml
Type: String
Parameter Sets: default
Aliases:
Accepted values: RestoreAll, RestoreNewer, RestoreOlder, RestoreDifferent, SkipExisting

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -LocalStorageMethod
Specify Local Storage Handling

```yaml
Type: String
Parameter Sets: default
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
Parameter Sets: default
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -OverwriteJunctionPoint
Specify whether to Overwrite Junction Points (Windows Only)

```yaml
Type: SwitchParameter
Parameter Sets: default
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RestoreMethod
Specify the File Restore Method

```yaml
Type: String
Parameter Sets: default
Aliases:
Accepted values: Fast, Save, UseBuffer

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RestorePermissions
Specify whether to Restore File Permissions

```yaml
Type: String
Parameter Sets: default
Aliases:
Accepted values: Yes, Skip, Only

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -SkipOfflineFiles
Specify whether to Skip Restoring Offline Files

```yaml
Type: SwitchParameter
Parameter Sets: default
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
Parameter Sets: default
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

### System.String

### System.Management.Automation.SwitchParameter

### System.Int32

## OUTPUTS

### PSAsigraDSClient.RestoreOptions_FileSystem

### PSAsigraDSClient.RestoreOptions_Win32FileSystem

## NOTES

## RELATED LINKS
