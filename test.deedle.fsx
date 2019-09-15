(*** hide ***)
#r "System.Xml.Linq.dll"
#load "packages/FsLab/Themes/AtomChester.fsx"
#load "packages/FsLab/FsLab.fsx"

open System
open Deedle
open FSharp.Data
open XPlot.GoogleCharts
open XPlot.GoogleCharts.Deedle

(**
Welcome to the Deedle test journal
=======================================


Retrieving World Bank information via XML
-----------------------------------------
*)

type WorldData = XmlProvider<"http://api.worldbank.org/countries/indicators/NY.GDP.PCAP.CD?date=2010:2010">
let indUrl = "http://api.worldbank.org/countries/indicators/"

let getData year indicator =
    let query =
        [("per_page","1000"); ("date",sprintf "%d:%d" year year)]
    let data = Http.RequestString(indUrl + indicator, query)
    let xml = WorldData.Parse(data)
    let orNaN value =
        defaultArg (Option.map float value) nan
    series [ for d in xml.Datas -> d.Country.Value, orNaN d.Value ]
 
let wb = WorldBankData.GetDataContext()
let inds = wb.Countries.World.Indicators
let code = inds.``CO2 emissions (kt)``.IndicatorCode

let co2000 = getData 2000 code
let co2010 = getData 2010 code

let change = (co2010 - co2000) / co2000 * 100.0

let colors = [| "#80E000";"#E0C000";"#E07B00";"#E02800" |]
let values = [| 0;+25;+50;+75 |]
let axis = ColorAxis(values=values, colors=colors)

change
|> Chart.Geo
|> Chart.WithOptions(Options(colorAxis=axis))
|> Chart.WithLabel "CO2 emissions rise since 2010"
