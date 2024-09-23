#include <string>
#include "Lexical.h"

bool isInt(std::string x) {
	//Functions that determines if a string is a valid int or not
	try {
		std::stoi(x);
		return true;
	}
	catch (...){
		return false;
	}
}

std::string getToken(std::string x) {
	//Takes in a string and detemines the token based off of it
	if ((x == "(") || (x == ")") || (x == "{") || (x == "}") || (x == ";") || (x == ",") || (x == "!") ||
		(x == "=") || (x == "+") || (x == "-") || (x == "*") || (x == "[") || (x == "]") || (x == "/")) {
		return ("'" + x + "'");
	}
	if (x == "void") return "T_Void";
	if (x == "func") return "T_Func";
	if (x == "int") return "T_Int";
	if (x == "string") return "T_String";
	if (x == "null") return "T_Null";
	if (x == "Print") return "T_Print";
	if (x == "return") return "T_Return";
	if ((x == "||") || (x == "&&")) return "T_logicaland";
	if (x == "if") return "T_If";
	if (x == "else") return "T_Else";
	if (x == "for") return "T_For";
	if (x == "while") return "T_While";
	if (x == "break") return "T_Break";
	if (x == "continue") return "T_Continue";
	if (x == "//") return "T_COMMENT";
	if (x == "==") return "T_Equivelent";
	if (x == "!=") return "T_NotEquivelent";
	if (x == "<") return "T_Less";
	if (x == "<=") return "T_LessEqual";
	if ((x[0] == '"') && (x[x.size() - 1] == '"')) return ("T_STRINGCONSTANT (value = " + x + ")");
	if (isInt(x)) return "T_IntConstant (value = " + x + ")";
	if ((x == "true") || (x == "false")) return "T_BoolConstant (value = " + x + ")";
	else {
		return "T_IDENTIFIER";
	}
}

void Lexical::setLexical(std::string x){
	lexical = x;
	token = getToken(x);
	//When functions call calls the getToken functions with given string
}
