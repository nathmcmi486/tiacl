namespace tiacl
{
    static class Program
    {
        [STAThread]
        static void Main(String[] args)
        {
            Cmd cmd = new Cmd();
            cmd.runCMdArgs(args);

            if (cmd.interpreting == true)
            {
                Console.WriteLine($"Interpreting {cmd.fileName}...");
            } else if (cmd.compiling == true)
            {
                Console.WriteLine($"Compiling {cmd.fileName}...");
            }
        }
    }
}