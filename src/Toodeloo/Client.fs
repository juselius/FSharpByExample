namespace Toodeloo

module Client =
    open Elmish
    open Elmish.React
    open Fable.Helpers.React
    open Fable.Helpers.React.Props
    open Fable.PowerPack.Fetch
    open Thoth.Json
    open Fulma

    open Toodeloo.Model
    open Toodeloo.View

    let init () : Model * Cmd<Msg> =
        Defaults.defaultModel, Cmd.none

    let update (msg : Msg) (model : Model) : Model * Cmd<Msg> = 
        match msg with
        | NewEntry msg -> handleNewEntry msg model
        | SaveEntry n -> saveEntry n model 
        | EditEntry msg -> handleEditEntry msg model 
        | StartEdit id -> startEdit id model 
        | SaveEdit -> saveEdit model
        | CancelEdit -> cancelEdit model 
        | DeleteEntry n -> deleteEntry n model 
        | NotifyError err -> { model with errorMsg = Some err }, Cmd.none
        | ClearError -> { model with errorMsg = None }, Cmd.none

    let view model dispatch =
        mainView model dispatch [
            Box.box' [] [
                Heading.h3 [] [ str "My Toodeloo" ]
                newEntryForm model dispatch
            ]
            Box.box' [] [ taskListView model dispatch ]
            infoPanel model dispatch
        ]


    Program.mkProgram init update view
    |> Program.withReact "elmish-app"
    |> Program.run