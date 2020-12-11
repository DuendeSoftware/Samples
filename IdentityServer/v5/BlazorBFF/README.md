# Blazor WASM with BFF

This sample shows our preferred approach for securing Blazor-based applications.

Instead of driving the the authentication and token request workflow from the client, we use the server-side OpenID Connect handler and session management. This allows for the very important security requirement of not having to store access or refresh tokens in the browser.

Things to note:

* This is very close to the standard Blazor template with added
  * Cookie & OpenID Connect handler
  * A global authorization policy to require an authenticated user
  * An added `User` property on the weather DTO to demonstrate that the API call is authenticated and has access to the user data
* The front-end to back-end communication is secured by SameSite cookies
* The IdentityServer is using the standard confidential client + authorization code flow + PKCE setup (which is the recommended configuration from a security point of view)
  * the demo server can be found at https://demo.duendesoftware.com