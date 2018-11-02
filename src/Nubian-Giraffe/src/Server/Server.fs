open System
open System.IO
open System.Threading.Tasks

open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection

open FSharp.Control.Tasks.V2
open Giraffe
open Shared
open Giraffe.Serialization

open Thoth.Json.Net

// types Foo and User are defined in ../Shared/Shared.fs

let user = {
    Id = 1
    Name = "Reodor Felgen"
    Email = "reodor@felgen.no"
    Foo = { Foo = 42.0; Bar = true }
}

let jsonStr = Encode.Auto.toString (0, user)

printfn "%s" jsonStr

let user' = Decode.Auto.fromString<User> jsonStr

let publicPath = Path.GetFullPath "../Client/public"
let port = 8085us

let getInitCounter () : Task<Counter> = task { return 42 }
let getInitCounter' () : Counter = 42 

let webApp =
    choose [
        GET >=> route "/api/init" >=>
            fun next ctx ->
                task {
                    let! counter  = getInitCounter () // extract 42 from a task
                    let  counter' = getInitCounter' () // 42 directly
                    return! Successful.OK counter next ctx
                }
        route "/api/user" >=>
            fun next ctx ->
                task {
                    return! Successful.OK user next ctx
                }
        routef "/api/init/%i" (
            fun x next ctx ->
                task {
                    return! Successful.OK x next ctx
                }
            )
        
    ]

let configureApp (app : IApplicationBuilder) =
    app.UseDefaultFiles()
       .UseStaticFiles()
       .UseGiraffe webApp

let configureServices (services : IServiceCollection) =
    services.AddGiraffe() |> ignore
    let fableJsonSettings = Newtonsoft.Json.JsonSerializerSettings()
    fableJsonSettings.Converters.Add(Fable.JsonConverter())
    services.AddSingleton<IJsonSerializer>(NewtonsoftJsonSerializer fableJsonSettings) |> ignore

WebHost
    .CreateDefaultBuilder()
    .UseWebRoot(publicPath)
    .UseContentRoot(publicPath)
    .Configure(Action<IApplicationBuilder> configureApp)
    .ConfigureServices(configureServices)
    .UseUrls("http://0.0.0.0:" + port.ToString() + "/")
    .Build()
    .Run()