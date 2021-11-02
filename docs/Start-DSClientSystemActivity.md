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

### daily
```
Start-DSClientSystemActivity [-DailyAdmin] [-PassThru] [<CommonParameters>]
```

### weekly
```
Start-DSClientSystemActivity [-WeeklyAdmin] [-PassThru] [<CommonParameters>]
```

### stats
```
Start-DSClientSystemActivity [-StatisticalAdmin] [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
Start a DS-Client System Activity

## EXAMPLES

### Example 1
```
PS C:\> Start-DSClientSystemActivity -Activity DailyAdmin
```

Start a Daily Admin task

## PARAMETERS

### -DailyAdmin
Start Daily Admin Activity

```yaml
Type: SwitchParameter
Parameter Sets: daily
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PassThru
Specify to Output Basic Activity Details

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

### -StatisticalAdmin
Start Statistical Admin Activity

```yaml
Type: SwitchParameter
Parameter Sets: stats
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WeeklyAdmin
Start Weekly Admin Activity

```yaml
Type: SwitchParameter
Parameter Sets: weekly
Aliases:

Required: True
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

### System.Void

### PSAsigraDSclient.StartDSClientSystemActivity+SystemActivityStart

## NOTES

## RELATED LINKS
