---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Start-DSClientSystemActivity

## SYNOPSIS
Start a DS-Client System Activity

## SYNTAX

```
Start-DSClientSystemActivity [-Activity] <String> [<CommonParameters>]
```

## DESCRIPTION
Start a DS-Client System Activity

## EXAMPLES

### Example 1
```powershell
PS C:\> Start-DSClientSystemActivity -Activity DailyAdmin
```

Start a Daily Admin task

## PARAMETERS

### -Activity
Sepcify the System Activity to Start

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: DailyAdmin, StatisticsAdmin, WeeklyAdmin

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
