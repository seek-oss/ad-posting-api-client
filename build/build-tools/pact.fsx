#r "../packages/FAKE/tools/FakeLib.dll" // include Fake lib
#r "System.Runtime.Serialization"

open Fake 
open System.Runtime.Serialization
open System.Runtime.Serialization.Json
open System
open System.IO
open System.Text
open System.Web
open System.Net
open System.IO

let pactBroker = "http://pact.seek.int:9292"

[<DataContract>]
type Provider = {
        [<field: DataMember(Name = "name")>]
        name:string
    }

[<DataContract>]
type Consumer = {
        [<field: DataMember(Name = "name")>]
        name:string
    }

[<DataContract>]
type Pact = {
        [<field: DataMember(Name = "consumer")>]
        consumer:Consumer
        [<field: DataMember(Name = "provider")>]
        provider:Provider
    }

let private deserialisePact (s:string) =
    let json = new DataContractJsonSerializer(typeof<Pact>)
    let byteArray = Encoding.UTF8.GetBytes(s)
    let stream = new MemoryStream(byteArray)
    json.ReadObject(stream) :?> Pact

let private pactVersionFromVersion (version:string[]) = 
    version.[1..3] |> String.concat "."

let PublishPact (version:string[]) pactfiles =
    pactfiles |>
        Seq.iter(fun file -> 
            let pactContent = File.ReadAllText(file)
            let pact = deserialisePact(pactContent)
            let url = sprintf "%s/pacts/provider/%s/consumer/%s/version/%s" pactBroker pact.provider.name pact.consumer.name (pactVersionFromVersion version)

            let request = WebRequest.Create url
            request.ContentType <- "application/json"
            request.Method <- "PUT"

            let bytes = Encoding.UTF8.GetBytes pactContent
            request.ContentLength <- int64 bytes.Length
            use requestStream = request.GetRequestStream()
            requestStream.Write(bytes, 0, bytes.Length)

            try
                use response = request.GetResponse() :?> HttpWebResponse
                trace ("Uploaded pact: " + url)
            with
                | a -> traceError "Error uploading pact file"; reraise()
            
            0 |> ignore
        )

    0 |> ignore