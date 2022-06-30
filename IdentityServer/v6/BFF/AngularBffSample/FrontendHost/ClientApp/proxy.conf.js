const { env } = require("process");

const target = env.ASPNETCORE_HTTPS_PORT
  ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
  : env.ASPNETCORE_URLS
  ? env.ASPNETCORE_URLS.split(";")[0]
  : "http://localhost:5266";

const PROXY_CONFIG = [
  {
    context: [
      "/favicon.ico",
      "/todos",
      "/bff",
      "/signin-oidc",
      "/signout-callback-oidc",
    ],
    target: target,
    secure: false,
    headers: {
      Connection: "Keep-Alive",
      // This can also be done with an Angular interceptor
      "X-CSRF": 1,
    },
  },
];

module.exports = PROXY_CONFIG;
