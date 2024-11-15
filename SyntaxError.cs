using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_4308_Term_Project
{
    internal class SyntaxError : Exception
    {
        public string OutPut;
        public SyntaxError(List<Lexical> lexs, string errorType): base () {
            string temp = "*** Error line: " + lexs[0].line + "\n";
            //Print the line the error is located on
            int size = lexs.Count;
            for (int x = 0; x < size; x++)
            {
                temp += (lexs[x].lexical + " ");
                //Print the line causing the error
            }
            temp += ("\n*** " + errorType);
            //Print Error type
            OutPut = temp;
        }
    }
}
