using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_4308_Term_Project
{
    internal class Node
    {
        Node parent;
        public List<Lexical> body;
        public List<Varable> varables;
        public List<Function> functions;
        public List<Statement> statements;
        public Node(Node parent)
        {
            this.parent = parent;
            varables = new List<Varable>();
            statements = new List<Statement>();
            functions = new List<Function>();
        }
        public static int expression(List<Lexical> lexs, int start)
        {
            int current = 0;
            while (current + start < lexs.Count && lexs[current + start].lexical != ";" && lexs[current + start].lexical != ")")
            {
                //Iterates though each lexical in an expression to determine if its valid
                if (current % 2 == 0)
                {

                    if ((lexs[current + start].token != "T_IDENTIFIER") && (lexs[current + start].token != "T_Int") && (lexs[current + start].token != "T_String") && (lexs[current + start].token != "T_IntConstant") && (lexs[current + start].token != "T_STRINGCONSTANT"))
                    {
                        throw new SyntaxError(lexs, "Value Expected");
                    }
                    //If even number lexical then a value is required or else an error will be thrown
                }
                else
                {
                    if ((lexs[current + start].lexical != "=") && (lexs[current + start].lexical != "+") && (lexs[current + start].lexical != "*") && (lexs[current + start].lexical != "-") && (lexs[current + start].lexical != "/"))
                    {
                        throw new SyntaxError(lexs, "Expression Expected");
                    }
                    //If odd number lexical then a arthimitic operation is required or else an error will be thrown
                }
                current++;
            }
            return current;
        }

        public static bool isFunction(Lexical identifier, Node node)
        {
            Node currentNode = node;
            while (currentNode != null)
            {
                foreach (Function x in currentNode.functions)
                {
                    if (x.functionName == identifier.lexical)
                    {
                        return true;
                        //If function with the same name is found
                    }
                    //Iterates though each function in the node
                }
                currentNode = currentNode.parent;
                //Moves to the currentNodes parent if
            }
            return false;
        }

        public static List<Varable> getFunctionArguments(Lexical identifier, Node node)
        {
            Node currentNode = node;
            while (currentNode != null)
            {
                foreach (Function x in currentNode.functions)
                {
                    if (x.functionName == identifier.lexical)
                    {
                        return x.arguments;
                        //If function with the same name is found return its arguments
                    }
                    //Iterates though each function in the node
                }
                currentNode = currentNode.parent;
                //Moves to the currentNodes parent if
            }
            return null;
        }

        private static void printArithmeticExpre(string whitespace, List <Lexical> lexicals)
        {
            for (int x = 0; x < lexicals.Count; x++)
            {
                Lexical lex = lexicals[x];
                //Iterates though each lexical in the given List
                switch (lex.token)
                {
                    case "T_IDENTIFIER":
                        if (lexicals[x + 1].lexical == "(")
                        {
                            //Runs if identifier is a function call
                            Console.WriteLine("{1}{0}\t\tcall:", whitespace, lexicals[0].line);
                            int startIndex = x;
                            int endIndex = x;
                            while (lexicals[endIndex].lexical != ")")
                            {
                                endIndex++;
                            }
                            endIndex++;
                            //Gets the start and end index of the function call
                            printCall(whitespace + "\t\t", lexicals.GetRange(startIndex, endIndex - startIndex));
                            //Calls the printCall function with the sublist from the start and end index
                            x = endIndex;
                        
                        }
                        else
                        {
                            Console.WriteLine("{1}{0}\t\tFieldAccess:\n{1}{0}\t\t\tIdentifier: {2}", whitespace, lex.line, lex.lexical);
                            //If not a function call then prints the identifier information
                        }
                        break;
                    case "'='":
                    case "'+'":
                    case "'/'":
                    case "'*'":
                    case "'-'":
                        Console.WriteLine("{1}{0}\t\tOperator: {2}", whitespace, lex.line, lex.lexical);
                        break;
                        //Prints the operator
                    case "T_STRINGCONSTANT":
                        Console.WriteLine("{1}{0}\t\tStringConstant: {2}", whitespace, lex.line, lex.lexical);
                        break;
                        //Prints the string constant
                    case "T_IntConstant":
                        Console.WriteLine("{1}{0}\t\tIntConstant: {2}", whitespace, lex.line, lex.lexical);
                        break;
                        //Prints the int constant
                }
            }
        }

        private static void printRelationalExpre(string whitespace, List <Lexical> lexicals)
        {
            foreach (Lexical lex in lexicals)
            {
                //Iterates though each lexical in the given List
                switch (lex.token)
                {
                    case "T_IDENTIFIER":
                        Console.WriteLine("{1}{0}\t\tFieldAccess:\n{1}{0}\t\t\tIdentifier: {2}", whitespace, lex.line, lex.lexical);
                        break;
                        //prints the identifier information

                    case "T_IntConstant":
                        Console.WriteLine("{1}{0}\t\tIntConstant: {2}", whitespace, lex.line, lex.lexical);
                        break;
                        //Prints the int constant

                    case "T_STRINGCONSTANT":
                        Console.WriteLine("{1}{0}\t\tStringConstant: {2}", whitespace, lex.line, lex.lexical);
                        break;
                        //Prints the string constant

                    case "T_LessEqual":
                    case "T_GreaterEqual":
                    case "T_Greater":
                    case "T_Less":
                    case "T_Equivelent":
                    case "T_NotEquivelent":
                        Console.WriteLine("{1}{0}\t\tOperator: {2}", whitespace, lex.line, lex.lexical);
                        break;
                        //Prints the relational operator
                }
            }
        }

        private static void printLogicalExpr(string whitespace, List <Lexical> lexical)
        {
            foreach (Lexical lex in lexical)
            {
                //Iterates though each lexical in the given List
                switch (lex.token)
                {
                    case "T_Logical":
                    case "'!'":
                        Console.WriteLine("{1}{0}\t\tOperator: {2}", whitespace, lex.line, lex.lexical);
                        break;
                        //prints the logical operator

                    case "T_IDENTIFIER":
                        Console.WriteLine("{1}{0}\t\tFieldAccess:\n{1}{0}\t\t\tIdentifier: {2}", whitespace, lex.line, lex.lexical);
                        break;
                        //prints the identifier information

                    case "T_BoolConstant":
                        Console.WriteLine("{1}{0}\t\tBoolConstant: {2}", whitespace, lex.line, lex.lexical);
                        break;
                        //prints the boolean constant
                }
            }
        }

        private static void printCall(string whitespace, List<Lexical> lexicals) {
            Lexical lex = lexicals[0];
            Console.WriteLine("{1}{0}\tIdentifier: {2}", whitespace, lex.line, lex.lexical);
            //Prints the identifier of the function
            int x = 2;
            lex = lexicals[x];
            while (lex.lexical != ")")
            {
                //Iterates though each lexical in the function call until all arguments have been found
                if ((lexicals[x + 1].lexical == "+") || (lexicals[x + 1].lexical == "-"))
                {
                    //Looks ahead to see if a argument is a arithmetic expression
                    Console.WriteLine("{1}{0}\t(actual) ArithmeticExpr:", whitespace, lex.line);
                    int startIndex = x;
                    int endIndex = x;
                    while ((lexicals[endIndex].lexical != ",") && (lexicals[endIndex].lexical != ")"))
                    {
                        endIndex++;
                    }
                    //Determine start and end index of the arithmetic expression
                    printArithmeticExpre(whitespace, lexicals.GetRange(startIndex, endIndex - startIndex));
                    //Calls the printArithmeticExpre with the whitespace and sublist of lexical
                    x = endIndex - 1;
                }
                else if (lexicals[x + 1].lexical == "<=")
                {
                    //Looks ahead to see if a argument is a relational expression
                    Console.WriteLine("{1}{0}\t(actual) RelationalExpr:", whitespace, lex.line);
                    int startIndex = x;
                    int endIndex = x;
                    while ((lexicals[endIndex].lexical != ",") && (lexicals[endIndex].lexical != ")"))
                    {
                        endIndex++;
                    }
                    //Determine start and end index of the relational expression
                    printRelationalExpre(whitespace, lexicals.GetRange(startIndex, endIndex - startIndex));
                    //Calls the printRelationalExpre with the whitespace and sublist of lexical
                    x = endIndex - 1;
                }
                else if ((lexicals[x].lexical == "!")||(lexicals[x + 1].lexical == "&&"))
                {
                    //Looks ahead to see if a argument is a logical expression or see if current lexical is !
                    Console.WriteLine("{1}{0}\t(actual) LogicalExpr:", whitespace, lex.line);
                    int startIndex = x;
                    int endIndex = x;
                    while ((lexicals[endIndex].lexical != ",") && (lexicals[endIndex].lexical != ")"))
                    {
                        endIndex++;
                    }
                    //Determine start and end index of the logical expression
                    printLogicalExpr(whitespace, lexicals.GetRange(startIndex, endIndex - startIndex));
                    //Calls the printLogicalExpre with the whitespace and sublist of lexical
                    x = endIndex - 1;
                }
                else if (lex.token == "T_IntConstant")
                {
                    Console.WriteLine("{1}{0}\t(args) IntConstant: {2}", whitespace, lex.line, lex.lexical);
                    //Print a int constant
                }
                else if (lex.token == "T_BoolConstant")
                {
                    Console.WriteLine("{1}{0}\t(args) BoolConstant: {2}", whitespace, lex.line, lex.lexical);
                    //Print a boolean constant
                }
                else if (lex.token == "T_IDENTIFIER")
                {
                    if (lexicals[x + 1].lexical == "(")
                    {
                        //Looks ahead to see if the identifier is a function call
                        Console.WriteLine("{1}{0}\t(actuals) Call:", whitespace, lex.line);
                        int startIndex = x;
                        int endIndex = x;
                        while (lexicals[endIndex].lexical != ")")
                        {
                            endIndex++;
                        }
                        //Determine start and end index of the function call
                        printCall(whitespace + "\t", lexicals.GetRange(startIndex, endIndex - startIndex + 1));
                        //Recursivly call the printCall function with the new sublist
                        x = endIndex + 1;
                    }
                    else
                    {
                        Console.WriteLine("{2}{0}\t(actual) FieldAccess:\n{2}{0}\t\tIdentifier: {1}", whitespace, lex.lexical, lex.line);
                        //Print out identifier information
                    }
                }
                x++;
                lex = lexicals[x];
                //Iterates to next lexical
            }
        }

        private static void printAssignExpre(string whitespace, List<Lexical> lexicals)
        {
            for (int y = 0; y < lexicals.Count; y++)
            {
                Lexical lex = lexicals[y];
                //Iterates though each lexical
                switch (lex.token)
                {
                    case "T_IDENTIFIER":
                        if (lexicals[y + 1].token != "'('")
                        {
                            //Looks ahead to determine if an identifier if not a function call
                            if ((lexicals[y + 1].lexical == "/")||(lexicals[y + 1].lexical == "+"))
                            {
                                //Look ahead to see if a arithmetic expression
                                Console.WriteLine("{1}{0}\tArithmeticExpr:", whitespace, lex.line);
                                int startIndex = y;
                                int endIndex = y ;
                                while (lexicals[endIndex].lexical != ";")
                                {
                                    endIndex++;
                                    if (endIndex == lexicals.Count)
                                    {
                                        break;
                                    }
                                }
                                //Determine the start and end index of the arithmetic expression
                                printArithmeticExpre(whitespace, lexicals.GetRange(startIndex, endIndex - startIndex));
                                //Calls the printArithmeticExpre function with whitespace and sublist
                                y = endIndex - 1;
                                //updates the y varable
                            }
                            else
                            {
                                Console.WriteLine("{1}{0}\tFieldAccess:\n{1}{0}\t\tIdentifier: {2}", whitespace, lex.line, lex.lexical);
                                //Prints the identifier information
                            }
                        }
                        else
                        {
                            Console.WriteLine("{1}{0}\tCall:", whitespace, lex.line);
                            int startIndex = y;
                            int endIndex = y;
                            while (lexicals[endIndex].lexical != ")")
                            {
                                endIndex++;
                            }
                            endIndex++;
                            //Retrieves the start and end index of the function call
                            printCall(whitespace + "\t", lexicals.GetRange(startIndex, endIndex - startIndex));
                            //calls the printCall function with the whitespaces and sublist
                            y = endIndex;
                        }
                        break;
                    case "'='":
                    case "'/'":
                    case "'*'":
                        Console.WriteLine("{1}{0}\tOperator: {2}", whitespace, lex.line, lex.lexical);
                        break;
                        //Prints the operator

                    case "T_STRINGCONSTANT":
                        Console.WriteLine("{1}{0}\tStringConstant: {2}", whitespace, lex.line, lex.lexical);
                        break;
                        //Prints the string constant

                    case "T_IntConstant":
                        Console.WriteLine("{1}{0}\tIntConstant: {2}", whitespace, lex.line, lex.lexical);
                        break;
                        //Prints the int constant
                }
            }
        }

        public static void printParseTree(Node node, int space)
        {
            string whitespace = "";
            for (int x = 0; x < space; x++)
            {
                whitespace += "\t";
                //Adds whitespaces for each line
            }
            foreach (Varable x in node.varables)
            {
                Console.WriteLine("{1}{0}VarDecl:\n{1}\t{0}Type: {2}\n{1}{0}\tIdentifier: {3}", whitespace, x.line, x.type, x.name);
                //Prints out each of the local varables
            }
            foreach(Statement x in node.statements)
            {
                Console.WriteLine("{1}{0}{2}:", whitespace, x.statementLexicals[0].line, x.statementType);
                //Prints out the statement type
                if (x.statementType == "AssignExpr")
                {
                    printAssignExpre(whitespace, x.statementLexicals);
                    //Calls the printAssignExpre if the statement type is an assignment expression
                }
                else if (x.statementType == "PrintStmt")
                {
                    int y = 2;
                    Lexical lex = x.statementLexicals[y];
                    while (lex.lexical != ")")
                    {
                        //Iterates though each lexical in the paretheses of the print statement
                        if (lex.token == "T_IntConstant")
                        {
                            Console.Write("{1}{0}\t(args) ", whitespace, lex.line);
                            Console.WriteLine("IntConstant: {0}", lex.lexical);
                            //Prints out IntConstatnt information
                        }
                        else if (lex.token == "T_STRINGCONSTANT")
                        {
                            Console.Write("{1}{0}\t(args) ", whitespace, lex.line);
                            Console.WriteLine("StringConstant: {0}", lex.lexical);
                            //Prints out StringConstatnt information
                        }
                        else if (lex.token == "T_IDENTIFIER")
                        {
                            if (x.statementLexicals[y + 1].lexical == "(")
                            {
                                //Determines if identifier is a function call or not
                                Console.WriteLine("{1}{0}\t(args) call:", whitespace, x.statementLexicals[0].line);
                                int startIndex = y;
                                int endIndex = y;
                                while (x.statementLexicals[endIndex].lexical != ")")
                                {
                                    endIndex++;
                                }
                                endIndex++;
                                printCall(whitespace + "\t", x.statementLexicals.GetRange(startIndex, endIndex - startIndex));
                                //Retireves the function call lexical's and calls printCall function
                                y = endIndex;
                                //Updates y
                            }
                            else
                            {
                                Console.WriteLine("{1}{0}\t(args) FieldAccess:\n{1}{0}\t\tIdentifier: {2}", whitespace, lex.line, lex.lexical);
                                //If not function call prints the identifier information
                            }
                        }
                        lex = x.statementLexicals[y + 1];
                        y++;
                        //Iterates to the next lexical
                    }
                }
                else if (x.statementType == "ReturnStmt")
                {
                    for (int y = 1; y < x.statementLexicals.Count; y++)
                    {
                        //Iterate though each lexical in the return statement
                        if (y + 1 < x.statementLexicals.Count)
                        {
                            if ((x.statementLexicals[y + 1].lexical == "+") || (x.statementLexicals[y + 1].lexical == "*"))
                            {
                                //Look ahead and if an arithmetic expression is being returned call the arithmeticExpr and break the for loop
                                Console.WriteLine("{1}{0}\tArithmeticExpr:", whitespace, x.statementLexicals[0].line);
                                printArithmeticExpre(whitespace, x.statementLexicals);
                                break;
                            }
                        }
                        if (x.statementLexicals[y].token == "T_IntConstant")
                        {
                            Console.WriteLine("{1}{0}\t(args) IntConstant: {2}", whitespace, x.statementLexicals[0].line, x.statementLexicals[1].lexical);
                            //Print out IntConstant information
                        }
                    }
                }
                else if (x.statementType == "IfStmt")
                {
                    if ((x.statementLexicals[3].lexical == "<=")||(x.statementLexicals[3].lexical == ">=")||(x.statementLexicals[3].lexical == "<")|| (x.statementLexicals[3].lexical == ">"))
                    {
                        //Looks to see if function is a relation expression, if so call the printRelationalExpre
                        Console.WriteLine("{1}{0}\t(test) RelationalExpr:", whitespace, x.statementLexicals[0].line);
                        int startIndex = 2;
                        int endIndex = 2;
                        while (x.statementLexicals[endIndex].lexical != ")")
                        {
                            endIndex++;
                        }
                        printRelationalExpre(whitespace, x.statementLexicals.GetRange(startIndex, endIndex - startIndex));
                    }
                    else
                    {
                        Console.WriteLine("{1}{0}\t(test) FieldAccess:\n{1}{0}\t\t Identifier: {2}", whitespace, x.statementLexicals[0].line, x.statementLexicals[2].lexical);
                        //Print out an identifier information
                    }
                    printParseTree(x, space + 1);
                    //Recursivly calls the printParseTree with the if statements node
                }
                else if (x.statementType == "ElseStmt")
                {
                    printParseTree(x, space + 1);
                    //Recursivly calls the printParseTree with the else statements node
                }
                else if (x.statementType == "ForStmt")
                {
                    Console.WriteLine("{1}{0}\t(init) AssignExpr:", whitespace, x.statementLexicals[0].line);
                    int startIndex = 2;
                    int endIndex = 2;
                    while(x.statementLexicals[endIndex].lexical != ";")
                    {
                        endIndex++;
                    }
                    printAssignExpre(whitespace + "\t", x.statementLexicals.GetRange(startIndex, endIndex - startIndex));
                    //Prints out the intilization expression

                    Console.WriteLine("{1}{0}\t(test) RelationalExpr:", whitespace, x.statementLexicals[0].line);
                    endIndex++;
                    startIndex = endIndex;
                    while (x.statementLexicals[endIndex].lexical != ";")
                    {
                        endIndex++;
                    }
                    printRelationalExpre(whitespace, x.statementLexicals.GetRange(startIndex, endIndex - startIndex));
                    //Prints out the conditional expression

                    Console.WriteLine("{1}{0}\t(step) AssignExpr:", whitespace, x.statementLexicals[0].line);
                    endIndex++;
                    startIndex = endIndex;
                    while (x.statementLexicals[endIndex].lexical != ")")
                    {
                        endIndex++;
                    }
                    printAssignExpre(whitespace + "\t", x.statementLexicals.GetRange(startIndex, endIndex - startIndex));
                    //Prints out the step to be taken each loop
                    printParseTree(x, space + 1);
                    //Recursivly calls the printParseTree with the for statements node
                }
                else if (x.statementType == "Call")
                {
                    printCall(whitespace, x.statementLexicals);
                    //Calls the printCall function
                }
            }
            foreach(Function x in node.functions)
            {
                Console.WriteLine("{1}{0}FuncDecl:\n{1}\t{0}(return type) Type: {2}\n{1}{0}\tIdentifier: {3}", whitespace, x.line, x.returnType, x.functionName);
                //Prints out the function information
                foreach (Varable argument in x.arguments)
                {
                    Console.WriteLine("{1}{0}\t(formals) VarDecl:\r\n{1}{0}\t\tType: {2}\r\n{1}{0}\t\tIdentifier: {3}", whitespace, argument.line, argument.type, argument.name);
                    //Prints out all the arguments in the function
                }
                Console.WriteLine("{1}{0}\t(body) StmtBlock:", whitespace, x.line);
                printParseTree(x, space + 2);
                //Recursivly call printParseTree with the function body
            }
        }
    }
}