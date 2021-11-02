---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# New-DSClientInitialBackupPath

## SYNOPSIS
Add a new Initial Backup Path

## SYNTAX

```
New-DSClientInitialBackupPath [-Path] <String> [[-Credential] <PSCredential>] [-EncryptionType <String>]
 [-EncryptionKey <String>] [<CommonParameters>]
```

## DESCRIPTION
Adds a new Path that can be used for Initial Backups.
Windows DS-Clients may specify a remote share, e.g. '\\computer\share'
Unix DS-Clients may only specify a local path.
Credentials only needed for remote shares.

## EXAMPLES

### Example 1
```powershell
PS C:\> New-DSClientInitialBackupPath -Path '\\computer\share' -Credential (Get-Credential username) -EncryptionType None
```

Adds the share path '\\computer\share' without Encryption

## PARAMETERS

### -Credential
Specify the Credentials for the Storage Path

```yaml
Type: PSCredential
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -EncryptionKey
Specify an Encryption Key to use (setting this implies EncryptionType as 'SpecifiedKey'

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

### -EncryptionType
Specify the Encryption Type

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: None, ClientPrivateKey, SpecifiedKey

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Path
Specify the Intial Backup Storage Path

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

### System.Management.Automation.PSCredential

## OUTPUTS

### System.Void

## NOTES

## RELATED LINKS
