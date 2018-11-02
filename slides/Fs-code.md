---
title: "F# UiT"
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

# CODE

```fsharp
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

    let fizzBuzz n =
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
        // |> List.iter (fun x -> Console.WriteLine (x.Show ()))
        |> List.iter (show >> Console.WriteLine)
```
```fsharp
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
```
```fsharp
namespace FsExamples

module Hanoi =
    let rec moveTowerUnsafe n f t o =
        if n > 0 then
            let n' = n - 1
            moveTowerUnsafe n' f o t
            printfn "Move %d from %d to %d" n f t
            moveTowerUnsafe n' o t f
        else
            ()

    let rec moveTower' n f t o r =
        if n > 0 then
            let n' = n - 1
            let r1 = moveTower' n' f o t r
            let r2 = (n, f, t) :: r1
            moveTower' n' o f t r2
        else
            r

    let moveTower n =
        moveTower' n 1 2 3 [] |> List.rev
```
```fsharp
namespace FsExamples

module Poetry =
    open System.IO

    let lines (s : string) = s.Split [|'\n'|] |> List.ofArray

    let unlines (sl : string list) =
        sl |> List.fold (fun a s -> a + s + "\n") ""

    let sortTextLines s = unlines (List.sort (lines s))
    let sortTextLines' s = s |> lines |> List.sort |> unlines
    let sortTextLines'' = lines >> List.sort >> unlines

    let sortLines s = s |> lines |> List.sort |> unlines
    let revLines s = s |> lines |> List.rev |> unlines
    let twoLines s = s |> lines |> List.take 2 |> unlines

    let byLines f s = s |> lines |> f |> unlines

    let sortLines' = byLines List.sort
    let revLines' = byLines List.rev
    let twoLines' = byLines (List.take 2)

    let indent s = "   " + s
    let indent' = (+) "   "

    let indentEachLine = byLines (List.map indent)

    let eachLine f s = s |> lines |>  List.map f |> unlines
    let eachLine' f = lines >>  List.map f >> unlines
    let eachLine'' f = byLines (List.map f)

    let indentEachLine' = eachLine indent

    let private yell (s : string) = s.ToUpper () + "!!! "

    let yellEachLine = eachLine yell

    let words (s : string) = s.Split [|' '|] |> List.ofArray

    let unwords = List.fold (+) ""

    let eachWord f = words >> List.map f >> unwords

    let yellEachWord = eachWord yell

    let eachWordOnEachLine f = eachLine (eachWord f)

    let yellEachWordOnEachLine = eachWordOnEachLine yell

    let poem = File.ReadAllText "siphonaptera.txt"
    let sortedPoem = sortLines poem

    let run () =
        printfn "%s" (revLines poem)
        printfn "%s" (poem |> revLines |> twoLines)
        printfn "%s" (poem |> indentEachLine)
        printfn "%s" (poem |> yellEachLine)
        printfn "%s" (poem |> yellEachWord)
        printfn "%s" (poem |> yellEachWordOnEachLine |> indentEachLine)
```
