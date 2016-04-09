Set shell = Wscript.CreateObject( "WScript.Shell" )

If Wscript.Arguments.Count < 1 Then
   usage = "USAGE: DeleteSampleVdir.vbs virtual_directory_name" & vbCrLf & vbCrLf
   usage = usage & "ARGUMENTS" & vbCrLf
   usage = usage & "=========" & vbCrLf
   usage = usage & "virtual_directory_name   : String representing the name of the" & vbCrLf
   usage = usage & "                           virtual directory to delete."
   Wscript.Echo usage
   Wscript.Quit
End If

vDirName = Wscript.Arguments(0)

' Does this IIS application already exist in the metabase?
On Error Resume Next
Set objIIS = GetObject("IIS://localhost/W3SVC/1/Root/" & vDirName)

If Err.Number > 0 Then
    shell.Popup "A virtual directory named " & vDirName & " does not exists. ", 0, "Error", 16 ' 16 = Stop
    Wscript.quit
End If

Set objIIS = GetObject("IIS://localhost/W3SVC/1/Root")
objIIS.Delete "IISWebVirtualDir", vDirName

If Err.Number = 0 Then
    shell.Popup "Virtual directory named " & vDirName & " was deleted sucessfully", 0, "All done", 64 ' 64 = Information
Else
   errorString = "Unable to delete the virtual directory. The virtual directory may have been already deleted or, maybe IIS is not installed. Install IIS by choosing the Application Server component in Add/Remove Windows Components from the Control Panel" 
   If Err.Description Is Nothing Then
      ' If the error description is empty. Then just print the error number.
      errorString = errorString & " Error Code : " & Err.Number
   Else
      ' Display the Error Description
      errorString = errorString & " Error Description: " & Err.Description
   End If
   shell.Popup errorString, 0, "Error", 16
End If
 
