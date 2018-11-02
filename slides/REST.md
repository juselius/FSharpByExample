name: inverse
layout: true
class: center, middle, inverse
---
# How to rest
## <jonas.juselius@itpartner.no>
### https://github.com/juselius
### https://twitter.com/copointfree

---
layout: false

## Giraffe

* Haskell: [Yesod](https://www.yesodweb.com/)
* F#: [Suave](https://suave.io)
* ASP.NET Core: [Giraffe](https://github.com/giraffe-fsharp/Giraffe)

#### Scaffold
```fsharp
dotnet new -i safe.template
dotnet new safe -n Nubian -o Nubian -s giraffe
cd Nubian
mono .paket/paket.exe update
yarn upgrade
cd src
```

#### Build
```fsharp
cd Server
dotnet run
cd Client
dotnet restore
dotnet fable webpack-dev-server -- --config $(pwd)/webpack.config.js
```

---
.left-column[
## Json
### Serializaton
]
.right-column[
* Current recommendation: [Thoth.Json](https://mangelmaxime.github.io/Thoth/json)
* Auto encode/decode vs. manual: safety and flexibility vs. ease of use

```fsharp
type User = {
    Id : int
    Name : string
    Email : string
    }

let user = {
    Id = 0
    Name = "Matti Meikäläinen"
    Email = "matti@putki.fi"
    }

let json = Encode.Auto.toString(4, user)
```
]

---
.left-column[
## Json
### Serializaton
### Deserializaton
]
.right-column[
```fsharp
type User = {
    Id : int
    Name : string
    Email : string
    }

Decode.Auto.fromString<User>(json)
```
]

---
## Dealing with HTTP

* Most HTTP libraries/frameworks are an almost impentetrable complex mass,
  which is really hard to reason about
* ASP.NET Core is a very complex behemoth relying heavily on *middleware*,
  *dependency injection* and *extension methods* (i.e. I have not the faintest
  clue how it works)
### Giraffe
Giraffe is an ASP.NET Core middleware which allows us to escape into the cozy
world of functional programming.

---
.left-column[
## HTTP
### Types
]
.right-colum[
```fsharp
type HttpContext = ...

type HttpFunc = HttpContext -> Task<HttpContext option>

type HttpFuncResult = Task<HttpContext option>

type HttpHandler = (HttpContext -> HttpFuncResult)
                     -> HttpContext -> HttpFuncResult

type HttpHandler = HttpFunc -> HttpFunc

val (>=>) : HttpHandler -> HttpHandler -> HttpHandler
```
]

---
.left-column[
## HTTP
### Types
### Handlers
]
.right-colum[
```fsharp
val route   = (path : string)      -> HttpHandler
val setBody = (bytes : byte array) -> HttpHandler
val choose  = HttpHandler list     -> HttpHandler
val setBodyString = string         -> HttpHandler
...
```
]

---
.left-column[
## HTTP
### Types
### Handlers
### Predicates
]
.right-colum[
```fsharp
let GET     : HttpHandler = httpVerb HttpMethods.IsGet
let POST    : HttpHandler = httpVerb HttpMethods.IsPost
let PUT     : HttpHandler = httpVerb HttpMethods.IsPut
let PATCH   : HttpHandler = httpVerb HttpMethods.IsPatch
let DELETE  : HttpHandler = httpVerb HttpMethods.IsDelete
let HEAD    : HttpHandler = httpVerb HttpMethods.IsHead
let OPTIONS : HttpHandler = httpVerb HttpMethods.IsOptions
let TRACE   : HttpHandler = httpVerb HttpMethods.IsTrace
let CONNECT : HttpHandler = httpVerb HttpMethods.IsConnect
```
]

---
.left-column[
## HTTP
### Types
### Handlers
### Predicates
### Result
]
.right-colum[
```fsharp
let ok x          = setStatusCode 200 >=> x
let notFound x    = setStatusCode 404 >=> x
let unauthorized scheme realm x =
    setStatusCode 401
    >=> setHttpHeader "WWW-Authenticate"
          (sprintf "%s realm=\"%s\"" scheme realm)
    >=> x
...
```
]

