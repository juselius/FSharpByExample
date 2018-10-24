module FableApp

open Fable.Core
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Elmish
open Elmish.React


type Model = unit

type Msg = unit

let init () : Model = ()

let update (msg : Msg) (model : Model) : Model = () 

let view (model : Model) (dispatch : Msg -> unit) =
    div [] [
        h1 [ Props.Class "foo" ] [ str "Hello world!" ]
        p [] [ str "goodbye!"]
    ]

Program.mkSimple init update view
|> Program.withReact "elmish-app"
|> Program.run