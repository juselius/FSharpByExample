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
        if x >= 0 && y > 0.0 then 
            Some { apples = x; oranges = y } 
        else 
            None 

    let compare' x y = 
        if x >= 0 && y > 0.0 then 
            Ok { apples = x; oranges = y } 
        else 
            Error "can't compare apples and oranges" 

    let compare'' x y = 
        if x >= 0 && y > 0.0 then 
            { apples = x; oranges = y } 
        else 
            failwith "can't compare apples and oranges" 


    let run () =
        let a = 
            match compare 0 42.0 with
            | Some x -> string x
            | None -> "that doesn't work"
        let b = 
            let cmp = compare' 1
            match cmp -1.0 with
            | Ok x -> string x
            | Error e -> e
        printfn "%s\n%s" a b
        compare'' -1 42.0 |> string

