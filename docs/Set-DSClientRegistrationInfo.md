---
external help file: PSAsigraDSClient.dll-Help.xml
Module Name: PSAsigraDSClient
online version:
schema: 2.0.0
---

# Set-DSClientRegistrationInfo

## SYNOPSIS
Configures the DS-Client Registration Information

## SYNTAX

```
Set-DSClientRegistrationInfo [[-DsSystemAddress] <String>] [[-AccountNumber] <String>]
 [[-CustomerName] <String>] [[-ClientNumber] <String>] [-AccountKey <String>] [-AccountEncryption <String>]
 [-PrivateKey <String>] [-PrivateKeyEncryption <String>] [-EscrowKeys] [-CountryCode <Int32>]
 [-Industry <String>] [-Employees <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Configures the DS-Client Registration Information

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-DSClientRegistrationInfo -DsSystemAddress "123.123.123.123" -EscrowKeys
```

Sets the DS-System Address to "123.123.123.123" and enables Encryption Key forwarding

## PARAMETERS

### -AccountEncryption
Specify Encryption for Account Key

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: DES, AES128, AES192, AES256, AES128IV, AES192IV, AES256IV

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -AccountKey
Specify the Account Key

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

### -AccountNumber
Specify Account Number

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -ClientNumber
Specify Client Number

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 3
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -CountryCode
Set the ISO 3166 Country Code

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CustomerName
Specify Customer Name

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DsSystemAddress
Specify the DS-System Address

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -Employees
Specify the Number of Employees of the End User Organisation

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -EscrowKeys
Specify if to Forward Encryption Keys to DS-System

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

### -Industry
Specify the Industry Vertical of the End User Organisation

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: ManufacturingAndEnergy, RetailAndWholesaleTrade, UtilitiesAndTelecommunications, ComputersHardwareSoftware, BusinessServicesAndConstruction, MediaEntertainmentAndLeisure, FinancialServicesAndInsurance, PublicSectorAndHealthcare

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PrivateKey
Specify Private Key

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

### -PrivateKeyEncryption
Specify Encryption for Private Key

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: DES, AES128, AES192, AES256, AES128IV, AES192IV, AES256IV

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
