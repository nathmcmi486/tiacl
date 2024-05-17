using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tiacl
{
    internal class Syntax
    {
        String fileName;

        List<Function> functions = new List<Function>();
        List<Variable> globalVariables = new List<Variable>();

        public static String[] symbols = { "!", "*", "-", "=", "+", "/", "^", "\"", "'", "{", "}", "&", "decfun", "decvar", "~" };
        public static String[] invalidSymbols = { "@", "#", "$", "%", "`", ".", "?", "\\", };

        public Syntax(String file)
        {
            fileName = file;
        }

        public void checkSyntax()
        {
            FileStream file = File.OpenRead(fileName);
            StreamReader fileReader = new StreamReader(file);

            String currentLineContent = fileReader.ReadLine();
            int lineNumber = 0;

            while (currentLineContent != null)
            {
                Function function = new Function();

                Console.WriteLine($"{lineNumber}: {currentLineContent}");
                Errors.SyntaxErrors ret = function.isLineFunctionDec(currentLineContent);
                Console.WriteLine($"{ret}");

                currentLineContent = fileReader.ReadLine();
                lineNumber++;
            }
        }
    }
}
