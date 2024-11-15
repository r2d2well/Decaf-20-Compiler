using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_4308_Term_Project
{
    class Lexical
    {
        public int line;
        public int startColumn;
        public int endColumn;
        public string token;
        public string lexical;
        public static bool isFloat(string str)
        {
            float result;
            return float.TryParse(str, out result);
        }

        static bool isInt(string str)
        {
            int result;
            return int.TryParse(str, out result);
        }


        public string getToken(string x)
        {
            //Takes in a string and detemines the token based off of it
            switch (x) {
                case "(":
                case ")":
                case "{":
                case "}":
                case ";":
                case ",":
                case "!":
                case "=":
                case "+":
                case "-":
                case "*":
                case "/":
                case "[":
                case "]":
                    return ("'" + x + "'");
                case "void":
                    return "T_Void";
                case "func":
                    return "T_Func";
                case "int":
                    return "T_Int";
                case "string":
                    return "T_String";
                case "float":
                    return "T_Float";
                case "null":
                    return "T_Null";
                case "Print":
                    return "T_Print";
                case "return":
                    return "T_Return";
                case "||":
                case "&&":
                    return "T_Logical";
                case "if":
                    return "T_If";
                case "else":
                    return "T_Else";
                case "for":
                    return "T_For";
                case "while":
                    return "T_While";
                case "break":
                    return "T_Break";
                case "continue":
                    return "T_Continue";
                case "//":
                    return "T_COMMENT";
                case "==":
                    return "T_Equivelent";
                case "!=":
                    return "T_NotEquivelent";
                case "<":
                    return "T_Less";
                case "<=":
                    return "T_LessEqual";
                case ">":
                    return "T_Greater";
                case ">=":
                    return "T_GreaterEqual";
                case "bool":
                    return "T_Bool";

                default:
                    if ((x[0] == '"') && (x[x.Length - 1] == '"')) return ("T_STRINGCONSTANT");
                    if (isInt(x)) return "T_IntConstant";
                    if (isFloat(x)) return "T_FloatConstant";
                    if ((x == "true") || (x == "false")) return "T_BoolConstant";
                    else
                    {
                        return "T_IDENTIFIER";
                    }
            }
        }

        public void setLexical(string x){
            lexical = x;
            token = getToken(x);
            //When functions call calls the getToken functions with given string
        }
    }
}