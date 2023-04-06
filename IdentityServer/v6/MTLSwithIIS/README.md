# mTLS Sample Hosted in IIS
This sample shows how to use host IdentityServer with mTLS in IIS.

# Setup
- Install [mkcert](https://github.com/FiloSottile/mkcert)
- Use mkcert to generate certificates
```
mkcert -install
mkcert -pkcs12 identity.mtls.dev
mkcert -pkcs12 api.mtls.dev
mkcert -pkcs12 -client client.mtls.dev
```
- Trust the root certificate authority that issued those certificates

mkcert generated a root certificate authority key and certificate, and stored
them on disk in %USERPROFILE%\AppData\Local. You can also verify where they are with `mkcert -CAROOT`. 

We're generating the server certificates for the api and identity server and the client certificate all from the same certificate authority. In a real deployment, you might have distinct CAs for the clients and servers. The issuer of the identity and api certs needs to be in the Trusted Root Certificate Authorities store, while the issuer of the client certificates needs to be in the Client Authentication Issuers store.

To do that, run the management console (mmc), add the certificate snap-in for the local computer, and make sure that the rootCA is in the Trusted Root Certification Authorities (`mkcert install` might have done this. If not, import the rootCA.pem file from `%USERPROFILE%\AppData\Local`) into the Trusted Root Certification Authorities. Then do the same in the Client Authentication Issuers.

- Add the server certificates to the Personal store for the local computer

This makes api.mtls.dev.p12 and identity.mtls.dev.p12 available in IIS.

- Create sites in IIS
  - Identity Server
    - Name: identity.mtls.dev
    - Physical Path: Full path to the IdentityServer directory 
    - Bindings: https binding for the host identity.mtls.dev on port 443
      - Disable TLS 1.3 over TCP
      
        Most browsers won't allow you to specify a certificate with TLS 1.3.
      
      - Choose the identity.mtls.dev certificate
    - Configuration Manager -> Section: system.webServer/security/access -> From: ApplicationHost.config -> Unlock Section (on the far right)
  - API
    - Name: api.mtls.dev
    - Physical Path: Full path to the Api directory 
    - Bindings: https binding for the host api.mtls.dev on port 44301
      - Choose the api.mtls.dev certificate
      - No need to disable tls 1.3 over TCP in this case, because machine to machine tls connections negotiate properly
    - Configuration Manager -> Section: system.webServer/security/access -> From: ApplicationHost.config -> Unlock Section (on the far right)


- Grant file permissions
  - Go to the IdentityServer directory in explorer
  - Right click, properties -> security tab -> Edit -> Add -> IIS AppPool\identity.mtls.dev -> Ok -> Enable Modify, Read & execute, List folder contents, Read -> Ok
  - Do the same for the IUSR account

Here identity.mtls.dev is the name of the IIS AppPool. We need modify permissions because IdentityServer sometimes writes signing keys to the filesystem.

  - Go to the Api directory in explorer
  - Right click, properties -> security tab -> Edit -> Add -> IIS AppPool\api.mtls.dev -> Ok -> Enable Read & execute, List folder contents, Read -> Ok
  - Do the same for the IUSR account

- Setup DNS
Add the following to the C:\Windows\System32\drivers\etc\hosts file:

```
127.0.0.1		identity.mtls.dev
127.0.0.1		api.mtls.dev
```

- Configure IdentityServer's client
  - Change the secret in IdentityServer\Clients.cs to either be the thumbprint or name of your client.mtls.dev certificate. 

- Configure client certificate
  - Change the thumbprint or name to match your client certificate in the GetHandler method, or copy the client's certificate to the build output, and switch to loading the certificate by filename.


# Container Builds
The IdentityServerHost and Api can be run as containerized applications within windows containers.
Here are the build steps necessary:

## Install docker
Docker desktop is a convenient way to get the docker tools you need. It can be downloaded from
https://www.docker.com/products/docker-desktop/

Alternatively, you can install docker desktop via chocolatey:
```
choco install docker-desktop
```

## Enable windows containers
As these containers will be windows containers, we need to enable the windows
containers feature in the host operating system. From an elevated powershell
command prompt, run the following:

```
Enable-WindowsOptionalFeature -Online -FeatureName $("Microsoft-Hyper-V", "Containers") -All
```

## Switch to windows containers
To switch docker to use windows containers, right click on docker desktop in the system tray and 
choose "Switch to windows containers".

## Publish the IdentityServerHost and Api Projects
From the ~/IdentityServerHost directory, run `dotnet publish`, and from the ~/Api directory, run 
`dotnet publish` again.

## Copy certificates to the ~/certificates folder
Copy the api.mtls.dev.p12, identity.mtls.dev.p12, and rootCA.pem to the ~/certificates folder.

## Build and run the container
From the ~/ directory, run 
```
docker build -t iis-mtls .
docker run --name iis-with-mtls -d -p 5001:5001 -p 6001:6001 iis-mtls
```

# Troubleshooting
## Set dns settings
If you get build failures related to failure to download files, you may need to configure DNS for docker. 
To do so in Docker Desktop, go to Settings (the gear icon in the upper right), choose the "Docker Engine" 
tab and add the following to the JSON config:

```
"dns": ["1.1.1.1"]
```