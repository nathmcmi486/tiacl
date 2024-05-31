using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tiacl
{
    internal class Variable
    {
        String[] contents;
        public String name;
        public Contents.Value value;
        public bool global;

        public Variable()
        {

        }

        public Errors.SyntaxErrors isLineVariableDec(String line)
        {
            if (line == null || line == "")
            {
                return Errors.SyntaxErrors.InvalidVariableDeclaration;
            }

            String[] split = line.Split(' ');

            if (split[0] != "decvar")
            {
                return Errors.SyntaxErrors.InvalidVariableDeclaration;
            }

            // Check name
            for (int i = 0; i < Syntax.invalidSymbols.Length; i++) {
                if (split[1].Contains(Syntax.invalidSymbols[i]))
                {
                    return Errors.SyntaxErrors.InvalidVariableName;
                }
            }

            int length = split.Length;
            // This is a string
            if (split[3].StartsWith("\"") && split[length - 1].EndsWith("\";"))
            {
                split[length - 1] = split[length - 1].Replace(';', ' ');
                String str = "";

                for (int i = 3; i < length; i++)
                {
                    str = $"{str}{split[i]}";
                }

                value = new Contents.Value(Contents.BuiltinType.String, str);
            } else
            {
                int testInt = 0;
                split[3] = split[3].Replace(';', ' ');

                try
                {
                    testInt = Convert.ToInt32(split[3]);
                }
                catch (FormatException ex)
                {
                    return Errors.SyntaxErrors.ExpectedInteger;
                }

                value = new Contents.Value(Contents.BuiltinType.Int, $"{testInt}");
            }

            return Errors.SyntaxErrors.None;
        }

        Errors.SyntaxErrors buildFromContents(bool isGlobal)
        {
            if (isGlobal) {
                global = isGlobal;
            }

            return Errors.SyntaxErrors.None;
        }
    }
}
