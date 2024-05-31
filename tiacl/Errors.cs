using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tiacl
{
    internal class Errors
    {
        public enum SyntaxErrors
        {
            InvalidFunctionName,
            InvalidVariableName,
            InvalidFunctionDeclaration,
            InvalidVariableDeclaration,
            FunctionMissingStart,
            ExpectedInteger,
            WarnFunctionEmpty,
            VariableOrValueNotFound,
            None,
        }

        public Errors()
        {

        }
    }
}
