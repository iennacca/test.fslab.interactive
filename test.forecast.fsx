(*** hide ***)
#load "packages/FsLab/Themes/AtomChester.fsx"
#load "packages/FsLab/FsLab.fsx"
#load "dotenv.fs"
#load "forecast.fs"

open System
open TestFsLab

(**
Welcome to the OpenWeather test journal
=======================================


Retrieving weather forecasts via OpenWeather
--------------------------------------------
*)

(*** define-output:info ***)
let f = API.Forecast()
let info = f.GetAllInformation "Manila"
printfn "%A" info
(*** include-output:info ***)
