using CS_4308_Term_Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CS_4308_Term_Project
{
    internal class Function : Node
    {
        public string returnType;
        public string functionName;
        public int argumentAmount;
        public List <Varable> arguments;
        public int line;
        public Function(List<Lexical> lexs, Node parent): base(parent) {
            arguments = new List<Varable> ();
            line = lexs[0].line;
            if ((lexs[0].token == "T_Void") || (lexs[0].token == "T_Int") || (lexs[0].token == "T_String")) {
		        returnType = lexs[0].lexical;
                //Sets the return type of the function
	        }
	        else {
		        throw new SyntaxError(lexs, "Invalid Return Type");
                //No return type specified or invalid return type specified
            }
            if (lexs[1].token != "T_IDENTIFIER")
            {
                throw new SyntaxError(lexs, "Invalid Function Name");
                //Not a valid function name is given
            }
            else
            {
                functionName = lexs[1].lexical;
            }
            if (lexs[2].lexical != "(")
            {
                throw new SyntaxError(lexs, "Parentheses Expected");
                //Function must have parentheses
            }
            int current = 3;
            List <Lexical> lexList = new List<Lexical>();
            while (lexs[current].lexical != ")")
            {
                if (lexs[current].lexical != ",")
                {
                    lexList.Add(lexs[current]);
                }
                if ((lexs[current + 1].lexical == ",") || (lexs[current + 1].lexical == ")"))
                {
                    Varable var = new Varable(lexList);
                    arguments.Add(var);
                    lexList.Clear();
                }
                current++;
                //Adds the given argument
            }
            argumentAmount = arguments.Count;
        }
    }
}
