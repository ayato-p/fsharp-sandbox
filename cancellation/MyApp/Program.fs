open System

open System.Threading
open System.Threading.Tasks
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting

type PongResponse = { Pong: bool }


[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)
    let app = builder.Build()

    app.MapGet("/", Func<string>(fun () -> "Hello World!"))
    |> ignore

    app.MapGet(
        "/cancellable1",
        Func<CancellationToken, Task<IResult>> (fun (cancellationToken) ->
            task {
                let! _ = Task.Delay(5000, cancellationToken)
                printfn "cancellable request finished!"
                return Results.Ok { Pong = true }
            })
    )
    |> ignore

    app.MapGet(
        "/cancellable2",
        Func<CancellationToken, IResult> (fun (cancellationToken) ->
            let mutable cnt = 0

            while cnt < 10 do
                Task.Delay(1000)
                |> Async.AwaitTask
                |> Async.RunSynchronously
                |> ignore

                printfn "%ds waited" cnt
                cancellationToken.ThrowIfCancellationRequested()
                cnt <- cnt + 1


            printfn "not cancellable request finished!"
            Results.Ok({ Pong = true }))
    )
    |> ignore

    app.Run()

    0 // Exit code
