module App.Handlers

open System
open System.Threading
open System.Threading.Tasks
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging

open FSharp.Control


type Pong = { pong: bool; data: int seq }

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
    doSomethingAsync ()
    |> Async.AwaitTask
    |> Async.RunSynchronously
    |> ignore

    Results.Ok { pong = true; data = seq { 1..10000 } }

let JustReturnAsyncPong () =
    async {
        let! _ = doSomethingAsync () |> Async.AwaitTask
        return Results.Ok { pong = true; data = seq { 1..10000 } }
    }

let JustReturnAsyncToTaskPong () =
    async {
        let! _ = doSomethingAsync () |> Async.AwaitTask
        return Results.Ok { pong = true; data = seq { 1..10000 } }
    }
    |> Async.StartAsTask

let JustReturnTaskPong () =
    task {
        let! _ = doSomethingAsync ()
        return Results.Ok { pong = true; data = seq { 1..10000 } }
    }

let getPong x =
    async {
        do! Async.Sleep 1000
        printfn "%d" x

        return
            { pong = true
              data = seq { 1..10 } |> List.ofSeq |> List.map ((*) 2) }
    }

let ReturnSomethingEnumerable (token: CancellationToken) =
    let mutable cnt = 0

    taskSeq {
        while cnt < 10 do
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

let private AsyncF1 () : Async<int> =
    async {
        printfn "Start AsyncF1"
        do! Async.Sleep 2000
        printfn "Finish AsyncF1"
        return 42
    }

let private AsyncF2 () : Async<string> =
    async {
        printfn "Start AsyncF2"
        do! Async.Sleep 5000
        printfn "Finish AsyncF2"
        return "Hello"
    }

let GoodComposeAsync () =
    async {
        let! n = AsyncF1() |> Async.StartChild
        let! s = AsyncF2() |> Async.StartChild
        let! n' = n
        let! s' = s
        return sprintf "%s, %d" s' n' |> Results.Ok
    }

let BadComposeAsync () =
    async {
        let! n = AsyncF1()
        let! s = AsyncF2()
        return sprintf "%s, %d" s n |> Results.Ok
    }
