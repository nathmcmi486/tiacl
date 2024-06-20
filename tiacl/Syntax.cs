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

        public List<Function> functions = new List<Function>();
        List<Variable> globalVariables = new List<Variable>();

        public static String[] symbols = { "!", "*", "-", "=", "+", "/", "^", "\"", "'", "{", "}", "&", "decfun", "decvar", "~" };
        public static String[] mathSymbols = { "+", "-", "*", "/" };
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
            List<String> temporaryLines = new List<String>();
            bool readingFunction = false;

            //List<Function> functions = new List<Function>();
            Function temporaryFunction = new Function();
            Variable temporaryVariable = new Variable();

            while (currentLineContent != null)
            {
                if (currentLineContent.StartsWith("\t//") || currentLineContent.StartsWith("//"))
                {
                    lineNumber++;
                    currentLineContent = fileReader.ReadLine();
                    continue;
                }

                if (readingFunction == true)
                {
                    if (currentLineContent.Contains("}"))
                    {
                        readingFunction = false;

                        lineNumber++;
                        currentLineContent = fileReader.ReadLine();
                        Errors.SyntaxErrors ret = temporaryFunction.buildFunctionFromContents();

                        if (((int)ret) < 3)
                        {
                            Console.WriteLine($"Error: {ret}");
                        }

                        functions.Add(temporaryFunction);
                        //Console.WriteLine($"finsihed going over {temporaryFunction.functionName}, contents len: {temporaryFunction.functionContents.Count()}, current line: {lineNumber}");
                        temporaryFunction = new Function();
                        continue;
                    }

                    if (currentLineContent != null || currentLineContent != "")
                    {
                        temporaryFunction.contents.Add(currentLineContent);
                    }
                    lineNumber++;
                    currentLineContent = fileReader.ReadLine();
                    continue;
                }

                Errors.SyntaxErrors fundecRet = temporaryFunction.isLineFunctionDec(currentLineContent);
                Errors.SyntaxErrors vardecRet = temporaryVariable.isLineVariableDec(currentLineContent);

                if (fundecRet == Errors.SyntaxErrors.None)
                {
                    temporaryLines.Add(currentLineContent);
                    readingFunction = true;
                } else if (vardecRet == Errors.SyntaxErrors.None)
                {
                    //globalVariables.Add(temporaryVariable.buildFromContents());
                }
                else
                {
                    Console.WriteLine($"Error on line: {lineNumber}: {fundecRet} - {currentLineContent}");
                }

                currentLineContent = fileReader.ReadLine();
                if (currentLineContent == null)
                {
                    currentLineContent = fileReader.ReadLine();
                    lineNumber++;
                    continue;
                }
                lineNumber++;
            }
        }
    }
}
