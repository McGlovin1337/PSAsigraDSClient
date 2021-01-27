---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientUserGroup

## SYNOPSIS
Return the DS-Client User Group Info

## SYNTAX

```
Get-DSClientUserGroup [-GroupId <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Return the DS-Client User Group Info

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientUserGroup
```

Return all the DS-Client User Groups

### Example 2
```powershell
PS C:\> Get-DSClientUserGroup -GroupId 2
```

Return the Group Info for the User Group with Id 2

## PARAMETERS

### -GroupId
Specify a specific GroupId

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

### None

## OUTPUTS

### PSAsigraDSClient.BaseDSClientUserManager+DSClientUserGroup

## NOTES

## RELATED LINKS
