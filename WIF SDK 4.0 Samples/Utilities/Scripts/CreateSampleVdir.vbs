Set shell = Wscript.CreateObject( "WScript.Shell" )

If Wscript.Arguments.Count < 2 Then 
   usage = "USAGE: CreateSampleVdir.vbs virtual_directory_name directory_location_to_map" & vbCrLf & vbCrLf 
   usage = usage & "ARGUMENTS" & vbCrLf 
   usage = usage & "=========" & vbCrLf 
   usage = usage & "virtual_directory_name   : String representing the name of the" & vbCrLf 
   usage = usage & "                           virtual directory." & vbCrLf 
   usage = usage & "directory_location_to_map: Directory to be mapped to the virtual" & vbCrLf 
   usage = usage & "                           directory. Can be relative to the" & vbCrLf 
   usage = usage & "                           current directory." & vbCrLf 
   usage = usage & "windowsAuth:               Optional string indicating that Windows Authentication" & vbCrLf 
   usage = usage & "                           has to be enabled for the virtual directory." & vbCrLf 
   WScript.Echo usage
   Wscript.Quit 
End If

vDirName = Wscript.Arguments(0)
vDirPath = Wscript.Arguments(1)
If Wscript.Arguments.Count = 3 Then 
   windowsAuth = Wscript.Arguments(2)
End If

' Get the name of the current directory
Set fso = WScript.CreateObject( "Scripting.FileSystemObject" )
vDirPath = fso.GetFolder( vDirPath ).Path

' Does this IIS application already exist in the metabase?
On Error Resume Next
Set objIIS = GetObject( "IIS://localhost/W3SVC/1/Root/" & vDirName )


If Err.Number = 0 Then
    result = shell.Popup( "A virtual directory named " & vDirName & " already exists. " & vbCrLf & vbCrLf & "Would you like it re-mapped for this sample?", 0 ,"Remap Virtual Directory?", 4 + 32 )' 4 = YesNo & 32 = Question
    If result = 6 Then ' 6 = Yes
        DeleteVirtualDirectory vDirName 
    Else
        WScript.Quit
    End If
End If

'Using IIS Administration object , turn on script/execute permissions and define the virtual directory as an 'in-process application. 
Set objIIS  = GetObject( "IIS://localhost/W3SVC/1/Root" )
Set vDirObj = objIIS.Create( "IISWebVirtualDir", vDirName )

vDirObj.Path                  = vDirPath
If windowsAuth = "windowsAuth" Then
   vDirObj.AuthNTLM           = True
   vDirObj.AuthAnonymous      = False
Else
   vDirObj.AuthNTLM           = False
   vDirObj.AuthAnonymous      = True
End If
vDirObj.AccessRead            = True
vDirObj.AccessWrite           = True 
vDirObj.AccessScript          = True
vDirObj.AccessExecute         = True
'vDirObj.AnonymousUserName     = owner
vDirObj.AnonymousPasswordSync = True
vDirObj.AppCreate True
vDirObj.SetInfo 

If Err.Number > 0 Then
    errorString = Err.Description & " IIS is not installed. Install IIS by choosing the Application Server component in Add/Remove Windows Components from the Control Panel" 
    shell.Popup errorString, 0, "Error", 16 ' 16 = Stop
    WScript.Quit
End If

' Get the name of the account for the anonymous user in IIS
owner = vDirObj.AnonymousUserName

' Change necessary folder permissions using CACLS.exe
aclCmd = "cmd /c echo y| CACLS "
aclCmd = aclCmd & """" & vDirPath & """"
aclCmd = aclCmd & " /E /G " & owner & ":C"
rtc = shell.Run( aclCmd , 0, True )

aclCmd = "cmd /c echo y| CACLS "
aclCmd = aclCmd & """" & vDirPath & """"
aclCmd = aclCmd & " /E /G ""VS Developers"":C"
rtc = shell.Run( aclCmd , 0, True )

If Err.Number > 0 Then
    shell.Popup Err.Description, 0, "Error", 16 ' 16 = Stop
    WScript.Quit
Else
    res = vDirName & " has been created at" & vbCrLf & vDirPath
    shell.Popup res, 0, "All done", 64 ' 64 = Information
End If

Sub DeleteVirtualDirectory( NameOfVdir )

    Set iis = GetObject("IIS://localhost/W3SVC/1/Root")
    iis.Delete "IISWebVirtualDir", vDirName
    
    If Err.Number <> 0 Then

      errorString = "Unable to delete exisiting virtual directory."

      If Err.Description Is Nothing Then
         errorString = errorString & "Error Code: " & Err.Number
      Else
         errorString = errorString & "Description: " & Err.Description
      End If

      shell.Popup errorString, 0, "Error", 16 ' 16 = Stop
    End If

End Sub
 
