---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientGridLog

## SYNOPSIS
Returns the Log from the DS-Client Grid

## SYNTAX

```
Get-DSClientGridLog [-From <DateTime>] [-To <DateTime>] [<CommonParameters>]
```

## DESCRIPTION
Use this Cmdlet to Return a List of Event from DS-Client Grid Configurations

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientGridLog -From "01/01/2020"
```

Return all log entries from 01/01/2020

## PARAMETERS

### -From
Specify From Date

```yaml
Type: DateTime
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -To
Specify To Date

```yaml
Type: DateTime
Parameter Sets: (All)
Aliases:

Required: False
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

### PSAsigraDSClient.GetDSClientGridLog+DSClientGridLog

## NOTES

## RELATED LINKS
