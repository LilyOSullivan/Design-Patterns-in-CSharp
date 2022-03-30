open System.Diagnostics
let doWork() =
    printfn "Doing some work"

let logger work name =
    let stopwatch = Stopwatch.StartNew()
    printfn "%s %s" "Entering function" name
    work()
    stopwatch.Stop()
    printfn "Exiting method %s: %fs elapsed" name stopwatch.Elapsed.TotalSeconds
    

[<EntryPoint>]
let main argv =
    logger doWork "do_work"
    //work()
    0
