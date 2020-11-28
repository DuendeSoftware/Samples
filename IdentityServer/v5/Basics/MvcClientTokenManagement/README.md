# MVC Client with automatic Access Token Management

This sample shows how to use [IdentityModel.AspNetCore](https://identitymodel.readthedocs.io/en/latest/aspnetcore/overview.html) to automatically manage access tokens.

The sample uses a special client ID in the sample IdentityServer with a short token lifetime (75 seconds). When repeating the API call, make sure you inspect the returnd *iat* and *exp* claims to observer how the token is slides.

You can also turn on debug tracing to get more insights in the token management library.

