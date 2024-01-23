# mTLS Sample

This sample shows how to use mTLS to bind tokens to a client. To run, follow these steps:

1. Configure DNS. Add the configuration below to your hosts file. On windows, this is the file
   C:\Windows\System32\drivers\etc\hosts. 
```
# Used by Duende mTLS sample
127.0.0.1	mtls.localhost
127.0.0.1	api.localhost
```
2. Install certificates. The mTLS connection requires both client and server to
   authenticate using an SSL certificate. This solution contains demo certificates that
   will be used. In order to trust those certificates, you must trust the signing
   certificates. To do that, import Api\api.localhost.cer and mtls.localhost.cert as
   Trusted Root Certification Authorities. On windows, double click those files -> Install
   Certificate -> Choose Current User -> Next -> Choose to Place certificates in a chosen
   store -> Browse to Trusted Root Certification Authorities -> Next -> Finish. Each .cer
   file needs to be trusted separately.
3. Start up the IdentityServer and API projects.
4. Run the ClientCredentials project.

After you are finished with the sample, you may wish to remove the certificates from your
trusted root authority.