namespace Day2

module Main =

    type MyString = string

    type Fruit = {
        orange : int
        apple : float
    }

    type Option<'T> =
        | Some of 'T
        | None

    type Result<'T, 'E> =
        | Ok of 'T
        | Error of 'E

    let compare' x y =
        if float x = y then
            Ok { orange = x; apple = y }
        else
            Error "aaaaaaa"

    let test () =
        match compare' 2 1.0 with
        | Ok x -> printfn "%A" x
        | Error e -> printfn "%s" e

    let compare x y =
        if float x = y then
            { orange = x; apple = y }
        else
            failwith "can't compare apples and oranges"

    let main argv =
        printfn "Hello World from F#!"
        0 // return an integer exit code

module Poetry =
    open System.IO

    let poem =
        try
          File.ReadAllText "siphonaptera.txt"
        with e ->
            printfn "%A" e.Message
            ""

    let lines (s: string) = s.Split [|'\n' |] |> List.ofArray

    let unlines (sl: string list) =
        sl |> List.fold (fun a x -> a + x + "\n") ""

    let sortLines s = s |> lines |> List.sort |> unlines
    let revLines s = s |> lines |> List.rev |> unlines
    let twoLines s = s |> lines |> List.take 2 |> unlines

    let byLines f s = s |> lines |> f |> unlines
    let byLines' f = lines >> f >> unlines


    let sortLines' s = byLines List.sort
    let revLines' s = byLines List.rev
    let twoLines' s = byLines (List.take 2)
