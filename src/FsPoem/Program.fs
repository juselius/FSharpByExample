namespace FsPoem

module Main =
    open Poetry

    [<EntryPoint>]
    let main argv =
        printfn "Hello World from F#!"
        Poetry.run ()
        0 
