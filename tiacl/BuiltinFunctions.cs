﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tiacl
{
    internal class BuiltinFunctions
    {
        public BuiltinFunctions()
        {

        }

        void display(String stuff)
        {
            Console.WriteLine($"{stuff}");
        }

        public void execute(String name, List<Contents.Value> arguments)
        {
            switch (name)
            {
                case "display":
                    //Console.WriteLine($"argument {arguments[0].type}");
                    Console.WriteLine("here");
                    display(arguments[0].value);
                    break;
                default:
                    break;
            }
        }
    }
}
