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
