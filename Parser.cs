using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CS_4308_Term_Project
{
    class Parser
    {
        private static List<Lexical> GetBody(List <Lexical> lexs, ref int position)
        {
            List <Lexical> lexList = new List<Lexical>();
            int index = 1;
            int currentIndex = 0;
            position++;
            while (currentIndex < index)
            {
                if (position >= lexs.Count)
                {
                    throw new SyntaxError(lexList, "} Expected");
                }
                if (lexs[position].lexical == "{")
                {
                    index++;
                }
                if (lexs[position].lexical == "}")
                {
                    currentIndex++;
                    if (currentIndex == index)
                    {
                        break;
                    }
                }
                lexList.Add(lexs[position]);
                position++;
            }
            return lexList;
            //Method to return a list of Lexicals in a body
        }
        public static void Parse(Node currentNode)
        {
            Queue<Node> parseQueue = new Queue<Node>();
            List<Lexical> lexs = currentNode.body;

            List<Lexical> lexList = new List<Lexical>();
            for (int x = 0; x < lexs.Count; x++)
            {
                lexList.Add(lexs[x]);
                if (lexs[x].lexical == "{")
                {
                    if ((lexList[0].token == "T_While") || (lexList[0].token == "T_If") || (lexList[0].token == "T_Else") || (lexList[0].token == "T_For"))
                    {
                        if (((currentNode.statements.Count == 0) || (currentNode.statements.Last().statementType != "If")) && (lexList[0].token == "T_Else"))
                        {
                            throw new SyntaxError(lexList, "Else Statement Must Follow If Statement");
                            //Ensures else statements follows an if statement
                        }
                        Statement state = new Statement(lexList, currentNode);
                        state.body = GetBody(lexs, ref x);
                        currentNode.statements.Add(state);
                        //Creates a new statement and adds the body to it
                        parseQueue.Enqueue(state);
                        //Adds the statement to the parseQueue
                        lexList.Clear();
                    }
                    else
                    {
                        Function func = new Function(lexList, currentNode);
                        func.body = GetBody(lexs, ref x);
                        currentNode.functions.Add(func);
                        parseQueue.Enqueue(func);
                        //If not an If, else, while, or for loop assume its a new functions and add it to the parseQueue
                        lexList.Clear();
                    }
                }
                else if (lexs[x].lexical == ";")
                {
                    if ((lexList[0].token == "T_Int") || (lexList[0].token == "T_String") || (lexList[0].token == "T_Float"))
                    {
                        Varable var = new Varable(lexList);
                        currentNode.varables.Add(var);
                        //Declaration of a new varable
                    }
                    else if (lexList[0].token == "T_For")
                    {
                        if (lexs[x].token != ")")
                        {
                            x++;
                            while (lexs[x].token != "')'")
                            {
                                lexList.Add(lexs[x]);
                                x++;
                            }
                            lexList.Add(lexs[x]);
                        }
                        //If the statement is a for loop then add each lexical until the ')' character into the lexlist as opposed to stopping at the ';' characters
                        Statement state = new Statement(lexList, currentNode);
                        lexList.Clear();
                        x++;
                        while (lexs[x].lexical != ";")
                        {
                            lexList.Add(lexs[x]);
                            x++;
                        }
                        lexList.Add(lexs[x]);
                        //Adds the body from ')' to the ';' charcter
                        state.body = new List<Lexical>(lexList);
                        currentNode.statements.Add(state);
                        parseQueue.Enqueue(state);
                        //Adds the statement into the parseQueue
                        lexList.Clear();
                    }
                    else if (lexList[0].token == "T_If")
                    {
                        Statement state = new Statement(lexList, currentNode);
                        for (int y = 0; y < lexList.Count; y++)
                        {
                            lexList.RemoveAt(0);
                            if (lexList[0].lexical == ")")
                            {
                                lexList.RemoveAt(0);
                                break;
                            }
                        }
                        //Removes all elements before and including the ) in the if statement declaraion
                        state.body = new List <Lexical>(lexList);
                        currentNode.statements.Add(state);
                        parseQueue.Enqueue(state);
                        //Adds the state to the parseQueue
                        lexList.Clear();
                    }
                    else if (lexList[0].token == "T_Else")
                    {
                        if ((currentNode.statements.Count == 0)||(currentNode.statements.Last().statementType != "IfStmt"))
                        {
                            throw new SyntaxError(lexList, "Else Statement Must Follow If Statement");
                            //Ensures the else statement follows an if statement
                        }
                        Statement state = new Statement(lexList, currentNode);
                        state.body = lexList.ToArray().Skip(1).ToList();
                        //Ignores the first element in the list but doesn't remove it
                        currentNode.statements.Add(state);
                        parseQueue.Enqueue(state);
                        //Adds the state to the parse Queue
                        lexList.Clear();
                    }
                    else if (lexList[0].token != "T_For")
                    {
                        Statement state = new Statement(lexList, currentNode);
                        currentNode.statements.Add(state);
                        //Creates a new statement unless is a For statement then it is ignored
                    }
                    if (lexList.Count != 0)
                    {
                        if (lexList[0].token != "T_For")
                        {
                            lexList.Clear();
                            //Resets the list if the line not a for loop
                        }
                    }
                }
            }
            foreach (Node x in parseQueue)
            {
                Parse(x);
                //Recursivly calls itself for each node in the parseQueue
            }
        }
    }
}