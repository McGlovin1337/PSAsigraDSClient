---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Remove-DSClientSchedule

## SYNOPSIS
Remove a Schedule from the DS-Client

## SYNTAX

```
Remove-DSClientSchedule [-ScheduleId] <Int32> [<CommonParameters>]
```

## DESCRIPTION
Removes a Schedule from the DS-Client

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-DSClientSchedule -ScheduleId 1
```

Removes the Schedule with Id of 1 from the DS-Client

## PARAMETERS

### -ScheduleId
The ScheduleId to remove

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
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
