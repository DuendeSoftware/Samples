# Introspection Sample

This sample shows how to use the reference tokens instead of JWTs.

Things of interest

* the client registration uses *AccessTokenType* of value *Reference*
* the client requests *scope2* - this scope is part of an API resource.
  * API resources allow defining API secrets, which can then be used to access the introspection endpoint
* The API supports both JWT and reference tokens, this is achieved by forwarding the token to the right handler at runtime

See the [reference token](https://docs.duendesoftware.com/identityserver/v5/tokens/reference/) documentation for an overview of that scenario.

