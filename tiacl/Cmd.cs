using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tiacl
{
    public class Cmd
    {
        public Cmd()
        {

        }

        public const String HELP1 = "--help";
        public const String HELP2 = "-h";
        public const String INTERPRET = "-i";
        public const String COMPILE = "-c";

        public bool interpreting = false;
        public bool compiling = false;
        public String fileName;

        void displayHelp()
        {
            Console.WriteLine("\nTIACL command options:");
            Console.WriteLine($"{HELP1} or {HELP2}:\n\tDisplays this message");
            Console.WriteLine($"{INTERPRET} <file_name>.tic\n\tStart interpreting program");
            Console.WriteLine($"{COMPILE} <file_name>.tic\n\tStart compiling program");
            Console.WriteLine("\n");
        }

        bool checkFile(String file)
        {
            if (file.Contains(".tic"))
            {
                bool exists = File.Exists(file);
                if (exists)
                {
                    fileName = file;
                    return true;
                } else
                {
                    Console.WriteLine($"{file} is an invalid file");
                    Environment.Exit(-1);
                    return false;
                }
            } else
            {
                Console.WriteLine($"{file} is an invalid file");
                Environment.Exit(-1);
                return false;
            }
        }

        public void runCMdArgs(String[] args)
        {
            if (args.Count() == 0)
            {
                Console.WriteLine("No arguments given");
                Environment.Exit(-1);
            }

            switch (args[0])
            {
                case HELP1: case HELP2:
                    displayHelp();
                    break;
                case INTERPRET:
                    checkFile(args[1]);
                    interpreting = true;
                    break;
                case COMPILE:
                    checkFile(args[1]);
                    compiling = true;
                    break;
                default:
                    Console.WriteLine($"{args[0]} is invalid");
                    break;
            }
        }
    }
}
