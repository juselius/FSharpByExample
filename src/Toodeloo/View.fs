module Toodeloo.View

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fulma
open Toodeloo.Model

let private button txt onClick =
    Button.button [
        Button.IsFullWidth
        Button.Color IsPrimary
        Button.OnClick onClick
    ] [ str txt ]

let newEntryForm (model : Model) (dispatch : Msg -> unit) =
    let dispatch' = NewEntry >> dispatch
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
            dispatch (CreateEntry model.createForm)) ]
    ]

// Add a double click event to each editable td
// It would be better to make the whole row double clickable
let clickToEdit id txt (dispatch : Msg -> unit) = 
    td [
        OnDoubleClick (fun _ -> dispatch <| StartEdit id)
    ] [ str txt ]

let taskListView (model : Model) (dispatch : Msg -> unit) =
    let editable id txt editor =
        match model.editForm with
        | Some n when n.taskId = id -> td [] editor
        | _ -> clickToEdit id txt dispatch
    let task t = 
        editable t.taskId t.task [ Input.text [ 
            Input.DefaultValue t.task
            Input.OnChange (fun e -> 
                dispatch <| EditEntry (UpdateTask e.Value))
        ]] 
    let due t = 
        editable t.taskId (string t.due) [ Input.date [ 
            Input.DefaultValue (string t.due)
            Input.OnChange (fun e -> 
                dispatch <| EditEntry (
                    UpdateDue <| System.DateTime.Parse e.Value)
                )
        ]] 
    let pri t = 
        editable t.taskId (string t.priority) [ Input.number [ 
            Input.DefaultValue (string t.priority)
            Input.OnChange (fun e -> 
                dispatch <| EditEntry (UpdatePri <| int e.Value))
       ]] 
    let button i =
        match model.editForm with
        | Some n when n.taskId = i.taskId ->
            td [] [
                Button.button [
                    Button.Color IsSuccess
                    Button.IsOutlined
                    Button.OnClick (fun _ -> dispatch <| SaveEdit)
                ] [ str "Save" ]
                Button.button [
                    Button.Color IsWarning
                    Button.IsOutlined
                    Button.OnClick (fun _ -> dispatch <| CancelEdit)
                ] [ str "Cancel" ]
            ]
        | _ -> td [] [
            Button.button [
                Button.Color IsDanger
                Button.IsOutlined
                Button.OnClick (fun _ -> dispatch <| DeleteEntry i.taskId)
            ] [ str "X" ]
        ]
    let cols = [ "Id"; "Priority"; "Task"; "Due"; "" ]
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

let private navbar =
    Navbar.navbar [ Navbar.Color IsDark ] [
        Navbar.Item.div [ ] [
            Heading.h3
                [ Heading.Modifiers [ Modifier.TextColor IsWhite ] ]
                [ str "Toodeloo" ]
        ]
    ]

let infoPanel model dispatch =
    Panel.panel [] [
        Box.box' [ 
            Props [Style [ BackgroundColor "lightgray"] ] 
        ] [ str (string model) ]
        Content.content [] [
            Button.button [ 
                Button.Color IsDanger
                Button.OnClick (fun _ -> 
                    dispatch (NotifyError "This is an error."))
            ] [ str "Generate error" ]
        ]
    ]

let mainView (model : Model) (dispatch : Msg -> unit) elt =
    div [] [
        navbar
        errorNotifier model dispatch
        Section.section [] [
            Container.container [ Container.IsFullHD ] elt
        ]
        Footer.footer [] [
            Content.content [ Content.Modifiers [
                Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ]
            ] [ str "May the foo be with you" ]
        ]
    ]