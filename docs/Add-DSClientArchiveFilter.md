---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Add-DSClientArchiveFilter

## SYNOPSIS
Adds an Archive Filter to an Archive Filter Rule

## SYNTAX

### File
```
Add-DSClientArchiveFilter [-ArchiveFilterRule] <String> [-Pattern] <String> [-Exclusion] [-IncludeSubDirs]
 [<CommonParameters>]
```

### Regex
```
Add-DSClientArchiveFilter [-ArchiveFilterRule] <String> [-Pattern] <String> [-Exclusion] [-NotCaseSensitive]
 [-MatchDirectories] [-Negate] [<CommonParameters>]
```

## DESCRIPTION
Adds an Archive Filter to an existing Archive Filter Rule

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-DSClientArchiveFilter -ArchiveFilterRule "FilterRule1" -Pattern "*.txt" -NotCaseSensitive
```

Adds a Filter to the Archive Filter Rule named "FilterRule1" that includes txt files and specifies the Pattern is NOT case sensitive

## PARAMETERS

### -ArchiveFilterRule
Specify Archive Filter Rule to add this Filter to

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

### -Exclusion
Specify this is an Exclusion Filter

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -IncludeSubDirs
Specify to include Sub-Directories in File Filter

```yaml
Type: SwitchParameter
Parameter Sets: File
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MatchDirectories
Specify Regex should Match Directory Names

```yaml
Type: SwitchParameter
Parameter Sets: Regex
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Negate
Specify Regex should be Negated

```yaml
Type: SwitchParameter
Parameter Sets: Regex
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -NotCaseSensitive
Specify Regex Filter is NOT Case Sensitive

```yaml
Type: SwitchParameter
Parameter Sets: Regex
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Pattern
Specify the Filter Pattern

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

### System.Management.Automation.SwitchParameter

## OUTPUTS

### System.Void

## NOTES

## RELATED LINKS
