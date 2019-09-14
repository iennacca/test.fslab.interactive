namespace TestFSLab

open System.IO 

module IO = 
    let writeText (filename:string) text = 
        use sw = new StreamWriter(filename)
        fprintf sw "%A" text
        sw.Close()

