---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientUser

## SYNOPSIS
Return the DS-Client User Info

## SYNTAX

```
Get-DSClientUser [-UserId <Int32>] [-GroupId <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Return the DS-Client User Info

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientUser
```

Return a list of all DS-Client Users

### Example 2
```powershell
PS C:\> Get-DSClientUser -UserId 2
```

Return the info for the User with UserId 2

### Example 3
```powershell
PS C:\> Get-DSClientUser -GroupId 3
```

Return all the users that belong to the User Group with GroupId 3

## PARAMETERS

### -GroupId
List All Users in Group by GroupId

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

### -UserId
Specify a specifc User by UserId

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

### PSAsigraDSClient.BaseDSClientUserManager+DSClientUser

## NOTES

## RELATED LINKS
