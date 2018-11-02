---
title: "F#, Fable, Elmish"
subtitle: "Live coding"
author: [Jonas Juselius, Serit IT Partner Tromsø AS]
subject: "F#"
date: 12.10.2018
titlepage: false
tags: [functional programming, F#]
titlepage-color: 06386e
titlepage-text-color: ffffff
titlepage-rule-color: ffffff
titlepage-rule-height: 1
...

# Client.fs

```fsharp
namespace Toodeloo
module Client =
    open Elmish
    open Elmish.React
    open Fable.Helpers.React
    open Fable.Helpers.React.Props
    open Elmish.Debug
    open Elmish.HMR
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
        | ToggleInfoPane ->
            { model with showInfoPane = not model.showInfoPane }, Cmd.none

    let view model dispatch =
        mainView model dispatch [
            Box.box' [] [
                Heading.h3 [] [ str "My Toodeloo" ]
                newEntryForm model dispatch
            ]
            Box.box' [] [ taskListView model dispatch ]
            (if model.showInfoPane then
                div [
                    Props.OnClick (fun _ -> dispatch ToggleInfoPane)
                ] [ infoPane model dispatch ]
            else
                Button.button [
                    Button.OnClick (fun _ -> dispatch ToggleInfoPane)
                    Button.Props [
                        Style [Props.CSSProp.BackgroundColor "orange"]
                    ]
                ] [ str "Developer info" ]
            )
        ]

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
```

# Model.fs

```fsharp
module Toodeloo.Model

open Elmish
open System

type TaskId = int

type Todo =
    { taskId   : TaskId
      priority : int
      task     : string
      due      : DateTime
    }

type Model =
    { entries    : Map<int, Todo>
      createForm : Todo
      editForm   : Todo option
      errorMsg   : string option
      currentId  : int
      showInfoPane : bool
    }

// local submodule
module Defaults =
    let defaultTodo =
        { taskId = 0
          priority = 0
          task = ""
          due = DateTime.Today.AddDays 1.0
        }

    let defaultModel = {
        entries = Map.empty
        createForm = defaultTodo
        errorMsg = None
        editForm = None
        currentId = 0
        showInfoPane = false
        }


// Example how to split Msg into submessages
type UpdateEntryMsg =
| UpdatePri of int
| UpdateTask of string
| UpdateDue of System.DateTime

type NewEntryMsg = UpdateEntryMsg

type EditEntryMsg = UpdateEntryMsg

type Msg =
| NewEntry of NewEntryMsg
| SaveEntry of Todo
| DeleteEntry of int
| StartEdit of int
| EditEntry of EditEntryMsg
| SaveEdit
| CancelEdit
| NotifyError of string
| ClearError
| ToggleInfoPane

let private notifyErr e = Cmd.ofMsg (Msg.NotifyError e)

let handleNewEntry (msg : NewEntryMsg) (model : Model) =
    let entry =
        match msg with
        | UpdatePri y -> { model.createForm with priority = y }
        | UpdateDue y ->  { model.createForm with due = y }
        | UpdateTask y -> { model.createForm with task = y }
    { model with createForm = entry }, Cmd.none

let saveEntry (x : Todo) (model : Model) =
    let newId = model.currentId + 1
    // Example validation, perform any kind of validation here and return
    match x.taskId with
    | 0 ->
        let todo' = model.entries |> Map.add newId { x with taskId = newId }
        let model' = {
            model with
                entries = todo'
                currentId = newId
                createForm = Defaults.defaultTodo
            }
        model', Cmd.none
    | _ -> model, notifyErr "What, what?"

let deleteEntry (x : int) (model : Model) =
    let model' = { model with entries = Map.remove x model.entries }
    model', Cmd.none

let private updateEntry (msg : EditEntryMsg) (entry : Todo) =
    match msg with
    | UpdateTask t -> { entry with task = t}
    | UpdatePri p -> { entry with priority = p}
    | UpdateDue d -> { entry with due = d}

let startEdit id model =
    match Map.tryFind id model.entries with
    | Some entry ->
        let model' = { model with editForm = Some entry }
        model', Cmd.none
    | None -> model, notifyErr <| "TaskId not found, " + string id

let handleEditEntry (msg : UpdateEntryMsg) (model : Model) =
    match model.editForm with
    | Some entry ->
        let entry' = updateEntry msg entry
        let model' = { model with editForm = Some entry' }
        model', Cmd.none
    | None -> model, notifyErr "Error in error message"

let saveEdit model =
    match model.editForm with
    Some entry ->
        let model' = {
            model with
                entries = Map.add entry.taskId entry model.entries
                editForm = None
            }
        model', Cmd.none
    | None -> model, notifyErr "Error in error message"

let cancelEdit model =
    let model' = { model with editForm = None }
    model', Cmd.none
```

# View.fs

```fsharp
module Toodeloo.View
open System
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fulma
open Toodeloo.Model
open Fable.Import

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
            dispatch' (UpdateDue (DateTime.Parse e.Value)))
          model.createForm.due.ToString "yyyy-MM-dd" |> Input.Value
          ]
        ]
        Field.div [] [ Label.label [] [ str "" ] ]
        Control.div [] [ button "Add entry" (fun _ ->
            dispatch (SaveEntry model.createForm)) ]
    ]

// Add a double click event to each editable td
// It would be better to make the whole row double clickable
let clickToEdit id txt (dispatch : Msg -> unit) =
    td [
        OnDoubleClick (fun _ -> dispatch <| StartEdit id)
    ] [ str txt ]

let styleIt (t : Todo) =
    let c =
        match t with
        | _ when t.due.Date <= DateTime.Now.Date -> "fuchsia"
        | _ when t.priority >= 10 && t.priority < 50 -> "limegreen"
        | _ when t.priority >= 50 && t.priority < 100 -> "gold"
        | _ when t.priority >= 100  -> "crimson"
        | _ -> ""
    Style [ BackgroundColor c ]


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
    let due model t =
        let duedate =
            match model.editForm with
            | Some x -> x.due
            | None ->  t.due
            |> fun x -> x.ToString "yyyy-MM-dd"
        editable t.taskId duedate [ Input.date [
            duedate |> Input.Value
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
                    td [ styleIt t ] [ str (string t.taskId) ]
                    pri t
                    task t
                    due model t
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

let infoPane model dispatch =
    Panel.panel [] [
        Box.box' [
            Props [Style [ BackgroundColor "lightgray"] ]
        ] [ str (string model) ]
        Content.content [] [
            Button.button [
                Button.Color IsDanger
                Button.OnClick (fun _ ->
                    dispatch (NotifyError "Error in error messgae."))
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
                Modifier.TextAlignment (Screen.All, TextAlignment.Centered)
                Modifier.TextSize (Screen.All, TextSize.Is4)
                ]
            ] [ str "May the foo be with you" ]
        ]
    ]
```
