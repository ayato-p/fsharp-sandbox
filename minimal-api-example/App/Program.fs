// App/Program.fs
open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Hosting

open App

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)
    let app = builder.Build()

    app.MapGet("/ping", new Func<IResult>(Controller.pong))
    |> ignore

    app.MapGet("/answer", Func<int>(fun () -> 42))
    |> ignore

    let articles = app.MapGroup("/articles")

    articles.MapPost("", Func<Controller.Article, IResult> Controller.createArticle)
    |> ignore

    articles.MapGet("/{id}", Func<Guid, IResult>(fun (id) -> Controller.showArticle id))
    |> ignore

    articles.MapGet("/s/{id}", Func<Guid, IResult> Controller.showArticle)
    |> ignore

    app.Run()

    0 // Exit code
