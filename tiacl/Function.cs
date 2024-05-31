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
                functionContents.Add(ret);

                if (ret is Variable)
                {
                    Console.WriteLine("whilst bulding function a variable was found");
                }
                c.context = functionContents;
            }

            return Errors.SyntaxErrors.None;
        }

        public void execute(Interpreter intr)
        {
            for (int i = 0; i < functionContents.Count; i++)
            {
                if (functionContents[i] is Contents.MathInfo)
                {
                    Contents.MathInfo mi = functionContents[i] as Contents.MathInfo;
                    mi.runOpperation();
                }
                else if (functionContents[i] is Contents.FunctionCall)
                {
                    Contents.FunctionCall fc = functionContents[i] as Contents.FunctionCall;

                    if (fc.builtin == true)
                    {
                        BuiltinFunctions builtinFunctions = new BuiltinFunctions();
                        Console.WriteLine($"fc {fc.args[0].value}");
                        builtinFunctions.execute(fc.name, fc.args);
                    } else
                    {
                        for (int j = 0; j < intr.code.Count; j++)
                        {
                            if (intr.code[j] is Function)
                            {
                                Function fn = (Function)intr.code[j];
                                if (fn.functionName == fc.name)
                                {
                                    fn.execute(intr);
                                    continue;
                                }
                            }
                        }
                    }
                    //functionContents.Add(fc);
                }
            }
        }
    }
}
