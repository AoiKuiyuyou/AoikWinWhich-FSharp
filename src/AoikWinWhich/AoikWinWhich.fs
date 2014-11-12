//
namespace AoikWinWhich

type Console = System.Console
type Environment = System.Environment
type File = System.IO.File
type Path = System.IO.Path
type String = System.String

//
module AoikWinWhich =

    let find_executable (prog:string) =
        // 8f1kRCu
        let env_var_PATHEXT = Environment.GetEnvironmentVariable("PATHEXT")
        /// can be null
        
        // 6qhHTHF
        // split into a list of extensions
        let ext_s =
            if env_var_PATHEXT = null then
                []
            else
                env_var_PATHEXT.Split(Path.PathSeparator) |> Array.toList

        // 2pGJrMW
        // strip
        let ext_s = ext_s |> List.map (fun x -> x.Trim())
        
        // 2gqeHHl
        // remove empty
        let ext_s = ext_s |> List.filter (fun x -> x <> "")
        
        // 2zdGM8W
        // convert to lowercase
        let ext_s = ext_s |> List.map (fun x -> x.ToLower())
        
        // 2fT8aRB
        // uniquify
        let ext_s = ext_s |> Seq.distinct |> Seq.toList
        
        // 4ysaQVN
        let env_var_PATH = Environment.GetEnvironmentVariable("PATH")
        /// can be null
        
        let dir_path_s =
            if env_var_PATH = null then
                []
            else
                env_var_PATH.Split(Path.PathSeparator) |> Array.toList

        // 5rT49zI
        // insert empty dir path to the beginning
        //
        // Empty dir handles the case that |prog| is a path, either relative or
        //  absolute. See code 7rO7NIN.
        let dir_path_s = "" :: dir_path_s
        
        // 2klTv20
        // uniquify
        let dir_path_s = dir_path_s |> Seq.distinct |> Seq.toList
        
        //
        let prog_lc = prog.ToLower()
        
        let prog_has_ext = ext_s |> List.exists (fun ext -> prog_lc.EndsWith(ext))
        
        // 6bFwhbv
        let mutable exe_path_s = []

        for dir_path in dir_path_s do
            // 7rO7NIN
            // synthesize a path with the dir and prog
            let path =
                if dir_path = "" then
                    prog
                else
                    Path.Combine(dir_path, prog)
  
            // 6kZa5cq
            // assume the path has extension, check if it is an executable
            if prog_has_ext && File.Exists(path) then
                exe_path_s <- path :: exe_path_s
                ()

            // 2sJhhEV
            // assume the path has no extension
            for ext in ext_s do
                // 6k9X6GP
                // synthesize a new path with the path and the executable extension
                let path_plus_ext = path + ext

                // 6kabzQg
                // check if it is an executable
                if File.Exists(path_plus_ext) then
                    exe_path_s <- path_plus_ext :: exe_path_s
                    ()

        // reverse
        let exe_path_s = exe_path_s |> List.rev

        // 8swW6Av
        // uniquify
        let exe_path_s = exe_path_s |> Seq.distinct |> Seq.toList

        // return
        exe_path_s

    [<EntryPoint>]
    let main args =
        // 9mlJlKg
        let args_len = Array.length args

        if args_len <> 1 then
            // 7rOUXFo
            // print program usage
            Console.WriteLine(@"Usage: aoikwinwhich PROG")
            Console.WriteLine(@"")
            Console.WriteLine(@"#/ PROG can be either name or path")
            Console.WriteLine(@"aoikwinwhich notepad.exe")
            Console.WriteLine(@"aoikwinwhich C:\Windows\notepad.exe")
            Console.WriteLine(@"")
            Console.WriteLine(@"#/ PROG can be either absolute or relative")
            Console.WriteLine(@"aoikwinwhich C:\Windows\notepad.exe")
            Console.WriteLine(@"aoikwinwhich Windows\notepad.exe")
            Console.WriteLine(@"")
            Console.WriteLine(@"#/ PROG can be either with or without extension")
            Console.WriteLine(@"aoikwinwhich notepad.exe")
            Console.WriteLine(@"aoikwinwhich notepad")
            Console.WriteLine(@"aoikwinwhich C:\Windows\notepad.exe")
            Console.WriteLine(@"aoikwinwhich C:\Windows\notepad")
                
            // 3nqHnP7
            2
        else
            // 9m5B08H
            // get name or path of a program from cmd arg
            let prog = args.[0]

            // 8ulvPXM
            // find executables
            let path_s = find_executable(prog)

            //
            if path_s.Length = 0 then
                // 5fWrcaF
                // has found none, exit

                // 3uswpx0
                1
            else
                // 9xPCWuS
                // has found some, output
                let txt = String.Join("\n", path_s)
                
                Console.WriteLine(txt)

                // 4s1yY1b
                0
