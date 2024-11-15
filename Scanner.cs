using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace CS_4308_Term_Project
{
    static class Scanner
    {
        private static void scanLine(string line, List<Lexical> list, int lineNumber)
        {
            Lexical lex = new Lexical();
            bool identifier = false;
            for (int x = 0; x < line.Length; x++)
            {
                //Iterates though each character in a given line
                switch (line[x])
                {
                    case ')':
                    case '=':
                    case '{':
                    case '}':
                    case ',':
                    case ';':
                    case '(':
                    case '[':
                    case ']':
                    case '+':
                    case '-':
                    case '*':
                        if (identifier)
                        {
                            //If its not a special character
                            lex.endColumn = x - 1;
                            lex.setLexical(line.Substring(lex.startColumn, (lex.endColumn - lex.startColumn) + 1));
                            list.Add(lex);
                            identifier = false;
                            lex = new Lexical();
                        }
                        lex.startColumn = x;
                        lex.endColumn = x;
                        lex.line = lineNumber;
                        lex.setLexical(line[x].ToString());
                        list.Add(lex);
                        lex = new Lexical();
                        break;

                    case '>':
                    case '<':
                        lex.startColumn = x;
                        lex.line = lineNumber;
                        if (line[x + 1] == '=')
                        {
                            lex.endColumn = x + 1;
                            string temp = line[x].ToString();
                            temp += "=";
                            lex.setLexical(temp);
                            x++;
                            //Determines if it is an equal symbol
                        }
                        else
                        {
                            lex.endColumn = x;
                            lex.setLexical(line[x].ToString());
                        }
                        list.Add(lex);
                        lex = new Lexical();
                        break;

                    case '/':
                        if (identifier)
                        {
                            //If its not a special character
                            lex.endColumn = x - 1;
                            lex.setLexical(line.Substring(lex.startColumn, (lex.endColumn - lex.startColumn) + 1));
                            list.Add(lex);
                            identifier = false;
                            lex = new Lexical();
                        }
                        lex.startColumn = x;
                        lex.line = lineNumber;
                        if (line[x + 1] == '/')
                        {
                            //Runs if its a comment
                            lex.endColumn = (line.Length - 1);
                            return;
                        }
                        //Runs if not a comment
                        else
                        {
                            lex.endColumn = x;
                            lex.setLexical(line[x].ToString());
                        }
                        list.Add(lex);
                        lex = new Lexical();
                        break;

                    case '"':
                        lex.startColumn = x;
                        lex.line = lineNumber;
                        do
                        {
                            x++;
                        } while (line[x] != '"');
                        //Iterates till anothre " character is found
                        lex.endColumn = x;
                        lex.setLexical(line.Substring(lex.startColumn, (lex.endColumn - lex.startColumn) + 1));
                        list.Add(lex);
                        lex = new Lexical();
                        //Adds everthing inbetween the quotes as a single lexical
                        break;

                    case ' ':
                    case '\t':
                        if (identifier)
                        {
                            lex.endColumn = x - 1;
                            lex.setLexical(line.Substring(lex.startColumn, (lex.endColumn - lex.startColumn) + 1));
                            list.Add(lex);
                            identifier = false;
                        }
                        lex = new Lexical();
                        break;

                    case '!':
                        if (line[x + 1] == '=')
                        {
                            //Runs if its a comment
                            lex.startColumn = x;
                            lex.endColumn = (line.Length - 1);
                            lex.line = lineNumber;
                            list.Add(lex);
                            return;
                        }
                        //Runs if not a comment
                        if (identifier)
                        {
                            lex.endColumn = x - 1;
                            lex.setLexical(line.Substring(lex.startColumn, (lex.endColumn - lex.startColumn) + 1));
                            list.Add(lex);
                            identifier = false;
                        }
                        lex.startColumn = x;
                        lex.endColumn = x;
                        lex.line = lineNumber;
                        lex.setLexical(line[x].ToString());
                        list.Add(lex);
                        lex = new Lexical();
                        break;

                    default:
                        if (!identifier)
                        {
                            //if this is the first character in identifier runs
                            identifier = true;
                            lex.startColumn = x;
                            lex.line = lineNumber;
                        }
                        break;
                }
            }
            if (identifier)
            {
                //If line ends and a multiCharacter is still active runs
                lex.endColumn = line.Length - 1;
                lex.setLexical(line.Substring(lex.startColumn, (lex.endColumn - lex.startColumn) + 1));
                list.Add(lex);
            }
        }

        public static List<Lexical> scan(string file)
        {
            List<Lexical> list = new List<Lexical>();
            //Opens file given from argument
            using (StreamReader sr = File.OpenText(file))
            {
                string line;
                int lineNumber = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    scanLine(line, list, lineNumber);
                    lineNumber++;
                }
            }
            return list;
        }
    }
}

