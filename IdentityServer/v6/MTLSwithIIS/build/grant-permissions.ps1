$path = "C:\inetpub\identity"
$acl = Get-Acl $path
$rule = New-Object System.Security.AccessControl.FileSystemAccessRule("BUILTIN\IIS_IUSRS", "FullControl", "ContainerInherit,ObjectInherit", "None", "Allow")

$acl.SetAccessRule($rule)
Set-Acl $path $acl

$path = "C:\inetpub\api"
$acl = Get-Acl $path
$rule = New-Object System.Security.AccessControl.FileSystemAccessRule("BUILTIN\IIS_IUSRS", "FullControl", "ContainerInherit,ObjectInherit", "None", "Allow")

$acl.SetAccessRule($rule)
Set-Acl $path $acl