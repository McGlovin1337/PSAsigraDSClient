---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Set-DSClientRetentionRule

## SYNOPSIS
Changes the configuration of a Retention Rule

## SYNTAX

```
Set-DSClientRetentionRule [-RetentionRuleId] <Int32> [-NewName <String>] [-CleanupDeletedFiles]
 [-CleanupDeletedAfterValue <Int32>] [-CleanupDeletedAfterUnit <String>] [-CleanupDeletedKeepGens <Int32>]
 [-DeleteGensPriorToStub] [-DeleteNonStubGens] [-LSRetentionTimeValue <Int32>] [-LSRetentionTimeUnit <String>]
 [-LSCleanupDeletedFiles] [-LSCleanupDeletedAfterValue <Int32>] [-LSCleanupDeletedAfterUnit <String>]
 [-LSCleanupDeletedKeepGens <Int32>] [-DeleteUnreferencedFiles] [-DeleteIncompleteComponents] [-WhatIf]
 [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Changes the configuration of a Retention Rule

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DSClientRetentionRule -RetentionRuleId 5 -NewName "New Retention"
```

Renames the Retention Rule with Id 5

## PARAMETERS

### -CleanupDeletedAfterUnit
Cleanup Files Deleted from Source after Time period

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Minutes, Hours, Days, Weeks, Months, Years

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -CleanupDeletedAfterValue
Cleanup Files Deleted from Source after Time period

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -CleanupDeletedFiles
Specify to Cleanup Files Deleted from Source

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -CleanupDeletedKeepGens
Specify Generations to keep of Files Deleted from Source

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DeleteGensPriorToStub
Specify to Delete All Generations prior to Stub

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DeleteIncompleteComponents
Specify to Delete Incomplete Components

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DeleteNonStubGens
Specify to Delete Non-stub Generations prior to Stub

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DeleteUnreferencedFiles
Specify to Delete Unreferenced Files

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -LSCleanupDeletedAfterUnit
Cleanup Files Deleted from Source after Time period on Local Storage

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Minutes, Hours, Days, Weeks, Months, Years

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -LSCleanupDeletedAfterValue
Cleanup Files Deleted from Source after Time period on Local Storage

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -LSCleanupDeletedFiles
Specify to Cleanup Files Deleted from Source on Local Storage

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -LSCleanupDeletedKeepGens
Specify Generations to keep of Files Deleted from Source

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -LSRetentionTimeUnit
Specify Local Storage Retention Time Unit

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Minutes, Hours, Days, Weeks, Months, Years

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -LSRetentionTimeValue
Specify Local Storage Retention Time Value

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -NewName
Specify a New Name for the Retention Rule

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

### -RetentionRuleId
Specify the RetentionRuleId to Modify

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

### -WhatIf
Shows what would happen if the cmdlet runs. The cmdlet is not run.

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

### System.Management.Automation.SwitchParameter

### System.String

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
