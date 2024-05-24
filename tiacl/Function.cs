using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tiacl
{
    internal class Function
    {
        public String functionName = "";
        public List<String> contents = new List<String>();
        public List<Contents.Variable> arguments = new List<Contents.Variable>();
        // Everything the function does in order
        public List<object> functionContents = new List<object>();

        public Function()
        {

        }
        
        public Errors.SyntaxErrors isLineFunctionDec(String line)
        {
            if (line == null || line == "")
            {
                return Errors.SyntaxErrors.InvalidFunctionDeclaration;
            }

            string[] functionDeclaration = line.Split(' ');

            if (functionDeclaration.Length < 3)
            {
                return Errors.SyntaxErrors.FunctionMissingStart;
            }

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

            functionName = functionDeclaration[1].Split('(')[0];

            // Does it have a starting bracket?
            if (functionDeclaration[functionDeclaration.Length - 1] == "{")
            {
                return Errors.SyntaxErrors.None;
            } else
            {
                return Errors.SyntaxErrors.FunctionMissingStart;
            }
        }

        public Errors.SyntaxErrors buildFunctionFromContents()
        {
            if (contents.Count == 0 || contents == null)
            {
                Console.WriteLine($"Function: {functionName} is empty, it must return a value");
                return Errors.SyntaxErrors.WarnFunctionEmpty;
            }

            contents.Add("");

            Contents c = new Contents();

            foreach (String line in contents)
            {
                object ret = c.readContent(line);

                if (ret is Contents.MathInfo)
                {
                    Contents.MathInfo mi = ret as Contents.MathInfo;
                    Console.WriteLine($"output: {mi.output.value}");
                    mi.runOpperation();
                    Console.WriteLine($"output: {mi.output.value}");
                    functionContents.Add(ret);
                }
                else if (ret is Contents.FunctionCall)
                {
                    Contents.FunctionCall fc = ret as Contents.FunctionCall;
                    Console.WriteLine($"calling function name: {fc.name}");
                    BuiltinFunctions builtinFunctions = new BuiltinFunctions();
                    builtinFunctions.execute(fc.name, fc.args);
                    functionContents.Add(fc);
                }
            }

            return Errors.SyntaxErrors.None;
        }
    }
}
