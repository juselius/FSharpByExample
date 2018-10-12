namespace FsExamples

open System

module FizzBuzz =
    type Fizz =
        | Fizz of int
        | Buzz of float
        | FizzBuzz of string
        | Number
        member this.Show () =
            match this with
            | Fizz x -> "fizz: " + string x
            | Buzz x -> "buzz: " + string x
            | FizzBuzz x -> "FIZZBUZZ: " + string x
            | Number -> ""

    let show foo =
        match foo with
        | Fizz x -> "fizz: " + string x
        | Buzz x -> "buzz: " + string x
        | FizzBuzz x -> "FIZZBUZZ: " + string x
        | Number -> ""

    let fizzBuzz n = 
        let fizzy x =
            match x with
            | y when y % 15 = 0 -> FizzBuzz (string y)
            | y when y % 5 = 0 -> Buzz (float y)
            | y when y % 3 = 0 -> Fizz y
            | _ -> Number
        [0..n] 
        |> List.map fizzy
        |> List.filter (fun x -> 
            match x with
            | Number -> false
            | _ -> true
            )
        // |> List.map (fun x -> x.Show ())
        |> List.iter (show >> Console.WriteLine)

    let foo a b = a + b
    let foo' a = fun b -> a + b
    let foo'' = (+)

    let bar = foo 5



    [<EntryPoint>]
    let main argv =
        printfn "Hello World from F#!"
        0 // return an integer exit code
