---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Disable-DSClientSchedule

## SYNOPSIS
Disables a Schedule

## SYNTAX

```
Disable-DSClientSchedule [-ScheduleId] <Int32> [<CommonParameters>]
```

## DESCRIPTION
Sets a Schedule to Disabled state

## EXAMPLES

### Example 1
```powershell
PS C:\> Disable-DSClientSchedule -ScheduleId 2
```

Sets the Schedule with Schedule Id 2 to Disabled

## PARAMETERS

### -ScheduleId
Specify the Schedule to Disable

```yaml
Type: Int32
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

### System.Int32

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
