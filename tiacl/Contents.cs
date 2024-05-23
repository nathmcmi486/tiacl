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
        }

        public enum Variable
        {

        }

        public enum BuiltinType
        {
            Int,
            String,
        }

        public enum FunctionCall
        {

        }

        public object readContents(List<String> contents)
        {
            for (int i = 0; i < Syntax.mathSymbols.Count(); i++)
            {
                if (contents[0].Contains(Syntax.mathSymbols[i]))
                {
                    return new MathInfo(contents[0]);
                }
            }

            return null;
        }

        public Contents()
        {

        }
    }
}
