namespace Toodeloo

module Client =
    open Elmish
    open Elmish.React
    open Fable.Helpers.React
    open Fable.Helpers.React.Props
    open Fable.PowerPack.Fetch
    open Thoth.Json
    open Fulma

    type Model = unit

    type Msg = unit

    let init () : Model * Cmd<Msg> = (), Cmd.none

    let update (msg : Msg) (model : Model) : Model * Cmd<Msg> = (), Cmd.none

    let view (model : Model) (dispatch : Msg -> unit) =
        h1 [] [ str "Hello world!" ]


    Program.mkProgram init update view
    |> Program.withReact "elmish-app"
    |> Program.run

