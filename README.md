# PSAsigraDSClient

## About
This is a collection of Powershell Cmdlets to Manage the Asigra DS-Client.\
It uses the Asigra DS-Client API Library distributed with the Asigra DS-Client Software.

This module supports both Windows and Linux DS-Clients and should be compatible with versions 13.0 through to 14.2

The main goal of this project and module is to allow the creation of automated processes, particularly with configuration and initial deployments of DS-Clients.

## Installation

### Pre-requisites
This module has the following requirements:
- PowerShell Version 5.1 (64bit)
- .NET Framwork 4.7.2
- Microsoft Visual C++ 2010 Runtime x64

### Install
You can install the module from the PowerShell Gallery:
```powershell
Install-Module -Name PSAsigraDSClient
```

## Usage
Once installed, to get started import the module to your PowerShell session:
```powershell
Import-Module -Name PSAsigraDSClient
```

Connect to your DS-Client, only PSCredentials are supported for Username/Password:
```powershell
Enter-DSClientSession -Host MyDsClientServer -Credential (Get-Credential)
```
This will attempt to establish a connection to the specified DS-Client using the API Port, which by default is TCP 4411. Use "-Port" to specify an alternative port if required.

From here you will now be able to use the Cmdlets in this module to manage the connected DS-Client.

Once finished, you should disconnect from the DS-Client:
```powershell
Exit-DSClientSession
```

Each Cmdlet has basic Cmdlet Help with an overall description and parameter info and examples, e.g.:
```powershell
Get-Help Get-DSClientActivityLog -Full
```

## Project/Module Status
This Project and Module is currently in a pre-release stage, but should provide a good level of functionality.

Please raise an issue should you discover any bugs, or have any suggestions for enhancements.
