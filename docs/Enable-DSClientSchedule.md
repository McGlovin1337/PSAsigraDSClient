---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Enable-DSClientSchedule

## SYNOPSIS
Enable a Schedule

## SYNTAX

```
Enable-DSClientSchedule [-ScheduleId] <Int32> [<CommonParameters>]
```

## DESCRIPTION
Sets a Schedule to Enabled State

## EXAMPLES

### Example 1
```powershell
PS C:\> Enable-DSClientSchedule -ScheduleId 2
```

Sets the Schedule with Id 2 to Enabled

## PARAMETERS

### -ScheduleId
Specify the Schedule to Enable

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
