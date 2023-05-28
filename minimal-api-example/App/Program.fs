open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting

type Response = { pong: bool }

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)
    let app = builder.Build()

    app.MapGet("/ping", Func<Response>(fun () -> { pong = true }))
    |> ignore

    app.Run()

    0 // Exit code
