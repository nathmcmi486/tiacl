using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

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

        void checkCompiler()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine("Running on Windows");
                bool gas = File.Exists("/ProgramFiles/as.exe");
                bool ld = File.Exists("/ProgramFiles/ld.exe");

                if (gas == false)
                {
                    Console.WriteLine("Missing GNU Assembler (GAS \"as\") command, required for compiling");
                }

                if (ld == false)
                {
                    Console.WriteLine("Missing GNU Linker (\"ld\") command, required for compiling");
                }

                if (ld == false || gas == false)
                {
                    Console.WriteLine("Cannot compile");
                    Environment.Exit(-1);
                }
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Console.WriteLine("Running on a POSIX OS");
                bool gas = File.Exists("/bin/as");
                bool ld = File.Exists("/bin/ld");

                if (gas == false)
                {
                    Console.WriteLine("Missing GNU Assembler (GAS \"as\") command, required for compiling");
                }

                if (ld == false)
                {
                    Console.WriteLine("Missing GNU Linker (\"ld\") command, required for compiling");
                }

                if (ld == false || gas == false)
                {
                    Console.WriteLine("Cannot compile");
                    Environment.Exit(-1);
                }
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
                    checkCompiler();
                    compiling = true;
                    break;
                default:
                    Console.WriteLine($"{args[0]} is invalid");
                    break;
            }
        }
    }
}
