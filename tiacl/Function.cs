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
            String[] splitName = functionDeclaration[1].Split('(');
            functionName = splitName[0];

            for (int i = 0; i < Syntax.symbols.Length; i++)
            {
                if (functionName.Contains(Syntax.symbols[i]))
                {
                    Console.WriteLine($"Cannot have \"{Syntax.symbols[i]}\" in function name");
                    return Errors.SyntaxErrors.InvalidFunctionName;
                }
            }
            for (int i = 0; i < Syntax.invalidSymbols.Length; i++)
            {
                if (functionName.Contains(Syntax.symbols[i]))
                {
                    Console.WriteLine($"Cannot have \"{Syntax.symbols[i]}\" in function name");
                    return Errors.SyntaxErrors.InvalidFunctionName;
                }
            }

            if (splitName[1] != ")")
            {
                functionDeclaration[1] = functionDeclaration[1].Split('(')[1];
                functionDeclaration[functionDeclaration.Length - 2] = functionDeclaration[functionDeclaration.Length - 2].Replace(")", "");

                for (int i = 3; i <= functionDeclaration.Length; i += 3)
                {
                    if (functionDeclaration[i].EndsWith(","))
                    {
                        functionDeclaration[i] = functionDeclaration[i].Replace(",", "");
                    }
                    Contents.Variable tmpVar = new Contents.Variable($"{functionDeclaration[i - 2]} {functionDeclaration[i - 1]} {functionDeclaration[i]}");
                    
                    arguments.Add(tmpVar);
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

        public Errors.SyntaxErrors buildFunctionFromContents()
        {
            if (contents.Count == 0 || contents == null)
            {
                Console.WriteLine($"Function: {functionName} is empty, it must return a value");
                return Errors.SyntaxErrors.WarnFunctionEmpty;
            }

            contents.Add("");

            Contents c = new Contents();
            c.arguments = arguments;
            /*if (arguments.Count != 0)
            {
                for (int i = 0; i < arguments.Count; i++)
                {
                    //Console.WriteLine($"Adding arguments as variables, {arguments[i].name}");
                    c.context.Add(arguments[i]);
                }
            }*/
            c.context = functionContents;

            foreach (String line in contents)
            {
                object ret = c.readContent(line);
                if (ret is Contents.IfElse)
                {
                    Contents.IfElse ifPart = ret as Contents.IfElse;
                    Contents.IfElse elsePart = c.readContent(line) as Contents.IfElse;
                    Contents.IfElse ifelse = new Contents.IfElse();

                    ifelse.left = ifPart.left;
                    ifelse.right = ifPart.right;
                    ifelse.a = ifPart.a;
                    ifelse.b = elsePart.b;
                    Console.WriteLine("If else built");
                }
                functionContents.Add(ret);

                if (ret is Variable)
                {
                    Console.WriteLine("whilst building function a variable was found");
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
                        if (fc.args[0].value == null)
                        {
                            foreach (Contents.Variable a in arguments)
                            {
                                if (a.name == fc.args[0].name)
                                {
                                    fc.args[0].value = a.value;
                                    break;
                                }
                            }
                        }
                        BuiltinFunctions builtinFunctions = new BuiltinFunctions();
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
                                    for (int k = 0; k < fc.args.Count; k++)
                                    {
                                        if (k >= fc.args.Count)
                                        {
                                            Console.WriteLine("More arguments given than needed");
                                        }

                                        fn.functionContents.Add(fc.args[k]);
                                        fn.arguments[k] = fc.args[k];

                                        fn.functionContents.Add(fn.arguments[k]);
                                    }
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
