module Toodeloo.Model

open Elmish
open System

type TaskId = int

type Todo = 
    { taskId : TaskId
      priority : int
      task : string
      due : DateTime option
    }

type Model =
    { entries    : Map<int, Todo>
      createForm : Todo
      editing    : int option
      errorMsg   : string option
      currentId  : int
    }

module Defaults =
    let defaultTodo = 
        { taskId = 0
          priority = 0
          task = ""
          due = None
        }

    let defaultModel = {
        entries = Map.empty
        createForm = defaultTodo
        errorMsg = None
        editing = None
        currentId = 0
        }


// Example how to split Msg into submessages
type UpdateEntryMsg =
| UpdatePri of int
| UpdateTask of string
| UpdateDue of System.DateTime

type Msg =
| Create of Todo
| CreateFormMsg of UpdateEntryMsg
| Delete of int
| Update of UpdateEntryMsg 
| NotifyError of string
| ClearError
| StartEdit of int
| SaveEdit 
| CancelEdit 

let notifyExn s (e : exn) = (s + ": " + e.Message) |> Msg.NotifyError

let notifyErr e = Cmd.ofMsg (Msg.NotifyError e)

let createEntry (x : Todo) (model : Model) =
    let todo = model.entries
    let newId = model.currentId
    // Example validation, perform any kind of validation here and return
    match x.taskId with
    | 0 -> 
        let todo' = todo |> Map.add newId { x with taskId = newId } 
        let model' = { 
            model with 
                entries = todo' 
                currentId = newId
                createForm = Defaults.defaultTodo
            }
        model', Cmd.none
    | _ -> model, notifyErr "What, what?"

let deleteEntry (x : int) (model : Model) =
    let model' = { 
            model with entries = Map.remove x model.entries 
        }
    model', Cmd.none

let editEntry (msg : UpdateEntryMsg) (model : Model) =
    let entry = Map.find model.editing.Value model.entries
    match msg with
    | UpdateTask t -> { entry with task = t}
    | UpdatePri p -> { entry with priority = p}
    | UpdateDue d -> { entry with due = Some d}

let updateEntry (msg : UpdateEntryMsg) (model : Model) =
    let entry = editEntry msg model
    let model' = { 
        model with 
            entries = Map.add entry.taskId entry model.entries
        }
    model', Cmd.none

let startEdit id model =
    let model' = { 
        model with 
            editing = Some id 
        }
    model', Cmd.none

let saveEntry model =
    let model' = {
        model with 
            editing = None 
        }
    model', Cmd.none

let handleCreateForm (msg : UpdateEntryMsg) (model : Model) =
    let entry = 
        match msg with
        | UpdatePri y -> { model.createForm with priority = y }
        | UpdateDue y ->  { model.createForm with due = Some y }
        | UpdateTask y -> { model.createForm with task = y }
    { model with createForm = entry }, Cmd.none
let cancelEdit model =
    let model' = { model with editing = None }
    model', Cmd.none