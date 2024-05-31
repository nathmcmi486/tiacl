using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tiacl
{
    internal class Interpreter
    {
        public List<object> code;

        public Interpreter(List<object> code_)
        {
            code = code_;
        }

        public void interpret()
        {
            for (int i = 0; i <  code.Count(); i++)
            {
                if (code[i] is Function)
                {
                    Function function = (Function)code[i];
                    if (function.functionName == "main" || function.functionName == "_start")
                    {
                        function.execute(this);
                    }
                }
            }
        }
    }
}
