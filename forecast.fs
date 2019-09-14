namespace TestFsLab

open System
open FSharp.Core
open FSharp.Data
open TestFsLab.Utilities


module API = 
    [<Literal>]
    let ForecastUrl = "http://samples.openweathermap.org/data/2.5/forecast?q=London,uk&appid=b6907d289e10d714a6e88b30761fae22"
    let PForecastUrl = "http://api.openweathermap.org/data/2.5/forecast?q={1}&appid={0}"

    type ForecastInformation = JsonProvider<ForecastUrl>

    type Forecast() = 
        static do
            Config.Load()

        static member APIKey = Environment.GetEnvironmentVariable("APIKEY_OPENWEATHER")
        // static member Seed = Random()

        member this.Version = "0.1"
        member this.GetTemperature location = 
            let urlForecast = String.Format(PForecastUrl, Forecast.APIKey, location) 
            try
                let f = ForecastInformation.Load(urlForecast)
                Some(f.List.[0].Main.Temp - 273m)
            with
                | _ -> None
            // Forecast.Seed.Next()

        member this.GetAllInformation location = 
            let urlForecast = String.Format(PForecastUrl, Forecast.APIKey, location) 
            ForecastInformation.Load(urlForecast)
