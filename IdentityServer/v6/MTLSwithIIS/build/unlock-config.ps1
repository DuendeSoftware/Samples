# This script unlocks the system.webServer/Security/access section of the application 
# host config, which allows the web.config files of the Api and IdentityServerHost to
# set the configuration they need to for mTLS.

# Source: https://stackoverflow.com/questions/5717154/programmatically-unlocking-iis-configuration-sections-in-powershell

$assembly = [System.Reflection.Assembly]::LoadFrom("$env:systemroot\system32\inetsrv\Microsoft.Web.Administration.dll")

$mgr = new-object Microsoft.Web.Administration.ServerManager
$conf = $mgr.GetApplicationHostConfiguration()
$conf.RootSectionGroup.SectionGroups["system.webServer"].SectionGroups["security"].Sections["access"].OverrideModeDefault = "Allow"
$mgr.CommitChanges()
