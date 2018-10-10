#r "paket: groupref netcorebuild //"
open Fake.Tools.Git
#load ".fake/build.fsx/intellisense.fsx"
#if !FAKE
#r "Facades/netstandard"
#r "netstandard"
#endif

#nowarn "52"

open System
open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.IO.FileSystemOperators
open Fake.Tools.Git
open Fake.JavaScript

let prefix = (+) (__SOURCE_DIRECTORY__  + "/")

Target.create "Clean" (fun _ ->
    !! "*/**/bin"
    ++ "*/**/obj"
    ++ "*/webroot"
    |> Seq.iter Shell.cleanDir
)

Target.create "DotnetRestore" (fun _ ->
    DotNet.restore
        (DotNet.Options.withWorkingDirectory __SOURCE_DIRECTORY__)
        "FsByExample.sln"
)

Target.create "Build" (fun _ ->
    DotNet.build
        (DotNet.Options.withWorkingDirectory __SOURCE_DIRECTORY__)
        "FsByExample.sln"
)

"DotnetRestore"
    ==> "Build"

Target.runOrDefault "Build"
