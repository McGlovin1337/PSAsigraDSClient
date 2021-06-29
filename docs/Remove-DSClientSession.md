---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Remove-DSClientSession

## SYNOPSIS
Disconnects and Removes the specified DS-Client Session(s)

## SYNTAX

### id (Default)
```
Remove-DSClientSession [-Id] <Int32[]> [-WhatIf] [-Confirm] [<CommonParameters>]
```

### name
```
Remove-DSClientSession -Name <String[]> [-WhatIf] [-Confirm] [<CommonParameters>]
```

### hostname
```
Remove-DSClientSession -HostName <String[]> [-WhatIf] [-Confirm] [<CommonParameters>]
```

### state
```
Remove-DSClientSession -State <String> [-WhatIf] [-Confirm] [<CommonParameters>]
```

### os
```
Remove-DSClientSession -OperatingSystem <String[]> [-WhatIf] [-Confirm] [<CommonParameters>]
```

### sessionObj
```
Remove-DSClientSession [-Session] <DSClientSession[]> [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Disconnects and Removes the specified DS-Client Session(s)

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-DSClientSession -Id 1
```

Removes the DS-Client Session with Id of 1

### Example 2
```powershell
PS C:\> Get-DSClientSession | Remove-DSClientSession
```

Removes all the current DS-ClientSessions in SessionState

## PARAMETERS

### -Confirm
Prompts you for confirmation before running the cmdlet.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: cf

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

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

### -Session
Specify DS-Client Session to remove by DSClientSession Object

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

### -WhatIf
Shows what would happen if the cmdlet runs.
The cmdlet is not run.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: wi

Required: False
Position: Named
Default value: None
Accept pipeline input: False
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

### System.Object
## NOTES

## RELATED LINKS
