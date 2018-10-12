namespace FsExamples

module FruitLoop =
    type Fruit = {
        apples : int
        oranges : float
        }

    type Option<'T> =
        | Some of 'T
        | None
    
    type Result<'T, 'E> =
        | Ok of 'T
        | Error of 'E

    let compare x y = 
        if x = int y then 
            Some { apples = x; oranges = y } 
        else 
            None 

    let compare' x y = 
        if float x = y then 
            Ok { apples = x; oranges = y } 
        else 
            Error "can't compare apples and oranges" 

    let compare'' x y = 
        if x = int (ceil y)  then 
            { apples = x; oranges = y } 
        else 
            failwith "potatoe tomatoe" 

    let run () =
        let a = 
            match compare 0 42.0 with
            | Some x -> string x
            | None -> "that didn't work!"
        let b = 
            let cmp = compare' 1
            match cmp -1.0 with
            | Ok x -> string x
            | Error e -> e
        printfn "%s\n%s" a b
        try
            compare'' -1 42.0 |> string
        with e -> e.Message
