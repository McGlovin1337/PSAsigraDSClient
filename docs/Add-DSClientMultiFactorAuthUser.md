---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Add-DSClientMultiFactorAuthUser

## SYNOPSIS
Adds a User for Multi-Factor Authentication

## SYNTAX

```
Add-DSClientMultiFactorAuthUser [-Email] <String> [-Username] <String> [[-Domain] <String>]
 [<CommonParameters>]
```

## DESCRIPTION
Adds a User for Multi-Factor Authentication

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-DSClientMultiFactorAuthUser -Email "joe.bloggs@example.com" -Username "jbloggs"
```

Adds the Username jbloggs as an MFA User

## PARAMETERS

### -Domain
Specify Domain

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Email
Specify Email Address to add

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

### -Username
Specify User Name

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
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
