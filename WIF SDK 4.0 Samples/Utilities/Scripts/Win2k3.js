var wsh = new ActiveXObject( "WScript.Shell" );
WScript.Echo( wsh.RegRead("HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders\\Common AppData" ) );
