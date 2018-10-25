module Client

open Elmish
open Elmish.React
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fulma

type Model = {
    text : string
    showModel : bool
}

type Msg = 
    | ToggleShowModel
    | UpdateText of string

let init () : Model * Cmd<Msg> = 
    let model = { 
        text = ""
        showModel = true 
        }
    model, Cmd.none

let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
    match msg with
    | ToggleShowModel -> 
        let model' = { model with showModel = not model.showModel }    
        model', Cmd.none
    | UpdateText s ->
        let model' = { model with text = s }    
        model', Cmd.none

let navbar = 
    Navbar.navbar [
       Navbar.Color IsDark 
    ] [
        Navbar.Item.div [] [
            Heading.h3 [
                Heading.Modifiers [ Modifier.TextColor IsWhite ]
            ] [ str "Silly demo" ]
        ]
    ]

let boxedSection model h e =
    Section.section [] [
        Heading.h3 [] [ str h ]
        Box.box' (
            match model.text with
            | "green" -> [ Props [Style [BackgroundColor "green"] ] ]
            | _ -> []
        ) [
            Content.content [] e
        ]
    ]

let sillyLoop n = 
    p [] [
        ul [] (
            [1..n] |> List.map (fun x -> 
                li [] [ str (string i) ]
            )
        )
    ]

let view (model : Model) (dispatch : Msg -> unit) =
    div [] [
        navbar
        boxedSection model "hello world" [
            Button.button [
                Button.Color IsDanger
                Button.OnClick (fun _ -> dispatch ToggleShowModel)
            ] [ str "Show model" ]
            (if model.showModel then
                p [] [ str (string model) ]
            else 
                p [] [ str "Nothing to see here..." ]
            )
        ]
        boxedSection model "hello interactive world" [
            Field.div [] [ Label.label [] [ str "Task" ] ]
            Control.div [] [ Input.text [
                Input.OnChange (fun e -> dispatch (UpdateText e.Value))
                Input.Placeholder "Text" 
                Input.Value model.text
                ] 
           ]
        ]
    ]


#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
