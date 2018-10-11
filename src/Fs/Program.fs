namespace FsExamples

module Main =
    open Poetry
    open Hanoi

    [<EntryPoint>]
    let main argv =
        printfn "Hello World from F#!"
        Poetry.run ()
        moveTowerUnsafe 3 1 2 3
        printfn "%A" (moveTower 3)
        printfn "%s" (FruitLoop.run ())
        FizzBuzz.fizzBuzz 50
        0 
