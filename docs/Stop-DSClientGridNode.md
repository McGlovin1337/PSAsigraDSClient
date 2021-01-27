---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Stop-DSClientGridNode

## SYNOPSIS
Stop, Shutdown or Reboot a DS-Client Grid Node

## SYNTAX

### stop
```
Stop-DSClientGridNode [-NodeId] <Int32> [-StopService] [[-Wait] <Int32>] [-PassThru] [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

### shutdown
```
Stop-DSClientGridNode [-NodeId] <Int32> [-Shutdown] [[-Wait] <Int32>] [-PassThru] [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

### restart
```
Stop-DSClientGridNode [-NodeId] <Int32> [-Restart] [[-Wait] <Int32>] [-PassThru] [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

## DESCRIPTION
Use this Cmdlet to Stop, Shutdown or Reboot/Restart a DS-Client Computer running as part of a DS-Client Grid

## EXAMPLES

### Example 1
```powershell
PS C:\> Stop-DSClientGridNode -NodeId 1 -StopService -Wait 30
```

Stops the Service Running on Node 1 and Waits 30 Minutes for Running Activities to complete

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

### -NodeId
Specify Grid NodeId

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

### -PassThru
Specify to Output Object

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

### -Restart
Restart DS-Client Computer

```yaml
Type: SwitchParameter
Parameter Sets: restart
Aliases: Reboot

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Shutdown
Shutdown DS-Client Computer

```yaml
Type: SwitchParameter
Parameter Sets: shutdown
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -StopService
Stop DS-Client Service

```yaml
Type: SwitchParameter
Parameter Sets: stop
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Wait
Specify Number of Minutes to Wait for Running Activities to Stop (0 = Infinate)

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: False
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

### System.Int32

## OUTPUTS

### PSAsigraDSClient.StopDSClientGridNode+DSClientGridNodeStatus

## NOTES

## RELATED LINKS
