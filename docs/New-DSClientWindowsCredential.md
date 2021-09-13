---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# New-DSClientWindowsCredential

## SYNOPSIS
Creates a new DS-Client Windows Credential Object

## SYNTAX

```
New-DSClientWindowsCredential [-Credential] <PSCredential> [<CommonParameters>]
```

## DESCRIPTION
Creates a new DS-Client Windows Credential Object. Note this Credential Object can also be used with MSSQL and VMware Backup Sets

## EXAMPLES

### Example 1
```powershell
PS C:\> New-DSClientWindowsCredential -Credential (Get-Credential)
```

Creates a new DS-Client Windows Credential Object

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Management.Automation.PSCredential

## OUTPUTS

### PSAsigraDSClient.DSClientCredential

## NOTES

## RELATED LINKS
