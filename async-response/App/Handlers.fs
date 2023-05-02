module App.Handlers

open System.Threading.Tasks
open Microsoft.AspNetCore.Http

open FSharp.Control

type Pong = { pong: bool }

let JustReturnPong () = Results.Ok { pong = true }

let JustReturnAsyncPong () =
    async { return Results.Ok { pong = true } }

let JustReturnAsyncToTaskPong () =
    async { return Results.Ok { pong = true } } |> Async.StartAsTask

let JustReturnTaskPong () =
    task { return Results.Ok { pong = true } }

let ReturnSomethingEnumerable () =

    taskSeq {
        yield { pong = true }
        let! _ = Task.Delay(5000)
        yield { pong = true }
        let! _ = Task.Delay(5000)
        yield { pong = true }
    }
