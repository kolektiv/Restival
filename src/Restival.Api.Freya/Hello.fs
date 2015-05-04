module Restival.Api.Freya

open System.Text
open Chiron
open Freya.Core
open Freya.Core.Operators
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

// Properties

let greeting =
        fun name ->
            match name with
            | Some name -> { Message = sprintf "Hello %s!" name }
            | _ -> { Message = "Hello World!" }
    <!> Freya.getLensPartial (Route.atom "name")

// Resources

let greetingOk _ =
        fun greeting ->
            { Data = encode greeting
              Description =
                { Charset = Some Charset.Utf8
                  Encodings = None
                  MediaType = Some MediaType.Json
                  Languages = None } }
    <!> greeting

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
