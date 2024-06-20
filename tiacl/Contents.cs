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
                    if (content == "true" || content == "false" && content.Contains("\"") == false)
                    {
                        type = BuiltinType.Bool;
                        value = content;
                        return;
                    }

                    type = BuiltinType.String;
                    value = content;
                    return;
                }

                type = BuiltinType.Int;
                value = content;
            }

            public Value makeBooleanFromStatement(Value r, Value l, String symbol)
            {
                Value v = new Value();
                if (r.type != l.type)
                {
                    v.type = BuiltinType.Unknown;
                    Console.WriteLine($"Can't compare: {r.type} with {l.type}");
                    return v;
                }

                v.type = BuiltinType.Bool;
                switch (symbol)
                {
                    case "==":
                        v.value = $"{r.value == l.value}";
                        break;
                    case "!=":
                        v.value = $"{r.value != l.value}";
                        break;
                    case ">":
                        if (r.type == BuiltinType.Int)
                        {
                            v.value = $"{Convert.ToInt32(r.value) > Convert.ToInt32(l.value)}";
                        } else if (r.type == BuiltinType.String || r.type == BuiltinType.Bool)
                        {
                            v.value = $"{r.value.Count() > l.value.Count()}";
                        }
                        break;
                    case "<":
                        if (r.type == BuiltinType.Int)
                        {
                            v.value = $"{Convert.ToInt32(r.value) < Convert.ToInt32(l.value)}";
                        }
                        else if (r.type == BuiltinType.String || r.type == BuiltinType.Bool)
                        {
                            v.value = $"{r.value.Count() < l.value.Count()}";
                        }
                        break;
                    default:
                        Console.WriteLine($"Unknown symbol: {symbol}");
                        break;
                }

                return v;
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
                    if (split[3] == "true" || split[3] == "false")
                    {
                        val.type = BuiltinType.Bool;
                    }
                    else
                    {
                        val.type = BuiltinType.Int;
                    }
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
            Bool,
            Unknown
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

        public class IfElse
        {
            public string id;
            public Value left;
            public Value right;
            public FunctionCall a;
            public FunctionCall b;
            public bool second = false;
            
            public IfElse()
            {

            }

            public IfElse(String ifl, String elsel)
            {
                Random r = new Random();
                id = $"l{r.Next(1000, 10000)}";

                if (ifl == "" || ifl == null)
                {
                    String[] splitIf = ifl.Split(' ');
                    String condition = "";
                    for (int i = 1; i < splitIf.Length; i++)
                    {
                        if (i == 1)
                        {
                            condition = splitIf[i].Replace("(", "");
                        }
                        condition = $"{condition} {splitIf[i]}";
                        if (splitIf[i].Contains(")"))
                        {
                            condition = condition.Replace(")", "");
                            break;
                        }
                    }
                }

                if (elsel == "" || elsel == null)
                {
                    return;
                }
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

            // If statement
            if (content.StartsWith("if "))
            {
                IfElse ifelse = new IfElse(content, "");
                return ifelse;
            }
            else if (content.StartsWith("else "))
            {
                IfElse ifelse = new IfElse("", content);
                return ifelse;
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
                
                return new FunctionCall(true, fname, args);
            }

            // Call regular function
            if (content.EndsWith(");"))
            {
                String[] split = content.Split('(');
                String fname = split[0];
                List<Variable> args = new List<Variable>();
                String argsString = split[1].Split(')')[0];

                String[] argsStringArray = argsString.Split(", ");
                argsStringArray.Append("");
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
