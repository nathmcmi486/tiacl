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

            public Variable(Value value_, String name_)
            {
                value = value_;
                name = name_;
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
            public List<Value> args;

            public FunctionCall(bool builtin_, String name_, List<Value> arguments_)
            {
                builtin = builtin_;
                name = name_;
                args = arguments_;
            }
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

            // Call built-in function
            if (content.StartsWith("!") && content.EndsWith("!"))
            {
                String[] split = content.Split('(');
                String fname = split[0].Split('!')[1];
                List<Value> args = new List<Value>();
                String argsString = split[1].Split(')')[0];

                if (argsString.Contains(", ") == false)
                {
                    args.Add(new Value(argsString));
                    return new FunctionCall(true, fname, args);
                }

                String[] argsStringArray = argsString.Split(", ");
                argsStringArray.Append("");

                for (int i = 0; i <= argsStringArray.Length; i++)
                {
                    args.Add(new Value(argsStringArray[i]));
                }

                return new FunctionCall(true, fname, args);
            }

            return null;
        }

        public Contents()
        {

        }
    }
}
