using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tiacl
{
    internal class Function
    {

        public enum MathOpperation
        {
            Addition,
            Subtraction,
            Multiplication,
            Division,
        }

        public Function()
        {

        }

        public Errors.SyntaxErrors isLineFunctionDec(String line)
        {
            if (line == null || line == "")
            {
                return Errors.SyntaxErrors.None;
            }

            string[] functionDeclaration = line.Split(' ');

            // Is this actually a function being declared?
            if (functionDeclaration[0] != "decfun")
            {
                return Errors.SyntaxErrors.InvalidFunctionDeclaration;
            }

            // Is it's name a valid function name?
            for (int i = 0; i < Syntax.symbols.Length; i++)
            {
                if (functionDeclaration[1].Contains(Syntax.symbols[i]))
                {
                    Console.WriteLine($"Cannot have \"{Syntax.symbols[i]}\" in function name");
                    return Errors.SyntaxErrors.InvalidFunctionName;
                }
            }
            for (int i = 0; i < Syntax.invalidSymbols.Length; i++)
            {
                if (functionDeclaration[1].Contains(Syntax.symbols[i]))
                {
                    Console.WriteLine($"Cannot have \"{Syntax.symbols[i]}\" in function name");
                    return Errors.SyntaxErrors.InvalidFunctionName;
                }
            }

            // Does it have a starting bracket?
            if (functionDeclaration[functionDeclaration.Length - 1] == "{")
            {
                return Errors.SyntaxErrors.None;
            } else
            {
                return Errors.SyntaxErrors.FunctionMissingStart;
            }
        }
    }
}
