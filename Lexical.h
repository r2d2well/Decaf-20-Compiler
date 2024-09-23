#include <string>

#pragma once

class Lexical{
public:
	int line;
	int startColumn;
	int endColumn;
	std::string token;
	std::string lexical;
	void setLexical(std::string);
};