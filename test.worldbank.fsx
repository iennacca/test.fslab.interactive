(*** hide ***)
#load "packages/FsLab/Themes/AtomChester.fsx"
#load "packages/FsLab/FsLab.fsx"

open Deedle
open FSharp.Data
open XPlot.GoogleCharts

(**
Welcome to the World Bank data journal
======================================

Accessing World Bank data with type providers
---------------------------------------------
*)

(*** define-output:loading ***)
let wb = WorldBankData.GetDataContext()
let ph = wb.Countries.``Philippines``.Indicators
let cz = wb.Countries.``Czech Republic``.Indicators
printfn "Loaded World Bank data"
(*** include-output:loading ***)

(*** define-output:chart1 ***)
let czschool = series cz.``School enrollment, tertiary (% gross)`` 
let phschool = series ph.``School enrollment, tertiary (% gross)`` 

[czschool.[1975 .. 2010]; phschool.[1975 .. 2010] ]
|> Chart.Line
|> Chart.WithOptions (Options(legend=Legend(position="bottom")))
|> Chart.WithLabels ["CZ"; "PH"]
(*** include-it:chart1 ***)

(*** define-output:chart2 ***)
let czgdp = series cz.``GDP (current US$)`` 
let phgdp = series ph.``GDP (current US$)``

[czgdp.[1970 .. 2019]; phgdp.[1970 .. 2019] ]
|> Chart.Line
|> Chart.WithOptions (Options(legend=Legend(position="bottom")))
|> Chart.WithLabels ["CZ"; "PH"]
(*** include-it:chart2 ***)

(*** define-output:APIKey ***)
#load "dotenv.fs"
#load "forecast.fs"

open System
open TestFsLab
      
let f = API.Forecast()
printfn "APIKey: %s" API.Forecast.APIKey
(*** include-output:APIKey ***)

(*** define-output:chart ***)
let cities = wb.Countries |> Seq.map (fun c -> c.CapitalCity, c.Name) |> Seq.filter (fun (c,n) -> not (String.IsNullOrEmpty c))

let locationTemps = [
    for (c,n) in cities ->
        let location = c + "," + n
        printfn "%s" location
        n, f.GetTemperature location
]

let cleanLocationTemps = locationTemps |> Seq.filter (fun (l,t) -> t <> None) |> Seq.map (fun (l,t) -> (l, t.Value))

open XPlot.GoogleCharts
Chart.Geo(cleanLocationTemps)

let colors = [| "#80E000";"#E0C000";"#E07B00";"#E02800" |]
let values = [| 0;+15;+30;+45 |]
let axis = ColorAxis(values=values, colors=colors)

cleanLocationTemps
|> Chart.Geo
|> Chart.WithOptions(Options(colorAxis=axis))
|> Chart.WithLabel "Temp"
(*** include-it:chart ***)
