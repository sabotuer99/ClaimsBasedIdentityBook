Examples Summary

Microsoft Patterns & Practices

This document describes the examples that illustrate the scenarios described in the book A Guide to Claims-based Identity and Access Control (2.0) (see http://claimsid.codeplex.com/). Each example is a Microsoft Visual Studio 2010 project that you can compile and run. For information about running each example, and the known issues and options available, see the Release Notes.
 
 
Before you start
 
To check for software prerequisites needed to use the examples, run CheckDependencies.cmd in the example scenarios folder. You must have .NET Framework version 4.0 installed to run the Dependency Checker.
 
NOTE: If you want to install the Windows Azure Tools on Windows Vista or Windows Server 2008 R2 you must install .NET Framework version 3.5.1 before you run the CheckDependencies.cmd batch file. The .NET Framework version 3.5.1 can be installed as a Feature using Windows Server Manager.
 
The CheckDependencies.cmd batch file launches a dependency checking tool that reports any components missing on your system, and provides links to obtain, install, and configure these missing components. Not all of examples require all of the components to run, but they do all require Windows Identity Foundation and IIS.
 
NOTE: Scenario 9 requires the Windows Phone 7 Developer Tools. This scenario has not been tested on Windows Server 2008 or Windows Server 2008 R2 because the tools require Windows Vista or Windows 7.
 
Some infrastructure is shared by all of the examples, and everything that is not specific to identity has been simulated for simplicity. For example, there are no bindings to databases in any of the examples. In addition, all of the examples use simulated security token issuers to mimic the behavior of a production issuer for the fictitious companies that participate in the scenarios. The goal was to simplify the deployment experience and make it possible to run the example applications on a single development machine. For production applications, it is recommended that you use ADFS V2.0.
 
Cryptographic certificates required by the examples are automatically created for you by the Dependency Checking tool. By default, these use the SHA-1 signature hashing algorithm. For production environments, you should consider using SHA-2. The samples also install a certificate for localhost that trusts a certificate named RootAgency. This is to avoid a certificate warning being shown in the browser each time you run the examples. You should remove these certificates from your computer after you have finished using the examples.
 
You will need a Windows Live or Google account to work with the examples that use ACS.
 
 
About The Examples
 
1 - Single Sign On
This is the most basic example. It includes two Web applications that have been updated to use claims, with before and after versions provided. It includes a claims issuer. The example's Web applications are based on ASP.NET Web Forms.
 
2 - Federation
This example extends Example 1 (Single Sign On) to show federated identity across security realms. It introduces the concepts of identity and federation providers. It shows some of the basic concepts of manual home realm discovery and claims transformation.
 
3 - Federation With Multiple Partners
This is a more advanced federation example. It was developed with the ASP.NET MVC framework. It demonstrates federated identity across multiple security realms, automated home realm discovery, and more complex claims transformation.
 
4 - Active Client Federation
This example demonstrates federated identity across security realms in the context of WCF Web services.
 
5 - Windows Azure
This example shows how to host Example 1 (Single Sign On) in Windows Azure.
 
6 - Federation And ACS
This example shows how to use federated identity across security realms with ACS. It extends Example 2 (Federation) to use ACS.
 
7 - Federation With Multiple Partners And ACS
This example shows how to use federated identity across security realms with multiple partners using ACS. It extends Example 3 (Federation With Multiple Partners) to use ACS.
 
8 - Active REST Client Federation
This example shows how to use ACS to secure a REST endpoint using an active client. It extends Example 4 (Active Client Federation) to use ACS and a REST endpoint.
 
9 - Windows Phone Client Federation
This example shows how to use claims and ACS to secure a REST endpoint for a Windows Phone 7 device.
 
Throughout the examples you will see blue information icons on the Web pages that they display. If you position the mouse over these icons, you will see additional information about what is happening behind the scenes: