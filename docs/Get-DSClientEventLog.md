---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Get-DSClientEventLog

## SYNOPSIS
Returns the DS-Client Event Log

## SYNTAX

```
Get-DSClientEventLog [[-DateStart] <DateTime>] [[-DateEnd] <DateTime>] [[-ActivityId] <Int32>]
 [[-EventType] <String[]>] [[-EventCategory] <String[]>] [[-User] <String>] [[-NodeId] <Int32>]
 [<CommonParameters>]
```

## DESCRIPTION
Returns the DS-Client Event Log

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-DSClientEventLog -ActivityId 1234
```

Returns all the Event Log items related to an Activity with Id 1234

## PARAMETERS

### -ActivityId
Specify a specific ActivityId

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -DateEnd
Specify Date and Time to Search to

```yaml
Type: DateTime
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DateStart
Specify Date and Time to Search from

```yaml
Type: DateTime
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -EventCategory
Filter for specific Event Categories

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:
Accepted values: Application, Socket, Message, Database, Exception, IO, System, Security, MAPI, Novell, Oracle, RMAN, XML, DB2

Required: False
Position: 4
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -EventType
Filter for specific Event Types

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:
Accepted values: Information, Warning, Error

Required: False
Position: 3
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -NodeId
Filter to specifc DS-Client NodeId

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: 6
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -User
Filter for specific User events

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 5
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

### System.String[]

### System.String

## OUTPUTS

### PSAsigraDSClient.GetDSClientEventLog+DSClientEventLog

## NOTES

## RELATED LINKS
