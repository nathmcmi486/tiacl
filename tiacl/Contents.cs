using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tiacl
{
    internal class Contents
    {
        // Math stuff
        public class MathInfo
        {
            public MathOpperation opp;
            public Value left;
            public Value right;
            public Value output;
            //List<String> stuff = new List<String>();

            public MathInfo(String line)
            {
                output = new Value(BuiltinType.Int, "");
                if (line != null || line != "")
                {
                    string[] split = line.Split(' ');
                    if (split.Length == 3)
                    {
                        left = new Value(BuiltinType.Int, split[0]);
                        right = new Value(BuiltinType.Int, split[2]);

                        switch (split[1])
                        {
                            case "+":
                                opp = MathOpperation.Addition;
                                break;
                            case "-":
                                opp = MathOpperation.Subtraction;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            public void runOpperation()
            {
                int l = Convert.ToInt32(left.value);
                int r = Convert.ToInt32(right.value);

                if (opp == MathOpperation.Addition)
                {
                    output.value = (l + r).ToString();
                } else if (opp == MathOpperation.Subtraction)
                {
                    output.value = (l - r).ToString();
                }
            }
        }

        public enum MathOpperation
        {
            Addition,
            Subtraction,
            Multiplication,
            Division,
        }
        // End of math stuff

        // A value
        public class Value
        {
            public BuiltinType type;
            public String value;

            public Value()
            {

            }

            public Value(BuiltinType t, String v)
            {
                type = t;
                value = v;
            }

            public Value(String content)
            {
                int testInt = 0;
                try
                {
                    testInt = Convert.ToInt32(content);
                } catch (FormatException fe)
                {
                    type = BuiltinType.String;
                    value = content;
                    return;
                }

                type = BuiltinType.Int;
                value = content;
            }
        }

        public class Variable
        {
            public Value value;
            public String name;
            public Errors.SyntaxErrors ret = Errors.SyntaxErrors.None;

            public Variable(Value value_, String name_)
            {
                value = value_;
                name = name_;
            }

            public Variable()
            {

            }

            public Variable(String line)
            {
                String[] split = line.Split(' ');

                name = split[1];

                Value val = new Value();

                if (split.Length == 3)
                {
                    if (split[2] == ":string" || split[2] == ":string,")
                    {
                        val.type = BuiltinType.String;
                        val.value = "";
                    } else if (split[2] == ":int" || split[2] == ":int,")
                    {
                        val.type = BuiltinType.Int;
                        val.value = "0";
                    }

                    return;
                }

                if (split[3].Contains("\""))
                {
                    val.type = BuiltinType.String;
                    for (int i = 3; i < split.Length; i++)
                    {
                        val.value = $"{val.value} {split[i]}";
                    }
                    val.value = val.value.Replace("\";", "\"");
                    value = val;
                } else
                {
                    split[3] = split[3].Replace(";", "");
                    val.type = BuiltinType.Int;
                    val.value = split[3];
                    value = val;
                }

                name = split[1];
            }
        }

        public enum BuiltinType
        {
            Int,
            String,
        }

        // Function calling
        public class FunctionCall
        {
            public bool builtin;
            public String name;
            public List<Variable> args;

            public FunctionCall(bool builtin_, String name_, List<Variable> arguments_)
            {
                builtin = builtin_;
                name = name_;
                args = arguments_;
            }
        }

        public List<object> context = new List<object>();
        public List<Variable> arguments = new List<Variable>();

        List<Variable> functionArguments(String argsString)
        {
            List<Variable> ret = new List<Variable>();
            Random r = new Random();

            if (argsString.Contains(", ") == false)
            {
                Value testValue = new Value(argsString);
                if (testValue.type == BuiltinType.String && argsString.Contains("\"") == false)
                {
                    foreach (object c in context)
                    {
                        if (c is Variable)
                        {
                            Variable v = (Variable)c;
                            if (v.name == testValue.value)
                            {
                                ret.Add(v);
                                //return ret;
                            }
                        } else
                        {
                            foreach (Variable a in arguments)
                            {
                                if (a.name == testValue.value)
                                {
                                    ret.Add(a);
                                    //Console.WriteLine($"{a.value.value}");
                                }
                            }
                            return ret;
                            Console.WriteLine($"Error: {Errors.SyntaxErrors.VariableOrValueNotFound} {testValue.value}");
                        }
                    }
                } else
                {
                    Variable v = new Variable();
                    v.name = $"v{r.Next(0, 1000)}";
                    v.value = testValue;
                    ret.Add(v);
                    return ret;
                }

            }

            String[] argsStringArray = argsString.Split(", ");
            argsStringArray.Append("");

            if (argsStringArray.Length == 0)
            {
                return ret;
            }

            for (int i = 0; i == argsStringArray.Length; i++)
            {
                Variable v = new Variable();
                v.value = new Value(argsStringArray[i]);
                v.name = $"v{r.Next(0, 10000)}";
                ret.Add(v);
            }

            return ret;
        }

        public object readContent(String content)
        {
            content = content.Trim();
            for (int i = 0; i < Syntax.mathSymbols.Count(); i++)
            {
                if (content.Contains(Syntax.mathSymbols[i]))
                {
                    return new MathInfo(content);
                }
            }

            // Variable
            if (content.StartsWith("decvar"))
            {
                Variable v = new Variable(content);
                return new Variable(content);
            }

            // Call built-in function
            if (content.StartsWith("!") && content.EndsWith("!"))
            {
                String[] split = content.Split('(');
                String fname = split[0].Split('!')[1];
                String argsString = split[1].Split(')')[0];
                List<Variable> args = functionArguments(argsString);

                /*foreach (Value v in args)
                {   
                    Console.WriteLine($"builtin function call value: {v.value}");
                }*/
                
                return new FunctionCall(true, fname, args);
            }

            // Call regular function
            if (content.EndsWith(");"))
            {
                String[] split = content.Split('(');
                String fname = split[0];
                List<Variable> args = new List<Variable>();
                String argsString = split[1].Split(')')[0];

                /*if (argsString.Contains(", ") == false)
                {
                    Value tmpVal = new Value(argsString);
                    if (tmpVal.type == BuiltinType.String && tmpVal.value.Contains("\"") == false)
                    {
                        for (int i = 0; i < context.Count; i++)
                        {
                            if (context[i] is Variable)
                            {
                                Variable v = (Variable)context[i];
                                Console.WriteLine("Adding variable argument");
                                Console.WriteLine($"{v.name} {v.value.value}");
                                args.Add(v.value);
                                Console.Write($"Variable value: {args[0].value}");
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Making regular function");
                        Console.WriteLine($"args: {argsString}");
                        args.Add(new Value(argsString));
                    }
                    return new FunctionCall(false, fname, args);
                }

                String[] argsStringArray = argsString.Split(", ");
                argsStringArray.Append("");*/
                args = functionArguments(argsString);
                /*for (int i = 0; i <= argsStringArray.Length; i++)
                {
                    args.Add(new Value(argsStringArray[i]));
                }*/

                return new FunctionCall(false, fname, args);
            }

            return null;
        }

        public Contents()
        {

        }
    }
}
