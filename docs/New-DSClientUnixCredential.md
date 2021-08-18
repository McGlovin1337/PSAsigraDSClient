---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# New-DSClientUnixCredential

## SYNOPSIS
Creates a new DS-Client Unix Credential Object

## SYNTAX

```
New-DSClientUnixCredential [-Credential] <PSCredential> [-SudoCredential <PSCredential>] [-SSHKeyFile <String>]
 [-SSHAccessType <String>] [-SSHInterpreterPath <String>] [<CommonParameters>]
```

## DESCRIPTION
Creates a new DS-Client Unix Credential Object

## EXAMPLES

### Example 1
```powershell
PS C:\> New-DSClientUnixCredential -Credential (Get-Credential) -SudoCredential (Get-Credential)
```

Creates a new Credential object with a Sudo User

## PARAMETERS

### -Credential
Specify PSCredentials

```yaml
Type: PSCredential
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -SSHAccessType
Specify the SSH Accessor Type

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Direct, Python, Perl

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SSHInterpreterPath
Specify a Path to the SSH Interpreter

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
Specify Path to SSH Key File on DS-Client

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
Specify Sudo User Credentials

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

### System.Management.Automation.PSCredential

## OUTPUTS

### PSAsigraDSClient.DSClientCredential

### PSAsigraDSClient.DSClientSSHCredential

## NOTES

## RELATED LINKS
