# No need for the default site, we're making our own
Remove-WebSite -Name "Default Web Site"

# Create the identity site
mkdir C:\inetpub\identity
New-WebAppPool -Name "identity"
New-WebSite -Name "identity" -port 5001 -PhysicalPath C:\inetpub\identity -ApplicationPool "identity"
# Swap its default http binding for an https binding
Remove-WebBinding -Port 5001
New-WebBinding -Name "identity" -IP "*" -port 5001 -Protocol https
# Use the server's certificate in the binding
$identityCert = Get-ChildItem Cert:\LocalMachine\WebHosting |? { $_.Subject -like "*identity.mtls.dev*" };
New-Item -Path IIS:\\SslBindings\0.0.0.0!5001 -Value $identityCert
# Finally, start the site
Start-WebSite -Name "identity"



# Create the api site
mkdir C:\inetpub\api
New-WebAppPool -Name "api"
New-WebSite -Name "api" -port 6001 -PhysicalPath C:\inetpub\api -ApplicationPool "api"
# Swap its default http binding for an https binding
Remove-WebBinding -Port 6001
New-WebBinding -Name "api" -IP "*" -port 6001 -Protocol https
# Use the server's certificate in the binding
$apiCert = Get-ChildItem Cert:\LocalMachine\WebHosting |? { $_.Subject -like "*api.mtls.dev*" };
New-Item -Path IIS:\\SslBindings\0.0.0.0!6001 -Value $apiCert
# Finally, start the site
Start-WebSite -Name "api"



