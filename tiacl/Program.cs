namespace tiacl
{
    static class Program
    {
        [STAThread]
        static void Main(String[] args)
        {
            switch (System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture)
            {
                case (System.Runtime.InteropServices.Architecture.X86):
                    Console.WriteLine("Tiacl running on x86-32");
                    break;
                case (System.Runtime.InteropServices.Architecture.X64):
                    Console.WriteLine("Tiacl running on x86-64");
                    break;
                default:
                    Console.WriteLine("Unsupported architecture");
                    break;
            }

            Cmd cmd = new Cmd();
            cmd.runCMdArgs(args);


            Syntax syntax = new Syntax(cmd.fileName);
            syntax.checkSyntax();

            if (cmd.interpreting == true)
            {
                Console.WriteLine($"Interpreting {cmd.fileName}...");
                
                List<object> code = new List<object>();
                for (int i = 0; i < syntax.functions.Count; i++)
                {
                    code.Add(syntax.functions[i]);
                }

                Interpreter interpreter = new Interpreter(code);
                interpreter.interpret();
            }
            else if (cmd.compiling == true)
            {
                Console.WriteLine($"Compiling {cmd.fileName}...");
            }
        }
    }
}