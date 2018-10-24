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
        | Create y -> createEntry y model 
        | Update msg -> updateEntry msg model 
        | Delete y -> deleteEntry y model 
        | NotifyError err -> { model with errorMsg = Some err }, Cmd.none
        | CreateFormMsg msg -> handleCreateForm msg model
        | ClearError -> { model with errorMsg = None }, Cmd.none
        | StartEdit id -> startEdit id model 
        | SaveEdit -> saveEntry model
        | CancelEdit -> cancelEdit model 

    let view (model : Model) (dispatch : Msg -> unit) =
        mainView model dispatch [
            Box.box' [           ] [
                Heading.h3 [] [ str "My Toodeloo" ]
                formAddTask model dispatch
            ]
            Box.box' [] [ mkEntryTable model dispatch ]
            Content.content [] [
                Button.button [ 
                    Button.Color IsDanger
                    Button.OnClick (fun _ -> 
                        dispatch (NotifyError "This is an error."))
                ] [ str "Generate error" ]
            ]
            Box.box' [] [ str (string model) ]
        ]


    Program.mkProgram init update view
    |> Program.withReact "elmish-app"
    |> Program.run