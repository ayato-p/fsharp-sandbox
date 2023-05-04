open System
open System.Collections.Generic
open System.Threading
open System.Threading.Tasks
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting

open App

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)
    let app = builder.Build()

    app.MapGet("/ping", Func<IResult> Handlers.JustReturnPong) |> ignore

    app.MapGet("/async-ping", Func<Async<IResult>> Handlers.JustReturnAsyncPong)
    |> ignore

    app.MapGet("async-task-ping", Func<Task<IResult>> Handlers.JustReturnAsyncToTaskPong)
    |> ignore

    app.MapGet("/task-ping", Func<Task<IResult>> Handlers.JustReturnTaskPong)
    |> ignore

    app.MapGet("/stream", Func<CancellationToken, IAsyncEnumerable<Handlers.Pong>> Handlers.ReturnSomethingEnumerable)
    |> ignore

    app.MapGet("/large", Func<IResult> Handlers.ReturnLargePongs) |> ignore
    app.Run()

    0 // Exit code
