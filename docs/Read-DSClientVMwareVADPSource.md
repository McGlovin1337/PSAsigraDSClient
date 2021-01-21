---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Read-DSClientVMwareVADPSource

## SYNOPSIS
Enumerates the objects of a VMware VCenter or ESXi Host

## SYNTAX

```
Read-DSClientVMwareVADPSource [-Computer] <String> [-Credential <PSCredential>] [[-Path] <String>] [-Recursive]
 [-RecursiveDepth <Int32>] [<CommonParameters>]
```

## DESCRIPTION
This Cmdlet will enumerate the objects of a VMware VCenter or ESXi Host, useful to assist with selecting items suitable for a Backup Set.

## EXAMPLES

### Example 1
```powershell
PS C:\> Read-DSClientVMwareVADPSource -Computer 'VMWARE\myVCenterServer' -Credential (Get-Credential user@domain.tld) -Recursive
```

Connects to the Host myVCenterServer using Domain Credentials and Enumerates objects from the root recursively.

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
Type: PSCredential
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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

## OUTPUTS

### PSAsigraDSClient.BaseDSClientBackupSource+SourceItemInfo

## NOTES

## RELATED LINKS
