In order to use the HttpModules, the web.config of the SharePoint site has to be updated.
Usually, the web.config of the SharePoint applications are placed in the following location:
C:\inetpub\wwwroot\wss\VirtualDirectories\XXXXX\web.config
[where XXXXX is the port that application is running on]

------------------------------------------
------    Single Sign Out module    ------
------------------------------------------
Add the following elements to the web.config of your application.

Under system.web:
  <system.web>
    <httpModules>
      <add name="SingleSignOut" type="SingleSignOutModule.SingleSignOutModule, SingleSignOutModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=996db41c32d1031c" />

Under system.webServer
  <system.webServer>
    <modules ...>
      <add name="SingleSignOut" type="SingleSignOutModule.SingleSignOutModule, SingleSignOutModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=996db41c32d1031c" />


------------------------------------------
------    Sliding Session module    ------
------------------------------------------
Add the following elements to the web.config of your application.

Under system.web:
  <system.web>
    <httpModules>
      <add name="SlidingSession" type="SlidingSessionModule.SlidingSessionModule, SlidingSessionModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b61de93f440f208f" />

Under system.webServer
  <system.webServer>
    <modules ...>
      <add name="SlidingSession" type="SlidingSessionModule.SlidingSessionModule, SlidingSessionModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b61de93f440f208f" />
