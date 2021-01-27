---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Add-DSClientUserGroupRole

## SYNOPSIS
Add a DS-Client Role

## SYNTAX

```
Add-DSClientUserGroupRole [-Name] <String> [[-From] <String>] [[-Role] <String>] [-IsGroup]
 [<CommonParameters>]
```

## DESCRIPTION
Add a new DS-Client User or Group Role

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-DSClientUserGroupRole -Name 'John Smith' -From . -Role 'BackupOperator'
```

Adds the Backup Operator Role to the Local User John Smith

### Example 2
```powershell
PS C:\> Add-DSClientUserGroupRole -Name 'Backup Teams' -From . -Role 'Administrator' -IsGroup
```

Adds the Administrator Role to the Backup Teams Local Group

## PARAMETERS

### -From
Specify the User/Group Domain or Computer

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

### -IsGroup
Specify that the Name is a Group

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

### -Name
Specify Name of User or Group

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

### -Role
Specify the Role to Add

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Administrator, BackupOperator, RegularUser

Required: False
Position: 2
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
