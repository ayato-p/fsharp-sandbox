open System
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
        "/cancellableRequest",
        Func<Task<IResult>> (fun () ->
            task {
                let! _ = Task.Delay(5000)
                printfn "waited!"
                return Results.Ok { Pong = true }
            })
    )
    |> ignore

    app.MapGet(
        "/notCancellableRequest",
        Func<IResult> (fun () ->
            Task.Delay(5000)
            |> Async.AwaitTask
            |> Async.RunSynchronously
            |> ignore

            printfn "waited!"
            Results.Ok({ Pong = true }))
    )
    |> ignore

    app.Run()

    0 // Exit code
