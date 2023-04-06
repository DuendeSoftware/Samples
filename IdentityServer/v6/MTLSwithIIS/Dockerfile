# escape=`
FROM mcr.microsoft.com/windows/servercore/iis

SHELL [ "powershell" ]

# Setup Remote IIS management
# This is not strictly necessary for mTLS, it is just convenient for 
# looking at the IIS configuration using IIS Manager
RUN Install-WindowsFeature Web-Mgmt-Service; `
    New-ItemProperty -Path HKLM:\software\microsoft\WebManagement\Server -Name EnableRemoteManagement -Value 1 -Force; `
    Set-Service -Name wmsvc -StartupType automatic;
# Add user for Remote IIS Manager Login
RUN net user iisadmin Password~1234 /ADD; `
    net localgroup administrators iisadmin /add;


# Download and install the .NET Hosting Bundle
RUN $download_path = \"C:\\temp\"; `
    New-Item $download_path -ItemType Directory; `
    $installer_url = \"https://download.visualstudio.microsoft.com/download/pr/e38901ef-e9ac-4331-a6aa-f2aec3b1754b/6d695fa51a4960393edaf725ce970a86/dotnet-hosting-6.0.15-win.exe\"; `
    $installer = $download_path + [System.IO.Path]::GetFileName($installer_url); `
    Try `
    { `
        Invoke-WebRequest -Uri $installer_url -OutFile $installer; `
    } `
    Catch `
    { `
        Write-Output ( $_.Exception.ToString() ); `
        Break; `
    } `
    $a = New-Object -TypeName System.Collections.Generic.List[System.String]; `
    $a.Add(\"/quiet\"); `
    $a.Add(\"/install\"); `
    $a.Add(\"/norestart\"); `
    Start-Process -FilePath $installer -ArgumentList $a -NoNewWindow -Wait -PassThru; 

# Setup DNS for identity (the api needs to invoke it)
RUN Add-Content c:\Windows\System32\Drivers\etc\hosts \"127.0.0.1    identity.mtls.dev\"

# Copy over the certificates and build scripts
COPY certificates certificates
COPY build build

WORKDIR /build

# Import certificates
RUN ./import-certificates.ps1

# Unlock the web.config
RUN ./unlock-config.ps1

# Create IIS sites and bindings
RUN ./create-websites.ps1
EXPOSE 5001
EXPOSE 6001

# Give write access to the identity site, so that we can write signing keys. In
# a production deployment, this would likely not be necessary, as the signing
# keys would be persisted elsewhere.
RUN ./grant-permissions.ps1

# Copy the published identity site
WORKDIR /inetpub/identity
COPY IdentityServer/bin/Debug/net6.0/publish .

# Copy the published api site
WORKDIR /inetpub/api
COPY Api/bin/Debug/net6.0/publish .
