---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# New-DSClientRetentionRule

## SYNOPSIS
Creates a New Retention Rule

## SYNTAX

```
New-DSClientRetentionRule [-Name] <String> [-ArchiveTimeValue <Int32>] [-ArchiveTimeUnit <String>]
 [-ArchiveFilterRule <String>] [-PassThru] [-CleanupDeletedFiles] [-CleanupDeletedAfterValue <Int32>]
 [-CleanupDeletedAfterUnit <String>] [-CleanupDeletedKeepGens <Int32>] [-DeleteGensPriorToStub]
 [-DeleteNonStubGens] [-LSRetentionTimeValue <Int32>] [-LSRetentionTimeUnit <String>] [-LSCleanupDeletedFiles]
 [-LSCleanupDeletedAfterValue <Int32>] [-LSCleanupDeletedAfterUnit <String>]
 [-LSCleanupDeletedKeepGens <Int32>] [-DeleteUnreferencedFiles] [-DeleteIncompleteComponents]
 [-KeepLastGens <Int32>] [-KeepAllGensTimeValue <Int32>] [-KeepAllGensTimeUnit <String>] [-DeleteObsoleteData]
 [-MoveObsoleteData] [-CreateNewBLMPackage] [<CommonParameters>]
```

## DESCRIPTION
Creates a New Retention Rule, additional time based rules can be added using Add-DSClientTimeRetentionRule

## EXAMPLES

### Example 1
```powershell
PS C:\> New-DSClientRetentionRule -Name "7 Day Retention" -KeepLastGens 1 -KeepAllGensTimeValue 12 -KeepAllGensTimeUnit "Hours" -DeleteObsoleteData -DeleteUnreferencedFiles -DeleteIncompleteComponents
```

Creates a New Retention Rule Named "7 Day Retention Rule"
Keeps the latest 1 Generation
Keeps All Generations for the last 12 Hours
Deletes Obsolete Data
Deletes Unreferenced Files
Deletes Incomplete Components

## PARAMETERS

### -ArchiveFilterRule
Specify an existing Archive Filter Rule

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -ArchiveTimeUnit
Specify Archive Rule Time Unit

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

### -ArchiveTimeValue
Specify Archive Rule Time Value

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

### -CreateNewBLMPackage
Specify to create new BLM Packages when moving to BLM

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

### -DeleteObsoleteData
Specify to Delete Obsolete Data

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

### -KeepAllGensTimeUnit
Specify Time Period Unit for keeping ALL Generations

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Minutes, Hours, Days

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -KeepAllGensTimeValue
Specify Time Period to keep ALL Generations

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

### -KeepLastGens
Specify the number of most recent Generations to keep

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

### -MoveObsoleteData
Specify to Move Obsolete Data to BLM

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

### -Name
Specify Retention Rule Name

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -PassThru
Specify to Output Retention Rule Overview

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

### System.Int32

### System.Management.Automation.SwitchParameter

## OUTPUTS

### PSAsigraDSClient.NewDSClientRetentionRule+DSClientNewRetentionRule

## NOTES

## RELATED LINKS
