# Duende IdentityServer with ASP.NET Identity

This sample shows using ASP.NET Identity with Duende IdentityServer. 
The intent was to show the least amount of code needed to get a working sample that used Microsoft's ASP.NET Identity user management library.

The first step in creating the sample was to create a new project that used the ASP.NET Identity templates from Visual Studio ("Individual Accounts" for the authentication type). This provides all of the "out of the box" features from ASP.NET Identity for user management with only minor modifications, which are described below.

Then Duende IdentityServer was added to add OIDC/OAuth2 capabilities to the application. Only the minimal configuration was done to get Duende IdentityServer functional for this sample.

Finally another project was added which acts as a OIDC client application to exercise the OIDC login (and logout) capabilities.

The changes to the template in the ASP.NET Identity project (i.e. "IdentityServerAspNetIdentity"):

* Sqlite support was added, replacing the default of SqlServer.
* Duende IdentityServer was configured in *Startup.cs* with the necessary information about the client application, and the OIDC scopes it would be requesting.
* Debug level logging was enabled for the "Duende" prefix to allow viewing the logging emitted during request processing.
* In the middleware pipeline, *UseIdentityServer* replaced *UseAuthentication*. 
* The logout page was scaffolded to allow modification (located in Areas/Identity/Pages/Account/Logout.cshtml). The default logout page from the template is unaware of OIDC single signout, so this feature was added.

In the client application:

* A simple ASP.NET Core Razor Web Application was used as the starting point.
* In *Startup.cs* the standard cookie and OIDC authentication configuration was added.
* A secure page (*Secure.cshtml*) that required an authenticated user will render the logged in user's claim in the page.
* The index page (*Index.cshtml*) was modified to allow a POST to trigger OIDC logout. 
* A logout button was added to trigger the POST.
