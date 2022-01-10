---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Set-DSClientSession

## SYNOPSIS
Modify an existing DS-Client Session

## SYNTAX

### sessionId
```
Set-DSClientSession [-Id] <Int32> [-NewName <String>] [-ConnectionAttempts <Int32>] [<CommonParameters>]
```

### sessionObject
```
Set-DSClientSession -Session <DSClientSession> [-NewName <String>] [-ConnectionAttempts <Int32>]
 [<CommonParameters>]
```

## DESCRIPTION
Modifies an existing DS-Client Session Configuration

## EXAMPLES

### Example 1 - Rename a Session
```powershell
PS C:\> Set-DSClientSession -Id 5 -NewName 'My Fave Session'
```

Changes the name of Session 5 to 'My Fave Session'

### Example 2 - Set the Connection Retries
```powershell
PS C:\> Set-DSClientSession -Id 1 -ConnectionAttempts 5
```

Sets the number of connection retries when executing a command to 5 attempts, after which the connection is considered disconnected.

## PARAMETERS

### -ConnectionAttempts
The number of connection attempts for the specified session

```yaml
Type: Int32
Parameter Sets: (All)
Aliases: ConnectionRetries

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
Specify DS-Client Session Id to Modify

```yaml
Type: Int32
Parameter Sets: sessionId
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -NewName
Specify a New Name for the Session

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Session
Specify a DS-Client Session Object to Modify

```yaml
Type: DSClientSession
Parameter Sets: sessionObject
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Int32

## OUTPUTS

### System.Void

## NOTES

## RELATED LINKS
