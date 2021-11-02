---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Set-DSClientRestoreSession

## SYNOPSIS
Updates the Specified Restore Session

## SYNTAX

### default
```
Set-DSClientRestoreSession [-RestoreId] <Int32> [-Computer <String>] [-ComputerCredential <DSClientCredential>]
 [-DatabaseCredential <DSClientCredential>] [-RestoreReason <String>] [-RestoreClassification <String>]
 [-Options <RestoreOptions_Base>] [-DateFrom <DateTime>] [-DateTo <DateTime>] [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

### sharemapping
```
Set-DSClientRestoreSession [-RestoreId] <Int32> [-DestinationId] <Int32> [-DestinationPath <String>]
 [-TruncateAmount <Int32>] [-DatabaseMapping <MSSQLDatabaseMap[]>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
This Cmdlet is used to change the settings of the specified Restore Session.

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DSClientRestoreSession -RestoreId 1 -Options $options
```

Applies new Restore Options as specified in the $options variable

### Example 2
```powershell
PS C:\> Set-DSClientRestoreSession -RestoreId 1 -DestinationId 2 -DestinationPath 'D$\Restore'
```

Updates the Restore Path for the specified Source Share

## PARAMETERS

### -Computer
Specify the Destination Computer

```yaml
Type: String
Parameter Sets: default
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

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

### -DestinationId
Specify a Destination Path Id to modify

```yaml
Type: Int32
Parameter Sets: sharemapping
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DestinationPath
Specify the Destination Path for the restore

```yaml
Type: String
Parameter Sets: sharemapping
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Options
Apply New Restore Options

```yaml
Type: RestoreOptions_Base
Parameter Sets: default
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RestoreClassification
Restore Classification

```yaml
Type: String
Parameter Sets: default
Aliases:
Accepted values: Production, Drill, ProductionDrill

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RestoreId
Specify the Restore Session Id

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

### -RestoreReason
Reason for the Restore

```yaml
Type: String
Parameter Sets: default
Aliases:
Accepted values: UserErrorDataDeletion, MaliciousIntent, DeviceLostOrStolen, HardwareMalfunction, SoftwareMalfunction, DataStolen, DataCorruption, NaturalDisasters, PowerOutages, OtherDisaster, PreviousGeneration, DeviceDamaged

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -TruncateAmount
Specify the amount to truncate the original path by

```yaml
Type: Int32
Parameter Sets: sharemapping
Aliases:

Required: False
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

### -ComputerCredential
Specify Destination Computer Credentials

```yaml
Type: DSClientCredential
Parameter Sets: default
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DatabaseCredential
Specify MSSQL Database Credentials

```yaml
Type: DSClientCredential
Parameter Sets: default
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DatabaseMapping
Update Database Mapping(s)

```yaml
Type: MSSQLDatabaseMap[]
Parameter Sets: sharemapping
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DateFrom
Filter Data Selection From Date

```yaml
Type: DateTime
Parameter Sets: default
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DateTo
Filter Data Selection To Date

```yaml
Type: DateTime
Parameter Sets: default
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

### System.Int32

### System.String

### System.Management.Automation.PSCredential

## OUTPUTS

### System.Void

## NOTES

## RELATED LINKS
