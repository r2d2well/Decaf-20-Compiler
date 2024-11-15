using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CS_4308_Term_Project
{
    class Varable
    {
        public string type;
        public string value;
        public string name;
        public int line;
        public Varable(List<Lexical> lexs)
        {
            if ((lexs[0].token == "T_Int") || (lexs[0].token == "T_String") || (lexs[0].token == "T_Float") || (lexs[0].token == "T_Bool"))
            {
                type = lexs[0].lexical;
                //Sets the varable type
            }
            else
            {
                throw new SyntaxError(lexs, "Incorrect Varable Type");
                //If incorrect varable type throw error
            }
            if (lexs.Count == 1)
            {
                throw new SyntaxError(lexs, "Identifier Expected");
                //If missing an identifier
            }
            if (lexs[1].token != "T_IDENTIFIER")
            {
                throw new SyntaxError(lexs, "Invalid Identifier");
            }
            else
            {
                name = lexs[1].lexical;
            }
            if (lexs.Count > 3)
            {
                //If varable is given a value at initilization
                if (lexs[2].lexical != "=")
                {
                    throw new SyntaxError(lexs, "Invalid Varable Initilization");
                }
                Node.expression(lexs, 3);
            }
            line = lexs[0].line;
        }
    }

}
