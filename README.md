## Table of Contents
- [Introduction](#introduction)
- [Links](#links)
- [Startup](#startup)
- [Base architecture](#base-architecture)
  - [Gateway routing](#gateway-routing)
  - [Required properties](#routing-required-properties)
  - [Authorization properties](#routing-authorization-properties)
  - [Template for routing creation](#routing-template)
- [Response template](#response-template)
  - [Custom response codes](#custom-response-codes)
- [Exception response template](#custom-exception-response)
  - [Basic response](#basic-exception-response)
  - [Validation response](#validation-exception-response)
  
<a name="introduction"></a>
## Introduction

This is backend part of the Hackathon app. Here's some info about architecture.

<a name="links"></a>
## Links

- <a href="https://docs.google.com/document/d/1yLcOgYDsvwNaAI41V1dh4oqhZ8OpsfVverin7bREaQk/edit?usp=sharing">List of API's</a>

<a name="startup"></a>
## Startup
54
After cloning a project, open Docker and type this into powershell:\
`docker-compose build` to build a solution\
`docker-compose up` to start an api\
`docker-compose down` to remove all containers. Be aware, this also will delete container with database. It's enough to press `Ctrl+C` to shutdown a containers.\
After starting up, you can get access to gateway via `http://localhost:9000`. To send requests to api use postman, for example.

<a name="base-architecture"></a>
## Base architecture

Web API is built using microservices architecture. Every API is independent.
There is main API called GatewayAPI. Gateway gets all request and provide redirecting to other microservices such as identity (authorization, etc.)
Other API's working on their own port and listening requests from gateway.

<a name="gateway-routing"></a>
### Gateway routing

All routes are listed in `Routes.json` in gateway api. Typical syntax looks like this:
```
{
  "Routes": [
    {
      "UpstreamPathTemplate": "/api/v1/user/login", // From where we get a request
      "UpstreamHttpMethod": [ "POST" ], // Allowed methods
      "DownstreamPathTemplate": "/identity/login", // Where we redirecting
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "identityapi",
          "Port": 8080
        }
      ]
    },
    {
      "UpstreamPathTemplate": "/api/v1/status", // From where we get a request
      "UpstreamHttpMethod": [ "GET" ], // Allowed methods
      "DownstreamPathTemplate": "/status", // Where we redirecting
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "statusapi",
          "Port": 8080
        }
      ],
      "AuthenticationOptions": {
        "AuthenticationProviderKey": "Bearer"
      }
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "http://localhost:9000"
  }
}
```

<a name="routing-required-properties"></a>
#### Required properties

`UpstreamPathTemplate` - incoming request from server. This means gateway will search for this route to redirecting.\
`UpstreamHttpMethod` - allowed methods for this route. This can contain all HTTP methods like `GET`, `POST` and etc.\
`DownstreamPathTemplate` - internal route to microservice's api. This is a route where our gateway will redirect the incoming request.\
`DownstreamScheme` - redirecting protocol (`http`, `https`)\
`DownstreamHostAndPorts` - array that contains hosts and ports for redirection. Since we are using docker containers, api can be accessed by their container's addresses and default 8080 port.

<a name="routing-authorization-properties"></a>
#### Authorization properties

`AuthenticationOptions` - options for authentication. This contains a method we are using for getting JWT tokens. (Default is Bearer)
> If route you are creating must be accessible only for authorized users, add following code to route:
> ```
> "AuthenticationOptions": {
>   "AuthenticationProviderKey": "Bearer"
> }
> ```
`AllowedScopes` - scopes that required for accessing this route. Scopes is an array of permissions. For example, scopes can contains something like this: `product.edit`, `product.delete`, `product.create` and etc.\
In code this looks like this:
```
"AuthenticationOptions": {
  "AuthenticationProviderKey": "Bearer",
  "AllowedScopes": [ "product.edit" ]
 }
```
`RouteClaimsRequirement` - claims that required for accessing this route. Our JWT contains some claims, such as `role`. If this route should be accessible only for `admin` role, use following code:
```
"RouteClaimsRequirement": {
  "Role": "admin"
}
```
This can be right after `AuthenticationOptions`.
So, the final routing can looks like this:
```
"AuthenticationOptions": {
  "AuthenticationProviderKey": "Bearer",
  "AllowedScopes": [ "product.delete" ]
},
"RouteClaimsRequirement": {
  "Role": "admin"
}
```

<a name="routing-template"></a>
#### Template for routing creation

Typical template for new routing should looks like this:
```
{
  "UpstreamPathTemplate": "/api/v1/status",
  "UpstreamHttpMethod": [ "GET" ],
  "DownstreamPathTemplate": "/status",
  "DownstreamScheme": "http",
  "DownstreamHostAndPorts": [
    {
      "Host": "statusapi",
      "Port": 8080
    }
  ]
}
```

If your routing should provide authorization, routing should be:
```
{
  "UpstreamPathTemplate": "/api/v1/status",
  "UpstreamHttpMethod": [ "GET" ],
  "DownstreamPathTemplate": "/status",
  "DownstreamScheme": "http",
  "DownstreamHostAndPorts": [
    {
      "Host": "statusapi",
      "Port": 8080
    }
  ],
  "AuthenticationOptions": {
  "AuthenticationProviderKey": "Bearer"
}
```
Remember to use roles and scopes if your route should be more secured.

<a name="response-template"></a>
## Response template

API's responses has 2 general templates:

```
{
  "code": response code,
  "codemessage": string representation of response code,
  "message": response message,
  "data": data object
}
```
and
```
{
  "code": response code,
  "codemessage": string representation of response code,
  "message": response message
}
```

If response does not providing this template, than server throws an unhandled exception.\
Along with HTTP codes (404, 500, 200, etc.) there are custom response codes used for clarification of response.\
Also code message contain string representation of response code for validation.

<a name="custom-response-codes"></a>
### Custom response codes

This are the codes that can be provided in API response for now:

| Code | Message | Type | Description |
| --- | --- | --- | --- |
| 200 | LoginSuccess | Success | Returning after successful user logging in |
| 201 | TokenRefreshed | Success | Returning after token been refreshed |
| 202 | TokenRevoked | Success | Returning after token been revoked |
| 203 | DataFound | Success | Returning after requested data was found |
| 204 | DataCreated | Success | Object was successfully created and saved in database |
| 205 | DataDeleted | Success | Object was succesfully deleted from database |
| 206 | DataUpdated | Success | Object was successfully updated |
| 207 | FileUploaded | Success | File was successfully uploaded |
| 400 | BadLoginRequest | Error | Returning if user provides incorrect password |
| 401 | UserNotFound | Error | Returning if API couldn't find user that accociated with request |
| 402 | BadTokenRequest | Error | Returning if token API's has bad request or incorrect data |
| 403 | ValidationError | Error | Returning if validation exception occures |
| 404 | EmptySequence | Error | Server returned an empty sequence |
| 405 | Unauthorized | Error | User authorized, but API couldn't get a user identity |
| 406 | ObjectNotFound | Error | Object couldn't be found |
| 407 | ObjectNotUpdated | Error | Object was not modified |
| 408 | NotUnique | Error | Provided data required to be unique |
| 409 | Restricted | Error | Action is prohibited by certain conditions |
| 500 | UndefinedServerException | Exception | Returning of there's unhandled exception made by server |

<a name="custom-exception-response"></a>
## Exception response template

API can handle exception while validating request data. Here's the template of exception response. It returned when data is not valid or some errors occured while handler execution.\
This response always provided with 400 status code (BadRequest), but has error code in it. Such codes are described [above](#custom-response-codes).

<a name="basic-exception-response"></a>
### Basic response

Basic exception response has following syntax:
```
{
  "code": response code,
  "codemessage": string representation of response code,
  "message": response message
}
```
`code` is error code. You can know what exact error occured by reading it.
`message` is additional information of error. We recommend not to display it to user.

<a name="validation-exception-response"></a>
### Validation response

There's another type of response, when data validation failed. It has the following structure:
```
{
  "code": 403,
  "codemessage": "ValidationError",
  "message": response message,
  "data": errors dictionary
}
```
`code` and `message` fields are the same as in the basic response. This response always has `403` code which means data validation error.\
`data` field has the dictionary of validation errors.\
Here's the example of validation error while trying user login:
```
{
    "code": 403,
    "codemessage": "ValidationError",
    "message": "Validation failed: \n -- Username: 'Username' must not be empty. Severity: Error\n -- Password: 'Password' must not be empty. Severity: Error\n -- Username: 'Username' must not be empty. Severity: Error\n -- Password: 'Password' must not be empty. Severity: Error",
    "data": {
        "Username": [
            "'Username' must not be empty."
        ],
        "Password": [
            "'Password' must not be empty."
        ]
    }
}
```
As you can see, `403` code is validation exception. And `data` field contains the dictionary of wrong request's fields.
