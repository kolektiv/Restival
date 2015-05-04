module Restival.Api.Freya

open System.Text
open Chiron
open Freya.Core
open Freya.Machine
open Freya.Machine.Extensions.Http
open Freya.Machine.Router
open Freya.Router
open Freya.Types.Http
open Freya.Types.Uri.Template

// Types

type Greeting =
    { Message: string }

    static member ToJson (x: Greeting) =
        Json.write "message" x.Message

// Encoding

let inline encode resource =
    resource
    |> Json.serialize
    |> Json.format
    |> Encoding.UTF8.GetBytes

// Resources

let greeting =
    freya {
        let! name = Freya.getLensPartial (Route.atom "name")

        match name with
        | Some name -> return { Message = sprintf "Hello %s!" name }
        | _ -> return { Message = "Hello World!" } }

let greetingOk _ =
    freya {
        let! data = greeting

        return {
            Data = encode data
            Description =
                { Charset = Some Charset.Utf8
                  Encodings = None
                  MediaType = Some MediaType.Json
                  Languages = None } } }

let greetingResource =
    freyaMachine {
        using http
        handleOk greetingOk } |> FreyaMachine.toPipeline

// Routes

let root =
    UriTemplate.Parse "/hello"

let routes =
    freyaRouter {
        resource (root) greetingResource
        resource (root + UriTemplate.Parse "/{name}") greetingResource } |> FreyaRouter.toPipeline

// App

let app =
    OwinAppFunc.ofFreya routes

// Startup (TODO)

type Hello () =
    member __.X = "F#"