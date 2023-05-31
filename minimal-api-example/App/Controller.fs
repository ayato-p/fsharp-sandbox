// App/Controller.fs
module App.Controller

open System
open System.Threading.Tasks

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http

type Status =
    | Activated
    | Deactivated

type PongResponse = { Pong: bool }

type Article = { title: string; author: string }

let pong () = Results.Ok { Pong = true }

let createArticle (article: Article) : IResult = Results.Created("/articles/someid", ())

let showArticle ([<RouteAttribute>] id: Guid) : IResult = Results.Ok(id)
