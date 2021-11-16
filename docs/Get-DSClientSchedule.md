---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientSchedule

## SYNOPSIS
Display the Schedules configured on the DS-Client computer

## SYNTAX

```
Get-DSClientSchedule [-ScheduleId <Int32[]>] [-Name <String[]>] [<CommonParameters>]
```

## DESCRIPTION
Display the Schedules configured on the DS-Client computer

## EXAMPLES

### Example 1
```
PS C:\> Get-DSClientSchedule
```

Lists all the DS-Client Schedules

### Example 2
```
PS C:\> Get-DSClientSchedule -ScheduleId 5,8 -Name '*week*'
```

List Schedules with Id's 5 and 8, and all Schedules that contain the name "week"

## PARAMETERS

### -Name
The Name of the Retention Rule

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: True
```

### -ScheduleId
The Retention Rule Id

```yaml
Type: Int32[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None
## OUTPUTS

### PSAsigraDSClient.BaseDSClientSchedule+DSClientScheduleInfo

## NOTES

## RELATED LINKS
