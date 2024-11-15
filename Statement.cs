using CS_4308_Term_Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CS_4308_Term_Project
{
    internal class Statement : Node
    {
        public string statementType;
        public List<Lexical> statementLexicals;

        int logicalExpression(List<Lexical> lexs)
        {
            int current = 2;
            while ((current < lexs.Count) && (lexs[current].lexical == ")"))
            {
                //Determine if a logical expression is valid
                if (current % 2 == 0)
                {
                    if ((lexs[current].token != "T_IDENTIFIER") && (lexs[current].token != "T_Int") && (lexs[current].token != "T_String") && (lexs[current].token != "T_IntConstant") && (lexs[current].token != "T_STRINGCONSTANT") && (lexs[current].token != "T_BoolConstant"))
                    {
                        throw new SyntaxError(lexs, "Value Expected");
                    }
                    //If an even number of lexical then a value is required
                }
                else
                {
                    if ((lexs[current].lexical != "+") && (lexs[current].lexical != "*") && (lexs[current].lexical != "-") && (lexs[current].lexical != "/") && (lexs[current].token != "T_Logical") && 
                        (lexs[current].token != "T_Equivelent") && (lexs[current].token != "T_NotEquivelent") && (lexs[current].token != "T_Less") && (lexs[current].token != "T_LessEqual") && (lexs[current].token != "T_Greater") && (lexs[current].token != "T_GreaterEqual"))
                    {
                        throw new SyntaxError(lexs, "Expression Expected");
                    }
                    //If an odd number of lexical then an arthimitic or logical operation is required
                }
                current++;
            }
            if (current == lexs.Count)
            {
                throw new SyntaxError(lexs, ") Expected");
                //If parenteshes are not closed then throw an error
            }
            return current;
        }

        public Statement(List<Lexical> lexs, Node parent) : base(parent)
        {
            statementLexicals = new List<Lexical>(lexs);
            if (lexs[0].token == "T_IDENTIFIER")
            {
                if (isFunction(lexs[0], this))
                {
                    //Checks whether or not statement is a function call
                    statementType = "Call";
                }
                else
                {
                    statementType = "AssignExpr";
                    if (lexs[1].lexical != "=")
                    {
                        throw new SyntaxError(lexs, "= Expected");
                    }
                    if (!isFunction(lexs[2], this))
                    {
                        expression(lexs, 2);
                    }
                    // If the statement starts with an identifier and is not a function call then it must set the identifier to a value
                }
            }
            else if (lexs[0].token == "T_Print")
            {
                statementType = "PrintStmt";
                if (lexs[1].lexical != "(")
                {
                    throw new SyntaxError(lexs, "( Expected");
                }
                //Special print statement syntax
            }
            else if (lexs[0].token == "T_Return")
            {
                statementType = "ReturnStmt";
            }
            else if ((lexs[0].token == "T_While") || (lexs[0].token == "T_If"))
            {
                switch (lexs[0].token)
                {
                    case "T_While":
                        statementType = "While";
                        break;
                    case "T_If":
                        statementType = "IfStmt";
                        break;
                    //Sets the return type
                }
                if (lexs[1].lexical != "(")
                {
                    throw new SyntaxError(lexs, "( Expected");
                }
                logicalExpression(lexs);
                //Ensure while and if statements contain a proper logical expression
            }
            else if (lexs[0].token == "T_Else")
            {
                statementType = "ElseStmt";
                //Else statement do not have condition so no extra parsing required
            }
            else if (lexs[0].token == "T_For")
            {
                statementType = "ForStmt";
                if (lexs[1].lexical != "(")
                {
                    throw new SyntaxError(lexs, "( Expected");
                }
                int temp = 6;
                if (lexs[2].token == "T_Int")
                {
                    Varable varable = new Varable(lexs.GetRange(2, 2));
                    if (lexs[4].lexical != "=")
                    {
                        throw new SyntaxError(lexs, "= Expected");
                    }
                    temp++;
                }
                int current = expression(lexs, 5) + temp;
                //Check proper Initilize
                List<Lexical> lexList = new List<Lexical>();
                while (lexs[current].lexical != ";")
                {
                    lexList.Add(lexs[current]);
                    current++;
                }
                current +=  logicalExpression(lexList) - 1;
                //Check proper condition
                lexList = new List<Lexical>();
                while (lexs[current].lexical != ")")
                {
                    lexList.Add(lexs[current]);
                    current++;
                }
                current = expression(lexList, 0);
                //Check proper post operation
            }
        }
    }
}