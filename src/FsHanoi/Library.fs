namespace FsHanoi

open System

module TowerOfHanoi =
    type TowerOfHanoi = {
        numDisks : int
    }

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

module Fizz =
    let fizzbuzz n = 
        [0..n] 
        |> List.map (fun x ->
            match x with
            | n when n % 15 = 0 -> "fizzbuzz"
            | n when n % 5 = 0 -> "buzz"
            | n when n % 3 = 0 -> "fizz"
            | n -> string n
        )

module Program =
    open TowerOfHanoi
    let main () =
        let tower = { numDisks = 3 }
        moveTowerUnsafe tower.numDisks 1 2 3
