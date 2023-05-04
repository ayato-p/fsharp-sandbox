module App.Handlers

open System.Threading
open System.Threading.Tasks
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging

open FSharp.Control


type Pong = { pong: bool }

let doSomethingAsync () =
    task {
        printfn "Start doSomethingAsync"
        let t = Task.Delay(5000)
        printfn "blah, blah, blah..."
        let! _ = t
        printfn "Done doSomethingAsync"
        return 0
    }

let JustReturnPong () =
    doSomethingAsync () |> Async.AwaitTask |> Async.RunSynchronously |> ignore
    Results.Ok { pong = true }

let JustReturnAsyncPong () =
    async {
        let! _ = doSomethingAsync () |> Async.AwaitTask
        return Results.Ok { pong = true }
    }

let JustReturnAsyncToTaskPong () =
    async {
        let! _ = doSomethingAsync () |> Async.AwaitTask
        return Results.Ok { pong = true }
    }
    |> Async.StartAsTask

let JustReturnTaskPong () =
    task {
        let! _ = doSomethingAsync ()
        return Results.Ok { pong = true }
    }

let getPong x =
    async {
        let! _ = Task.Delay(100) |> Async.AwaitTask
        printfn "%d" x
        return { pong = true }
    }

let ReturnSomethingEnumerable (token: CancellationToken) =
    let mutable cnt = 0

    taskSeq {
        while cnt < 100 do
            cnt <- cnt + 1
            let! result = getPong cnt |> Async.StartAsTask
            yield result
    }

let ReturnLargePongs () =
    seq { 1..100 }
    |> Seq.map getPong
    |> Async.Parallel
    |> Async.RunSynchronously
    |> Seq.ofArray
    |> Results.Ok
