---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Invoke-DSClientCommand

## SYNOPSIS
Execute Commands within the context of a specific DS-Client Session

## SYNTAX

### id
```
Invoke-DSClientCommand -ScriptBlock <ScriptBlock> [-Id] <Int32[]> [<CommonParameters>]
```

### name
```
Invoke-DSClientCommand -ScriptBlock <ScriptBlock> -Name <String[]> [<CommonParameters>]
```

### hostname
```
Invoke-DSClientCommand -ScriptBlock <ScriptBlock> -HostName <String[]> [<CommonParameters>]
```

### state
```
Invoke-DSClientCommand -ScriptBlock <ScriptBlock> -State <String> [<CommonParameters>]
```

### os
```
Invoke-DSClientCommand -ScriptBlock <ScriptBlock> -OperatingSystem <String[]> [<CommonParameters>]
```

### sessionObj
```
Invoke-DSClientCommand -ScriptBlock <ScriptBlock> [-Session] <DSClientSession[]> [<CommonParameters>]
```

## DESCRIPTION
Executes Commands and Scripts within the context of a specified DS-Client Session

## EXAMPLES

### Example 1
```powershell
PS C:\> Invoke-DSClientCommand -Session $session -ScriptBlock { Start-DSClientSystemActivity -DailyAdmin }
```

Starts a Daily Admin System Activity against the Sessions specified in $session

## PARAMETERS

### -HostName
Specify DS-Client Sessions by HostName

```yaml
Type: String[]
Parameter Sets: hostname
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: True
```

### -Id
Specify DS-Client Session by Id

```yaml
Type: Int32[]
Parameter Sets: id
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -Name
Specify DS-Client Sessions by Name

```yaml
Type: String[]
Parameter Sets: name
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: True
```

### -OperatingSystem
Specify DS-Client Sessions by Operating System

```yaml
Type: String[]
Parameter Sets: os
Aliases:
Accepted values: Linux, Mac, Windows

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -ScriptBlock
Specify Command(s) to Execute

```yaml
Type: ScriptBlock
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Session
Specify DS-Client Session by DSClientSession Object

```yaml
Type: DSClientSession[]
Parameter Sets: sessionObj
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -State
Specify DS-Client Sessions by Connection State

```yaml
Type: String
Parameter Sets: state
Aliases:
Accepted values: Connected, Disconnected

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32[]

### System.String[]

### System.String

### PSAsigraDSClient.DSClientSession[]

## OUTPUTS

### System.Management.Automation.PSObject

## NOTES

## RELATED LINKS
