# This script imports the certificate authorities and server certificates needed
# for mTLS.

$password = ConvertTo-SecureString changeit -AsPlainText -Force

# Trust the issuer of the server certificates
Import-Certificate -FilePath C:/certificates/rootCA.pem -CertStoreLocation Cert:\LocalMachine\Root;  

# Import the server certificates into the WebHosting store
Import-PfxCertificate -FilePath C:/certificates/identity.mtls.dev.p12 -Password $password -CertStoreLocation Cert:\LocalMachine\WebHosting; 
Import-PfxCertificate -FilePath C:/certificates/api.mtls.dev.p12 -Password $password -CertStoreLocation Cert:\LocalMachine\WebHosting; 

# Trust the issuer of the client certificates (happens to be the same issuer as server) 
#   First, add missing registry key for ClientAuthIssuer store 
New-Item -Path HKLM:\Software\Microsoft\SystemCertificates\ClientAuthIssuer; 
#   Second, import certificate 
Import-Certificate -FilePath C:/certificates/rootCA.pem -CertStoreLocation Cert:\LocalMachine\ClientAuthIssuer;
