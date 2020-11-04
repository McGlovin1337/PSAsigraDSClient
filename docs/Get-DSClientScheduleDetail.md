---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientScheduleDetail

## SYNOPSIS
Display Detailed Schedule Configuration

## SYNTAX

```
Get-DSClientScheduleDetail [-ScheduleId] <Int32> [[-Type] <String>] [<CommonParameters>]
```

## DESCRIPTION
Display the Detailed Schedule Configuration associated with a Schedule

## EXAMPLES

### Example 1
```
PS C:\> Get-DSClientScheduleDetail -ScheduleId 3
```

List all the detailed schedules associated with ScheduleId 3.
Use Get-DSClientSchedule to find ScheduleId

### Example 2
```
PS C:\> Get-DSClientScheduleDetail -ScheduleId 3 -Type Daily
```

List all the detailed schedules of Type Daily associated with ScheduleId 3

## PARAMETERS

### -ScheduleId
Specify the Schedule Id

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

### -Type
Get the Schedule Details for the specified type: OneTime, Daily, Weekly, Monthly, Undefined

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: OneTime, Daily, Weekly, Monthly, Undefined

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32
## OUTPUTS

### PSAsigraDSClient.BaseDSClientSchedule+DSClientScheduleDetail
## NOTES

## RELATED LINKS
