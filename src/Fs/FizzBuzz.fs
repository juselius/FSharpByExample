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

    let show x = 
        match x with
        | Fizz x -> "fizz: " + string x
        | Buzz x -> "buzz: " + string x
        | FizzBuzz x -> "FIZZBUZZ: " + string x
        | Number -> ""

    let fizzbuzz n = 
        [0..n] 
        |> List.map (fun x ->
            match x with
            | n when n % 15 = 0 -> FizzBuzz <| string n
            | n when n % 5 = 0 -> Buzz <| float n
            | n when n % 3 = 0 -> Fizz n
            | n -> Number 
        )
        |> List.filter (
            function 
            | Number -> false
            | _ -> true
        )
        // |> List.iter (fun x -> Console.Write (x.Show ()))
        |> List.iter (show >> Console.Write)
