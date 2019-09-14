namespace TestFsLab

open System
open System.Text.RegularExpressions

module Utilities = 
    type Config =
        static member Load ?envFile = 
            let ef =
                match envFile with
                | None -> ".env"
                | Some s ->  
                if String.IsNullOrEmpty(s) then
                    ".env"
                else
                    s
            if IO.File.Exists(ef) then 
                IO.File.ReadAllLines(ef) |> Array.iter (fun line ->
                // Remove comments
                let line' = Regex.Replace(line, "#.*", "")

                // Split into <key>=<value>
                let parts = line'.Split([|'='|]) |> Array.map (fun x -> x.Trim())
                if Array.length parts = 2 then
                    let (key, value) = (parts.[0], parts.[1])

                    // Only add key if not already there
                    if String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(key)) then 
                        Environment.SetEnvironmentVariable(key, value)
                    else 
                        ()
                else 
                    ()
                )
