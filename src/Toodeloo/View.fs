module Toodeloo.View

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack.Fetch
open Fulma
open Toodeloo.Model

let navbar =
    Navbar.navbar [ Navbar.Color IsDark ] [
        Navbar.Item.div [ ] [
            Heading.h3
                [ Heading.Modifiers [ Modifier.TextColor IsWhite ] ]
                [ str "Toodeloo" ]
        ]
    ]

let button txt onClick =
    Button.button [
        Button.IsFullWidth
        Button.Color IsPrimary
        Button.OnClick onClick
    ] [ str txt ]

let formAddTask (model : Model) (dispatch : Msg -> unit) =
    let dispatch' = CreateFormMsg >> dispatch
    p [] [
        Field.div [] [ Label.label [] [ str "Task" ] ]
        Control.div [] [ Input.text [
          Input.OnChange (fun e -> dispatch' (UpdateTask e.Value))
          Input.Placeholder "Todo" 
          Input.Value model.createForm.task
          ] 
        ]
        Field.div [] [ Label.label [] [ str "Priority" ] ]
        Control.div [] [ Input.number [
          Input.OnChange (fun e -> dispatch' (UpdatePri (int e.Value)))
          Input.Placeholder "1" 
          Input.Value (string model.createForm.priority)
          ] 
        ]
        Field.div [] [ Label.label [] [ str "Due" ] ]
        Control.div [] [ Input.date [
          Input.OnChange (fun e -> 
            dispatch' (UpdateDue (System.DateTime.Parse e.Value)))
          ] 
        ]
        Field.div [] [ Label.label [] [ str "" ] ]
        Control.div [] [ button "Add entry" (fun _ -> 
            dispatch (Create model.createForm)) ]
    ]

// Add a double click event to each editable td
// It would be better to make the whole row double clickable
let clickToEdit id txt (dispatch : Msg -> unit) = 
    td [
        OnDoubleClick (fun _ -> dispatch <| StartEdit id)
    ] [ str txt ]

let mkEntryTable (model : Model) (dispatch : Msg -> unit) =
    let editable id txt editor =
        match model.editing with
        | Some n when n = id -> td [] editor
        | _ -> clickToEdit id txt dispatch
    let task t = 
        editable t.taskId t.task [ Input.text [ 
            Input.DefaultValue t.task
            Input.OnChange (fun e -> 
                dispatch <| Update (UpdateTask e.Value))
        ]] 
    let due t = 
        editable t.taskId (string t.due) [ Input.date [ 
            Input.DefaultValue (string t.due)
            Input.OnChange (fun e -> 
                dispatch <| Update (
                    UpdateDue <| System.DateTime.Parse e.Value)
                )
        ]] 
    let pri t = 
        editable t.taskId (string t.priority) [ Input.number [ 
            Input.DefaultValue (string t.priority)
            Input.OnChange (fun e -> 
                dispatch <| Update (UpdatePri <| int e.Value))
       ]] 
    let button i =
        match model.editing with
        | Some n when n = i.taskId ->
            td [] [
                Button.button [
                    Button.Color IsSuccess
                    Button.IsOutlined
                    Button.OnClick (fun _ -> dispatch <| SaveEdit)
                ] [ str "Save" ]
            ]
        | _ -> td [] [
            Button.button [
                Button.Color IsDanger
                Button.IsOutlined
                Button.OnClick (fun _ -> dispatch <| Delete i.taskId)
            ] [ str "X" ]
        ]
    let cols = [ "Id"; "Priority"; "Task"; "Due"; "Delete" ]
    Table.table [] [
        thead [] [
            for i in cols do yield td [] [str i]
        ]
        tbody [] [
            for p in model.entries do
                let t = p.Value
                yield tr [] [
                    td [] [ str (string t.taskId) ]
                    pri t
                    task t
                    due t
                    button t
                ]
          ]
      ]

let errorNotifier (model : Model) (dispatch : Msg -> unit) =
    match model.errorMsg with
    | Some err ->
          Notification.notification [ Notification.Color IsDanger ] [
              Notification.delete [ GenericOption.Props
                [ OnClick (fun _ -> dispatch ClearError)] ] []
              str err
           ]
    | None -> div [] []

let mainView (model : Model) (dispatch : Msg -> unit) elt =
    div [] [
        navbar
        errorNotifier model dispatch
        Container.container [] elt
        Footer.footer [] [
            Content.content [ Content.Modifiers [
                Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ]
            ] [ str "May the foo be with yougg" ]
        ]
    ]