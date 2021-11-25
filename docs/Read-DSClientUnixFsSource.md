---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Read-DSClientUnixFsSource

## SYNOPSIS
Enumerates the items of a Unix File System

## SYNTAX

```
Read-DSClientUnixFsSource [-SSHKeyFile <String>] [-SSHInterpreter <String>] [-SSHInterpreterPath <String>]
 [-SudoCredential <PSCredential>] [-Computer] <String> [-Credential <DSClientCredential>] [[-Path] <String>]
 [-Recursive] [-RecursiveDepth <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Enumerates the items of a Unix File System useful to assist in selecting items for a Backup Set

## EXAMPLES

### Example 1
```powershell
PS C:\> Read-DSClientUnixFsSource -Computer "UNIX-SSH\LinuxServer01" -SSHKeyFile "/home/user/key" -Path "/" -Recursive -RecursiveDepth 1
```

Connects to the Computer "LinuxServer01" using an SSH Key File stored in "/home/user/key" on the DS-Client and enumerates all items in "/" upto 1 Sub-Directory deep

## PARAMETERS

### -Computer
Specify the Source Computer

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -Credential
Specify Credentials for specified Computer

```yaml
Type: DSClientCredential
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Path
Specify an initial Path to browse

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

### -Recursive
Specify if items should be returned recursively

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

### -RecursiveDepth
Specify the recursive depth

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

### -SSHInterpreter
SSH Interpreter to access the data

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Perl, Python, Direct

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SSHInterpreterPath
SSH Interpreter path

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

### -SSHKeyFile
Path to SSH Key File on DS-Client

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

### -SudoCredential
Specify SUDO User Credentials

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

## OUTPUTS

### PSAsigraDSClient.BaseDSClientBackupSource+SourceItemInfo

## NOTES

## RELATED LINKS
